using System;
using System.Linq;

namespace DalApi;

public interface IDal
{
    IAgent Agent { get; }
    ITask Task { get; }
    IDependency Dependency { get; }


}

