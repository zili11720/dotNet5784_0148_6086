namespace Dal;

using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Running numbers
/// Dates of the start+end of the project
/// Lists of the entities agent,task and dependency
/// </summary>
internal static class DataSource
{
    //Running id numbers for the entities task and dependency
    internal static class Config
    {
         internal const int startTaskId = 1;
         private static int nextTaskId = startTaskId;
         internal static int NextTaskId { get => nextTaskId++; }

         internal const int startDependencyId = 1;
         private static int nextDependencyId = startDependencyId;
         internal static int NextDependencyId { get => nextDependencyId++; }

        public static DateTime? StartProjectDate { get; set; } = null;//Start date of the project
        public static DateTime? EndProjectDate { get; set; } = null;//End date of the project
    }

    //Lists of the entities agent,task and dependency
    internal static List<DO.Agent> Agents { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.User> Users { get; } = new();

}