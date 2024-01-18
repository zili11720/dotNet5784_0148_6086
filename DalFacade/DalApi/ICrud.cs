namespace DalApi;

using DO;

/// <summary>
///Represents an interface for managing the enteties:agent,task and dependency
///the interface contains the CRUD methods
/// </summary>

public interface ICrud<T> where T : class 
{
    int Create(T item); //Create a new T
    T? Read(int id); //Read a T by its ID 
    T? Read(Func<T, bool> filter);//Return the first T that satisfies the condition of the func filter
    IEnumerable<T?> ReadAll(Func<T,bool>? filter = null); //Read all T
    void Update(T item); //Update a T
    void Delete(int id); //Delete a T by its Id
    void Clear();
}



