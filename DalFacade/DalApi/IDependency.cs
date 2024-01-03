
namespace DalApi;
using DO;
 
public interface IDependency
{
    int Create(Dependency item); //Creates a new dependency in DAL
    Dependency? Read(int id); //Reads ea dependency its ID 
    List<Dependency> ReadAll(); //Reads all dependencies
    void Update(Dependency item); //Updates a dependency
    void Delete(int id); //Deletes a dependency
}
