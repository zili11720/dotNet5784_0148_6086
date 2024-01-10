using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;
public interface ICrud<T> where T : class 
{
    int Create(T item); //Create a new T
    T? Read(int id); //Read a T by its ID 
    List<T> ReadAll(); //Read all T
    void Update(T item); //Update a T
    void Delete(int id); //Delete a T by its Id
}



