using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Agent;

/// <summary>
/// Interaction logic for AgentListWindow.xaml
/// </summary>
public partial class AgentListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public AgentListWindow()
    {
        InitializeComponent();
        AgentList = s_bl?.Agent.ReadAll()!;
    }

    public IEnumerable<BO.AgentInList> AgentList
    {
        get { return (IEnumerable<BO.AgentInList>)GetValue(AgentListProperty); }
        set { SetValue(AgentListProperty, value); }
    }

    public static readonly DependencyProperty AgentListProperty =
        DependencyProperty.Register("AgentList", typeof(IEnumerable<BO.AgentInList>), typeof(AgentListWindow), new PropertyMetadata(null));
    public BO.AgentExperience Experience { get; set; } = BO.AgentExperience.None;


    private void cbAgentExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AgentList = (Experience == BO.AgentExperience.None) ?
        s_bl?.Agent.ReadAll()! : s_bl?.Agent.ReadAll(item => item.Specialty == Experience)!;
    }

    private void btnAddNewAgent_Click(object sender, RoutedEventArgs e)
    {
        new AgentWindow().ShowDialog();
    }

    private void lsUpdateAgent_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.AgentInList? agentInList = (sender as ListView)?.SelectedItem as BO.AgentInList;
        if (agentInList is not null)
        {
            new AgentWindow(agentInList.Id).ShowDialog() ;
        }
    }


    private void reLoadList_activated(object sender, EventArgs e)
    {
        AgentList = (Experience == BO.AgentExperience.None) ?
        s_bl?.Agent.ReadAll()! : s_bl?.Agent.ReadAll(item => item.Specialty == Experience)!;
    }
}
