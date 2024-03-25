using BO;

namespace BlApi;
/// <summary>
/// Interface of a task
/// All availabe operations for a logic task 
/// </summary>
public interface ITask
{
    int Create(BO.Task boTask);
    void Delete(int id);
    BO.Task? Read(int id);
    IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? func = null);
    void Update(BO.Task boTask);
    IEnumerable<BO.TaskInList> GetDependenciesList(int id);
    void Clear();
    void CreateAutomaticSchedule();
    TaskInList AddDependency(int taskId,int depId);
    void RemoveDependency(int taskId,int depId);
    BO.TaskStatus CalcStatus(DO.Task task);
    TaskInList ConvertTaskToTaskInList(DO.Task task);
    IEnumerable<BO.Task> ReadAllTasks();

    IEnumerable<BO.GanttRow> GetDetailsForGattRow(Func<BO.GanttRow, bool>? filter = null);
}
