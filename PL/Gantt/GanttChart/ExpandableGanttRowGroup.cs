using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PL.Gantt.GanttChart;

public class ExpandableGanttRowGroup : HeaderedGanttRowGroup
{
    public bool IsExpanded { get; set; }
}
