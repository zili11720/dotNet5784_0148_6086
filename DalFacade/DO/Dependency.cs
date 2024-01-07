
using System.Data;

namespace DO;
/// <summary>
/// A link between tow tasks to represent their performance order
/// </summary>
/// <param name="Id">Personal unique Id of a link between tasks</param>
/// <param name="DependentTask"> ID number of a dependent task</param>
/// <param name="DependsOnTask">Previous assignment ID number</param>
public record Dependency
(
  int Id,
  int DependentTask,
  int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }//empty ctr
    public Dependency(int _DependentTask, int _DependsOnTask) : this()
    { DependentTask = _DependentTask; 
     DependsOnTask = _DependsOnTask; }


}