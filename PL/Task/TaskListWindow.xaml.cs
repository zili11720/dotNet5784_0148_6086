using PL.Tools;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public bool isManager { get; set; } = true;

    public TaskListWindow(bool _isManager = true, int employeeId = 0)
    {
        isManager = _isManager;
        InitializeComponent();
        if (isManager)
            TaskList = s_bl.Task.ReadAll().ToObservableCollection();
        else
            TaskList = s_bl.Agent.AvailableTasks(employeeId).ToObservableCollection();
    }

    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

    public BO.AgentExperience complexity { get; set; } = BO.AgentExperience.None;//A variable for complexity combo box

    private void cbTaskComplexity_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (complexity == BO.AgentExperience.None) ?
        s_bl?.Task.ReadAll()!.ToObservableCollection() : s_bl?.Task.ReadAll(item => item.Status == status && item.Complexity == complexity)!.ToObservableCollection()!;
    }
    public BO.TaskStatus status { get; set; } = BO.TaskStatus.None;//A variable for status combo box

    private void cbTaskStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (status == BO.TaskStatus.None) ?
       s_bl?.Task.ReadAll()!.ToObservableCollection() : s_bl?.Task.ReadAll(item => item.Status == status && item.Complexity == complexity)!.ToObservableCollection();
    }
    private void btnAddNewTask_Click(object sender, RoutedEventArgs e)
    {
        new TaskWindow(0, AddOrUpdate).ShowDialog();
    }

    private void dgUpdateTask_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.TaskInList? taskInList = (sender as DataGrid)?.SelectedItem as BO.TaskInList;
        if (taskInList is not null)
        {
            new TaskWindow(taskInList.Id, AddOrUpdate).ShowDialog();
        }

    }

    /// <summary>
    /// Update taskList according to the action done
    /// </summary>
    /// <param name="Id">Id of the task added/updated</param>
    /// <param name="_updated">A boolien variable that indicates wether a task was updated/added</param>
    private void AddOrUpdate(int Id, bool _updated)
    {
        BO.TaskInList taskInList = new BO.TaskInList()
        {
            Id = Id,
            Alias = s_bl.Task.Read(Id)!.Alias,
            Description = s_bl.Task.Read(Id)!.Description,
            Status = s_bl.Task.Read(Id)!.Status,
            Complexity = (BO.AgentExperience?)complexity
        };
        if (_updated)//case update
        {
            var oldTask = TaskList.FirstOrDefault(item => item.Id == Id);
            TaskList.Remove(oldTask!);//Remove old task before adding the updated version
        }
        TaskList.Add(taskInList);
    }

    private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (isManager is false)
                MessageBox.Show("Onle a manager can delete tasks", "invalid action", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the task?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (MessageBoxResult.Yes == result)
                {
                    BO.TaskInList? task = (sender as Button)!.CommandParameter as BO.TaskInList;
                    s_bl.Task.Delete(task!.Id);
                    TaskList.Remove(task);
                    MessageBox.Show("The task was deleted successfuly", "Well done!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}
