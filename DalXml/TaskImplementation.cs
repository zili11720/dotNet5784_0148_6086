using DalApi;
using DO;
using System.Data.Common;
namespace Dal;
/// <summary>
/// 
/// </summary>
internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        int nextId = Config.NextTaskId;
        DO.Task copy=item with { Id= nextId };
        Tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);

        return nextId;
    }

    public void Delete(int id)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        if (Tasks.RemoveAll(it => it.Id == id) == 0)//Delete the task with the requested id if it exists
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);//Write the updated list to the xml file

    }

    public DO.Task? Read(int id)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        return Tasks.FirstOrDefault(it => it.Id == id);
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        return Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        if (filter == null)
            return Tasks.Select(item => item);
        else
            return Tasks.Where(filter);
    }

    public void Update(DO.Task item)
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        if (Tasks.RemoveAll(it => it.Id == item.Id) == 0)//Remove the old task if it exists
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        Tasks.Add(item);//Add the updated task to the list
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);//Write the updated list to the xml file
    }
    public void Clear()
    {
        List<DO.Task> Tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);//Load the list from the xml file
        Tasks.Clear();//Earase all the agents in the list
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);//Write the updated list to the xml file
    }
}    