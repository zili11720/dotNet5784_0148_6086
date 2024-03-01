using PL.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private event Action <int, bool>? _AddOrUpdate;

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    public TaskWindow(int TaskId=0, Action<int, bool>? AddOrUpdate = null)
    {
        InitializeComponent();
        _AddOrUpdate = AddOrUpdate!;
        try
        {
            //Fetch the task with the given id or create a new one with defult values
            CurrentTask = (TaskId is not 0 ) ? s_bl.Task.Read(TaskId)! : new BO.Task()
            { 
                //id=next running number
                Alias = "",
                Description="",
                Status=BO.TaskStatus.Unscheduled ,
                CreatedAtDate=DateTime.Now,
                ScheduledDate=null,
                StartDate=null,
                RequiredEffortTime=null,
                EstimatedCompleteDate=null,
                DeadlineDate=null,
                CompleteDate=null,
                Deliverables="",
                Remarks="",
                Complexity = null,
                TaskAgent=null,
                DependenciesList=null,
            };
        }
        catch (BO.BlDoesNotExistException ex)
        {
            CurrentTask = null!;
            MessageBox.Show(ex.Message, "Could not find a task with the a given id", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (s_bl.Task.ReadAll().Any(a => a.Id == CurrentTask.Id) is true)
            {
                s_bl.Task.Update(CurrentTask);
                if(_AddOrUpdate is not null)
                    _AddOrUpdate(CurrentTask.Id, true);
                MessageBox.Show("Task was successfuly updated", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                s_bl.Task.Create(CurrentTask);
                _AddOrUpdate(CurrentTask.Id,false);
                MessageBox.Show("Task was successfuly added", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Worng input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    private void btnOpenDepencyList_Click(object sender, RoutedEventArgs e)
    {
        if (((sender as Button)?.DataContext as BO.Task) is BO.Task task)
        {
            new DependencyListWindow(task.Id).ShowDialog();
        }
    }
}


