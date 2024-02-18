
namespace BlImplementation;
using BlApi;


/// <summary>
///Main logic implementation for BL
///Each field cointains an object of the matching interface
/// </summary>
internal class Bl : IBl
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    public IAgent Agent => new AgentImplementation();
    public ITask Task => new TaskImplementation();
    //public static DateTime? StartProjectDate { get; set; } = null;//Start date of the project
   // public static DateTime? EndProjectDate { get; set; } = null;//End date of the project

    //// <summary>
    ///Returns the current status of the project:Planning time/Schedule time/Execution time
    /// </summary>
    /// <returns></returns>
    public BO.ProjectStatus GetProjectStatus()
    {
        if (_dal.StartProjectDate is null)
            return BO.ProjectStatus.PlanningTime;
        if (_dal.Task.ReadAll(task => task.ScheduledDate is null).Any())
            return BO.ProjectStatus.ScheduleTime;
        return BO.ProjectStatus.ExecutionTime;
    }
    /// <summary>
    /// Set the project with a start date according to the manager input
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.SetProjectStartDate()
    {
        Console.WriteLine("Enter project start date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
            throw new FormatException("Wrong input");
        _dal.StartProjectDate = _startDate;
    }
    /// <summary>
    /// Reset the data of the project. Erase all agents,tasks,dependencies and project start date
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.ResetData()
    {
        Agent.Clear();
        Task.Clear();
        _dal.StartProjectDate = null;
        _dal.EndProjectDate = null;
    }
    /// <summary>
    /// Initialize the database with basic entities (dalTest.initialization)
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.InitializeData()
    {
        Agent.Clear();
        Task.Clear();
        _dal.StartProjectDate = null;
        DalTest.Initialization.Do();
    }
}