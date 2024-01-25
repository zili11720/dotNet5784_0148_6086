namespace BlApi;
/// <summary>
/// Interface of an agent
/// All availabe operations for a logic agent 
/// </summary>
public interface IAgent
{
    public BO.Agent? Read(int id);
    public int Create(BO.Agent boAgent);
    public IEnumerable<BO.Agent> ReadAll(Func<BO.Agent, bool>? func=null);
    public void Delete(int id);
    public void Update(BO.Agent boAgent);
    public BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId);
}
