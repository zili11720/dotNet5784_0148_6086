namespace BO;
/// <summary>
///A sub logic entity of a task as a link in a list
/// </summary>
/// <param name="Id">Personal unique Id of a task</param>
/// <param name="Alias">A nickname for the task</param>
/// <param name="Description">A description of the task</param>
/// <param name="Status">Status of current task accordind to the dated</param>
/// <param name="Copmlexity">The minimum agent level required in ordet to complete the task</param>
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public BO.AgentExperience? Complexity { get; init; }
    public override string ToString() => this.ToStringProperty();
}
