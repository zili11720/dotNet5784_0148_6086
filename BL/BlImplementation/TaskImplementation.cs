namespace BlImplementation;
using BlApi;
using BO;
using System.ComponentModel.Design;

internal class TaskImplementation : ITask
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task? boTask)
    {
        if (boTask!.Id <= 0)
            throw new BlNumberCanNotBeNegetiveException("Id can't be negative");
        if (boTask.Alias == "")
            throw new BlNullPropertyException("Task's alias must have a value");
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
            AgentId=boTask.TaskAgent!.Id
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

    public void Delete(int id)
    {
       DO.Task? doTask = _dal.Task.Read(id);
       if (doTask == null)
           throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        DO.Dependency? doDep = _dal.Dependency.Read(dep => dep.DependsOnTask == id);
        if (doDep != null)
            throw new BO.BlDeletionImpossibleException("Deletion is impossible");
         _dal.Task.Delete(id);
        ////////////לשאול אם צריך לתפוס חריגה מהדל למרות שכבר בדקנו שזה קיים
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        DO.Agent? doAgent=_dal.Agent.Read(agent=>agent.Id==id);
             
        BO.Task boTask=new BO.Task()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Status=Tools.CalcStatus(doTask),
            DependenciesList = null,
            CreatedAtDate = doTask.CreatedAtDate,
            SchedualedDate=doTask.SchedualedDate,
            StartDate = doTask.StartDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            EstimatedCompleteDate=null,
            DeadlineDate=doTask.DeadlineDate,
            CompleteDate=doTask.CompleteDate,
            Deliverables =doTask.Deliverables,
            Remarks =doTask.Remarks,
            Copmlexity=(BO.AgentExperience?)doTask.Copmlexity,
        };
        if (doAgent != null)
        {
           boTask.TaskAgent!.Id = doAgent.Id;
           boTask.TaskAgent.Name = doAgent.Name;
        }
        else
            boTask.TaskAgent = null;
        return boTask;
    }
    //public IEnumerable<BO.TaskInList> ReadAllWithFilter(IEnumerable<BO.TaskInList> taskInLists, )
    public IEnumerable<BO.TaskInList> ReadAll()
    {

        return from DO.Task doTask in _dal.Task.ReadAll()
               select new BO.TaskInList
               {
                   Id = doTask.Id,
                   Alias = doTask.Alias,
                   Description = doTask.Description,
                   Status = Tools.CalcStatus(doTask)
               };
    }

    public void Update(BO.Task boTask)
    {
        if (boTask!.Id <= 0)
            throw new BlNumberCanNotBeNegetiveException("Id can't be negative");
        if (boTask.Alias == "")
            throw new BlNullPropertyException("Task's alias must have a value");
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
        IEnumerable<DO.Task> doTasks = from DO.Dependency dep in _dal.Dependency.ReadAll(dep => dep.DependsOnTask == taskId)
                                       select _dal.Task.Read(dep.DependsOnTask);
                                     //  where this.ScheduledDate < start;
        ///IEnumerable<DO.Task> tasks=doTasks.Where 
    }
}
