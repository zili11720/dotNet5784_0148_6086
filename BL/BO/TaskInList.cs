namespace BO;
/// <summary>
///A sub logic entity of a task as a link in a list
/// </summary>
/// <param name="Id">Personal unique Id of a task</param>
/// <param name="Alias">A nickname for the task</param>
/// <param name="Description">A description of the task</param>
/// <param name="Status">Status of current task accordind to the dated</param>

public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; init; }
    public string? Description { get;set; }
    public TaskStatus Status { get; set; }
}
