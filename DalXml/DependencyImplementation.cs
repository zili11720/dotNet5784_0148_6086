using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;
/// <summary>
/// Implementation of the interface that manages a dependency between two tasks
///the interface contains the CRUD methods and execute them using xml files
/// </summary>
internal class DependencyImplementation:IDependency
{
    readonly string s_dependencies_xml = "dependencies";//Name of the file that contains all the dependencies

    /// <summary>
    /// Convert an XElememnt to a dependency
    /// </summary>
    /// <param name="elem">XElement to be converted</param>
    /// <returns>A new dependency</returns>
    /// <exception cref="FormatException">Failed to convert xml string to intger</exception>
    static Dependency getDependencyFromXElement(XElement elem)
    {
        return new Dependency()
        {
            Id = elem.ToIntNullable(nameof(Dependency.Id))?? throw new FormatException("Can't convert id"),
            DependentTask = elem.ToIntNullable(nameof(Dependency.DependentTask))?? throw new FormatException("Can't convert id"),
            DependsOnTask = elem.ToIntNullable(nameof(Dependency.DependsOnTask))?? throw new FormatException("Can't convert id")
        };
    }
    /// <summary>
    /// Convert a dependency to an XElememnt
    /// </summary>
    /// <param name="dep">The dependency to be converted</param>
    /// <returns>new XElement</returns>
    static XElement getXElementFromDependency(Dependency dep)
    {
        return new XElement("Dependency",
            new XElement("Id", dep.Id),
            new XElement("DependentTask", dep.DependentTask),
            new XElement("DependsOnTask", dep.DependsOnTask)
        );
    }


    /// <summary>
    /// Add a new dependency to the file
    /// </summary>
    /// <param name="item">A 'temporary' dependency with the details of the dependency that needs to be added</param>
    // <returns>The id of the new dependency</returns>
    public int Create(Dependency item)
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        int nextId = Config.NextDependencyId;
        Dependency newDep = item with { Id = nextId };
        dependencies.Add(getXElementFromDependency(newDep));
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);

        return nextId;
    }
     
    /// <summary>
    /// Delete the dependency with the id given from the file if it exists
    /// </summary>
    /// <param name="id">Id of the dependency that needs to be deleted</param>
    /// <exception cref="DalDoesNotExistException">A dependency with the given id does not exist in the file</exception>
    /// <summary>
    public void Delete(int id)
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? elemToDelete = dependencies.Elements().FirstOrDefault(dep => (int?)dep.Element("Id") == id);
        if (elemToDelete != null)
        {
            elemToDelete.Remove();
            XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
        }
        else
           throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
    }

    /// <summary>
    /// Return the dependency with the given id if it exists
    /// </summary>
    /// <param name="id">Id of the requested dependency</param>
    /// <returns>The requested dependency if it exists.else, a default value</returns>
    public Dependency? Read(int id)
    {
        XElement? dep = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        dep= dep.Elements().FirstOrDefault(dep=> (int?)dep.Element("Id")==id);
        return dep is null ? null : getDependencyFromXElement(dep);

    }
    /// <summary>
    /// Return the first dependency that meets the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>The requested dependency if it exists.else, a default value</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? dep=XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        return dep.Elements().Select(dep=> getDependencyFromXElement(dep)).FirstOrDefault(filter);
    }

    /// <summary>
    /// Return all the dependencies in the file that meet the condition of the method 'filter'
    /// </summary>
    /// <param name="filter">A boolien method</param>
    /// <returns>All the dependencies in the file that meet the condition</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {

        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        if(filter==null)
            return dependencies.Elements().Select(dep=> getDependencyFromXElement(dep));
        else
            return dependencies.Elements().Select(dep => getDependencyFromXElement(dep)).Where(filter);
    }
    /// <summary>
    /// Update a dependency with new information
    /// </summary>
    /// <param name="item">The updated dependency with the old id and updated details</param>
    /// <exception cref="DalDoesNotExistException">A dependency with the given id does not exist in the file</exception>
    public void Update(Dependency item)
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? tmpDep = XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().FirstOrDefault(dep => (int?)dep.Element("Id") == item.Id);
        if(tmpDep is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        tmpDep.Remove();
        dependencies.Add(item);
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
    }
    /// <summary>
    /// Clear the file completely from all the dependencies
    /// </summary>
    public void Clear()
    {
        XElement? dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        dependencies.RemoveAll();
        XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);

        Config.NextDependencyId = 1;//initialize id running number
    }
}




    