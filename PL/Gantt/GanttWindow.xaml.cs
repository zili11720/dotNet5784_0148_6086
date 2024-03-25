using PL.Gantt;
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

namespace PL.Gantt;

/// <summary>
/// Interaction logic for GanttWindow.xaml
/// </summary>
public partial class GanttWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public IEnumerable<BO.GanttRow> TaskOfGantt
    {
        get { return (IEnumerable<BO.GanttRow>)GetValue(TaskOfGanttProperty); }
        set { SetValue(TaskOfGanttProperty, value); }
    }

    public static readonly DependencyProperty TaskOfGanttProperty =
        DependencyProperty.Register("TaskOfGantt", typeof(IEnumerable<BO.GanttRow>), typeof(GanttWindow), new PropertyMetadata(null));
    public GanttWindow()
    {
        InitializeComponent();
        TaskOfGantt = s_bl?.Task.GetDetailsForGattRow();
    }
}








