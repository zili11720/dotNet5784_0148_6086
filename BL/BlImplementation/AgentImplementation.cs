namespace BlImplementation;
using BlApi;
using System.Collections.Generic;
//using System.Reflection;

internal class AgentImplementation : IAgent
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    

    public int Create(BO.Agent boAgent)
    {
        //boAgent.Id.IsGreaterThenZero();
        //boAgent.Name.IsEmptuString();
        //boAgent.Email.IsNotEmail();
        //boAgent.Cost.IsGreaterThenZero();
        if (boAgent.Id < 0 || boAgent.Name == "" || boAgent.Cost < 0 || boAgent.Email!.Contains("@gmail.com") == false)
            return 0;
        DO.Agent newDoAgent = new DO.Agent(boAgent.Id, boAgent.Email, boAgent.Cost, boAgent.Name, (DO.AgentExperience?)boAgent.Specialty);
        try
        {
            int agentId = _dal.Agent.Create(newDoAgent);
            return agentId;
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Agent with ID={boAgent.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
            if (doTask == null)
                throw new BO.BlDoesNotExistException($"Agent with ID={id} does Not exist");

            //if (CurrentTask == null)
            //    throw new BO.BlDeletionImpossibleException("Agent can't be deleted");

            IEnumerable<DO.Task> doTasks = from DO.Task task in _dal.Task.ReadAll(task => task.Agentld == id)
                                           where task.StartDate >= DateTime.Now
                                           select task;
            if (doTasks.Any())
                throw new BO.BlDeletionImpossibleException("Agent can't be deleted");
            _dal.Agent.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"An agent with ID={id} does not exist", ex);
        }
    }


    public BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId)
    {
        DO.Task? doTask = _dal.Task.Read(TaskId);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={TaskId} does Not exist");
        if (doTask.Agentld != agentId)
            throw new BO.BlWrongAgentForTaskException($"The Agent with the id= {agentId} does not have a task with id={TaskId}");
        return new BO.TaskInList
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Status = BO.Tools.CalcStatus(doTask),
        };
        
    }

    public BO.Agent? Read(int id)
    {
       DO.Agent? doAgent=_dal.Agent.Read(id);
       if (doAgent == null)
            throw new BO.BlDoesNotExistException($"Agent with ID={id} does Not exist");
        DO.Task? doTask= _dal.Task.Read(task => task.Agentld == id && task.StartDate < DateTime.Now && task.CompleteDate > DateTime.Now);
        BO.Agent boAgent= new BO.Agent()
        {
            Id = doAgent.Id,
            Email = doAgent.Email,
            Cost = doAgent.Cost,
            Name = doAgent.Name,
            Specialty = (BO.AgentExperience?)doAgent.Specialty,
        };
        if (doTask != null)
        {
            boAgent.CurrentTask.Id = doTask.Id;
            boAgent.CurrentTask.Alias = doTask.Alias;
        }
        else
            boAgent.CurrentTask = null;
        return boAgent;
    }

    public IEnumerable<BO.AgentInList> ReadAll(Func<BO.Agent, bool>? func = null)
    {
        return from DO.Agent doAgent in _dal.Agent.ReadAll()
                select new BO.AgentInList
                {
                    Id=doAgent.Id,
                    Name=doAgent.Name,
                    Specialty = (BO.AgentExperience?)doAgent.Specialty,
                };        
    }

    public void Update(BO.Agent boAgent)
    {
        throw new NotImplementedException();
    }
}
