namespace BO;

/// <summary>
/// An enum that represents three possible specialties of a secret agent
/// </summary>
public enum AgentExperience
{
    Field_agent = 1, Hacker, Investigator ,None
};
/// <summary>
/// An enum that represents a task's status
/// </summary>
public enum TaskStatus
{
    Unscheduled, Scheduled, OnTrack, Done, Delayed ,None
};
/// <summary>
/// An enum that represents the project's status
/// </summary>
public enum ProjectStatus
{
    PlanningTime, ScheduleTime, ExecutionTime
};


