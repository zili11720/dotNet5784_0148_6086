﻿namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;
//using System.Reflection;

internal class AgentImplementation : IAgent
{

    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Add an agent to dal according to the given Bl agent
    /// </summary>
    /// <param name="boAgent">Alogic agent</param>
    /// <returns>id of the agent adda</returns>
    /// <exception cref="BlWrongInputException">negetive id/name is missing/negative cost/wrong email format</exception>
    /// <exception cref="BO.BlAlreadyExistsException">new id isn't unique</exception>
    public int Create(BO.Agent boAgent)
    {
        CheckValidation(boAgent);

        try
        {
            DO.Agent newDoAgent = new DO.Agent(boAgent.Id, boAgent.Email, boAgent.Cost, boAgent.Name, (DO.AgentExperience?)boAgent.Specialty);
            int agentId = _dal.Agent.Create(newDoAgent);
            return agentId;
        }
        catch (DO.DalAllreadyExistsException ex)
        {
            throw new BO.BlAllreadyExistsException($"Agent with ID={boAgent.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Delete the agent with the id given
    /// An agent can be deleted only if there arn't tasks on track/finished tasks
    /// </summary>
    /// <param name="id">id of the agent to delete</param>
    /// <exception cref="BO.BlDeletionImpossibleException">A task that belongs to this agent already began/was done</exception>
    /// <exception cref="BO.BlDoesNotExistException">The agent with the given id doesn't exist</exception>
    public void Delete(int id)
    {
        try
        {
            DO.Agent? doAgent = _dal.Agent.Read(id);
            if (doAgent is null)
                throw new BO.BlDoesNotExistException($"An agent with ID={id} does not exist");

            //check if this agent allresdy started/finished any tasks
            IEnumerable<DO.Task> doTasks = from DO.Task task in _dal.Task.ReadAll(task => task.AgentId == id)
                                           where task.StartDate >= DateTime.Now
                                           select task;
            if (doTasks.Any())
                throw new BO.BlDeletionImpossibleException("Agent with allocated tasks can't be deleted");

            _dal.Agent.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"An agent with ID={id} does not exist", ex);
        }
    }
    /// <summary>
    /// Returns a logic agent with the id given based on the matching agent from dal
    /// </summary>
    /// <param name="id">Id of the wanted agent</param>
    /// <returns>A logic agent with the given id</returns>
    /// <exception cref="BO.BlDoesNotExistException">Agent with the given id wasn't found</exception>
    public BO.Agent? Read(int id)
    {
        DO.Agent? doAgent = _dal.Agent.Read(id);
        if (doAgent == null)
            throw new BO.BlDoesNotExistException($"An agent with ID={id} does not exist");

        DO.Task? doTask = _dal.Task.Read(task => task.AgentId == id && task.CalcStatus() == BO.TaskStatus.OnTrack/*StartDate < DateTime.Now && task.CompleteDate > DateTime.Now*/);

        BO.Agent boAgent = new BO.Agent()
        {
            Id = doAgent.Id,
            Email = doAgent.Email,
            Cost = doAgent.Cost,
            Name = doAgent.Name,
            Specialty = (BO.AgentExperience?)doAgent.Specialty,
            CurrentTask = (doTask == null) ? null : new BO.TaskInAgent() { Id = doTask.Id, Alias = doTask.Alias }
        };

        return boAgent;
    }
    /// <summary>
    /// Returns all the agents/the agents who answer to a given condition 
    /// as a logic 'agents in list'
    /// </summary>
    /// <param name="filter">A boolien condition for an agent</param>
    /// <returns>IEnumarable of logic agents in a list</returns>
    public IEnumerable<BO.AgentInList> ReadAll(Func<BO.AgentInList, bool>? filter = null)
    {
        if (filter == null)
        {
            return from DO.Agent doAgent in _dal.Agent.ReadAll()
                   let CurrentTsk = GetAllAgentTasks(doAgent.Id).FirstOrDefault(t => t.Status == TaskStatus.OnTrack)
                   select new BO.AgentInList
                   {
                       Id = doAgent.Id,
                       Name = doAgent.Name,
                       Specialty = (BO.AgentExperience?)doAgent.Specialty,
                       CurrentTask = new BO.TaskInAgent()
                       {
                           Id = CurrentTsk.Id,
                           Alias = CurrentTsk.Alias,
                       }
                   };
        }
        else//filter isn't null
            return from DO.Agent doAgent in _dal.Agent.ReadAll()//get all agents from dal
                   let CurrentTsk = GetAllAgentTasks(doAgent.Id).FirstOrDefault(t => t.Status == TaskStatus.OnTrack)
                   let boAgentInList = new BO.AgentInList()//convert them to 'AgentInList'
                   {
                       Id = doAgent.Id,
                       Name = doAgent.Name,
                       Specialty = (BO.AgentExperience?)doAgent.Specialty,
                       CurrentTask = new BO.TaskInAgent()
                       {
                           Id = CurrentTsk.Id,
                           Alias = CurrentTsk.Alias,
                       }
                   }
                   where filter(boAgentInList)//choose those who fulfill the condition
                   select boAgentInList;
    }
    /// <summary>
    /// Update an agent
    /// </summary>
    /// <param name="boAgent"></param>
    /// <exception cref="BlWrongInputException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Agent boAgent)
    {
        CheckValidation(boAgent);

        DO.Agent newDoAgent = new DO.Agent(boAgent.Id, boAgent.Email, boAgent.Cost, boAgent.Name, (DO.AgentExperience?)boAgent.Specialty);
        try
        {
            _dal.Agent.Update(newDoAgent);

            if (boAgent.CurrentTask is not null)
            {
                if (Bl.GetProjectStatus() != BO.ProjectStatus.ExecutionTime)
                    throw new BO.BlProjectStageException("Agent can't start a task on the current project stage");
               //update the task allocated to this agent
                DO.Task? taskToUpdate = _dal.Task.Read(boAgent.CurrentTask.Id);
                if (taskToUpdate is null)
                    throw new BO.BlDoesNotExistException($"Task with ID={boAgent.CurrentTask.Id} does Not exist,please try to allocate a different task");
                DO.Task newTask = taskToUpdate with { AgentId = boAgent.Id };
                _dal.Task.Update(newTask);

            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Agent with ID={boAgent.Id} deo's not exist", ex);
        }
    }

    /// <summary>
    /// Get the details of a task with the given id that belongs to the current agent
    /// </summary>
    /// <param name="agentId">The id of the agent</param>
    /// <param name="TaskId">The id of the requested task</param>
    /// <returns>A logic entity 'TaskInList of the wanted task</returns>
    /// <exception cref="BO.BlDoesNotExistException">A task with the given id does not exist</exception>
    /// <exception cref="BO.BlWrongAgentForTaskException">The task with given id doesn't belong to this agent</exception>
    public BO.TaskInList GetDetailedTaskForAgent(int agentId, int TaskId)
    {
        DO.Task? doTask = _dal.Task.Read(TaskId);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={TaskId} does Not exist");
        if (doTask.AgentId != agentId)
            throw new BO.BlWrongAgentForTaskException($"The Agent with the id= {agentId} does not have a task with id={TaskId}");
        return new BO.TaskInList
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Status = doTask.CalcStatus()
        };
    }
    /// <summary>
    /// Return all the tasks that were assigned to the agent with the given id
    /// </summary>
    /// <param name="agentId">Id of the agent</param>
    /// <returns>The tasks of the wanted agent as 'TaskInList' logic entities</returns>
    public IEnumerable<BO.TaskInList> GetAllAgentTasks(int agentId)
    {
        return from DO.Task doTask in _dal.Task.ReadAll()
               where doTask.AgentId == agentId
               orderby doTask.Id descending   ///////////
               select new BO.TaskInList
               {
                   Id = doTask.Id,
                   Alias = doTask.Alias,
                   Description = doTask.Description,
                   Status = doTask.CalcStatus()
               };
    }

    /// <summary>
    /// Check validation for the fields id,name,cost and email
    /// </summary>
    /// <param name="boAgent">A bl logic agent</param>
    /// <exception cref="BO.BlWrongInputException">Wrong value</exception>
    private void CheckValidation(BO.Agent boAgent)
    {
        if (boAgent!.Id <= 0)
            throw new BO.BlWrongInputException("Id can't be negative");
        if (string.IsNullOrEmpty(boAgent.Name))
            throw new BO.BlWrongInputException("Agent's name must have a value");
        if (boAgent.Cost < 0)
            throw new BO.BlWrongInputException("Agent's cost can't be negative");
        if (boAgent.Email!.Contains("@gmail.com") == false)
            throw new BO.BlWrongInputException("Worng email format");
    }
    
}


