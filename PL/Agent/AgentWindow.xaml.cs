using System.Windows;

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


    public AgentWindow(int AgentId = 0/*default Id of the agent*/)
    {
        InitializeComponent();
        try
        {
            //Fetch the agent with the given id or create a new one with defult values if the id does not exist
            CurrentAgent = (AgentId is not 0) ? s_bl.Agent.Read(AgentId)! : new BO.Agent() { Id = 0, Email = "", Cost = 0, Name = "", Specialty = BO.AgentExperience.None, CurrentTask = null };
        }
        catch (BO.BlDoesNotExistException ex)
        {
            CurrentAgent = null;
            MessageBox.Show(ex.Message, "Could not find an agent with a given id", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (s_bl.Agent.ReadAll().Any(a => a.Id == CurrentAgent.Id) is true)
            {
                s_bl.Agent.Update(CurrentAgent);
                MessageBox.Show("Agent was successfuly updated", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                s_bl.Agent.Create(CurrentAgent);
                MessageBox.Show("Agent was successfuly added", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Worng input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
