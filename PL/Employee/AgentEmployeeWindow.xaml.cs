
using System.Windows;

namespace PL.Employee;
    /// <summary>
    /// Interaction logic for AgentEmployeeWindow.xaml
    /// </summary>
public partial class AgentEmployeeWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public AgentEmployeeWindow()
    {
        InitializeComponent();
    }

    private void btnTaskdDetails_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnAvailableTasks_Click(object sender, RoutedEventArgs e)
    {

    }
}
