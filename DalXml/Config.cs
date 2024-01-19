namespace Dal;
/// <summary>
/// Class Config
/// General data of the project
/// </summary>
internal static class Config
{
    //Name of the config file
    static string s_data_config_xml = "data-config";

    //Running numbers for task and dependency
   internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
   internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
}

