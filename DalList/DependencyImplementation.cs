
namespace Dal;
using DalApi;
using DO;

using System.Collections.Generic;
/// <summary>
///Implementation of the interface that manages a dependency betweem two tasks
///the interface contains the CRUD methods
/// </summary>

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)//add a new dependency to the list
    {
        Dependency newDep = item with {Id= DataSource.Config.NextDependencyId}; 
        DataSource.Dependencies.Add(newDep);
        return newDep.Id;
    }
    public void Delete(int id)//Throw an exception, a dependency musn't be deleted
    {
        throw new Exception($"Dependency with ID={id} does Not exist");
    }

    public Dependency? Read(int id)//Return the dependency with the id given if it exists
    {
        return DataSource.Dependencies.Find(Dependency => Dependency.Id == id);
    }

    public List<Dependency> ReadAll()//return all the depencies in the list
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)//Replace the dependency with the id identical to the id of the dependency 'item' with item
    {
        Dependency? existingItem=DataSource.Dependencies.Find(Dependency => Dependency.Id == item.Id);
        if (existingItem != null)
        {
            DataSource.Dependencies.Remove(existingItem); //remove the dependency with this id from the list
            DataSource.Dependencies.Add(item);
        }
        else
            throw new Exception($"Dependency with ID={item.Id} does Not exist");
    }
}
