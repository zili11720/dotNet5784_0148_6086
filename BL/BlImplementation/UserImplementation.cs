using BlApi;
using BO;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private readonly IBl _bl;
    internal UserImplementation(IBl bl) => _bl = bl;//Dependency injection

    public void Clear()
    {
        _dal.User.Clear();
    }

    public string Create(User item)
    {
        _bl.Agent.Read(item.UserId);//Check if the id belongs to an existing agent

        try
        {
            if (_dal.User.Read(item.UserId) is null)//Check if this agent stiil doesn't have a user
            {
                DO.User newDoUser = new DO.User(item.UserId, item.UserName, item.Password, item.IsManager);
                string userPassword = _dal.User.Create(newDoUser);
            }
            return item.Password;
        }
        catch (DO.DalAllreadyExistsException ex)
        {
            throw new BO.BlAllreadyExistsException($"A user with ID={item.UserId} allready exist", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.User.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"A user with ID={id} does not exist", ex);
        }
    }

    public User? Read(int id)
    {
        DO.User? doUser = _dal.User.Read(id);
        if (doUser is null)
            return null; // throw new BO.BlDoesNotExistException($"A user with id {id} does not exist");
        BO.User boUser = new BO.User()
        {
            UserId = doUser.UserId,
            UserName = doUser.UserName,
            Password = doUser.Password,
            IsManager = doUser.IsManager
        };

        return boUser;
    }
    public User? Read(string userName)
    {
        DO.User? doUser = _dal.User.Read(userName);
        if (doUser is null)
            throw new BO.BlDoesNotExistException($"A user with User name {userName} does not exist");

        BO.User boUser = new BO.User()
        {
            UserId = doUser.UserId,
            UserName = doUser.UserName,
            Password = doUser.Password,
            IsManager = doUser.IsManager
        };
        return boUser;
    }

    public void Update(User item)
    {
        DO.User newDoUser = new DO.User(item.UserId, item.UserName, item.Password, item.IsManager);
        try
        {
            _dal.User.Update(newDoUser);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"User with ID={item.UserId} deos not exist", ex);
        }
    }
}
