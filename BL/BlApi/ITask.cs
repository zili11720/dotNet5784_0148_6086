namespace BlApi;
/// <summary>
/// Interface of a task
/// All availabe operations for a logic task 
/// </summary>
public interface ITask
{
    public int Create(BO.Task boTask);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? func = null);
    public void Delete(int id);
    public void Update(BO.Task boTask);
    public void UpdateScheduledStartDate(int taskId,DateTime? start);
}
