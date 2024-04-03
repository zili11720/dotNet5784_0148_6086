using PL.Agent;
using PL.Employee;
using PL.Gantt;
using PL.Task;
using System.Windows;

namespace PL.Manager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    //public User User { get; }
    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));



    public DateTime? ProjectStartDate
    {
        get { return (DateTime?)GetValue(ProjectStartDateProperty); }
        set { SetValue(ProjectStartDateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProjectStartDate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProjectStartDateProperty =
        DependencyProperty.Register("ProjectStartDate", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null));

    public MainWindow()
    {
        CurrentTime = s_bl.Clock;
        ProjectStartDate = (DateTime?)s_bl.GetProjectStartDate()!;
        InitializeComponent();
    }
    private void btnAgents_Click(object sender, RoutedEventArgs e)
    {
        //new AgentEmployeeWindow(203441715).Show();
        new AgentListWindow().Show();
    }
    private void btnTasks_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow().Show();
    }

    private void btnInitialize_Click(object sender, RoutedEventArgs e)
    {
        string message = "Are you sure you want to initialize the data?";
        string title = "Data Initialization";
        MessageBoxButton buttons = MessageBoxButton.YesNo;
        MessageBoxResult result = MessageBox.Show(message, title, buttons);
        if (result == MessageBoxResult.Yes)
            s_bl.InitializeData();
    }

    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        string message = "Are you sure you want to reset the data?";
        string title = "Data Reset";
        MessageBoxButton buttons = MessageBoxButton.YesNo;
        MessageBoxResult result = MessageBox.Show(message, title, buttons);
        if (result == MessageBoxResult.Yes)
            s_bl.ResetData();
    }

    private void btnResetClock_Click(object sender, RoutedEventArgs e)
    {
        CurrentTime = s_bl.ResetClock();
    }

    private void btnAddDay_Click(object sender, RoutedEventArgs e)
    {
        CurrentTime = s_bl.updateDay();
    }

    private void btnAddMonth_Click(object sender, RoutedEventArgs e)
    {
        CurrentTime = s_bl.updateMonth();
    }

    private void btnAddYear_Click(object sender, RoutedEventArgs e)
    {
        CurrentTime = s_bl.updateYear();
    }

    private void btnAutomaticSchedule_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string message = "Are you sure you want to create a schedule?" +
                "after the schedule is set you won't be able to add/earase tasks or agents";
            string title = "Automatic schedule";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
                s_bl.Task.CreateAutomaticSchedule();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Worng input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnSetProjectStartDate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string message = "Are you sure you want set the project start date?";
            string title = "Project start date";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
                s_bl.SetProjectStartDate(ProjectStartDate);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Worng input", MessageBoxButton.OK, MessageBoxImage.Error);
            ProjectStartDate = s_bl.GetProjectStartDate();///לסדר את הבבינג
        }
    }

    private void btnWatchGantt_Click(object sender, RoutedEventArgs e)
    {
        if (s_bl.GetProjectStatus() != BO.ProjectStatus.ExecutionTime)
            MessageBox.Show("To craete a gantt please set a schedule for the project", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else
            new GanttWindow().Show();
    }
}

