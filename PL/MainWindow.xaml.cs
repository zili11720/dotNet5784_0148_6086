using PL.Agent;
using PL.Task;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    //public User User { get; }
    public MainWindow()
    {
        InitializeComponent();

    }
    private void btnAgents_Click(object sender, RoutedEventArgs e)
    {
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
        MessageBoxResult result= MessageBox.Show(message,title, buttons);
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
}
