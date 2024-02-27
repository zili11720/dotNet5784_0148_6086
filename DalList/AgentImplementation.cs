namespace Dal;
using DalApi;
using DO;
using System.Linq;

/// <summary>
///Implementation of the interface that manages an Agent entity
///the interface contains the CRUD methods
/// </summary>

internal class AgentImplementation : IAgent
{ 
    public int Create(Agent item)//Add a new agent to the list
    {
        if (Read(item.Id) is not null)//Check if this id already exists in the database
            throw new DalAllreadyExistsException($"An agent with ID={item.Id} already exists");
        //else
        DataSource.Agents.Add(item);
        return item.Id;
    }

    public void Delete(int id)//Delete the agent with the id given if he/she exists
    {
        if (DataSource.Agents.RemoveAll(Agent => Agent.Id == id) == 0)
            throw new DalDoesNotExistException($"An agent with ID={id} does not exist");
    }

    public Agent? Read(int id)//Return the agent with the id given if he/she exists
    {
        return DataSource.Agents.FirstOrDefault(Agent => Agent.Id == id);
    }

    public Agent? Read(Func<Agent, bool> filter)//Return the first agent that satisfies the condition of the func filter
    {
        return DataSource.Agents.FirstOrDefault(filter);
    }
    public IEnumerable<Agent> ReadAll(Func<Agent, bool>? filter = null)//return all the agents or just the agents that fulfiil the condition
    {
        if (filter == null)
            return DataSource.Agents.Select(item => item);
        else
            return DataSource.Agents.Where(filter);
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
            throw new DalDoesNotExistException($"An agent with ID={item.Id} does not exist");
    }

    public void Clear()
    {
        DataSource.Agents.Clear();
    }
}