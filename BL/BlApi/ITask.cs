namespace BlApi;
/// <summary>
/// Interface of a task
/// All availabe operations for a logic task 
/// </summary>
public interface ITask
{
   int Create(BO.Task boTask);
   BO.Task? Read(int id);
   IEnumerable<BO.TaskInList> ReadAll();
    //public IEnumerable<BO.TaskInList> ReadAllWithFilter(IEnumerable<BO.>    Func<BO.Task, bool>? func = null);
   void Delete(int id);
   void Update(BO.Task boTask);
   void UpdateScheduledStartDate(int taskId,DateTime? start);
}
