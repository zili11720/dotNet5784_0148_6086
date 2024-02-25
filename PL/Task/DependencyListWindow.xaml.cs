using System.Windows;

namespace PL.Task;

/// <summary>
/// Interaction logic for DependencyListWindow.xaml
/// </summary>
public partial class DependencyListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DependencyListWindow(int id)
    {
        InitializeComponent();
        DependencyList = s_bl!.Task.GetDependenciesList(id);
    }

    public IEnumerable<BO.TaskInList> DependencyList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(DependencyListProperty); }
        set { SetValue(DependencyListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DependencyListProperty =
        DependencyProperty.Register("DependencyListProperty", typeof(IEnumerable<BO.TaskInList>), typeof(DependencyListWindow), new PropertyMetadata(null));
}
