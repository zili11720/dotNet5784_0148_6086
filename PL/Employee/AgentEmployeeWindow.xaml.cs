
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

