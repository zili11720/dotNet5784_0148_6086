

namespace DO;
/// <summary>
/// A secret agent entity represents an agent with all its props
/// </summary>
/// <param name="Id">Personal unique Id of an agent</param>
/// <param name="Email">Personal Email of the agent</param>
/// <param name="Cost">Salary per hour</param>
/// <param name="Name">A secret nickname of an agent</param>
/// <param name="Specialty">The specialty of an agent(hacker/field agent/investigator etc.)</param>
/// </summary>
public record Agent
(
    int Id,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.AgentExperience? Specialty = null
)
{
    public Agent() : this(0) { }//empty constructor
}

