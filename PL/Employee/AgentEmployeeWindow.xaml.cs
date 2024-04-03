
using PL.Agent;
using PL.Task;
using System.ComponentModel;
using System.Windows;

namespace PL.Employee;
/// <summary>
/// Interaction logic for AgentEmployeeWindow.xaml
/// </summary>
public partial class AgentEmployeeWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    int employeeId = 0;
    public AgentEmployeeWindow(int _agentId)
    {
        CurrentTime = s_bl.Clock;
        CurrentEmployee = s_bl.Agent.Read(_agentId)!;
        employeeId = _agentId;
        InitializeComponent();
    }


    public BO.Agent CurrentEmployee
    {
        get { return (BO.Agent)GetValue(CurrentEmployeeProperty); }
        set { SetValue(CurrentEmployeeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEmployee.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEmployeeProperty =
        DependencyProperty.Register("CurrentEmployee", typeof(BO.Agent), typeof(AgentEmployeeWindow), new PropertyMetadata(null));

    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(AgentEmployeeWindow), new PropertyMetadata(null));


    private void btnTaskDetails_Click(object sender, RoutedEventArgs e)
    {
        //If this agent isn't working on any task right now
        if(CurrentEmployee.CurrentTask is null|| CurrentEmployee.CurrentTask.Id == 0)
            MessageBox.Show("your'e not working on any task right now! please choose a new task","Error", MessageBoxButton.OK, MessageBoxImage.Information);
        else//Open new TaskWindow with current task details
          new TaskWindow(CurrentEmployee.CurrentTask!.Id).Show();
    }

    private void btnAvailableTasks_Click(object sender, RoutedEventArgs e)
    {
        //List of the available tasks for an agent
        new TaskListWindow(false,CurrentEmployee.Id).Show();
    }

    private void EmployeeWindow_Activated(object sender, EventArgs e)
    {
        CurrentEmployee = s_bl.Agent.Read(employeeId)!;
    }
}


