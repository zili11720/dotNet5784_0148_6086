namespace BO;
/// <summary>
/// A sub logic entity of an agent as a link in a list
/// </summary>
/// <param name="Id">Personal unique Id of an agent</param>
/// <param name="Name">A secret nickname of an agent</param>
/// <param name="Specialty">The specialty of an agent(hacker/field agent/investigator etc.)</param>
/// </summary>
public class AgentInList
{
    public required int Id { get; init;}
    public string? Name { get; init;}
    public BO.AgentExperience? Specialty { get; init;}

    public BO.TaskInAgent? CurrentTask { get; set; }

    public override string ToString() => this.ToStringProperty();
}
