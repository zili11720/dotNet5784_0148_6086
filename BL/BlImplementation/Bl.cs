﻿
namespace BlImplementation;
using BlApi;


/// <summary>
///Main logic implementation for BL
///Each field cointains an object of the matching interface
/// </summary>
internal class Bl : IBl
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    public IAgent Agent => new AgentImplementation(this);
    public ITask Task => new TaskImplementation(this);
    public IUser User => new UserImplementation(this);

    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock.Date; } private set { s_Clock = value; } }

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
    void IBl.SetProjectStartDate(DateTime? _startDate)
    {
        if(GetProjectStatus()== BO.ProjectStatus.ExecutionTime)
            throw new BO.BlProjectStageException("Can't change the project start date after the schedule has been set");
        _dal.StartProjectDate = _startDate;
    }
    /// <summary>
    /// Returns the project start date
    /// </summary>
    DateTime? IBl.GetProjectStartDate()
    {
        return _dal.StartProjectDate;
    }
    /// <summary>
    /// Reset the data of the project. Erase all agents,tasks,dependencies and project start date
    /// </summary>
    /// <exception cref="FormatException"></exception>
    void IBl.ResetData()
    {
        Agent.Clear();
        Task.Clear();
        User.Clear();
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
        User.Clear();
        _dal.StartProjectDate = null;
        DalTest.Initialization.Do();
    }

    public DateTime updateYear()
    {
        return Clock = Clock.AddYears(1);
    }

    public DateTime updateDay()
    {
        return Clock = Clock.AddDays(1);
    }

    public DateTime updateMonth()
    {
        return Clock = Clock.AddMonths(1);
    }

    public DateTime ResetClock()
    {
       return Clock = DateTime.Now;
    }

}