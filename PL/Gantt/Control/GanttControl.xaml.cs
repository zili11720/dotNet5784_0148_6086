using nGantt.PeriodSplitter;
using PL.Gantt;
using PL.Gantt.GanttChart;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
#nullable disable

namespace PL;

/// <summary>
/// Interaction logic for GanttControler.xaml
/// </summary>
public partial class GanttControl : UserControl
{
    public enum SelectionMode
    {
        None,
        Single,
        Multiple
    }

    private double selectionStartX;
    public bool AllowUserSelection { get; set; }

    public event EventHandler SelectedItemChanged;
    public event EventHandler<PeriodEventArgs> GanttRowAreaSelected;

    public delegate string PeriodNameFormatter(Period period);
    public delegate Brush BackgroundFormatter(TimeLineItem timeLineItem, System.Windows.Media.Color color);

    private ObservableCollection<TimeLine> gridLineTimeLines = new ObservableCollection<TimeLine>();
    public ObservableCollection<ContextMenuItem> GanttTaskContextMenuItems { get; set; }
    public ObservableCollection<SelectionContextMenuItem> SelectionContextMenuItems { get; set; }
    public ObservableCollection<TimeLine> GridLineTimeLine { get { return gridLineTimeLines; } }
    public ObservableCollection<TimeLine> TimeLines { get; private set; }

    private GanttChartData ganttChartData = new GanttChartData();///

    private TimeLine gridLineTimeLine;
    public GanttChartData GanttData => ganttChartData;
    public Period SelectionPeriod { get; private set; }
    public SelectionMode TaskSelectionMode { get; set; }

    private System.Windows.Media.Color[] _colors;

    private readonly Random _randomColors = new();

    public List<GanttTask> SelectedItems
    {
        get
        {
            List<GanttTask> selectedItems = new List<GanttTask>();
            foreach (var group in ganttChartData.RowGroups)
            {
                foreach (GanttRow row in group.Rows)
                {
                    var items = from ganttTask in row.Tasks where ganttTask.IsSelected == true select ganttTask;
                    foreach (var item in items)
                        selectedItems.Add(item);
                }
            }
            return selectedItems;
        }
    }

    public GanttControl()
    {
        initColors();
        InitializeComponent();
    }

    private void initColors()
    {
        _colors = typeof(Colors)
            .GetProperties()
            .Select(property => (System.Windows.Media.Color)property
            .GetValue(null)!)
            .ToArray();
    }

    public void Initialize(DateTime minDate, DateTime maxDate)
    {
        ganttChartData.MinDate = minDate;
        ganttChartData.MaxDate = maxDate;
    }

    public void AddGanttTask(GanttRow row, GanttTask task)
    {
        if (task.Start < ganttChartData.MaxDate && task.End > ganttChartData.MinDate)
            row.Tasks.Add(task);
    }

    public TimeLine CreateTimeLine(PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter)
    {
        if (splitter.MaxDate != GanttData.MaxDate || splitter.MinDate != GanttData.MinDate)
            throw new ArgumentException("The timeline must have the same max and min -date as the chart");

        var timeLineParts = splitter.Split();

        var timeline = new TimeLine();
        foreach (var p in timeLineParts)
        {
            timeline.Items.Add(new TimeLineItem() { Name = PeriodNameFormatter(p), Start = p.Start, End = p.End.AddSeconds(-1) });
        }

        ganttChartData.TimeLines.Add(timeline);
        return timeline;
    }

    public void ClearGantt()
    {
        ganttChartData.RowGroups.Clear();
        ganttChartData.TimeLines.Clear();
        SelectedItems.Clear();
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (TaskSelectionMode == SelectionMode.None)
            return;

        if (TaskSelectionMode == SelectionMode.Single)
            DeselectAllTasks();

        var gantTask = ((GanttTask)((FrameworkElement)(sender)).DataContext);
        gantTask.IsSelected = !gantTask.IsSelected;

        if (SelectedItemChanged != null)
            SelectedItemChanged(this, EventArgs.Empty);
    }

    private void DeselectAllTasks()
    {
        foreach (var group in ganttChartData.RowGroups)
        {
            foreach (GanttRow row in group.Rows)
            {
                foreach (var task in row.Tasks)
                    task.IsSelected = false;
            }
        }
    }

    public GanttRowGroup CreateGanttRowGroup()
    {
        var rowGroup = new GanttRowGroup() { };
        ganttChartData.RowGroups.Add(rowGroup);
        return rowGroup;
    }

    public HeaderedGanttRowGroup CreateGanttRowGroup(string name)
    {
        var rowGroup = new HeaderedGanttRowGroup() { Name = name };
        ganttChartData.RowGroups.Add(rowGroup);
        return rowGroup;
    }

    public ExpandableGanttRowGroup CreateGanttRowGroup(string name, bool isExpanded)
    {
        var rowGroup = new ExpandableGanttRowGroup() { Name = name, IsExpanded = isExpanded };
        ganttChartData.RowGroups.Add(rowGroup);
        return rowGroup;
    }

    public GanttRow CreateGanttRow(GanttRowGroup rowGroup, string name)
    {
        var rowHeader = new GanttRowHeader() { Name = name };
        var row = new GanttRow() { RowHeader = rowHeader, Tasks = new ObservableCollection<GanttTask>() };
        rowGroup.Rows.Add(row);
        return row;
    }

    public void SetGridLinesTimeline(TimeLine timeline)
    {
        if (!ganttChartData.TimeLines.Contains(timeline))
            throw new Exception("Invalid timeline");

        gridLineTimeLine = timeline;
    }

    public void SetGridLinesTimeline(TimeLine timeline, BackgroundFormatter backgroundFormatter)
    {
        if (!ganttChartData.TimeLines.Contains(timeline))
            throw new Exception("Invalid timeline");

        var color = _colors[_randomColors.Next(_colors.Length)];

        foreach (var item in timeline.Items)
            item.BackgroundColor = backgroundFormatter(item, color);

        gridLineTimeLines.Clear();
        gridLineTimeLines.Add(timeline);
        //gridLineTimeLine = timeline;
    }

    #region "Handle selection rectangle -area"

    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!AllowUserSelection)
            return;

        // TODO:: Set visibillity to hidden for all selectionRectangles
        var canvas = ((Canvas)UIHelper.FindVisualParent<Grid>(((DependencyObject)sender))!.FindName("selectionCanvas"));
        Border selectionRectangle = (Border)canvas.FindName("selectionRectangle");
        selectionStartX = e.GetPosition(canvas).X;
        selectionRectangle.Margin = new Thickness(selectionStartX, 0, 0, 5);
        selectionRectangle.Visibility = Visibility.Visible;
        selectionRectangle.IsEnabled = true;
        selectionRectangle.IsHitTestVisible = false;
        selectionRectangle.Width = 0;
    }

    private void selectionRectangle_MouseMove(object sender, MouseEventArgs e)
    {
        ChangeSelectionRectangleSize(sender, e);
    }

    private void ChangeSelectionRectangleSize(object sender, MouseEventArgs e)
    {
        var canvas = ((Canvas)UIHelper.FindVisualParent<Grid>(((DependencyObject)sender))!.FindName("selectionCanvas"));
        Border selectionRectangle = (Border)canvas.FindName("selectionRectangle");
        if (selectionRectangle.IsEnabled)
        {


            double SelectionEndX = e.GetPosition(canvas).X;
            double selectionWidth = SelectionEndX - selectionStartX;
            if (selectionWidth > 0)
            {
                selectionRectangle.Width = selectionWidth;
            }
            else
            {
                selectionWidth = -selectionWidth;
                double selectionRectangleStartX = selectionStartX - selectionWidth;
                selectionRectangle.Width = selectionWidth;
                selectionRectangle.Margin = new Thickness(selectionRectangleStartX, 0, 0, 5);
            }
        }
    }

    private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        StopSelection(sender, e);
    }

    private void selectionCanvas_MouseLeave(object sender, MouseEventArgs e)
    {
        StopSelection(sender, e);
    }

    private void StopSelection(object sender, MouseEventArgs e)
    {
        var canvas = ((Canvas)UIHelper.FindVisualParent<Grid>(((DependencyObject)sender))!.FindName("selectionCanvas"));
        Border selectionRectangle = (Border)canvas.FindName("selectionRectangle");

        if (selectionRectangle.IsEnabled)
        {
            ChangeSelectionRectangleSize(sender, e);
            selectionRectangle.IsEnabled = false;
            selectionRectangle.IsHitTestVisible = true;

            if (selectionRectangle.Visibility == System.Windows.Visibility.Visible)
            {
                if (GanttRowAreaSelected != null)
                {
                    if (selectionRectangle.Width > 0)
                    {
                        double totalWidth = canvas.ActualWidth;
                        var tsTaskStart = new TimeSpan(Convert.ToInt64((ganttChartData.MaxDate.Ticks - ganttChartData.MinDate.Ticks) * (selectionStartX / totalWidth)));
                        var tsTaskEnd = new TimeSpan(Convert.ToInt64((ganttChartData.MaxDate.Ticks - ganttChartData.MinDate.Ticks) * ((selectionStartX + selectionRectangle.Width) / totalWidth)));
                        var selctionStartDate = ganttChartData.MinDate.Add(tsTaskStart);
                        var selctionEndDate = ganttChartData.MinDate.Add(tsTaskEnd);
                        SelectionPeriod.Start = selctionStartDate;
                        SelectionPeriod.End = selctionEndDate;
                        GanttRowAreaSelected(this, new PeriodEventArgs() { SelectionStart = selctionStartDate, SelectionEnd = selctionEndDate });
                    }
                }
            }
        }
    }

    #endregion

    private void selectionRectangle_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        ((Border)sender).ContextMenu.IsOpen = true;
    }
}
