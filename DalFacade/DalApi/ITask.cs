
namespace DalApi;
using DO;

public interface ITask
{
    int Create(Task item); //Creates a new task in DAL
    Task? Read(int id); //Reads a task its ID 
    List<Task> ReadAll(); //Reads all tasks
    void Update(Task item); //Updates a task
    void Delete(int id); //Deletes a task by its Id

}
