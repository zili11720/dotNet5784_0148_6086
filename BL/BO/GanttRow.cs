using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
///A task as a row in the gantt
/// <summary>
/// <param name="Id">Personal id of a task</param>
/// <param name="Name">Alias of the task</param>
/// <param name="dependencies">A string with the dependencies id's</param>
/// <param name="TaskDays">Requried time for the task</param>
/// <param name="StartOffser">Where the row starts from according to the start date of the task</param>
/// <param name="EndOffset">Where the row ends from according to the estimated complete date of the task</param>
/// <param name="Status">Status of the task</param>
/// </summary>
public class GanttRow
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Dependencies { get; set; }
    public int TasksDays { get; set; }
    public int StartOffset { get; set; }
    public int EndOffset { get; set; }
    public BO.TaskStatus Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}