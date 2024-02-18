﻿namespace BlImplementation;
using BlApi;
using BO;
using System.Linq;

internal class TaskImplementation : ITask
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Add new task to dal based on a logic tasl 'boTask'
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns>Id of rhe added task</returns>
    /// <exception cref="BlWrongInputException">negative id/alias is empty</exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.PlanningTime)
            throw new BO.BlProjectStageException("Can't create new tasks after the project has started");

        CheckValidation(boTask);

        DO.Task newDoTask = new DO.Task()
        {
            Id = boTask.Id,
            Alias = boTask.Alias!,
            Description = boTask.Description!,
            CreatedAtDate = boTask.CreatedAtDate,
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
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.PlanningTime)
            throw new BO.BlProjectStageException("Can't deךete tasks after the project has started");

        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        try
        {
            DO.Dependency? doDep = _dal.Dependency.Read(dep => dep.DependsOnTask == id);
            if (doDep is not null)
                throw new BO.BlDeletionImpossibleException("Deletion is impossible");
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

        DO.Agent? doAgent = _dal.Agent.Read(agent => agent.Id == doTask!.AgentId);

        return new BO.Task()
        {
            Id = id,
            Alias = doTask!.Alias,
            Description = doTask.Description,
            Status = CalcStatus(doTask),
            DependenciesList = GetDependenciesList(id).ToList(),
            CreatedAtDate = doTask.CreatedAtDate,
            ScheduledDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            EstimatedCompleteDate = null,
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
            return _dal.Task.ReadAll().Select(t => new BO.TaskInList()
            {
                Id = t.Id,
                Alias = t.Alias,
                Description = t.Description,
                Status = CalcStatus(t)
            });
        else
            return _dal.Task.ReadAll().Select(t => new BO.TaskInList()
            {
                Id = t.Id,
                Alias = t.Alias,
                Description = t.Description,
                Status = CalcStatus(t)
            }).Where(filter);
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
                Complexity = (DO.AgentExperience?)boTask.Complexity,
                StartDate = boTask.StartDate,
                DeadlineDate = boTask.DeadlineDate,
                CompleteDate = boTask.CompleteDate,
                Deliverables = boTask.Deliverables,
                Remarks = boTask.Remarks,
                AgentId = boTask.TaskAgent!.Id
            };
            UpdateScheduledStartDate(boTask.Id, boTask.ScheduledDate);

            _dal.Task.Update(newDoTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist", ex);
        }
    }
    /// <summary>
    /// Updates a task's planned start date
    /// </summary>
    /// <param name="taskId">Id of the task to update</param>
    /// <param name="start">The wanted start date</param>
    /// <exception cref="BlWrongDateException">Date is impossible du to previous tasks dates</exception>
    public void UpdateScheduledStartDate(int taskId, DateTime? start)
    {
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.ScheduleTime)
            throw new BO.BlProjectStageException("Can't update a start date for a task on the current project stage");

        BO.Task boTask = Read(taskId)!;
        if (boTask.DependenciesList is not null && boTask.DependenciesList.Any())
        {
            var previousTasks = boTask.DependenciesList!
                .Select(dep => _dal.Task.Read(dep.Id))
                .Where(dep => dep is not null);

            DO.Task? previousTask = previousTasks.FirstOrDefault(task => task!.ScheduledDate == null);
            if (previousTask is not null)
                throw new BlWrongDateException("Can't schedule a start date because previous tasks don't have a starting date");
            previousTask = previousTasks.FirstOrDefault(task => (task!.ScheduledDate + task.RequiredEffortTime) > start);
            if (previousTask is not null)
                throw new BlWrongDateException("Start date musn't be earlier than previous task's complete date");
        }
        else//task has no dependencies
            if (_dal.StartProjectDate is null || start < _dal.StartProjectDate)
            throw new BlWrongDateException("Start date musn't be earlier than project start date");


        boTask.ScheduledDate = start;
        boTask.EstimatedCompleteDate = start + boTask.RequiredEffortTime;
        Update(boTask);
    }

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
                              .Where(d => d != null)
                              .Select(d => new BO.TaskInList()
                              {
                                  Id = d!.Id,
                                  Alias = d.Alias,
                                  Description = d.Description,
                                  Status = CalcStatus(d),
                              });
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
            throw new BO.BlWrongInputException("Id can't be negative");
        if (string.IsNullOrEmpty(boTask.Alias))
            throw new BO.BlWrongInputException("Task's alias must have a value");
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

        if (s_bl.GetProjectStatus() == BO.ProjectStatus.PlanningTime)
        {
            if (updatedTask.ScheduledDate is not null || updatedTask.TaskAgent is not null)
                throw new BO.BlProjectStageException("Can't update start date or assign an agent on current project stage");
        }
        if (s_bl.GetProjectStatus() == BO.ProjectStatus.ExecutionTime)
        {
            if (updatedTask.RequiredEffortTime != taskToUpdate!.RequiredEffortTime)
                throw new BO.BlProjectStageException("Duration time required for a task can't be changed on current project stage");
        }
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.ExecutionTime)
        {
            if (updatedTask.TaskAgent is not null)
                throw new BO.BlProjectStageException("Can't assign an agent for a task on current project stage");
            DO.Agent? agentOfTask = _dal.Agent.Read(taskToUpdate.TaskAgent!.Id);
            if ((BO.AgentExperience)updatedTask.Complexity! > (BO.AgentExperience)agentOfTask.Specialty!)
                throw new BO.BlWrongAgentForTaskException("Agent specialty can't be lower than task comlexity");
        }
    }
    /// <summary>
    /// Create start date for all the tasks automaticaly
    /// </summary>
    /// <exception cref="BO.BlProjectStageException">Can't assign satart date for tasks on the current project stage</exception>
    public void CreateSchedule()
    {
        //////להוסיף בדיקות....
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.ScheduleTime)
            throw new BO.BlProjectStageException("Can't update a start date for the tasks on the current project stage");

        foreach (var boTaskInList in ReadAll())
        {
            var FinishDates = _dal.Dependency.ReadAll(t => t.DependentTask == boTaskInList.Id)
                                      .Select(t => _dal.Task.Read(t.Id))
                                      .Where(t => t != null)
                                      .Select(t => t.ScheduledDate + t.RequiredEffortTime);
            DateTime? ScheduledStartDate = null;
            if (FinishDates.Any())
                ScheduledStartDate = FinishDates.Max();
            else
                ScheduledStartDate = _dal.StartProjectDate;

            BO.Task botask = Read(boTaskInList.Id)!;

            DO.Task newDoTask = new DO.Task()
            {
                Id = botask.Id!,
                Alias = botask.Alias!,
                Description = botask.Description!,
                CreatedAtDate = botask.CreatedAtDate,
                RequiredEffortTime = botask.RequiredEffortTime,
                Complexity = (DO.AgentExperience?)botask.Complexity,
                StartDate = null,
                DeadlineDate = null,
                CompleteDate = null,
                Deliverables = botask.Deliverables,
                Remarks = botask.Remarks,
                AgentId = null,

                ScheduledDate = DateTime.Now//ScheduledStartDate
            };

            _dal.Task.Update(newDoTask);
        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BlWrongDateException"></exception>
    public TaskStatus CalcStatus(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return TaskStatus.Unscheduled;
        if (task.ScheduledDate != null && task.StartDate < DateTime.Now || task.StartDate == null)
            return TaskStatus.Scheduled;
        if (task.StartDate >= DateTime.Now && task.CompleteDate < DateTime.Now || task.CompleteDate == null)
            return TaskStatus.OnTrack;
        if (task.CompleteDate >= DateTime.Now)
            return TaskStatus.Done;
        else
            throw new BlWrongDateException("Task's dates are impossible");
    }
}
