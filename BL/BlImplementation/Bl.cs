namespace BlImplementation;
using BlApi;
/// <summary>
///Main logic implementation for BL
///Each field cointains an object of the matching interface
/// </summary>
internal class Bl : IBl
{
    public IAgent Agent => new AgentImplementation();

    public ITask Task =>new TaskImplementation();

    public ITaskInList TaskInList => new TaskInListImplementation();

    public IAgentInList AgentInList =>  new AgentInListImplementation();
}
