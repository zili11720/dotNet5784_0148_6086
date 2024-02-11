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
    public IEnumerable<BO.AgentInList> AgentList
    {
        get { return (IEnumerable<BO.AgentInList>)GetValue(AgentListProperty); }
        set { SetValue(AgentListProperty, value); }
    }

    public static readonly DependencyProperty AgentListProperty =
        DependencyProperty.Register("AgentList", typeof(IEnumerable<BO.AgentInList>), typeof(AgentListWindow), new PropertyMetadata(null));


    public AgentListWindow()
    {
        InitializeComponent();
        AgentList = s_bl?.Agent.ReadAll()!;
    }
}
