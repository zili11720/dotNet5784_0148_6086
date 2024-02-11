using PL.Agent;
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
    public MainWindow()
    {
        InitializeComponent();
    }
    private void btnAgents_Click(object sender, RoutedEventArgs e)
    {
        new AgentListWindow().Show();
    }

    private void btnInitialize_Click(object sender, RoutedEventArgs e)
    {
        string message = "Are you sure you want to initialize the data?";
        string title = "Date Initialization";
        MessageBoxButton buttons = MessageBoxButton.YesNo;
        MessageBoxResult result= MessageBox.Show(message,title, buttons);
        if (result == MessageBoxResult.Yes)
            DalTest.Initialization.Do();
        else
            this.Close();
    }

}
