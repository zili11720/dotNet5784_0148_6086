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
/// Interaction logic for AgentWindow.xaml
/// </summary>
public partial class AgentWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Agent CurrentAgent
    {
        get { return (BO.Agent)GetValue(CurrentAgentProperty); }
        set { SetValue(CurrentAgentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentAgent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentAgentProperty =
        DependencyProperty.Register("CurrentAgent", typeof(BO.Agent), typeof(AgentWindow), new PropertyMetadata(null));


    public AgentWindow(int AgentId = 0)
    {
        InitializeComponent();
        try
        {
            CurrentAgent = (AgentId is not 0) ? s_bl.Agent.Read(AgentId)! : new BO.Agent() { Id = 0, Email = "", Cost = 0, Name = "", Specialty = BO.AgentExperience.None, CurrentTask = null };
        }
        catch(BO.BlDoesNotExistException ex)
        {
            CurrentAgent = null;
            MessageBox.Show(ex.Message,"Could not find an agent with a given id",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            this.Close();
        }
    }  
}
