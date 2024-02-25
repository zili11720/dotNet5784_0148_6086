using DalApi;
using DO;
namespace Dal;
/// <summary>
///Implementation of the interface that manages a user entity
///The interface contains the CRUD methods and executes them using xml files
/// </summary>
internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";//Name of the file that contains all the users
    public void Create(Agent item)
    {
        throw new NotImplementedException();
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Agent item)
    {
        throw new NotImplementedException();
    }
}
