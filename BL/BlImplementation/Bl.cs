﻿namespace BlImplementation;
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
    public static DateTime? StartProjectDate { get; set; } = null;//Start date of the project
    public static DateTime? EndProjectDate { get; set; } = null;//End date of the project
    public static BO.ProjectStatus GetProjectStatus()
    {
        if (StartProjectDate is null)
            return BO.ProjectStatus.PlanningTime;
        if (_dal.Task.ReadAll(task=>task.StartDate is null).Any())
            return BO.ProjectStatus.ScheduleTime;
        else 
            return BO.ProjectStatus.ExecutionTime;
    }

}
