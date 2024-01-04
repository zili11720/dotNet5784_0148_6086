
namespace DalApi;
using DO;
/// <summary>
///Represents an interface for managing an Agent entity
///the interface contains the CRUD methods
/// </summary>
public interface IAgent
{
    int Create(Agent item); //Create a new Agent
    Agent? Read(int id); //Read an agent by its ID 
    List<Agent> ReadAll(); //Read all Agents
    void Update(Agent item); //Update an Agent
    void Delete(int id); //Delete an Agent by its Id

}
