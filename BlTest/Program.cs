namespace BlTest;
using BO;

/// <summary>
/// Test for Bl
/// </summary>
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        try
        {
            do
            {
                Console.WriteLine("Choose one of the following:");
                Console.WriteLine("press 1 for an agent");
                Console.WriteLine("prees 2 for a task");
                Console.WriteLine("press 3 for more options");
                Console.WriteLine("press 0 to exit");

                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        CaseAgent();
                        break;
                    case 2:
                        CaseTask();
                        break;
                    case 3:
                        GenralOperations();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
            while (true);
        }
        catch (Exception e)//Catch all exeptions and print them
        {
            Console.WriteLine(e);
        }
    }
    static void GenralOperations()
    {
        try
        {
            do
            {
                Console.WriteLine("Choose one of the following:");
                Console.WriteLine("press 1 to reset the data base:");
                Console.WriteLine("prees 2 to initialize the data base:");
                Console.WriteLine("press 3 to get project status");
                Console.WriteLine("press 4 to enter project start date:");
                Console.WriteLine("press 0 to return to the main menu");

                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        Console.Write("Would you like to reset the project data? (Yes/No)");
                        string? answer1 = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        if (answer1 == "Yes")
                            s_bl.ResetData();
                        break;
                    case 2:
                        Console.Write("Would you like to create Initial data? (Yes/No)");
                        string? answer2 = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        if (answer2 == "Yes")
                            s_bl.InitializeData();
                        break;
                    case 3:
                        Console.WriteLine(s_bl.GetProjectStatus());
                        break;
                    case 4:
                        s_bl.SetProjectStartDate();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            while (true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    /// <summary>
    /// The function represents a sub menu for an agent 
    /// </summary>
    static void CaseAgent()
    {
        try
        {
            do
            {
                Console.WriteLine("Choose one of the following:");
                Console.WriteLine("press 1 to add an agent");
                Console.WriteLine("prees 2 to read an agent");
                Console.WriteLine("press 3 to read all agents");
                Console.WriteLine("press 4 to update an agent");
                Console.WriteLine("press 5 to delete an agent");
                Console.WriteLine("press 6 to get detailed task for agent");
                Console.WriteLine("press 7 to get all agent tasks");
                Console.WriteLine("press 8 to get all available tasks for an agent");
                Console.WriteLine("press 0 to return to the main menu");

                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        CreateA();
                        break;
                    case 2:
                        ReadA();
                        break;
                    case 3:
                        ReadAllA();
                        break;
                    case 4:
                        UpdateA();
                        break;
                    case 5:
                        DeleteA();
                        break;
                    case 6:
                        GetDetailedTaskForAgentA();
                        break;
                    case 7:
                        GetAllAgentTasksA();
                        break;
                    case 8:
                        GetAllAvailableTasks();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            while (true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    /// <summary>
    /// create an agent according to the user input
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void CreateA()
    {
        Console.WriteLine("Enter agent id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter agent email:");
        string _email = Console.ReadLine()!;
        Console.WriteLine("Enter agent cost:");
        if (!int.TryParse(Console.ReadLine(), out int _cost))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter agent name:");
        string _name = Console.ReadLine()!;
        Console.WriteLine("Enter agent specialty(1- for field agent,2- for hacker, 3- for invetgator:");
        if (!int.TryParse(Console.ReadLine(), out int _specialty))
            throw new FormatException("Wrong input");
        BO.Agent newA = new BO.Agent()
        {
            Id = _id,
            Email = _email,
            Cost = _cost,
            Name = _name,
            Specialty = (BO.AgentExperience?)_specialty,
            CurrentTask = null
        };
        Console.WriteLine(s_bl!.Agent.Create(newA));
    }
    /// <summary>
    /// Read the agent with the given id
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void ReadA()
    {
        Console.WriteLine("Enter agent id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine(s_bl!.Agent.Read(_id));
    }

    /// <summary>
    /// Get all the agents that exist and print them
    /// </summary>
    static void ReadAllA()
    {
        Console.WriteLine("All agents:");
        IEnumerable<BO.AgentInList> _listA;
        _listA = s_bl!.Agent.ReadAll(/*a => a.Specialty == BO.AgentExperience.Field_agent*/);
        foreach (BO.AgentInList a in _listA)//print all the agents in the list
            Console.WriteLine(a);
    }
    /// <summary>
    /// Enter id of an agent and update his details according to user input.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void UpdateA()
    {
        Console.WriteLine("Enter id of the agent you want to update:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter new Name:");
        string _name = Console.ReadLine()!;
        Console.WriteLine("Enter new email:");
        string _email = Console.ReadLine()!;
        Console.WriteLine("Enter new cost:");
        if (!int.TryParse(Console.ReadLine(), out int _cost))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter agent specialty(1- for field agent,2- for hacker, 3- for invetgator:  new level can't be lower than old level");
        if (!int.TryParse(Console.ReadLine(), out int _specialty))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter current task id:");
        if (!int.TryParse(Console.ReadLine(), out int _taskId))
            throw new FormatException("Wrong input");
        BO.Agent newA = new BO.Agent()
        {
            Id = _id,
            Email = _email,
            Cost = _cost,
            Name = _name,
            Specialty = (BO.AgentExperience?)_specialty,
            CurrentTask = new BO.TaskInAgent { Id = _taskId, Alias = null }
        };
        s_bl!.Agent.Update(newA);
    }
    /// <summary>
    /// Delete the agent with the given id 
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void DeleteA()
    {
        Console.WriteLine("Enter agent id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        s_bl!.Agent.Delete(_id);
    }
    static void GetDetailedTaskForAgentA()
    {
        Console.WriteLine("Enter agent id and task id:");
        if (!int.TryParse(Console.ReadLine(), out int _idTask))
            throw new FormatException("Wrong input");
        if (!int.TryParse(Console.ReadLine(), out int _idAgent))
            throw new FormatException("Wrong input");
        BO.TaskInList tmp = s_bl!.Agent.GetDetailedTaskForAgent(_idTask, _idAgent);
        Console.WriteLine(tmp);
    }
    static void GetAllAgentTasksA()
    {
        Console.WriteLine("Enter agent id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        IEnumerable<BO.TaskInList> _list = s_bl!.Agent.GetAllAgentTasks(_id);
        foreach (BO.TaskInList a in _list)
            Console.WriteLine(a);
    }
    static void GetAllAvailableTasks()
    {
        Console.WriteLine("Enter agent id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        IEnumerable<BO.TaskInList> _list = s_bl!.Agent.AvailableTasks(_id);
        foreach (BO.TaskInList a in _list)
            Console.WriteLine(a);
    }

    static void CaseTask()
    {
        try
        {
            do
            {
                Console.WriteLine("Choose one of the following:");
                Console.WriteLine("press 1 to add a Task");
                Console.WriteLine("prees 2 to read a Task");
                Console.WriteLine("press 3 to read all Tasks");
                Console.WriteLine("press 4 to update a Task");
                Console.WriteLine("press 5 to delete a Task");
                Console.WriteLine("press 6 to update scheduled start date");
                Console.WriteLine("press 7 to get previous tasks a task depends on");
                Console.WriteLine("press 8 to initialize automatically statrts date for all the tasks");
                Console.WriteLine("press 0 to return to the main menu");

                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        CreateT();
                        break;
                    case 2:
                        ReadT();
                        break;
                    case 3:
                        ReadAllT();
                        break;
                    case 4:
                        UpdateT();
                        break;
                    case 5:
                        DeleteT();
                        break;
                    case 6:
                        UpdateScheduledStartDateT();
                        break;
                    case 7:
                        GetDependenciesListT();
                        break;
                    case 8:
                        s_bl!.Task.CreateSchedule();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
            while (true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// create a task according to the user input
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void CreateT()
    {
        Console.WriteLine("Enter task alias:");
        string _alias = Console.ReadLine()!;
        Console.WriteLine("Enter task description:");
        string _description = Console.ReadLine()!;
        DateTime _createdAtDate = DateTime.Now;
        Console.WriteLine("Enter task required effort time:");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _requiredEffortTime))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task deliverables:");
        string _deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter task remarks:");
        string _remarks = Console.ReadLine()!;
        Console.WriteLine("Enter task complexity(1- for field agent,2- for hacker, 3- for invetgator):");
        if (!BO.AgentExperience.TryParse(Console.ReadLine(), out BO.AgentExperience _complexity))
            throw new FormatException("Wrong input");
        BO.Task newTask = new BO.Task()
        {
            Alias = _alias,
            Description = _description,
            Status = BO.TaskStatus.Unscheduled,
            CreatedAtDate = _createdAtDate,
            ScheduledDate = null,
            StartDate = null,
            RequiredEffortTime = _requiredEffortTime,
            EstimatedCompleteDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = _deliverables,
            Remarks = _remarks,
            TaskAgent = null,
            Complexity = (BO.AgentExperience?)_complexity,
        };
        Console.WriteLine("Enter number of tasks this task depends on:");
        if (!int.TryParse(Console.ReadLine(), out int num))
            throw new FormatException("Wrong input");
        newTask.DependenciesList = new List<BO.TaskInList>();
        for (int i = 0; i < num; i++)
        {
            Console.WriteLine("Enter id of the task:");
            if (!int.TryParse(Console.ReadLine(), out int _id))
                throw new FormatException("Wrong input");
            BO.TaskInList? _dependencyTask = new BO.TaskInList { Id = _id, Alias = null, Description = null, Status = null };
            newTask.DependenciesList!.Add(_dependencyTask);
        }
        Console.WriteLine(s_bl!.Task.Create(newTask));
    }
    /// <summary>
    /// Read the task with the given id
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void ReadT()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        BO.Task? tmp = s_bl!.Task.Read(_id);
        if (tmp != null)
            Console.WriteLine(tmp);
        else
            Console.WriteLine($"Task with id={_id} doe's not exist");
    }
    /// <summary>
    /// Get all the tasks that exist and print them
    /// </summary>
    static void ReadAllT()
    {
        Console.WriteLine("All the tasks:");
        IEnumerable<BO.TaskInList?> _listT;
        _listT = s_bl!.Task.ReadAll();
        foreach (BO.TaskInList? t in _listT)// print all the tasks in the list
            Console.WriteLine(t);
    }
    /// <summary>
    /// Enter id of a task and update it according to the user input.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void UpdateT()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        BO.Task? oldTask = s_bl!.Task.Read(_id);

        Console.WriteLine("Enter new task alias:");
        string _alias = Console.ReadLine()!;
        Console.WriteLine("Enter new task description:");
        string _description = Console.ReadLine()!;
        //if( Bl.GetProjectStatus())
        //{
        //}
        Console.WriteLine("Enter new  task schedualed date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _schedualedDate))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task start date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task required effort time:");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _requiredEffortTime))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task estimated complete date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _estimatedCompleteDate))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task deadline:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _deadlineDate))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task complete date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _completeDate))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task deliverables:");
        string _deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter task remarks:");
        string _remarks = Console.ReadLine()!;
        int _agentid = 0;
        string? _agentName = null;
        if (s_bl.GetProjectStatus() == ProjectStatus.ExecutionTime)
        {
            Console.WriteLine("Enter agent id:");
            if (!int.TryParse(Console.ReadLine(), out int _AgentId))
                throw new FormatException("Wrong input");
            Console.WriteLine("Enter agent name:");
            string _AgentName = Console.ReadLine()!;
            _agentid = _AgentId;
            _agentName = _AgentName;
        }
        Console.WriteLine("Enter task complexity(1- for field agent,2- for hacker, 3- for invetgator):");
        if (!BO.AgentExperience.TryParse(Console.ReadLine(), out BO.AgentExperience _complexity))
            throw new FormatException("Wrong input");
        BO.Task newTask = new BO.Task()
        {
            Id = _id,
            Alias = _alias,
            Description = _description,
            DependenciesList = oldTask.DependenciesList,
            Status = oldTask.Status,
            CreatedAtDate = oldTask.CreatedAtDate,
            ScheduledDate = _schedualedDate,
            StartDate = _startDate,
            RequiredEffortTime = _requiredEffortTime,
            EstimatedCompleteDate = _estimatedCompleteDate,
            DeadlineDate = _deadlineDate,
            CompleteDate = _completeDate,
            Deliverables = _deliverables,
            Remarks = _remarks,
            TaskAgent = _agentName is null ? null : new BO.AgentInTask { Id = _agentid, Name = _agentName },
            Complexity = (BO.AgentExperience?)_complexity,
        };
        s_bl!.Task.Update(newTask);
    }
    /// <summary>
    /// Delete the task with the given id
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void DeleteT()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        s_bl!.Task.Delete(_id);
    }
    static void UpdateScheduledStartDateT()
    {
        Console.WriteLine("Enter task id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine("Enter task start date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
            throw new FormatException("Wrong input");
        s_bl!.Task.UpdateScheduledStartDate(_id, _startDate);
    }

    static void GetDependenciesListT()
    {
        Console.WriteLine("Enter task id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        IEnumerable<BO.TaskInList?> _listT;
        _listT = s_bl!.Task.GetDependenciesList(_id);
        foreach (BO.TaskInList? t in _listT)// print all the tasks in the list
            Console.WriteLine(t);
    }
}