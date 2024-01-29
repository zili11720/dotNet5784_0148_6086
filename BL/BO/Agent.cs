namespace BO;
/// <summary>
/// A secret agent logic entity represents an agent with all its props
/// </summary>
/// <param name="Id">Personal unique Id of an agent</param>
/// <param name="Email">Personal Email of the agent</param>
/// <param name="Cost">Salary per hour</param>
/// <param name="Name">A secret nickname of an agent</param>
/// <param name="Specialty">The specialty of an agent(hacker/field agent/investigator etc.)</param>
/// <param name="CurrentTask">A task's id and alias ,the agent currently working on</param>
/// </summary>
public class Agent
{
    public int Id { get; init; }
    public string? Email { get; set; }
    public double? Cost { get; set; }
    public string? Name { get; init; }
    public BO.AgentExperience? Specialty { get; set; }
    public BO.TaskInAgent? CurrentTask { get; set; } //{ get => CurrentTask; set => CurrentTask = value; }/////////////////////

    public override string ToString() => this.ToStringProperty();
    
}
