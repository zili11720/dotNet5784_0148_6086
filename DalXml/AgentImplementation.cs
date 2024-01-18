using DalApi;
using DO;
namespace Dal;
/// <summary>
/// 
/// </summary>
internal class AgentImplementation:IAgent
{
    readonly string s_agents_xml = "agents";

    public int Create(Agent item)
    {
        if (Read(item.Id) is not null)//Check if this id already exists in the database
            throw new DalAlreadyExistsException($"An agent with ID={item.Id} already exists");
        
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the old list from the xml file
        Agents.Add(item);//Update the list
        XMLTools.SaveListToXMLSerializer(Agents, s_agents_xml);//Write the updated list to the xml file

        return item.Id;
    }

    public void Delete(int id)
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        if (Agents.RemoveAll(it => it.Id == id) == 0)//Delete the agent with the requested id if he/she exists
            throw new DalDoesNotExistException($"Agent with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Agents, s_agents_xml);//Write the updated list to the xml file

    }

    public Agent? Read(int id)
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        return Agents.FirstOrDefault(it => it.Id == id);
    }

    public Agent? Read(Func<Agent, bool> filter)
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        return Agents.FirstOrDefault(filter);
    }

    public IEnumerable<Agent?> ReadAll(Func<Agent, bool>? filter = null)
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        if (filter == null)
            return Agents.Select(item => item);
        else
            return Agents.Where(filter);
    }

    public void Update(Agent item)
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        if (Agents.RemoveAll(it => it.Id == item.Id) == 0)//Remove the old agent if it exists
            throw new DalDoesNotExistException($"Agent with ID={item.Id} does Not exist");
        Agents.Add(item);//Add the updates agent to the list
        XMLTools.SaveListToXMLSerializer(Agents, s_agents_xml);//Write the updated list to the xml file
    }

    public void Clear()
    {
        List<Agent> Agents = XMLTools.LoadListFromXMLSerializer<Agent>(s_agents_xml);//Load the list from the xml file
        Agents.Clear();//Earase all the agents in the list
        XMLTools.SaveListToXMLSerializer(Agents, s_agents_xml);//Write the updated list to the xml file
    }
}
