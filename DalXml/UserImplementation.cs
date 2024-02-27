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

    public string Create(User item)
    {
        if (Read(item.UserId) is null)
            throw new DalDoesNotExistException($"An agent with ID={item.UserId} deosn't exist");

        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);

        return item.Password;
    }

    public void Delete(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        if (users.RemoveAll(it => it.UserId == id) == 0)
            throw new DalDoesNotExistException($"A user with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }

    public User? Read(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(it => it.UserId == id);
    }

    public void Update(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        if (users.RemoveAll(it => it.UserId == item.UserId) == 0)
            throw new DalDoesNotExistException($"A user with ID={item.UserId} does Not exist");
        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }

    public void Clear()
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.Clear();//Earase all the users in the list
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);//Write the empty list to the xml file
    }
}