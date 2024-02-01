using System.Security.Cryptography;

namespace BlTest;
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Yes/No)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Yes")
            DalTest.Initialization.Do();
        try
        {
            do
            {
                Console.WriteLine("Choose one of the following:");
                Console.WriteLine("press 1 for an agent");
                Console.WriteLine("prees 2 for a task");
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

    /// <summary>
    /// The function represents a sub menu of an agent's 'SCRUB' functions
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
                        //UpdateA();
                        break;
                    case 5:
                        DeleteA();
                        break;
                    case 6:
                        //GetDetailedTaskForAgentA();
                        break;
                    case 7:
                        //GetAllAgentTasksA();
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
    /// create an agent according to the user input and print the agent(ToString)
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
        Console.WriteLine("Enter agent id:");
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
        //BO.Agent newA = new BO.Agent(_id, _email, _cost, _name, (BO.AgentExperience)_specialty,_currentTask);
        Console.WriteLine(s_bl!.Agent.Create(newA));
    }
    /// <summary>
    /// Read the agent with the given id and print the agent(ToString)
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
    /// Create a list with all the agents that exist
    /// </summary>
    static void ReadAllA()
    {
        Console.WriteLine("All the agents:");
        IEnumerable<BO.AgentInList> _listA;
        _listA = s_bl!.Agent.ReadAll();
        foreach (BO.AgentInList a in _listA)//print all the agents in the list
            Console.WriteLine(a);
    }
    /// <summary>
    /// Enter id of an agent, print this agent and then update it.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    //static void UpdateA()
    //{
    //    Console.WriteLine("Enter id:");
    //    if (!int.TryParse(Console.ReadLine(), out int _id))
    //        throw new FormatException("Wrong input");

    //    Console.WriteLine(s_bl!.Agent.Read(_id));//If the agent with this id exists print the agent,else don't print anything
    //    Console.WriteLine("Enter:name,email,cost, and specialty(1- for field agent,2- for hacker, 3- for invetgator):");//Enter new data for this agent
    //    string _name = Console.ReadLine()!;
    //    string _email = Console.ReadLine()!;
    //    if (!int.TryParse(Console.ReadLine(), out int _cost))
    //        throw new FormatException("Wrong input");
    //    if (!int.TryParse(Console.ReadLine(), out int _specialty))
    //        throw new FormatException("Wrong input");

    //    BO.Agent newA = new BO.Agent(_id, _email, _cost, _name, (BO.AgentExperience)_specialty);
    //    s_bl.Agent.Update(newA); //zili
    //}
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
        foreach(BO.TaskInList a in _list)
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
                        //UpdateT();
                        break;
                    case 5:
                        DeleteT();
                        break;
                    case 6:
                        UpdateScheduledStartDateT();
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
    /// create a task according to the user input and print it
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void CreateT()
    {
        Console.WriteLine("Enter task alias:");
        string _alias = Console.ReadLine()!;
        Console.WriteLine("Enter task description:");
        string _description = Console.ReadLine()!;
        Console.WriteLine("Enter task's list of dependencies:");
        // List<BO.TaskInList>? DependenciesList            //צילי//
        //Console.Write("Task create at date:");
        DateTime _createdAtDate = DateTime.Now;
        //if (!DateTime.TryParse(Console.ReadLine(), out DateTime _schedualedDate))
        //    throw new FormatException("Wrong input");
        //if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
        //    throw new FormatException("Wrong input");
        Console.WriteLine("Enter task required effort time:");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _requiredEffortTime))
            throw new FormatException("Wrong input");
        //if (!DateTime.TryParse(Console.ReadLine(), out DateTime _estimatedCompleteDate))
        //    throw new FormatException("Wrong input");
        //if (!DateTime.TryParse(Console.ReadLine(), out DateTime _deadlineDate))
        //    throw new FormatException("Wrong input");
        //if (!DateTime.TryParse(Console.ReadLine(), out DateTime _completeDate))
        //    throw new FormatException("Wrong input");
        Console.WriteLine("Enter task deliverables:");
        string _deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter task remarks:");
        string _remarks = Console.ReadLine()!;
        Console.WriteLine("Enter task complexity(1- for field agent,2- for hacker, 3- for invetgator):");
        if (!BO.AgentExperience.TryParse(Console.ReadLine(), out BO.AgentExperience _complexity))
            throw new FormatException("Wrong input");
        BO.Task newTask = new BO.Task()
        {
            Alias = _alias!,
            Description = _description!,
            Status = BO.TaskStatus.Unscheduled,
            CreatedAtDate = _createdAtDate,
            SchedualedDate = null,
            StartDate = null,
            RequiredEffortTime = _requiredEffortTime,
            EstimatedCompleteDate = null,
            DeadlineDate = null,
            CompleteDate = null,
            Deliverables = _deliverables,
            Remarks = _remarks,
            TaskAgent = null,
            //= new AgentInTask{ Id = _agentId,Name = _agentName},
            Copmlexity = (BO.AgentExperience?)_complexity,
        };
        Console.WriteLine(s_bl!.Task.Create(newTask));
    }
    /// <summary>
    /// Read the task with the given id and print it
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
    /// Create a list with all the tasks that exist
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
    /// Enter id of a task, print this task and then update it.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    //static void UpdateT()
    //{
    //    Console.WriteLine("Enter id:");
    //    if (!int.TryParse(Console.ReadLine(), out int _id))
    //        throw new FormatException("Wrong input");
    //    Console.WriteLine(s_bl!.Task.Read(_id));// If the task with this id exists print it,else don't print anything
    //    Console.WriteLine(@"Enter:alias,description,complexity(1- for field agent,2- for hacker, 3- for invetgator),deliverables and remarks:");
    //    string _alias = Console.ReadLine()!;
    //    string _descripition = Console.ReadLine()!;
    //    if (!int.TryParse(Console.ReadLine(), out int _complexity))
    //        throw new FormatException("Wrong input");
    //    string _deliverables = Console.ReadLine()!;
    //    string _remarks = Console.ReadLine()!;
    //    DateTime _createAtDate = DateTime.Now;

    //    BO.Task newTask = new BO.Task(_id, _alias, _descripition, _createAtDate, null, false, (AgentExperience)_complexity, null, null, null, null, _deliverables, _remarks, null);
    //    s_bl!.Task.Update(newTask);//zili

    //}
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
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime _startDate))
            throw new FormatException("Wrong input");
        s_bl!.Task.UpdateScheduledStartDate(_id, _startDate);
    }
}