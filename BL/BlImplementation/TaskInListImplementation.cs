
namespace BlImplementation;
using BlApi;

internal class TaskInListImplementation : ITaskInList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.TaskInList botaskList)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.TaskInList? Read(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.TaskInList botaskList)
    {
        throw new NotImplementedException();
    }
}
