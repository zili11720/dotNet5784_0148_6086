
namespace DO;
/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Alias"></param>
/// <param name="Description"></param>
/// <param name="CreatedAtData"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="IsMilestone"></param>
/// <param name="Copmlexity"></param>
/// <param name="StartData"></param>
/// <param name="SchedualedData"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteData"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
/// <param name="Agentld"></param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime? CreatedAtData = null,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone = false,
    Do.AgentExperience? Copmlexity = null,
    DateTime? StartData = null,
    DateTime? SchedualedData = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteData = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? Agentld = 0
    )
{
    public Task() : this(0, "", "") { }
    public Task(DateTime _CreatedAtData, TimeSpan _RequiredEffortTime, bool _IsMilestone, Do.AgentExperience _Copmlexity, DateTime _StartData, DateTime _SchedualedData, DateTime _DeadlineDate, DateTime _CompleteData, string _Deliverables, string _Remarks, int _Agentld) : this() 
    { 
      CreatedAtData = _CreatedAtData;
      RequiredEffortTime = _RequiredEffortTime;
      IsMilestone = _IsMilestone;
      Copmlexity = _Copmlexity; 
      StartData=_StartData;
      SchedualedData = _SchedualedData;
      DeadlineDate = _DeadlineDate;
      CompleteData = _CompleteData;
      Deliverables = _Deliverables;
      Remarks = _Remarks;
      Agentld = _Agentld;
     }
};
