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
