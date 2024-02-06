namespace BlImplementation;
using BlApi;
using BO;


/// <summary>
///Main logic implementation for BL
///Each field cointains an object of the matching interface
/// </summary>
internal class Bl : IBl
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    public IAgent Agent => new AgentImplementation();
    public ITask Task => new TaskImplementation();
    public static DateTime? StartProjectDate { get; set; } = null;//new DateTime(2024,5,7);//Start date of the project
    public static DateTime? EndProjectDate { get; set; } = null;//End date of the project

    public static BO.ProjectStatus GetProjectStatus()
    {
        if (StartProjectDate is null)
            return BO.ProjectStatus.PlanningTime;
        if (_dal.Task.ReadAll(task => task.ScheduledDate is null).Any())
            return BO.ProjectStatus.ScheduleTime;
        return BO.ProjectStatus.ExecutionTime;
    }
    //public TaskStatus CalcStatus(DO.Task task)
    //{
    //    if (task.ScheduledDate == null)
    //        return TaskStatus.Unscheduled;
    //    if (task.ScheduledDate != null && task.StartDate < DateTime.Now || task.StartDate == null)
    //        return TaskStatus.Scheduled;
    //    if (task.StartDate >= DateTime.Now && task.CompleteDate < DateTime.Now || task.CompleteDate == null)
    //        return TaskStatus.OnTrack;
    //    if (task.CompleteDate >= DateTime.Now)
    //        return TaskStatus.Done;
    //    else
    //        throw new BlWrongDateException("Task's dates are impossible");
    //}
    //init
    //reset
    //creteluz-bonos
    //CalcStatus
}
