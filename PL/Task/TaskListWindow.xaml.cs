using BO;
using PL.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
//using PL.Tools.ToObservableCollectionExtansion;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskListWindow()
    {
        InitializeComponent();
        TaskList = s_bl.Task.ReadAll().ToObservableCollection();
    }

    public ObservableCollection<BO.TaskInList> TaskList    
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskListProperty", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
    
    public BO.AgentExperience Complexity { get; set; } = BO.AgentExperience.None;
    private void cbTaskComplexity_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TaskList = (Complexity == BO.AgentExperience.None) ?
        s_bl?.Task.ReadAll()!.ToObservableCollection() : s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!.ToObservableCollection();
    }

    private void btnAddNewTask_Click(object sender, RoutedEventArgs e)
    {
        new TaskWindow(AddOrUpdate).ShowDialog();
    }

    private void dgUpdateTask_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.TaskInList? taskInList=(sender as DataGrid)?.SelectedItem as BO.TaskInList;
        if(taskInList is not null) 
        {
            new TaskWindow(AddOrUpdate,taskInList.Id).ShowDialog();
        }

    }

    private void AddOrUpdate(int Id,bool _updated)
    {
        BO.TaskInList taskInList = new BO.TaskInList()
        {
            Id = Id,
            Alias = s_bl.Task.Read(Id)!.Alias,
            Description = s_bl.Task.Read(Id)!.Description,
            Status = s_bl.Task.Read(Id)!.Status,
            Complexity = (BO.AgentExperience?)Complexity
        };
        if(_updated) 
        {
            var oldTask=TaskList.FirstOrDefault(item => item.Id==Id);
            TaskList.Remove(oldTask!);
            //TaskList.OrderBy(item => item.Id == Id); //TO.
        }
        TaskList.Add(taskInList);
    }


   

    //private void reLoadList_activated(object sender, EventArgs e)
    //{
    //    TaskList = (Complexity == BO.AgentExperience.None) ?
    //    s_bl?.Task.ReadAll()!.ToObservableCollection() : s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!.ToObservableCollection();
    //}
}
