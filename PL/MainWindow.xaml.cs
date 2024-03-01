using PL.Employee;
using PL.Task;
using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    //public User User { get; }
    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimePropert); }
        set { SetValue(CurrentTimePropert, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimePropert =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));
    public MainWindow()
    {
        CurrentTime = s_bl.Clock;
        InitializeComponent();
    }
    private void btnAgents_Click(object sender, RoutedEventArgs e)
    {
        new AgentEmployeeWindow(293821292).Show();
        //new AgentListWindow().Show();
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
        s_bl.ResetClock();
    }

    private void btnAddDay_Click(object sender, RoutedEventArgs e)
    {
        s_bl.updateDay();
    }

    private void btnAddHour_Clock(object sender, System.Windows.Controls.ContextMenuEventArgs e)
    {
        s_bl.updateHour();
    }

    private void btnAddYear_Click(object sender, RoutedEventArgs e)
    {
       s_bl.updateYear();
    }
}
