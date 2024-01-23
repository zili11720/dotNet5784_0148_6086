namespace BO;

public class Task
{
    public int Id { get; init; }
    public string? Alias { get; set; }////////////////////init/set
    public string? Descriptionv { get; set; }////////init/set
    public TaskStatus? Status { get; set; }
    public TaskInList? DependenciesList { get; set; }
    //Milestone////////////////////
    public DateTime? CreatedAtDate { get; set; }
    public DateTime? SchedualedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? EstimatedCompleteDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public int? Agentld { get; set; }
    public DO.AgentExperience? Copmlexity {  get; set; }
}
