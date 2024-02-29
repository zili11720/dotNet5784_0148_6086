
using PL.Agent;
using PL.Task;
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
        this.employeeId = _agentId;
        BO.Agent? employee = s_bl.Agent.Read(employeeId);
        InitializeComponent();
    }

    public BO.Agent CurrentEmployee
    {
        get { return (BO.Agent)GetValue(CurrentEmployeeProperty); }
        set { SetValue(CurrentEmployeeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentAgent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEmployeeProperty =
        DependencyProperty.Register("CurrentEmployee", typeof(BO.Agent), typeof(AgentEmployeeWindow), new PropertyMetadata(null));
    private void btnTaskDetails_Click(object sender, RoutedEventArgs e)
    {

        //new TaskWindow(employee.CurrentTask.Id).Show();
    }

    private void btnAvailableTasks_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow(false, employeeId).Show();
    }
}

