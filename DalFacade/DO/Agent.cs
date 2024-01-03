﻿
namespace DO;
/// <summary>
/// A secret agent entity represents an agent with all its props
/// </summary>
/// <param name="Id">Personal unique Id of an agent</param>
/// <param name="Specialty">The specialty of an agent(hacker/field agent/investigator etc.)</param>
/// <param name="Email">Personal Email of the agent</param>
/// <param name="Cost">Salary per hour</param>
/// <param name="Name">A secret nickname of an agent</param>
/// <param name="Level">The professional level of an agent</param>
public record Agent
(
    int Id,
    string? Specialty=null,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    Do.AgentExperience? Level = null
)
{
    public Agent() : this(0) { }//empty ctr
    public Agent(int _Id, string _Specialty, string _Email, string _Name, Do.AgentExperience _Level) : this() 
    { Id = _Id; Specialty = _Specialty; Email = _Email;  Name = _Name; Level = _Level; }//parameters ctr
    

}

