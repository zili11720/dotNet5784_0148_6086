using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

internal class AgentExperienceCollection: IEnumerable
{
    static readonly IEnumerable<BO.AgentExperience> exp_enums= 
       (Enum.GetValues(typeof(BO.AgentExperience)) as IEnumerable<BO.AgentExperience>)!;

    public IEnumerator GetEnumerator() => exp_enums.GetEnumerator();
 
}

internal class TaskStatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.TaskStatus> status_enums =
       (Enum.GetValues(typeof(BO.TaskStatus)) as IEnumerable<BO.TaskStatus>)!;

    public IEnumerator GetEnumerator() => status_enums.GetEnumerator();

}
