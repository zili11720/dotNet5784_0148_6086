using PL.Tools;
using System.Collections.ObjectModel;
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
        AgentList = s_bl?.Agent.ReadAll().ToObservableCollection()!;
    }

    public ObservableCollection<BO.AgentInList> AgentList
    {
        get { return (ObservableCollection<BO.AgentInList>)GetValue(AgentListProperty); }
        set { SetValue(AgentListProperty, value); }
    }

    public static readonly DependencyProperty AgentListProperty =
        DependencyProperty.Register("AgentList", typeof(IEnumerable<BO.AgentInList>), typeof(AgentListWindow), new PropertyMetadata(null));
    public BO.AgentExperience Experience { get; set; } = BO.AgentExperience.None;


    private void cbAgentExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AgentList = (Experience == BO.AgentExperience.None) ?
        s_bl?.Agent.ReadAll()!.ToObservableCollection() : s_bl?.Agent.ReadAll(item => item.Specialty == Experience)!.ToObservableCollection();
    }

    private void btnAddNewAgent_Click(object sender, RoutedEventArgs e)
    {
        new AgentWindow(AddOrUpdate).ShowDialog();
    }

    private void lsUpdateAgent_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.AgentInList? agentInList = (sender as DataGrid)?.SelectedItem as BO.AgentInList;
        if (agentInList is not null)
        {
            new AgentWindow(AddOrUpdate,agentInList.Id).ShowDialog();
        }
    }
    private void AddOrUpdate(int Id, bool _updated)
    {
        BO.AgentInList agentInList = new BO.AgentInList()
        {
            Id = Id,
            Name = s_bl.Agent.Read(Id)!.Name,
            Specialty = (BO.AgentExperience?)s_bl.Agent.Read(Id)!.Specialty,
            CurrentTask = null//s_bl.Agent.Read(Id).CurrentTask
        };
        if (_updated)
        {
            var oldTask = AgentList.FirstOrDefault(item => item.Id == Id);
            AgentList.Remove(oldTask!);
        }
        AgentList.Add(agentInList);
    }

    private void btnDeleteAgent_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the agent?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning); 
            if(MessageBoxResult.Yes==result)
            {
                BO.AgentInList agent = (sender as Button)?.CommandParameter as BO.AgentInList;
                s_bl.Agent.Delete(agent!.Id);
                AgentList.Remove(agent);
                MessageBox.Show("The agent was deleted successfuly", "Well done!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    //private void reLoadList_activated(object sender, EventArgs e)
    //{
    //    AgentList = (Experience == BO.AgentExperience.None) ?
    //    s_bl?.Agent.ReadAll()!.ToObservableCollection() : s_bl?.Agent.ReadAll(item => item.Specialty == Experience)!.ToObservableCollection();
    //}
}
