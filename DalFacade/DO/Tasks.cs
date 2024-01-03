
namespace DO;

public record Tasks
(
     int Id,
    string? Alias = null,  // מתלבטת אם זה צריך להיות בלי סימן שאלה וכאחד השדות של המפתח
    string? Description = null,
    DateTime? CreatedAtData = null,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone = false,
    Do.AgentExperience? Copmlexity=null,
    DateTime? StartData = null,
    DateTime? SchedualedData = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteData = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? Agentld = 0
    );
