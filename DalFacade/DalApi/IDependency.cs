
namespace DalApi;
using DO;
/// <summary>
///Represents an interface for managing a dependency betweem two tasks
///the interface contains the CRUD methods
/// </summary>
public interface IDependency
{
    int Create(Dependency item); //Create a new dependency in DAL
    Dependency? Read(int id); //Read ea dependency its ID 
    List<Dependency> ReadAll(); //Read all dependencies
    void Update(Dependency item); //Update a dependency
    void Delete(int id); //Delete a dependency
}
