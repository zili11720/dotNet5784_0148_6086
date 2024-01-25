namespace BO;
/// <summary>
///Logic entity for a task- represents a task given to secret agent
/// </summary>
/// <param name="Id">Personal unique Id of a task</param>
/// <param name="Alias">A nickname for the task</param>
/// <param name="Description">A description of the task</param>
/// <param name="Status">Status of current task accordind to the dated</param>
/// <param name="DependenciesList">A list of tasks the cuurent task depends on</param>
/// <param name="CreatedAtDate">The creation date of the task</param>
/// <param name="SchedualedDate">Planned start date for a task</param>
/// <param name="StartDate">Actual start date of a task</param>
/// <param name="RequiredEffortTime">The amount of time (in days) required in order to complete the task</param>
/// <param name="EstimatedCompleteDate">Estimated complete date according to dependent tasks</param>
/// <param name="DeadlineDate">Planned deadline for a task</param>
/// <param name="CompleteDate">The actual date the task was completed</param>
/// <param name="Deliverables">Deliverables of the task</param>
/// <param name="Remarks">Special remarks about the task</param>
/// <param name="Agent">The id and name of the agent assigned to the task</param>
/// <param name="Copmlexity">The minimum agent level required in ordet to complete the task</param>

public class Task
{
    public int Id { get; init; }
    public string? Alias { get;init; }////////////////////init/set
    public string? Description { get; set; }////////init/set
    public TaskStatus? Status { get; set; }  
    public List<BO.TaskInList>? DependenciesList { get; set; }
    public DateTime? CreatedAtDate { get; init; }
    public DateTime? SchedualedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? EstimatedCompleteDate { get; set; }
    public DateTime? DeadlineDate { get; init; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public Tuple <int,string>? TaskAgent { get; set; }
    public BO.AgentExperience? Copmlexity {  get; set; }
}
