namespace Dal;
using DalApi;
using DO;

/// <summary>
///Implementation of the interface that manages a task given to a secret agent
///the interface contains the CRUD methods
/// </summary>

internal class TaskImplementation : ITask
{
    public int Create(Task item)//Add a new task to the list
    {
        Task newtask = item with { Id = DataSource.Config.NextTaskId }; 
        DataSource.Tasks.Add(newtask);
        return newtask.Id;
    }

    public void Delete(int id)//Delete the task with the given id
    {
        if(DataSource.Tasks.RemoveAll(Task => Task.Id ==id)==0)
               throw new DalDoesNotExistException( $"Task with ID={id} does Not exist");
    }

    public Task? Read(int id)//Return the task with the id given if it exists
    {
        return DataSource.Tasks.FirstOrDefault(Task => Task.Id == id);
    }
    public Task? Read(Func<Task, bool> filter)//Return the first task that satisfies the condition of the func filter
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)//Return all the tasks in the list or just the tasks that fulfiil the condition
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);

    }
    public void Update(Task item)//Replace the task with the id identical to the id of the task 'item' ,with item
    {
        Task? existingItem = DataSource.Tasks.Find(Task => Task.Id == item.Id);
        if (existingItem != null)
        {
            DataSource.Tasks.Remove(existingItem);//Remove the task with this id from the list
            DataSource.Tasks.Add(item);//Add the updated task to the list
        }
        else
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
    }

    public void Clear()
    {
    }
}
