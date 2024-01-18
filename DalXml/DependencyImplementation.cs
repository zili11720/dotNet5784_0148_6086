using DalApi;
using DO;
namespace Dal;
internal class DependencyImplementation:IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        int nextId = Config.NextDependencyId;
        Dependency copy = item with { Id = nextId };
        Dependencies.Add(copy);
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);

        return nextId;
    }

    public void Delete(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        if (Dependencies.RemoveAll(it => it.Id == id) == 0)//Delete the dependency with the requested id if it exists
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);//Write the updated list to the xml file
    }

    public Dependency? Read(int id)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        return Dependencies.FirstOrDefault(it => it.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        return Dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        if (filter == null)
            return Dependencies.Select(item => item);
        else
            return Dependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        if (Dependencies.RemoveAll(it => it.Id == item.Id) == 0)//Remove the old dependency if it exists
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        Dependencies.Add(item);//Add the updated dependency to the list
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);//Write the updated list to the xml file
    }

    public void Clear()
    {
        List<Dependency> Dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);//Load the list from the xml file
        Dependencies.Clear();//Earase all the dependencies in the list
        XMLTools.SaveListToXMLSerializer(Dependencies, s_dependencies_xml);//Write the updated list to the xml file
    }
}
