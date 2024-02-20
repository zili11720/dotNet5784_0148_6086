using BO;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAll();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskListProperty", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.AgentExperience Complexity { get; set; } = BO.AgentExperience.None;
        private void cbTaskComplexity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Complexity == BO.AgentExperience.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.GetTasksByComplexity(Complexity)!; //לא עובד
        }

        private void btnAddNewTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }

        private void dgUpdateTask_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? taskInList=(sender as DataGrid)?.SelectedItem as BO.TaskInList;
            if(taskInList is not null) 
            {
                new TaskWindow(taskInList.Id).ShowDialog();
            }
        }

        private void reLoadList_activated(object sender, EventArgs e)
        {
            TaskList = (Complexity == BO.AgentExperience.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.GetTasksByComplexity(Complexity)!; //לא עובד
        }
    }


    




}
