namespace DalTest;

using Dal;
//using System.Diagnostics;
using DalApi;
using DO;
using System.Xml.Linq;

internal class Program
{
    private static IAgent? s_dalAgent = new AgentImplementation();
    private static ITask? s_dalTask = new TaskImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();

    private static readonly Random s_rand = new();

    static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalAgent, s_dalTask, s_dalDependency);
            do
            {
                Console.WriteLine(@"Choose one of the following:
        press 1 for an agent
        prees 2 for a task
        press 3 for a dependency
        press 0 to exit");
                string tmp = Console.ReadLine(); ;
                int choise = int.Parse(tmp);
                switch (choise)
                {
                    case 1:
                        CaseAgent();
                        break;
                    case 2:
                        CaseTask();
                        break;
                    case 3:
                        CaseDepedency();
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
            while(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }

    void CaseAgent()
    {
        do
        {
            Console.WriteLine(@"Choose one of the following:
        press 1 to add an agent
        prees 2 to read an agent
        press 3 to read all agents
        press 4 to update an agent
        press 5 to delete an agent
        press 0 to return to the main menu
        ");
            string tmp = Console.ReadLine(); ;
            int choise = int.Parse(tmp);
            switch (choise)
            {
                case 1:
                    CreateA(s_dalAgent);
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
        while(true);


    }
    ////////////////////////////////////////////////////// Agent ///////////////////////////////////////////////////
    /// <summary>
    /// create an agent and call to create function
    /// </summary>
    void CreateA(IAgent s_dalA) 
    {
        Console.WriteLine("Enter:Id,email,cost, and specialty:");
        string d = Console.ReadLine();
        int _id = int.Parse(d);
        string _name = Console.ReadLine();
        string _email = Console.ReadLine();
        string c = Console.ReadLine();
        int _cost = int.Parse(c);
        string s = Console.ReadLine();
        DO.AgentExperience? _specialty = (DO.AgentExperience)int.Parse(s);
        Agent newA = new Agent(_id, _email, _cost, _name, _specialty);
        int? a = s_dalA.Create(newA);
        if(a!=null)
            Console.WriteLine(a);
    }
    void ReadA()
    {
        Console.WriteLine(@"Enter id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Agent? a = new Agent();
        a = s_dalAgent.Read(_id);
        if (a != null)
            Console.WriteLine(a);
    }
    void ReadAllA()
    {
        List<Agent> _listA;
        _listA = s_dalAgent.ReadAll();
        foreach (Agent a in _listA)
            Console.WriteLine(a);
    }
    void UpdateA()
    {
        Console.WriteLine(@"Enter id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Agent? a = new Agent();
        a = s_dalAgent.Read(_id);
        if (a != null)
        {
            Console.WriteLine(a);
            Console.WriteLine("Enter:email,cost, and specialty:");
            string _name = Console.ReadLine();
            string _email = Console.ReadLine();
            string c = Console.ReadLine();
            int _cost = int.Parse(c);
            string s = Console.ReadLine();
            DO.AgentExperience? _specialty = (DO.AgentExperience)int.Parse(s);
            Agent newA = new Agent(_id, _email, _cost, _name, _specialty);
            s_dalAgent.Update(newA);
        }
    }

    void DeleteA()
    {
        Console.WriteLine(@"Enter id:");
        string d = Console.ReadLine();
        int _id = int.Parse(d);
        s_dalAgent.Delete(_id);
    }
    ////////////////////////////// Task ////////////////////////////////////
    void CaseTask()
    {
        do
        {
            Console.WriteLine(@"Choose one of the following:
        press 1 to add a Task
        prees 2 to read a Task
        press 3 to read all Tasks
        press 4 to update a Task
        press 5 to delete a Task
        press 0 to return to the main menu
        ");
            string tmp = Console.ReadLine(); ;
            int choise = int.Parse(tmp);
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
   
    void CreateT()
    {
        Console.WriteLine(@"Enter:alias,description,complexity,deliverables and remarks:");
        string? _alias = Console.ReadLine();
        string? _descripition = Console.ReadLine();
        string? c = Console.ReadLine();
        DO.AgentExperience _complexity=(AgentExperience)int.Parse(c);
        string? _deliverables = Console.ReadLine();
        string? _remarks = Console.ReadLine();
        DateTime start = new DateTime(2024, 1, 1);
        int range = (DateTime.Today - start).Days;
        DateTime _createAtDate = start.AddDays(s_rand.Next(range));
        Task newTask=new Task(0,_alias,_descripition, _createAtDate ,null,false,_complexity,null,null,null,null,null,null);
        int? t= s_dalTask.Create(newTask);
        if (t != null)
            Console.WriteLine(t);
    }
    void ReadT()
    {
        Console.WriteLine(@"Enter id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Task? t = new Task();
        t = s_dalTask.Read(_id);
        if (t != null)
            Console.WriteLine(t);
    }
    void ReadAllT()
    {
        List<Task> _listT;
        _listT = s_dalTask.ReadAll();
        foreach (Task t in _listT)
            Console.WriteLine(t);
    }
    void UpdateT()
    {
        Console.WriteLine(@"Enter id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Task? t = new Task();
        t = s_dalTask.Read(_id);
        if (t != null)
        {
            Console.WriteLine(t);
            Console.WriteLine(@"Enter:alias,description,complexity,deliverables and remarks:");
            string? _alias = Console.ReadLine();
            string? _descripition = Console.ReadLine();
            string? c = Console.ReadLine();
            DO.AgentExperience _complexity = (AgentExperience)int.Parse(c);
            string? _deliverables = Console.ReadLine();
            string? _remarks = Console.ReadLine();
            DateTime start = new DateTime(2024, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _createAtDate = start.AddDays(s_rand.Next(range));
            Task newTask = new Task(0, _alias, _descripition, _createAtDate, null, false, _complexity, null, null, null, null, null, null);
            s_dalTask.Update(newTask);
        }
    }

    void DeleteT()
    {
        Console.WriteLine(@"Enter id:");
        string d = Console.ReadLine();
        int _id = int.Parse(d);
        s_dalTask.Delete(_id);
    }
    /////////////////////////////////////// Dependency////////////////////////////////////
    /// <summary>
    ///
    /// </summary>
    void CaseDependency()
    {
        do
        {
        Console.WriteLine(@"Choose one of the following:
        press 1 to add a dependency
        prees 2 to read a dependency
        press 3 to read all dependencies
        press 4 to update a dependency
        press 5 to delete a dependency
        press 0 to return to the main menu
        ");
            string tmp = Console.ReadLine(); ;
            int choise = int.Parse(tmp);
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
        while(true);
        

    }
    void CreateD()
    {
        Console.WriteLine(@"Enter:depentTask and depedsOnTask:");
        string? d1 = Console.ReadLine();
        int _DependentTask = int.Parse(d1);
        string? d2 = Console.ReadLine();
        int _DependsOnTask = int.Parse(d2);
        Dependency dep = new Dependency(_DependentTask, _DependsOnTask);
        int? d= s_dalDependency.Create(dep);
        if (d != null)
            Console.WriteLine(d);
    }
    void ReadD()
    {
        Console.WriteLine("Enter: id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Dependency? dep = new Dependency();
        dep = s_dalDependency.Read(_id);
        if (dep != null)
            Console.WriteLine(dep);
    }
    void ReadAllD()
    {
        List<Dependency> _listD;
        _listD = s_dalDependency.ReadAll();
        foreach (Dependency dep in _listD)
            Console.WriteLine(dep);
    }
    void UpdateD()
    {
        Console.WriteLine("Enter: id:");
        string? d = Console.ReadLine();
        int _id = int.Parse(d);
        Dependency? dep = new Dependency();
        dep = s_dalDependency.Read(_id);
        if (dep != null)
        {
            Console.WriteLine(dep);
            Console.WriteLine(@"Enter:depentTask and depedsOnTask:");
            string? d1 = Console.ReadLine();
            int _DependentTask = int.Parse(d1);
            string? d2 = Console.ReadLine();
            int _DependsOnTask = int.Parse(d2);
            Dependency newdep = new Dependency(_DependentTask, _DependsOnTask);
            s_dalDependency.Update(newdep);
        }
    }

    void DeleteD()
    {
        Console.WriteLine("Enter: id:");
        string d = Console.ReadLine();
        int _id = int.Parse(d);
        s_dalDependency.Delete(_id);
    }
}





