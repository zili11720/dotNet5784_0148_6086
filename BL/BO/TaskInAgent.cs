
namespace BO;
/// <summary>
///A logic entity for a task as a property in the entity agent
/// </summary>
/// <param name="Id">Personal unique Id of a task</param>
/// <param name="Alias">A nickname for the task</param>
/// </summary>

public class TaskInAgent
{
    public int Id { get;init; }
    public string? Alias { get; set;}

    public override string ToString() => this.ToStringProperty();
}
