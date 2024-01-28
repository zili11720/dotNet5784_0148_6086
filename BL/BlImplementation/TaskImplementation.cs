namespace BlImplementation;
using BlApi;
using BO;

internal class TaskImplementation : ITask
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task? boTask)
    {
        if (boTask!.Id <= 0 || boTask.Alias == "")
            return 0;
        IEnumerable<int> dependencies = boTask.DependenciesList!.Select(d => _dal.Dependency.Create(new DO.Dependency(0, d.Id, boTask.Id)));

        DO.Task newDoTask = new DO.Task(boTask.Id, boTask.Alias!, boTask.Description!, boTask.CreatedAtDate, boTask.RequiredEffortTime, false, (DO.AgentExperience?)boTask.Copmlexity, boTask.StartDate, boTask.SchedualedDate, boTask.DeadlineDate, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks, boTask.TaskAgent!.Item1);
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
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Status=TaskStatus.Unscheduled, //////////////
            DependenciesList = null,
            CreatedAtDate = doTask.CreatedAtDate,
            SchedualedDate=doTask.SchedualedDate,
            StartDate = doTask.StartDate,
            RequiredEffortTime = doTask.RequiredEffortTime,
            EstimatedCompleteDate=null,
            DeadlineDate=doTask.DeadlineDate,
            CompleteDate=doTask.CompleteDate,
            Deliverables =doTask.Deliverables,
            Remarks=doTask.Remarks,
            TaskAgent=null,/////////////////
            Copmlexity=(BO.AgentExperience?)doTask.Copmlexity,
     
        };

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
                   Status = TaskStatus.Unscheduled
               };
                

               
    }

    public void Update(BO.Task boTask)
    {
        throw new NotImplementedException();
    }

    public void UpdateScheduledStartDate(int taskId, DateTime? start)
    {
        throw new NotImplementedException();
    }
}
