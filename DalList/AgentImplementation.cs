namespace Dal;
using DalApi;
using DO;

/// <summary>
///Implementation of the interface that manages an Agent entity
///the interface contains the CRUD methods
/// </summary>

internal class AgentImplementation : IAgent
{
    public int Create(Agent item)//Add a new agent to the list
    {
        if(Read(item.Id) is not null)//Check if this id already exists in the database
            throw new Exception($"An agent with ID={item.Id} already exists");
        //else
        
        DataSource.Agents.Add(item);
        return item.Id; 
    }

    public void Delete(int id)//Delete the agent with the id given if he/she exists
    {
        if (DataSource.Agents.RemoveAll(Agent => Agent.Id == id) == 0)
            throw new Exception($"An agent with ID={id} does not exist");
    }

    public Agent? Read(int id)//Return the agent with the id given if he/she exists
    {
        return DataSource.Agents.Find(Agent => Agent.Id == id);
    }

    public List<Agent> ReadAll()//Return all the agents in the list
    {
        return new List<Agent>(DataSource.Agents);
    }

    public void Update(Agent item)//Replace the agent with the id identical to the id of the agent' item' with item
    {
        Agent? existingItem = DataSource.Agents.Find(Agent => Agent.Id == item.Id);
        if (existingItem != null)
        {
            DataSource.Agents.Remove(existingItem);
            DataSource.Agents.Add(item);
        }
        else
            throw new Exception($"An agent with ID={item.Id} does not exist");
    }
}
