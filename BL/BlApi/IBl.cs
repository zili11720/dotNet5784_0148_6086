namespace BlApi;
/// <summary>
/// Interface of IBl
/// Access to all the logic interfaces in Bl
/// </summary>
public interface IBl
{
    IAgent Agent { get; }
    ITask Task { get; }
    void SetProjectStartDate();
    void ResetData();
    void InitializeData();
    BO.ProjectStatus GetProjectStatus();

}
