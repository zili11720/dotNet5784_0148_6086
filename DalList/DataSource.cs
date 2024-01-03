namespace Dal;
/// <summary>
/// Lists of the entities Agent,task and dependency
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
    }
    internal static List<DO.Agent> Agents { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();

}