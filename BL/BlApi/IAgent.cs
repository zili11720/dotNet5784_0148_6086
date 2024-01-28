namespace BlApi;
/// <summary>
/// Interface of an agent
/// All availabe operations for a logic agent 
/// </summary>
public interface IAgent
{
   BO.Agent? Read(int id);
   int Create(BO.Agent boAgent);
   IEnumerable<BO.AgentInList> ReadAll(Func<BO.Agent, bool>? func=null);
   void Delete(int id);
   void Update(BO.Agent boAgent);
   BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId);
}
