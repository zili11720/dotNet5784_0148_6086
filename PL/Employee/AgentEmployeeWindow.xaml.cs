
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
        //this.employeeId = _agentId;
        CurrentEmployee = s_bl.Agent.Read(_agentId)!;
        ///לאתחל ב0 ערכים של המשימה אם היא לא קיימת
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
        if(CurrentEmployee.CurrentTask is null|| CurrentEmployee.CurrentTask.Id == 0)
            MessageBox.Show("your'e not working on any task right now! please choose a new task","Error", MessageBoxButton.OK, MessageBoxImage.Information);
        else
          new TaskWindow(CurrentEmployee.CurrentTask!.Id).Show();
    }

    private void btnAvailableTasks_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow(false,CurrentEmployee.Id).Show();
    }
}

