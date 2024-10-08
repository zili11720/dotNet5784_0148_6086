﻿using System.Xml.Linq;

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
    internal static int NextTaskId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);
    }
    internal static int NextDependencyId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextDependencyId", value);
    }
    /// <summary>
    /// Set method for the project start/end dates
    /// </summary>
    /// <param name="name">name of the variable:startprojectdate/endprojectdate</param>
    /// <returns>Start/end data of the project</returns>
    internal static DateTime? GetProjectDate(string name)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
        return DateTime.TryParse(root.Element(name)?.Value, out DateTime dateTime) ? dateTime : null;
    }

    internal static void SetProjectDate(string name ,DateTime? dateTime)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
        XElement dateToUpdate = root.Element(name)!;

        if(dateToUpdate is not null)
        {
            dateToUpdate.ReplaceWith(new XElement(name, dateTime.ToString()));
            XMLTools.SaveListToXMLElement(root, s_data_config_xml);
        }
    }

}

