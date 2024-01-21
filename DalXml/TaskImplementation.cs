using DalApi;
using DO;
using System.Data.Common;
namespace Dal;
/// <summary>
///Implementation of the interface that manages a task given to a secret agent
///The interface contains the CRUD methods and execute them using xml files
/// </summary>
internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";//Name of the file that contains all the tasks
    /// <summary>
    /// Add a new task to the file
    /// </summary>
    /// <param name="item">A 'temporary' task with the details of the task that needs to be added</param>
    /// <returns>The id of the new task</returns>
    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        int nextId = Config.NextTaskId;//Get the next running number
        DO.Task copy=item with { Id= nextId };
        tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        return nextId;
    }
    /// <summary>
    /// Delete the task with the id given from the file if it exists
    /// </summary>
    /// <param name="id">Id of the task that needs to be deleted</param>
    /// <exception cref="DalDoesNotExistException">A task with the given id does not exist in the file</exception>
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (tasks.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

    }
    /// <summary>
    ///Return the task with the given id if it exists
    /// </summary>
    /// <param name="id">Id of the requested task</param>
    /// <returns>The requested task if it exists.else, a default value</returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(it => it.Id == id);
    }
    /// <summary>
    /// Return the first task that meets the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>The requested task if it exists.else, a default value</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }
    /// <summary>
    /// Return all the tasks in the file that meet the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>All the tasks in the file that meet the condition</returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (filter == null)
            return tasks.Select(item => item);
        else
            return tasks.Where(filter);
    }
    /// <summary>
    /// Update a task with new information
    /// </summary>
    /// <param name="item">The updated task with the 'old' id and updated details</param>
    /// <exception cref="DalDoesNotExistException">A task with the given id does not exist in the file</exception>
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (tasks.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }
    /// <summary>
    /// Clear the file completely from all the tasks
    /// </summary>
    public void Clear()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        tasks.Clear();//Earase all the agents in the list
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);//Write the updated list to the xml file
    }
}    