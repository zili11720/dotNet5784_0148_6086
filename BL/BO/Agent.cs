namespace BO;

public class Agent
{
    public int Id { get; init; }
    public string? Email { get; set; }
    public double? Cost { get; init; }
    public string? Name { get; init; }
    DO.AgentExperience? Specialty { get; init; }
    //CurrentTask ? CurrentTask;
}
