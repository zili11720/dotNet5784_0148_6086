using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IUser
{

    string Create(User item);
    void Delete(int id);
    User? Read(int id);
    User? Read(string userName);
    void Update(User item);
    void Clear();
}

