namespace BlApi;
/// <summary>
/// Interface of an agent
/// All availabe operations for a logic agent 
/// </summary>
public interface IAgent
{
    int Create(BO.Agent boAgent);
    void Delete(int id);
    BO.Agent? Read(int id);
    IEnumerable<BO.AgentInList> ReadAll(Func<BO.AgentInList, bool>? filter = null);
    void Update(BO.Agent boAgent);
    BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId);
    IEnumerable<BO.TaskInList> GetAllAgentTasks(int agentId);
}
