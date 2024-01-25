namespace BlTest;
using BlApi;
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
                Console.WriteLine(@"
Choose one of the following:
press 1 for an agent
prees 2 for a task
press 3 for a task in list
press 4 for an agent in list
press 0 to exit");
                if (!int.TryParse(Console.ReadLine(), out int choise))
                    throw new FormatException("Wrong input");
                switch (choise)
                {
                    case 1:
                        //CaseAgent();
                        break;
                    case 2:
                        //CaseTask();
                        break;
                    case 3:
                        //CaseTaskInList();
                        break;
                    case 4:
                        //CaseAgentInList();
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

}