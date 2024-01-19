using DalApi;
using DO;
namespace Dal;
/// <summary>
/// Implementation of the interface that manages a dependency between two tasks
///the interface contains the CRUD methods and execute them using xml files
/// </summary>
internal class DependencyImplementation:IDependency
{
    readonly string s_dependencies_xml = "dependencies";//Name of the file that contains all the dependencies

    /// <summary>
    /// Add a new dependency to the file
    /// </summary>
    /// <param name="item">A 'temporary' dependency with the details of the dependency that needs to be added</param>
    /// <returns>The id of the new dependency</returns>
    public int Create(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        int nextId = Config.NextDependencyId;//Get the next running number
        Dependency copy = item with { Id = nextId };
        Dependencies.Add(copy);
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);

        return nextId;
    }
    /// <summary>
    /// Delete the dependency with the id given from the file if it exists
    /// </summary>
    /// <param name="id">Id of the dependency that needs to be deleted</param>
    /// <exception cref="DalDoesNotExistException">A dependency with the given id does not exist in the file</exception>
    public void Delete(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        if (Dependencies.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);
    }
    /// <summary>
    /// Return the dependency with the given id if it exists
    /// </summary>
    /// <param name="id">Id of the requested dependency</param>
    /// <returns>The requested dependency if it exists.else, a default value</returns>
    public Dependency? Read(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return Dependencies.FirstOrDefault(it => it.Id == id);
    }
    /// <summary>
    /// Return the first dependency that meets the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>The requested dependency if it exists.else, a default value</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return Dependencies.FirstOrDefault(filter);
    }
    /// <summary>
    /// Return all the dependencies in the file that meet the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>All the dependencies in the file that meet the condition</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        if (filter == null)
            return Dependencies.Select(item => item);
        else
            return Dependencies.Where(filter);
    }
    /// <summary>
    /// Update a dependency with new information
    /// </summary>
    /// <param name="item">The updated dependency with the old id and updated details</param>
    /// <exception cref="DalDoesNotExistException">A dependency with the given id does not exist in the file</exception>
    public void Update(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        if (Dependencies.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        Dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);
    }
    /// <summary>
    /// Clear the file completely from all the dependencies
    /// </summary>
    public void Clear()
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        Dependencies.Clear();//Earase all the dependencies in the list
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);//Write the updated list to the xml file
    }
}
