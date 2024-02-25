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
    public void Create(Agent item)
    {
        User user = new User(item.Name, item.Id);
        DataSource.Users.Add(user);
    }

    public void Delete(int id)
    {
        DataSource.Users.RemoveAll(User => User.Password == id);
    }

    void IUser.Update(Agent item)
    {
        
    }
}
