namespace DalTest;
using DalApi;
using DO;
using System;
using Dal;

/// <summary>
/// Main program
/// the program allows to create/delete/update/read the 3 entities:
/// agent,task and dependency
/// </summary>
internal class Program
{
    private static readonly Random s_rand = new();
    //static readonly IDal s_dal = new DalList(); //stage 2
    static readonly IDal s_dal = new DalXml();

    /// <summary>
    /// The main function presents a main menu with the options:
    /// agent,task,dependency and exit, and sends the user to the matching sub menu.
    /// </summary>

    static void Main(string[] args)
    {
        try
        {
           // Initialization.Do(s_dal);//First initialization of the database
            do
            {
                Console.WriteLine(@"
Choose one of the following:
press 1 for an agent
prees 2 for a task
press 3 for a dependency
press 4 in order to initialize the data base 
press 0 to exit");
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
                        CaseDependency();
                        break;
                     case 4:
                        Console.Write("Would you like to create Initial data? (Yes/No)");
                        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                        if (ans == "Yes")
                        {
                            EaraseData();//Delete the old data base
                            Initialization.Do(s_dal);
                        }
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
    /// Reset all the files
    /// </summary>
    static void EaraseData()
    {
        s_dal!.Agent.Clear();
        s_dal!.Task.Clear();
        s_dal!.Dependency.Clear();
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
                Console.WriteLine(@"
Choose one of the following:
press 1 to add an agent
prees 2 to read an agent
press 3 to read all agents
press 4 to update an agent
press 5 to delete an agent
press 0 to return to the main menu
        ");
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
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            while (true);
        }
        catch(Exception e)
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
        Console.WriteLine("Enter:Id,email,cost per hour,name and specialty(1- for field agent,2- for hacker, 3- for invetgator):");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        string _email = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _cost))
            throw new FormatException("Wrong input");
        string _name = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _specialty))
            throw new FormatException("Wrong input");

        Agent newA = new Agent(_id, _email, _cost, _name, (AgentExperience)_specialty);
        Console.WriteLine(s_dal!.Agent.Create(newA));

    }
    /// <summary>
    /// Read the agent with the given id and print the agent(ToString)
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>

    static void ReadA()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Agent? tmp = s_dal!.Agent.Read(_id);
        if (tmp != null)
            Console.WriteLine(tmp);
        else
            Console.WriteLine($"Agents with id={_id} doe's not exist");
    }
    /// <summary>
    /// Create a list with all the agents that exist
    /// </summary>
    static void ReadAllA()
    {
        IEnumerable<Agent?> _listA;
        _listA = s_dal!.Agent.ReadAll();
        foreach (Agent? a in _listA)//print all the agents in the list
            Console.WriteLine(a);
    }

    /// <summary>
    /// Enter id of an agent, print this agent and then update it.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void UpdateA()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");

        Console.WriteLine(s_dal!.Agent.Read(_id));//If the agent with this id exists print the agent,else don't print anything
        Console.WriteLine("Enter:name,email,cost, and specialty(1- for field agent,2- for hacker, 3- for invetgator):");//Enter new data for this agent
        string _name = Console.ReadLine()!;
        string _email = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _cost))
            throw new FormatException("Wrong input");
        if (!int.TryParse(Console.ReadLine(), out int _specialty))
            throw new FormatException("Wrong input");

        Agent newA = new Agent(_id, _email, _cost, _name, (AgentExperience)_specialty);
        s_dal.Agent.Update(newA); //zili
    }
    /// <summary>
    /// Delete the agent with the given id 
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void DeleteA()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        s_dal!.Agent.Delete(_id);
    }
    ////////////////////////////// Task ////////////////////////////////////
    ////// <summary>
    /// The function represents a sub menu of an agent's 'SCRUB' functions
    /// </summary>
    static void CaseTask()
    {
        try
        {
            do
            {
                Console.WriteLine(@"
Choose one of the following:
press 1 to add a Task
prees 2 to read a Task
press 3 to read all Tasks
press 4 to update a Task
press 5 to delete a Task
press 0 to return to the main menu
        ");
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
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
            while (true);
        }
        catch(Exception e)
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
        Console.WriteLine(@"Enter:alias,description,complexity(1- for field agent,2- for hacker, 3- for invetgator),deliverables and remarks:");

        string _alias = Console.ReadLine()!;
        string _descripition = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _complexity))
            throw new FormatException("Wrong input");
        string _deliverables = Console.ReadLine()!;
        string _remarks = Console.ReadLine()!;
        DateTime _createAtDate = DateTime.Now;
        Task newTask = new Task(0, _alias, _descripition, _createAtDate, null, false, (AgentExperience)_complexity, null, null, null, null, null, null);
        Console.WriteLine(s_dal!.Task.Create(newTask));
    }
    /// <summary>
    /// Read the task with the given id and print it
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void ReadT()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Task? tmp = s_dal!.Task.Read(_id);
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
        IEnumerable<Task?> _listT;
        _listT = s_dal!.Task.ReadAll();
        foreach (Task? t in _listT)// print all the tasks in the list
            Console.WriteLine(t);
    }
    /// <summary>
    /// Enter id of a task, print this task and then update it.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void UpdateT()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine(s_dal!.Task.Read(_id));// If the task with this id exists print it,else don't print anything
        Console.WriteLine(@"Enter:alias,description,complexity(1- for field agent,2- for hacker, 3- for invetgator),deliverables and remarks:");
        string _alias = Console.ReadLine()!;
        string _descripition = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _complexity))
            throw new FormatException("Wrong input");
        string _deliverables = Console.ReadLine()!;
        string _remarks = Console.ReadLine()!;
        DateTime _createAtDate = DateTime.Now;

        Task newTask = new Task(_id, _alias, _descripition, _createAtDate, null, false, (AgentExperience)_complexity, null, null, null, null, _deliverables, _remarks, null);
        s_dal!.Task.Update(newTask);//zili

    }
    /// <summary>
    /// Delete the task with the given id
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void DeleteT()
    {
        Console.WriteLine(@"Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        s_dal!.Task.Delete(_id);
    }
    /////////////////////////////////////// Dependency ////////////////////////////////////
    ////// <summary>
    /// The function represents a sub menu of a dependency 'SCRUB' functions
    /// </summary>
    static void CaseDependency()
    {
        try
        {
            do
            {
                Console.WriteLine(@"
Choose one of the following:
press 1 to add a dependency
prees 2 to read a dependency
press 3 to read all dependencies
press 4 to update a dependency
press 5 to delete a dependency
press 0 to return to the main menu
        ");
                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        CreateD();
                        break;
                    case 2:
                        ReadD();
                        break;
                    case 3:
                        ReadAllD();
                        break;
                    case 4:
                        UpdateD();
                        break;
                    case 5:
                        DeleteD();
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
    /// create a dependency according to the user input and print it
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void CreateD()
    {
        Console.WriteLine(@"Enter:dependentTask id and depedsOnTask id:");
        if (!int.TryParse(Console.ReadLine(), out int _DependentTask))
            throw new FormatException("Wrong input");
        if (!int.TryParse(Console.ReadLine(), out int _DependsOnTask))
            throw new FormatException("Wrong input");
        Dependency dep = new Dependency(0, _DependentTask, _DependsOnTask);
        Console.WriteLine(s_dal!.Dependency.Create(dep));
    }
    /// <summary>
    /// Read the dependency with the given id and print it
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void ReadD()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Dependency? tmp = s_dal!.Dependency.Read(_id);
        if (tmp != null)
            Console.WriteLine(tmp);
        else
            Console.WriteLine($"Agents with id={_id} doe's not exist");
    }
    /// <summary>
    /// Create a list with all the dependencies that exist
    /// </summary>
    static void ReadAllD()
    {
        IEnumerable<Dependency?> _listD;
        _listD = s_dal!.Dependency.ReadAll();
        foreach (Dependency? dep in _listD)// print all the dependencies in the list
            Console.WriteLine(dep);
    }
    /// <summary>
    /// Enter id of a dependency, print this dependency and then update it.
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void UpdateD()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        Console.WriteLine(s_dal!.Task.Read(_id));// If the task with this id exists print it,else don't print anything

        Console.WriteLine(@"Enter:dependentTask id and depedsOnTask id:");
        if (!int.TryParse(Console.ReadLine(), out int _DependentTask))
            throw new FormatException("Wrong input");
        if (!int.TryParse(Console.ReadLine(), out int _DependsOnTask))
            throw new FormatException("Wrong input");
        Dependency dep = new Dependency(_id, _DependentTask, _DependsOnTask);
        s_dal!.Dependency.Update(dep);
    }
    /// <summary>
    /// Delete the dependency with the given id
    /// </summary>
    /// <exception cref="FormatException">wrong input</exception>
    static void DeleteD()
    {
        Console.WriteLine("Enter id:");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        s_dal!.Dependency.Delete(_id);
    }
}





