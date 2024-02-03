using DalApi;
using DO;
namespace Dal;
/// <summary>
///Implementation of the interface that manages an Agent entity
///The interface contains the CRUD methods and execute them using xml files
/// </summary>
internal class AgentImplementation:IAgent
{
    readonly string s_agents_xml = "agents";//Name of the file that contains all the dependencies

    /// <summary>
    /// Add a new agent to the file
    /// </summary>
    /// <param name="item">The agent to add</param>
    /// <returns>added agent id</returns>
    /// <exception cref="DalAllreadyExistsException">An agent with the given id already exists</exception>
    public int Create(Agent item)
    {
        if (Read(item.Id) is not null)
            throw new DalAllreadyExistsException($"An agent with ID={item.Id} already exists");
        
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        agents.Add(item);
        XMLTools.SaveListToXMLSerializer(agents, s_agents_xml);

        return item.Id;
    }
    /// <summary>
    /// Delete an agent from the file
    /// </summary>
    /// <param name="id">Id of the agent that needs to be deleted</param>
    /// <exception cref="DalDoesNotExistException">An agent with the given id does not exist in the file</exception>
    public void Delete(int id)
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        if (agents.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Agent with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(agents, s_agents_xml);
    }
    /// <summary>
    /// Return the agent with the given id
    /// </summary>
    /// <param name="id">id of the requested agent</param>
    /// <returns>The requested agent if he exists.else, a default value</returns>
    public Agent? Read(int id)
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        return agents.FirstOrDefault(it => it.Id == id);
    }
    /// <summary>
    /// Return the first agent who meets the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>The requested agent if exists,else return a default value</returns>
    public Agent? Read(Func<Agent, bool> filter)
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        return agents.FirstOrDefault(filter);
    }
    /// <summary>
    /// Return all the agents in the file that meet the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>All the agents in the file that meet the condition</returns>
    public IEnumerable<Agent?> ReadAll(Func<Agent, bool>? filter = null)
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        if (filter == null)
            return agents.Select(item => item);
        else
            return agents.Where(filter);
    }
    /// <summary>
    /// Update an agent with new information
    /// </summary>
    /// <param name="item">The updated agent with the old id and updated details</param>
    /// <exception cref="DalDoesNotExistException">An agent with the given id does not exist in the file</exception>
    public void Update(Agent item)
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        if (agents.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Agent with ID={item.Id} does Not exist");
        agents.Add(item);
        XMLTools.SaveListToXMLSerializer(agents, s_agents_xml);
    }

    /// <summary>
    /// Clear the file completely
    /// </summary>
    public void Clear()
    {
        List<Agent> agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);
        agents.Clear();//Earase all the agents in the list
        XMLTools.SaveListToXMLSerializer(agents, s_agents_xml);//Write the empty list to the xml file
    }
}
