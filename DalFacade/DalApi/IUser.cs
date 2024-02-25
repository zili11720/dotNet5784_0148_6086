namespace DalApi;
using DO;
/// <summary>
///Represents an interface for managing a user entity
/// </summary>
public interface IUser
{
    void Create(Agent item);
    void Delete(int id);
    void Update(Agent item);

}
