﻿using DalApi;
namespace Dal;
/// <summary>
/// Singleton static class DalXml
/// Data of the project is stored in xml files
/// </summary>
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IAgent Agent =>  new AgentImplementation();

    public ITask Task =>  new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IUser User => new UserImplementation();

    public DateTime? StartProjectDate { get { return Config.GetProjectDate(nameof(StartProjectDate)); } set { Config.SetProjectDate(nameof(StartProjectDate), value); } } 

    public DateTime? EndProjectDate { get { return Config.GetProjectDate(nameof(EndProjectDate)); } set { Config.SetProjectDate(nameof(EndProjectDate), value); } } 
}
