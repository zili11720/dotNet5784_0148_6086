
using DalApi;
namespace Dal;
sealed public class DalXml : IDal
{
    public IAgent Agent =>  new AgentImplementation();

    public ITask Task =>  new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}
