namespace BlImplementation;
using BlApi;
using BO;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

internal class TaskImplementation : ITask
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Add new task to dal based on a logic tasl 'boTask'
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns>Id of rhe added task</returns>
    /// <exception cref="BlWrongInputException">negative id/alias is empty</exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task? boTask)
    {
        if (boTask!.Id <= 0)
            throw new BlWrongInputException("Id can't be negative");
        if (boTask.Alias == "")
            throw new BlWrongInputException("Task's alias must have a value");

        //create new dependencies based on the dependencies list of task
        IEnumerable<int> dependencies = boTask.DependenciesList!.Select(d => _dal.Dependency.Create(new DO.Dependency(0, d.Id, boTask.Id)));

        DO.Task newDoTask = new DO.Task()
        {   Id=boTask.Id,
            Alias=boTask.Alias!,
            Description=boTask.Description!,
            CreatedAtDate=boTask.CreatedAtDate,
            RequiredEffortTime=boTask.RequiredEffortTime,
            Copmlexity=(DO.AgentExperience?)boTask.Copmlexity,
            StartDate=boTask.StartDate,
            SchedualedDate=boTask.SchedualedDate,
            DeadlineDate=boTask.DeadlineDate,
            CompleteDate=boTask.CompleteDate,
            Deliverables=boTask.Deliverables,
            Remarks=boTask.Remarks,
            AgentId=(boTask.TaskAgent==null)?null:boTask.TaskAgent.Id
        };
        try
        {
            int TaskId = _dal.Task.Create(newDoTask);
            return TaskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);

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

        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
  
            DO.Dependency? doDep = _dal.Dependency.Read(dep => dep.DependsOnTask == id);
            if (doDep != null)
                throw new BO.BlDeletionImpossibleException("Deletion is impossible");
            _dal.Task.Delete(id);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist",ex);
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
        if (doTask != null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        DO.Agent? doAgent = _dal.Agent.Read(agent => agent.Id == doTask!.AgentId);

        BO.Task boTask = new BO.Task()
        {
            Id = id,
            Alias = doTask!.Alias,
            Description = doTask.Description,
            Status = doTask.CalcStatus(),
            DependenciesList = null,
            CreatedAtDate = doTask.CreatedAtDate,
            SchedualedDate = doTask.SchedualedDate,
            StartDate = doTask.StartDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            EstimatedCompleteDate = null,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            TaskAgent = (doAgent == null) ? null : new AgentInTask() { Id = doAgent.Id, Name = doAgent.Name },
            Copmlexity = (BO.AgentExperience?)doTask.Copmlexity,
        };

        return boTask;
    }
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? func = null)
    {
        
        return _dal.Task.ReadAll().Select(t => new BO.TaskInList
        {
            Id = t.Id,
            Alias = t.Alias,
            Description = t.Description,
            Status = t.CalcStatus()
        });

        //להוסיף אופציה לפילטר
    }

    public void Update(BO.Task boTask)
    {
        if (boTask!.Id <= 0)
            throw new BlWrongInputException("Id can't be negative");
        if (boTask.Alias == "")
            throw new BlWrongInputException("Task's alias must have a value");
        ////אם צריך לעדכן תלויות ומהנס

        DO.Task newDoTask = new DO.Task()
        {
            Id = boTask.Id,
            Alias = boTask.Alias!,
            Description = boTask.Description!,
            CreatedAtDate = boTask.CreatedAtDate,
            RequiredEffortTime = boTask.RequiredEffortTime,
            Copmlexity = (DO.AgentExperience?)boTask.Copmlexity,
            StartDate = boTask.StartDate,
            SchedualedDate = boTask.SchedualedDate,
            DeadlineDate = boTask.DeadlineDate,
            CompleteDate = boTask.CompleteDate,
            Deliverables = boTask.Deliverables,
            Remarks = boTask.Remarks,
            AgentId = boTask.TaskAgent!.Id
        };

        try
        {
           _dal.Task.Create(newDoTask);  
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist",ex);
        }
    }

    public void UpdateScheduledStartDate(int taskId, DateTime? start)
    {
        IEnumerable<DO.Task?> doTasks = _dal.Dependency.ReadAll(dep => dep.DependsOnTask == taskId).Select(dep => _dal.Task.Read(dep.Id)).Where(t=>t!=null);
        DO.Task? previousTask = doTasks.FirstOrDefault(task => task.StartDate == null);
        if (previousTask != null)
            throw new BlWrongDateException("Previous tasks don't have a starting date");
        previousTask = doTasks.FirstOrDefault(task => task.CompleteDate>start);
        if (previousTask != null)
            throw new BlWrongDateException("Start date is too early");
        /////לבדוקקקקקקקקק
    }

}
