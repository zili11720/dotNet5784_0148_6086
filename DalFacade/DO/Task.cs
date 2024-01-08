
namespace DO;
/// <summary>
/// The entity Task represents a task given to secret agent
/// </summary>
/// <param name="Id">Personal unique Id of a task</param>
/// <param name="Alias">A nickname for the task</param>
/// <param name="Description">A description of the task</param>
/// <param name="CreatedAtDate">The creation date of the task</param>
/// <param name="RequiredEffortTime">The amount of time (in days) required in order to complete the task</param>
/// <param name="IsMilestone">Milestones of the task</param>
/// <param name="Copmlexity">The minimum agent level required in ordet to complete the task</param>
/// <param name="StartDate">Actual start date of a task</param>
/// <param name="SchedualedDate">Planned start date for a task</param>
/// <param name="DeadlineDate">Planned deadline for a task</param>
/// <param name="CompleteDate">The actual date the task was completed</param>
/// <param name="Deliverables">Deliverables of the task</param>
/// <param name="Remarks">Special remarks about the task</param>
/// <param name="Agentld">The Id of the agent assigned to the task</param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime? CreatedAtDate=null,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone=false,
    DO.AgentExperience? Copmlexity = null,
    DateTime? StartDate = null,
    DateTime? SchedualedDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? Agentld = 0
)
{
    public Task() : this(1, "", "") { }//empty constructor

};
