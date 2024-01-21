namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

/// <summary>
///Implementation of the interface that manages a dependency between two tasks
///the interface contains the CRUD methods
/// </summary>

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)//add a new dependency to the list
    {
        Dependency newDep = item with {Id= DataSource.Config.NextDependencyId}; 
        DataSource.Dependencies.Add(newDep);
        return newDep.Id;
    }
    public void Delete(int id)//Throw an exception, a dependency musn't be deleted
    {
        if (DataSource.Dependencies.RemoveAll(Dependency => Dependency.Id == id) == 0)
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
    }
    public Dependency? Read(int id)//Return the dependency with the id given if it exists
    {
        return DataSource.Dependencies.FirstOrDefault(Dependency=> Dependency.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)//Return the first dependency that satisfies the condition of the func filter
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)//return all the dependencies or just the dependencies which fulfiil the condition
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }

    public void Update(Dependency item)//Replace the dependency with the id identical to the id of the dependency 'item' with item
    {
        Dependency? existingItem=DataSource.Dependencies.Find(Dependency => Dependency.Id == item.Id);
        if (existingItem != null)
        {
            DataSource.Dependencies.Remove(existingItem); //Remove the dependency with this id from the list
            DataSource.Dependencies.Add(item);
        }
        else
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }
}
