
namespace DO;

public record Agent
(
    int Id,
    string? Specialty=null,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    Do.AgentExperience? Level = null
);

