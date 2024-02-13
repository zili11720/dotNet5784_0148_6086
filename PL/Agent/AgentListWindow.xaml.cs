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
    public BO.AgentExperience Experience{ get; set; } = BO.AgentExperience.All;
    

    private void cbAgentExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AgentList= (Experience == BO.AgentExperience.All)?
        s_bl?.Agent.ReadAll()! : s_bl?.Agent.ReadAll(item => item.Specialty == Experience)!;
    }
}
