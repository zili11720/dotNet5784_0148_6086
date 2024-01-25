namespace BlApi;
/// <summary>
/// Interface of IBl
/// Access to all the logic interfaces in Bl
/// </summary>
public interface IBl
{
    public IAgent Agent { get; }
    public ITask Task { get; }
    public ITaskInList TaskInList { get; }
    public IAgentInList AgentInList {  get; }   
}
