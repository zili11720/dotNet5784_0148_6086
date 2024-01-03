
namespace DalApi;
using DO;

public interface IAgent
{
    int Create(Agent item); //Creates a new Agent
    Agent? Read(int id); //Reads an agent by its ID 
    List<Agent> ReadAll(); //Reads all Agents
    void Update(Agent item); //Updates an Agent
    void Delete(int id); //Deletes an Agent by its Id

}
