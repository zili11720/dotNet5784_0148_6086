namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
internal class UserImplementation : IUser
{
    public string Create(User item)
    {
        if (Read(item.UserId) is not null)//Check if this id already exists in the database
            throw new DalAllreadyExistsException($"A user with ID={item.UserId} already exists");
        DataSource.Users.Add(item);
        return item.Password;
    }
        
    public void Delete(int id)
    {
        if(DataSource.Users.RemoveAll(User => User.UserId == id) ==0)
            throw new DalDoesNotExistException($"A user with ID={id} does not exist");
    }

    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(User => User.UserId == id);
    }
    public User? Read(string userName)
    {
        return DataSource.Users.FirstOrDefault(User => User.UserName == userName);
    }

    public void Update(User item)
    {
        User? existingItem = DataSource.Users.Find(User => User.UserId == item.UserId);
        if (existingItem is not null)
        {
            DataSource.Users.Remove(existingItem);
            DataSource.Users.Add(item);
        }
        else
            throw new DalDoesNotExistException($"A user with ID={item.UserId} does not exist");
    }

    public void Clear()
    {
        DataSource.Users.Clear();
    }
}
