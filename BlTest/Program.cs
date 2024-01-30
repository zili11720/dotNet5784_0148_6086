using DalApi;

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
        Console.WriteLine("Enter:Id,email,cost per hour,name and specialty(1- for field agent,2- for hacker, 3- for invetgator):");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong input");
        string _email = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _cost))
            throw new FormatException("Wrong input");
        string _name = Console.ReadLine()!;
        if (!int.TryParse(Console.ReadLine(), out int _specialty))
            throw new FormatException("Wrong input");
        BO.Agent newA = new BO.Agent()
        {
            Id = _id,
            Email = _email,
            Cost = _cost,
            Name = _name,
            Specialty = (BO.AgentExperience?)_specialty,
            //CurrentTask = new CurrentTask
            //{
            //   
            //}
            // CurrentTask = (doTask == null) ? null : new BO.TaskInAgent() { Id = _id, Alias = _alias }
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
                        //CreateT();
                        break;
                    case 2:
                        //ReadT();
                        break;
                    case 3:
                        //ReadAllT();
                        break;
                    case 4:
                        //UpdateT();
                        break;
                    case 5:
                        // DeleteT();
                        break;
                    case 6:
                        //UpdateScheduledStartDateT();
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
}