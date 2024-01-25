
namespace BlImplementation;
using BlApi;

internal class AgentImplementation : IAgent
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Agent? boAgent)
    {
        if(boAgent!.Id<=0||boAgent.Name==""||boAgent.Cost<=0||boAgent.Email!.Contains("@gmail.com")==false)
            return 0;
        DO.Agent newDoAgent = new DO.Agent(boAgent.Id, boAgent.Email, boAgent.Cost, boAgent.Name, (DO.AgentExperience?)boAgent.Specialty);
        try
        {
            int agentId = _dal.Agent.Create(newDoAgent);
            return agentId;
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Agent with ID={boAgent.Id} already exists", ex); /////
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId)
    {
        throw new NotImplementedException();
    }

    public BO.Agent? Read(int id)
    {
       DO.Agent? doAgent=_dal.Agent.Read(id);
        if (doAgent == null)
            throw new BO.BlDoesNotExistException($"Agent with ID={id} does Not exist");
        //TaskInList agentCurrentTask=
        return new BO.Agent()
        {
            Id = doAgent.Id,
            Email = doAgent.Email,
            Cost=doAgent.Cost,
            Name=doAgent.Name,
            Specialty=(BO.AgentExperience?)doAgent.Specialty,
            //CurrentTask=(  )
        };
    }

    public IEnumerable<BO.Agent> ReadAll(Func<BO.Agent, bool>? func = null)
    {
        return ((IEnumerable<BO.Agent>)(from DO.Agent doAgent in _dal.Agent.ReadAll()
                select new BO.AgentInList
                {
                    Id=doAgent.Id,
                    Name=doAgent.Name,
                    Specialty = (BO.AgentExperience?)doAgent.Specialty,
                }));        
    }

    public void Update(BO.Agent boAgent)
    {
        throw new NotImplementedException();
    }
}
