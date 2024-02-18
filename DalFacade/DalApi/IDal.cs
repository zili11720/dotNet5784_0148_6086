using System;
using System.Linq;

namespace DalApi;

public interface IDal
{
    IAgent Agent { get; }
    ITask Task { get; }
    IDependency Dependency { get; }

    public DateTime? StartProjectDate { get; set; }//Start date of the project
    public DateTime? EndProjectDate { get; set; }//End date of the project

}

