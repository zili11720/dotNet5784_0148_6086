namespace BlImplementation;
using BlApi;
using BO;
using System.Linq;

internal class TaskImplementation : ITask
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;//Dependency injection


    /// <summary>
    /// Add new task to dal based on a logic tasl 'boTask'
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns>Id of rhe added task</returns>
    /// <exception cref="BlWrongInputException">negative id/alias is empty</exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (_bl.GetProjectStatus() != BO.ProjectStatus.PlanningTime)
            throw new BO.BlProjectStageException("Can't create new task after the project has started");

        CheckValidation(boTask);

        DO.Task newDoTask = new DO.Task()
        {
            // Id = boTask.Id,
            Alias = boTask.Alias!,
            Description = boTask.Description!,
            CreatedAtDate = _bl.Clock,
            RequiredEffortTime = boTask.RequiredEffortTime,
            Complexity = (DO.AgentExperience?)boTask.Complexity,
            StartDate = null,
            ScheduledDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = boTask.Deliverables,
            Remarks = boTask.Remarks,
            AgentId = null//(boTask.TaskAgent == null) ? null : boTask.TaskAgent.Id
        };
        try
        {
            int TaskId = _dal.Task.Create(newDoTask);
            if (boTask.DependenciesList is not null)
            {
                var newDependencies = (from dep in boTask.DependenciesList
                                       select _dal.Dependency.Create(new DO.Dependency(0, TaskId, dep!.Id))).ToList();
            }
            return TaskId;
        }
        catch (DO.DalAllreadyExistsException ex)
        {
            throw new BO.BlAllreadyExistsException($"Task with ID={boTask.Id} already exists", ex);

        }
    }

    /// <summary>
    /// Delete the task with the id given
    /// </summary>
    /// <param name="id">Id of the task to delete</param>
    /// <exception cref="BO.BlDeletionImpossibleException">Can't delete the task because other tasks depend on her</exception>
    /// <exception cref="BO.BlDoesNotExistException">A task with the id given doesn't exist</exception>
    public void Delete(int id)
    {
        if (_bl.GetProjectStatus() != BO.ProjectStatus.PlanningTime)
            throw new BO.BlProjectStageException("Can't delete tasks after the project has started");

        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        try
        {
            DO.Dependency? doDep = _dal.Dependency.Read(dep => dep.DependsOnTask == id);
            if (doDep is not null)
                throw new BO.BlDeletionImpossibleException("Deletion is impossible, other tasks depend on this task!");
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist", ex);
        }
    }

    /// <summary>
    /// Returns logic task based on the matching dal task with the id given
    /// </summary>
    /// <param name="id">Id of the requested task</param>
    /// <returns>A logic task with the given id</returns>
    /// <exception cref="BO.BlDoesNotExistException">A task with the given id doesn't exist</exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);

        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return doToBoTask(doTask);
    }

    private Task doToBoTask(DO.Task doTask)
    {
        var doAgent = _dal.Agent.Read(agent => agent.Id == doTask!.AgentId);
        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask!.Alias,
            Description = doTask.Description,
            Status = CalcStatus(doTask),
            DependenciesList = GetDependenciesList(doTask.Id).ToList(),
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            EstimatedCompleteDate = doTask.ScheduledDate + doTask.RequiredEffortTime,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            TaskAgent = (doAgent == null) ? null : new BO.AgentInTask() { Id = doAgent.Id, Name = doAgent.Name },
            Complexity = (BO.AgentExperience?)doTask.Complexity,
        };
    }

    /// <summary>
    /// Returns all the tasks/the tasks that answer to a given condition
    /// </summary>
    /// <param name="filter">A boolien condition for a task</param>
    /// <returns>IEnumarable of logic tasks in a list</returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null)
    {
        if (filter == null)
            return _dal.Task.ReadAll().Select(t => ConvertTaskToTaskInList(t));
        else
            return _dal.Task.ReadAll().Select(t => ConvertTaskToTaskInList(t)).Where(filter);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task boTask)
    {
        try
        {
            CheckValidation(boTask);
            IsUpdatePossible(boTask);

            DO.Task newDoTask = new DO.Task()
            {
                Id = boTask.Id,
                Alias = boTask.Alias!,
                Description = boTask.Description!,
                CreatedAtDate = boTask.CreatedAtDate,
                RequiredEffortTime = boTask.RequiredEffortTime,
                ScheduledDate = boTask.ScheduledDate,
                Complexity = (DO.AgentExperience?)boTask.Complexity,
                StartDate = boTask.StartDate,
                DeadlineDate = boTask.DeadlineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                AgentId = boTask.TaskAgent is null ? null : boTask.TaskAgent.Id
            };
            _dal.Task.Update(newDoTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist", ex);
        }
    }
    /// <summary>
    /// Updates a task's planned start date menually
    /// </summary>
    /// <param name="taskId">Id of the task to update</param>
    /// <param name="start">The wanted start date</param>
    /// <exception cref="BlWrongDateException">Date is impossible du to previous tasks dates</exception>
    //public void UpdateScheduledStartDate(int taskId, DateTime? start)
    //{
    //    if (_bl.GetProjectStatus() != BO.ProjectStatus.ScheduleTime)
    //        throw new BO.BlProjectStageException("Can't update a start date for a task on the current project stage");

    //    BO.Task boTask = Read(taskId)!;
    //    //Check if the given scheduled start date is later than planned complete dates of previous tasks
    //    if (boTask.DependenciesList is not null && boTask.DependenciesList.Any())
    //    {
    //        var previousTasks = boTask.DependenciesList!
    //            .Select(dep => _dal.Task.Read(dep.Id))
    //            .Where(dep => dep is not null);

    //        DO.Task? previousTask = previousTasks.FirstOrDefault(task => task!.ScheduledDate == null);
    //        if (previousTask is not null)
    //            throw new BlWrongDateException("Can't schedule a start date because previous tasks don't have a starting date");
    //        previousTask = previousTasks.FirstOrDefault(task => (task!.ScheduledDate + task.RequiredEffortTime) > start);
    //        if (previousTask is not null)
    //            throw new BlWrongDateException("Start date musn't be earlier than previous task's complete date");
    //    }
    //    else//task has no dependencies
    //        if (_dal.StartProjectDate is null || start < _dal.StartProjectDate)
    //        throw new BlWrongDateException("Start date musn't be earlier than project start date");


    //    boTask.ScheduledDate = start;
    //    boTask.EstimatedCompleteDate = start + boTask.RequiredEffortTime;
    //    Update(boTask);
    //}

    /// <summary>
    /// The methos gets a task's id and finds all the tasks that this task depends on them
    /// (All the tasks that happen before the cuurent task and connected to this task)
    /// </summary>
    /// <param name="id">Id of a task</param>
    /// <returns>A list of tasks this task depends on</returns>
    public IEnumerable<BO.TaskInList> GetDependenciesList(int id)
    {
        return _dal.Dependency.ReadAll(d => d.DependentTask == id)
                              .Select(d => _dal.Task.Read(d.DependsOnTask))
                              .Where(d => d is not null)
                              .Select(d => ConvertTaskToTaskInList(d!));
    }

    public void Clear()
    {
        _dal!.Task.Clear();
        _dal!.Dependency.Clear();
    }
    /// <summary>
    /// Check validation of the fields id and alias
    /// </summary>
    /// <param name="boTask">a logic bl task</param>
    /// <exception cref="BlWrongInputException">Wrong value</exception>
    private void CheckValidation(BO.Task boTask)
    {
        if (boTask!.Id < 0)
            throw new BO.BlWrongInputException("Id is not valid");
        if (string.IsNullOrEmpty(boTask.Alias))
            throw new BO.BlWrongInputException("Task's alias must have a value");
        if (boTask.Complexity is null || boTask.Complexity is AgentExperience.None)
            throw new BO.BlWrongInputException("Task's complexity must be declared");
        if (boTask.RequiredEffortTime is null)
            throw new BO.BlWrongInputException("Task's required effort time must have a value");
        if (boTask.StartDate is not null && boTask.StartDate < boTask.ScheduledDate)
            throw new BO.BlWrongInputException("Task's start date can't be earlier than the scheduled date");
        if (boTask.StartDate is not null && boTask.CompleteDate is not null && boTask.StartDate > boTask.CompleteDate)
            throw new BO.BlWrongInputException("Task's complete date can't be earlier than the start date");
        if (boTask.StartDate is not null && boTask.EstimatedCompleteDate is not null && boTask.StartDate > boTask.EstimatedCompleteDate)
            throw new BO.BlWrongInputException("Task's complete date can't be earlier than the start date");
    }
    /// <summary>
    /// Check if the update of certain fields is possible according to the current project status and other parameters
    /// </summary>
    /// <param name="updatedTask">Task with the updated data from the user</param>
    /// <exception cref="BO.BlProjectStageException">Project stage doesn't allow changes to the field</exception>
    /// <exception cref="BO.BlWrongAgentForTaskException">Agent specialty is lower than the task complexity</exception>
    private void IsUpdatePossible(BO.Task updatedTask)
    {
        BO.Task? taskToUpdate = Read(updatedTask.Id);

        if (_bl.GetProjectStatus() != BO.ProjectStatus.ExecutionTime)
        {
            if (updatedTask.StartDate is not null)
                throw new BO.BlProjectStageException("Can't update start on current project stage");
            if (updatedTask.CompleteDate is not null)
                throw new BO.BlProjectStageException("Can't update complete date on current project stage");
            if (updatedTask.TaskAgent is not null)
                throw new BO.BlProjectStageException("Can't assign an agent for a task on current project stage");
        }
        if (updatedTask.StartDate is not null && updatedTask.TaskAgent.Id == 0)
            throw new BO.BlProjectStageException("The start date can't be updated because no agent is performing the task");
        if (_bl.GetProjectStatus() != BO.ProjectStatus.PlanningTime)
        {
            if (updatedTask.RequiredEffortTime != taskToUpdate!.RequiredEffortTime)
                throw new BO.BlProjectStageException("Duration time required for a task can't be changed on current project stage");
        }
        if (_bl.GetProjectStatus() == BO.ProjectStatus.ExecutionTime)
        {

            if (updatedTask.TaskAgent is not null && updatedTask!.TaskAgent.Id!=0)
            {
                BO.Agent? agentOfTask = _bl.Agent.Read(updatedTask!.TaskAgent.Id);
                if ((BO.AgentExperience)updatedTask.Complexity! > (BO.AgentExperience)agentOfTask!.Specialty!)
                    throw new BO.BlWrongAgentForTaskException("Agent specialty can't be lower than task comlexity");
                if (taskToUpdate.TaskAgent is not null && updatedTask.TaskAgent.Id != taskToUpdate.TaskAgent.Id && agentOfTask.CurrentTask is not null && agentOfTask.CurrentTask.Id!=0)
                    throw new BO.BlProjectStageException("This agent is allready working on a task right now");
            }
            if (updatedTask.Complexity > taskToUpdate!.Complexity)
                throw new BO.BlProjectStageException("Task's complexity can't be raised on current project stage");
            if (updatedTask.CompleteDate is not null && updatedTask.StartDate is null)
                throw new BO.BlProjectStageException("Task's complete date can't be declared before the task has started");
        }
    }
    /// <summary>
    /// Create start date for all the tasks automaticaly
    /// </summary>
    /// <exception cref="BO.BlProjectStageException">Can't assign start date for tasks on the current project stage</exception>
    public void CreateAutomaticSchedule()
    {
        if (_bl.GetProjectStatus() == BO.ProjectStatus.PlanningTime)
            throw new BO.BlProjectStageException("Can't update a start date for the tasks before the project start date is decided");
        if (_bl.GetProjectStatus() == BO.ProjectStatus.ExecutionTime)
            throw new BO.BlProjectStageException("Can't update a start date for the tasks after the project has lunched");
        foreach (DO.Task doTask in _dal.Task.ReadAll())
        {
            BO.Task botask = Read(doTask.Id)!;
            RecursiveAutomaticSchedule(botask);
        };
        foreach (DO.Task doTask in _dal.Task.ReadAll())
        {
            BO.Task botask = Read(doTask.Id)!;
            RecursiveAutomaticSchedule(botask);
        };
    }
    /// <summary>
    /// Recursive function for creation of start dates
    /// </summary>
    /// <param name="boTask">The task that needs a scheduled date</param>
    private void RecursiveAutomaticSchedule(BO.Task boTask)
    {
        if (boTask.ScheduledDate is not null)
            return;
        IEnumerable<BO.Task> dependentTasks = _dal.Dependency.ReadAll(t => t.DependentTask == boTask.Id)
                                       .Select(t => Read(t.DependsOnTask))
                                       .Where(t => t is not null && t.ScheduledDate is null).Select(t => t)!;

        //If all previous tasks have scheduled start dates
        if (!dependentTasks.Any())
        {
            CreateScheduledStartDate(boTask);
            return;
        }
        //else, recursive call
        foreach (BO.Task task in dependentTasks)
        {
            RecursiveAutomaticSchedule(task);
        }
        


    }
    /// <summary>
    /// Create a scheduled start date for a certain task
    /// </summary>
    /// <param name="botask">The task that needs a scheduled date</param>
    private void CreateScheduledStartDate(BO.Task botask)
    {
        //If the task has no previous tasks
        if (!GetDependenciesList(botask.Id).Any())
        {
            botask.ScheduledDate = _dal.StartProjectDate;
            botask.EstimatedCompleteDate = botask.ScheduledDate + botask.RequiredEffortTime;
            Update(botask);
            return;
        }
        //Get finish dates of previous tasks
        //var finishDates = dependenciesList.Select(t=>Read(t.Id)).Select(t=>t.EstimatedCompleteDate);
        var finishDates = _dal.Dependency.ReadAll(t => t.DependentTask == botask.Id)
                                     .Select(t => _dal.Task.Read(t.DependsOnTask))
                                     .Where(t => t is not null)
                                     .Select(t => t!.ScheduledDate + t.RequiredEffortTime);
        //Set scheduled start date as the maximal finish date of the previous tasks
        DateTime? ScheduledStartDate = finishDates.Max();
        botask.ScheduledDate = ScheduledStartDate;
        botask.EstimatedCompleteDate = botask.ScheduledDate + botask.RequiredEffortTime;
        Update(botask);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BlWrongDateException"></exception>
    public TaskStatus CalcStatus(DO.Task task)
    {
        if (task.ScheduledDate + task.RequiredEffortTime < _bl.Clock && task.CompleteDate == null)
            return TaskStatus.Delayed;
        if (task.CompleteDate is not null)
            return TaskStatus.Done;
        if (task.ScheduledDate == null)
            return TaskStatus.Unscheduled;
        if (task.ScheduledDate != null && task.StartDate < _bl.Clock || task.StartDate == null)
            return TaskStatus.Scheduled;
        if (task.StartDate != null && task.StartDate >= _bl.Clock && task.CompleteDate < _bl.Clock || task.CompleteDate == null)
            return TaskStatus.OnTrack;
        else
            throw new BlWrongDateException("Task's dates are impossible");
    }
    /// <summary>
    /// Convert a DO.task to BO.TaslInList
    /// </summary>
    /// <param name="task">DO.task</param>
    /// <returns>A BO.TaskInList</returns>
    public TaskInList ConvertTaskToTaskInList(DO.Task task)
    {
        return new BO.TaskInList()
        {
            Id = task!.Id,
            Alias = task.Alias,
            Description = task.Description,
            Status = CalcStatus(task),
            Complexity = (BO.AgentExperience?)task.Complexity,
        };
    }
    /// <summary>
    /// check if the dependencies create a circle
    /// </summary>
    /// <param name="dependsTask"></param>
    /// <param name="dependOnTask"></param>
    /// <param name="dependencies"></param>
    /// <returns></returns>
    private bool isThereCircle(int dependsTask, int dependOnTask, IEnumerable<DO.Dependency> dependencies)
    {
        if (dependsTask == dependOnTask)
            return true;

        foreach (var dependncy in dependencies.Where(dependncy => dependncy.DependentTask == dependOnTask))
            return isThereCircle(dependsTask,dependncy.DependentTask, dependencies);

        return false;


        //if (isThereCircle(taskId, depId, _dal.Dependency.ReadAll()))
        //    throw new BO.BlWrongInputException("This dependency create a circle");
    }
    /// <summary>
    /// Adds a dependency between two tasks
    /// </summary>
    /// <param name="taskId">Id of the current task</param>
    /// <param name="depId">Id of the dependent task</param>
    /// <returns>The new dependency as T</returns>
    /// <exception cref="BO.BlDoesNotExistException">The id given doesn't belong to any task</exception>
    /// <exception cref="BO.BlWrongInputException">The two id's are identical</exception>
    /// <exception cref="BO.BlAllreadyExistsException">The dependency between the two tasks allready exists</exception>
    TaskInList ITask.AddDependency(int taskId, int depId)
    {
        if (_bl.GetProjectStatus() == BO.ProjectStatus.ExecutionTime)
            throw new BO.BlProjectStageException("Can't add dependencies for a task after the project has lunched");
        DO.Task? depTask = _dal.Task.Read(depId);
        if (depTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID={depId} does Not exist");

        if (taskId == depId)
            throw new BO.BlWrongInputException("A task can't depend on itself");

        IEnumerable<TaskInList> dependencies = GetDependenciesList(taskId).Where(t => t.Id == depId);
        if (dependencies.Any())
            throw new BO.BlAllreadyExistsException("This dependency allready exists");
        _dal.Dependency.Create(new DO.Dependency(0, taskId, depId));
        DO.Task task = _dal.Task.Read(taskId)!;
        return ConvertTaskToTaskInList(task);
    }


    /// <summary>
    /// Deletes a dependency from a task's dependency
    /// </summary>
    /// <param name="taskId">Id of the current task</param>
    /// <param name="depId">Id of the dependent task</param>
    public void RemoveDependency(int taskId, int depId)
    {
        if (_bl.GetProjectStatus() == BO.ProjectStatus.ExecutionTime)
            throw new BO.BlProjectStageException("Can't remove dependencies from a task after the project has lunched");
        var dependency = _dal.Dependency.Read(d => d.DependentTask == taskId && d.DependsOnTask == depId);
        if (dependency is not null)
            _dal.Dependency.Delete(dependency.Id);
    }

    public IEnumerable<Task> ReadAllTasks() =>
   _dal.Task.ReadAll().Select(task => doToBoTask(task));

    public IEnumerable<BO.GanttRow> GetDetailsForGattRow(Func<BO.GanttRow, bool>? filter = null)
    {
        return (from task in ReadAll()
                let t = _dal.Task.Read(task.Id)
                let start = (int)(t.ScheduledDate - _bl.GetProjectStartDate()).Value.TotalDays
                select new BO.GanttRow()
                {
                    ID = task.Id,
                    Name = task.Alias,
                    TasksDays = (int)t.RequiredEffortTime.Value.TotalDays * 16,
                    StartOffset = start * 16,
                    EndOffset = (int)(start + t.RequiredEffortTime.Value.TotalDays) * 16,
                    Dependencies = StringDependencies(task.Id),
                    Status = CalcStatus(t),
                }).ToList().OrderBy(x => x.ID);
    }

    private string StringDependencies(int id)
    {
        var depTasks = _dal.Dependency.ReadAll(x => x.DependentTask == id)
                                    .Where(dependency => dependency.DependentTask == id)
                                    .Select(dependency => dependency.DependsOnTask.ToString() + " ");
        string dep = "";
        foreach (string tmp in depTasks)
        {
            dep += tmp.ToString();
        }
        return dep;
    }

}