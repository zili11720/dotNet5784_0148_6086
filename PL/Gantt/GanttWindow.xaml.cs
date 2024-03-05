using nGantt;
using nGantt.GanttChart;
using nGantt.PeriodSplitter;
using System.Collections.ObjectModel;
using System.Windows;

namespace PL.Gantt;

/// <summary>
/// Interaction logic for GantWindow.xaml
/// </summary>
public partial class GanttWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public GanttControl GanttControl
    {
        get { return (GanttControl)GetValue(GanttControlProperty); }
        set { SetValue(GanttControlProperty, value); }
    }

    // Using a DependencyProperty as the backing store for GanttChartData.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GanttControlProperty =
        DependencyProperty.Register(nameof(GanttControl), typeof(GanttControl), typeof(GanttWindow));

    public ObservableCollection<BO.Task> Tasks
    {
        get { return (ObservableCollection<BO.Task>)GetValue(TasksProperty); }
        set { SetValue(TasksProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Tasks.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TasksProperty =
        DependencyProperty.Register("Tasks", typeof(ObservableCollection<BO.Task>),
            typeof(GanttWindow), new PropertyMetadata(new ObservableCollection<BO.Task>(
                s_bl.Task.ReadAllTasks())));

    public GanttWindow()
    {
        GanttControl = new GanttControl();
        GantLenght = 50;
        // dateTimePicker.Value = DateTime.Parse("2024-02-01");
        CreateData(Tasks.Min(t => t.ScheduledDate).GetValueOrDefault()
            ,Tasks.Max(t => t.EstimatedCompleteDate).GetValueOrDefault());
        // Set selection -mode
        GanttControl.TaskSelectionMode = GanttControl.SelectionMode.Single;

        // Enable GanttTasks to be selected
        GanttControl.AllowUserSelection = true;

        // listen to the GanttRowAreaSelected event
        GanttControl.GanttRowAreaSelected += new EventHandler<PeriodEventArgs>(ganttControl1_GanttRowAreaSelected);

        // define ganttTask context menu and action when each item is clicked
        ganttTaskContextMenuItems.Add(new ContextMenuItem(ViewClicked, "View..."));
        ganttTaskContextMenuItems.Add(new ContextMenuItem(EditClicked, "Edit..."));
        ganttTaskContextMenuItems.Add(new ContextMenuItem(DeleteClicked, "Delete..."));
        GanttControl.GanttTaskContextMenuItems = ganttTaskContextMenuItems;

        // define selection context menu and action when each item is clicked
        selectionContextMenuItems.Add(new SelectionContextMenuItem(NewClicked, "New..."));
        GanttControl.SelectionContextMenuItems = selectionContextMenuItems;
        InitializeComponent();
    }

    private int GantLenght { get; set; }
    private ObservableCollection<ContextMenuItem> ganttTaskContextMenuItems = new ObservableCollection<ContextMenuItem>();
    private ObservableCollection<SelectionContextMenuItem> selectionContextMenuItems = new ObservableCollection<SelectionContextMenuItem>();


    private void NewClicked(Period selectionPeriod)
    {
        MessageBox.Show("New clicked for task " + selectionPeriod.Start.ToString() + " -> " + selectionPeriod.End.ToString());
    }

    private void ViewClicked(GanttTask ganttTask)
    {
        MessageBox.Show("New clicked for task " + ganttTask.Name);
    }

    private void EditClicked(GanttTask ganttTask)
    {
        MessageBox.Show("Edit clicked for task " + ganttTask.Name);
    }

    private void DeleteClicked(GanttTask ganttTask)
    {
        MessageBox.Show("Delete clicked for task " + ganttTask.Name);
    }

    void ganttControl1_GanttRowAreaSelected(object sender, PeriodEventArgs e)
    {
        MessageBox.Show(e.SelectionStart.ToString() + " -> " + e.SelectionEnd.ToString());
    }

    private System.Windows.Media.Brush DetermineBackground(TimeLineItem timeLineItem)
    {
        if (timeLineItem.End.Date.DayOfWeek == DayOfWeek.Saturday || timeLineItem.End.Date.DayOfWeek == DayOfWeek.Sunday)
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue);
        else
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
    }

    private void CreateData(DateTime minDate, DateTime maxDate)
    {
        // Set max and min dates
        GanttControl.Initialize(minDate, maxDate);

        // Create timelines and define how they should be presented
        GanttControl.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear);
        GanttControl.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth);
        var gridLineTimeLine = GanttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay);
        GanttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDayName);

        // Set the timeline to atatch gridlines to
        GanttControl.SetGridLinesTimeline(gridLineTimeLine, DetermineBackground);

        // Create and data
        var rowgroup1 = GanttControl.CreateGanttRowGroup("HeaderdGanttRowGroup");
        var row1 = GanttControl.CreateGanttRow(rowgroup1, "GanttRow 1");
        foreach (var task in Tasks)
        {
            GanttControl.AddGanttTask(row1,
                new GanttTask() { Start = task.ScheduledDate!.Value, End =task.EstimatedCompleteDate!.Value,
                    Name = task.Alias, TaskProgressVisibility = Visibility.Visible });

        }
    //    GanttControl.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2024-03-05"), End = DateTime.Parse("2024-05-01"), Name = "GanttRow 1:GanttTask 2" });
    //    GanttControl.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2024-06-01"), End = DateTime.Parse("2024-06-15"), Name = "GanttRow 1:GanttTask 3" });

    //    var rowgroup2 = GanttControl.CreateGanttRowGroup("ExpandableGanttRowGroup", true);
    //    var row2 = GanttControl.CreateGanttRow(rowgroup2, "GanttRow 2");
    //    var row3 = GanttControl.CreateGanttRow(rowgroup2, "GanttRow 3");
    //    GanttControl.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2024-02-10"), End = DateTime.Parse("2024-03-10"), Name = "GanttRow 2:GanttTask 1" });
    //    GanttControl.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2024-03-25"), End = DateTime.Parse("2024-05-10"), Name = "GanttRow 2:GanttTask 2" });
    //    GanttControl.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2024-06-10"), End = DateTime.Parse("2024-09-15"), Name = "GanttRow 2:GanttTask 3", PercentageCompleted = 0.375 });
    //    GanttControl.AddGanttTask(row3, new GanttTask() { Start = DateTime.Parse("2024-01-07"), End = DateTime.Parse("2024-09-15"), Name = "GanttRow 3:GanttTask 1", PercentageCompleted = 0.5 });

    //    var rowgroup3 = GanttControl.CreateGanttRowGroup();
    //    var row4 = GanttControl.CreateGanttRow(rowgroup3, "GanttRow 4");
    //    GanttControl.AddGanttTask(row4, new GanttTask() { Start = DateTime.Parse("2024-02-14"), End = DateTime.Parse("2024-02-27"), Name = "GanttRow 4:GanttTask 1", PercentageCompleted = 1 });
    //    GanttControl.AddGanttTask(row4, new GanttTask() { Start = DateTime.Parse("2024-04-8"), End = DateTime.Parse("2024-09-19"), Name = "GanttRow 4:GanttTask 2" });
   }

    private string FormatYear(Period period)
    {
        return period.Start.Year.ToString();
    }

    private string FormatMonth(Period period)
    {
        return period.Start.Month.ToString();
    }

    private string FormatDay(Period period)
    {
        return period.Start.Day.ToString();
    }

    private string FormatDayName(Period period)
    {
        return period.Start.DayOfWeek.ToString();
    }

    private void buttonPrevious_Click(object sender, EventArgs e)
    {
        dateTimePicker.SelectedDate = GanttControl.GanttData.MinDate.AddDays(-GantLenght);
    }

    private void buttonNext_Click(object sender, EventArgs e)
    {
        dateTimePicker.SelectedDate = GanttControl.GanttData.MaxDate;
    }

    private void dateTimePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (dateTimePicker.SelectedDate is DateTime minDate)
        {
            DateTime maxDate = minDate.AddDays(GantLenght);
            GanttControl.ClearGantt();
            CreateData(minDate, maxDate);
        }
    }
}
