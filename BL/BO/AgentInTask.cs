namespace BO;
/// <summary>
/// A logic entity for an agent as a property in the entity task
/// </summary>
/// <param name="Id">Personal Id of an agent</param>
/// <param name="Name">A name of an agent</param>
/// </summary>
public class AgentInTask
{
    public int Id { get; init; }
    public string? Name { get; init; }

    public override string ToString() => this.ToStringProperty();
}
