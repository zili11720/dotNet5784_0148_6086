
namespace BlApi;
/// <summary>
/// Factory class allows Pl to create Bl objects 
/// without knowing implementation datails
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}
