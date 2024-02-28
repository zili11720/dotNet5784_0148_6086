namespace DalApi;
using DO;
/// <summary>
///Represents an interface for managing a user entity
/// </summary>
public interface IUser
{
    string Create(User item);
    void Delete(int id);
    User? Read(int id);
    User? Read(string UserName);
    void Update(User item);
    void Clear();
}
