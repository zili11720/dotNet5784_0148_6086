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
    public static DateTime? StartProjectDate { get; set; } = null;//new DateTime(2024,5,7);//Start date of the project
    public static DateTime? EndProjectDate { get; set; } = null;//End date of the project
                                                                //// <summary>
    ///Returns the current status of the project:Planning time/Schedule time/Execution time
    /// </summary>
    /// <returns></returns>
    public BO.ProjectStatus GetProjectStatus()
    {
        if (StartProjectDate is null)
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
        StartProjectDate = _startDate;
    }
    /// <summary>
    /// Reset the data of the project. Erase all agents,tasks,dependencies and project start date
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.ResetData()
    {
        Console.Write("Would you like to reset the project data? (Yes/No)");
        string? answer = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (answer == "Yes")
        {
            Agent.Clear();
            Task.Clear();
            StartProjectDate = null;
        }
    }
    /// <summary>
    /// Initialize the database with basic entities (dalTest.initialization)
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.InitializeData()
    {
        Console.Write("Would you like to create Initial data? (Yes/No)");
        string? answer = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (answer == "Yes")
        {
            Agent.Clear();
            Task.Clear();
            StartProjectDate = null;
            //DalTest.Initialization.Do();
        }
    }
}