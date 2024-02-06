namespace DalTest;
using DalApi;
using DO;
using System;

/// <summary>
/// The class initialization initializes the database with 5 agents, 
/// 28 tasks and 40 dependencies between the tasks
/// </summary>
public static class Initialization
{

    //a variable to access to the inplementations
    private static IDal? s_dal;//Stage 2

    //A variable to create rendom numbers
    private static readonly Random s_rand = new();
    private const int MIN_ID = 200000000; //zili
    private const int MAX_ID = 400000000; //zili

    static DateTime currentDate = DateTime.Now;
    //A variable which represents the start date of the project
    static DateTime startProjectDate = currentDate.AddMonths(1);

    //// <summary>
    /// The method createAgent creates 5 agents. 
    /// </summary>
    private static void createAgent()
    {
        string[] agentsNames =
        {
        "Eli Choen","Alex Cooper", "James Bond", "Jone Alon","Cris Klark"
        };
        string[] agentsEmail =
        {
        "EliChoen@gmai.com","AlexCooper@gmai.com", "JamesBond@gmai.com", "JoneAlon@gmai.com","CrisKlark@gmai.com"
        };
        int[] agentsSpecialities =
        {
           1,1,1,2,3//The numbers will be converted to type enum-AgentExperience
        };
        for (int i = 0; i < 5; i++)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);//Random id
            while (s_dal!.Agent.Read(_id) != null);//make sure each id is unique

            int _cost = s_rand.Next(300, 500);//random cost per hour

            Agent newAgent = new(_id, agentsEmail[i], _cost, agentsNames[i], (DO.AgentExperience)agentsSpecialities[i]);
            s_dal!.Agent.Create(newAgent);
        }
    }
    /// <summary>
    ///The method creates 28 different tasks that make up a larger goal:catching a criminal 
    /// </summary>
    private static void createTask()
    {
        string[] _aliasses =
        {
            "Finger ptints",   //1
            "Security cameras photage",//2
            "Confirm identity",//3
            "Bank activity",//4
            "Locate house",//5
            "Locate car", //6
            "Citation device",//7
            "Security Cameras control", //8
            "Find witnesses", //9
            "witness1",//10
            "witness2",//11
            "witness3",//12
            "Past report",//13
            "Compare testimonies",//14
            "Witnesses report",//15
            "House watch",//16
            "House sketch",//17
            "Daily following",//18
            "daily schedule report",//19
            "Chip implantation",//20
            "Staff meeting",//21
            "Equipment inspection",//22
            "Complete equipment deficiencies",//23
            "Ranges",//24
            "Build simulation",//25
            "Practice simulatin",//26
            "Quote to implanted devices",//27
            "Create final raid plan"//28
        };
        string[] _descriptions =
        {
            "Check for finger prints in the crime scene in order to to obtain incriminating evidence",
            "Go over security cameras phootages in order to identify the criminal's face",
            "Look for the criminal's identity in government databases according to the fingerprints and photoges found",
            "Search previous bank activities of the criminal",
            "Locate the criminal's house",
            "Locate the criminal's car",
            "Plant a citation device in the criminal's car",
            "Hack security Cameras in the criminal's house",
            "Find optional witnesses in order to get more evidences",
            "Investigate first witness",
            "Investigate second witness",
            "Investigate third witness",
            "Make a detailed report about the criminal's past",
            "Compare the testimonies of the witnesses in order to check reliability",
            "Make a report on the witnesses testimonies and their conclusions",
            "Watch the criminal's house and check for suspicious activities",
            "Make a live model of the criminal's house in order to prepare for the raid",
            "Follow the criminal around for a whole day in order to understand his daily routine",
            "Make a report about the daily schedule of the criminal",
            "Implant a chip in the criminal's phone",
            "Have a staff meeting about your finding and plan the final act",
            "Check the functionality of the equipment needed to the final mission",
            "Buy/fix equipment as needed",
            "Practice in Ranges",
            "Build a simulation of the final mission",
            "Practice the simulatin",
            "Quote to implanted devices in the criminal's care and personal phone",
            "Decide on the final raid details according to the results of the simulation",
        };
        string[] _deliverables =
        {
            "Confirmed finger ptints",
            "Photage of the criminal's face",
            "Confirmed identity of the criminal",
            "Bank activities details",
            "House location",
            "Car license plate number",
            "Implanted citation device in car",
            "Security Cameras control",
            "Suitable witnesses",
            "Testimony of a first witness",
            "Testimony of a second witness",
            "Testimony of a third witness",
            "Full report on the criminal's past",
            "Conclusions from the testimonies",
            "Full documanted report on the testimonies",
            "Information about the conduct of the criminal's home",
            "Full sketch of the criminal's house",
            "Accurate information on the criminal's daily schedule",
            "Full documanted report on the criminal's daily schedule",
            "implanted chip in the criminal's phone",
            "An updated staff with a plan for the next move",
            "A list of needed equipment/repair",
            "Full functioning equipment",
            "Reaching Maximum shooting skill",
            "Simulation of the day of the raid",
            "Good performances on the simulation",
            "Information about phone calls and secret meetings",
            "A final raid plan"
        };
        string[] _remarks =
        {
            "Check for finger prints of possible  partners in crime",
            "Check for photages of possible  partners in crime",
             "No remarks",
            "Keep track on current activities",
            "Pay attention if there are multiple adresses",
            "No remarks",
            "No remarks",
            "Hack to the street security cameras to surround the whole area",
            "No remarks",
            "Check witnesses record before investigation ",
            "Check witnesses record before investigation",
            "Check witnesses record before investigation",
            "Include jobs, family situation and acquaintances",
            "take into account real photages from the crime ciene",
            "No remarks",
            "Suspiciouse activities may be leaving the house at strange hours, unknown visiters etc.",
            "Pay attention to multiple entrences and escape routes ",
            "No remarks",
            "No remarks",
            "No remarks",
            "Take into account the past report, witnesses testemonies and daily schedule.",
            "No remarks",
            "No remarks",
            "No remarks",
            "pay attention to possible surprises ike people who might appear in the ciene",
            "No remarks",
            "No remarks",
            "The details contain:the agents who will take part in the mission, the time of the raid etc.",
        };
        int[] _complexity =
        {
            1,2,2,2,2,2,1,2,3,3,3,3,3,3,3,1,3,1,1,1,3,1,1,1,3,1,2,3
        };

        for (int i = 0; i < 28; i++)
        {
            //A range of a month between the currrent date and the day of the begining of the project 
            int range = (startProjectDate - currentDate).Days;

            //DateTime? _createAtDate = currentDate.AddDays(s_rand.Next(range+1));
            DateTime? _createAtDate = DateTime.Now;


            Task newTask = new(0, _aliasses[i], _descriptions[i], _createAtDate, null, false, (AgentExperience)_complexity[i], null, null, null, null, _deliverables[i], _remarks[i], null);
            s_dal!.Task.Create(newTask);
        }
    }
    /// <summary>
    /// The method creates 40 dependencies between the tasks above
    /// </summary>
    private static void createDependency()
    {

        s_dal!.Dependency.Create(new Dependency(0, 3, 1));
        s_dal!.Dependency.Create(new Dependency(0, 3, 2));
        s_dal!.Dependency.Create(new Dependency(0, 4, 3));
        s_dal!.Dependency.Create(new Dependency(0, 5, 3));
       //s_dal!.Dependency.Create(new Dependency(0, 3, 1));
        s_dal!.Dependency.Create(new Dependency(0, 6, 3));
        s_dal!.Dependency.Create(new Dependency(0, 13, 4));
        s_dal!.Dependency.Create(new Dependency(0, 19, 4));
        s_dal!.Dependency.Create(new Dependency(0, 18, 8));
        s_dal!.Dependency.Create(new Dependency(0, 16, 8));
        s_dal!.Dependency.Create(new Dependency(0, 16, 5));
        s_dal!.Dependency.Create(new Dependency(0, 18, 5));
        s_dal!.Dependency.Create(new Dependency(0, 18, 6));
        s_dal!.Dependency.Create(new Dependency(0, 13, 10));
        s_dal!.Dependency.Create(new Dependency(0, 14, 10));
        s_dal!.Dependency.Create(new Dependency(0, 13, 11));
        s_dal!.Dependency.Create(new Dependency(0, 14, 11));
        s_dal!.Dependency.Create(new Dependency(0, 13, 12));
        s_dal!.Dependency.Create(new Dependency(0, 14, 12));
        s_dal!.Dependency.Create(new Dependency(0, 19, 16));
        s_dal!.Dependency.Create(new Dependency(0, 17, 16));
        s_dal!.Dependency.Create(new Dependency(0, 19, 18));
        s_dal!.Dependency.Create(new Dependency(0, 20, 18));
        s_dal!.Dependency.Create(new Dependency(0, 21, 13));
        s_dal!.Dependency.Create(new Dependency(0, 15, 14));
        s_dal!.Dependency.Create(new Dependency(0, 21, 15));
        s_dal!.Dependency.Create(new Dependency(0, 21, 19));
        s_dal!.Dependency.Create(new Dependency(0, 19, 20));
        s_dal!.Dependency.Create(new Dependency(0, 23, 22));
        s_dal!.Dependency.Create(new Dependency(0, 24, 22));
        s_dal!.Dependency.Create(new Dependency(0, 25, 22));
        s_dal!.Dependency.Create(new Dependency(0, 21, 22));
        s_dal!.Dependency.Create(new Dependency(0, 25, 17));
        s_dal!.Dependency.Create(new Dependency(0, 25, 21));
        s_dal!.Dependency.Create(new Dependency(0, 26, 25));
        s_dal!.Dependency.Create(new Dependency(0, 28, 26));
        s_dal!.Dependency.Create(new Dependency(0, 27, 20));
        s_dal!.Dependency.Create(new Dependency(0, 27, 7));
        s_dal!.Dependency.Create(new Dependency(0, 19, 27));
        s_dal!.Dependency.Create(new Dependency(0, 17 ,8));
        s_dal!.Dependency.Create(new Dependency(0, 14, 2));

    }

    public static void Do()
    {
        s_dal = DalApi.Factory.Get;
        createAgent();
        createTask();
        createDependency();
    }
}







