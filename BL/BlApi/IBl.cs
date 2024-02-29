namespace BlApi;
/// <summary>
/// Interface of IBl
/// Access to all the logic interfaces in Bl
/// </summary>
public interface IBl
{
    IAgent Agent { get; }
    ITask Task { get; }
    IUser User { get; }
    void SetProjectStartDate();
    void ResetData();
    void InitializeData();
    BO.ProjectStatus GetProjectStatus();
    #region
    DateTime Clock { get; }
    DateTime updateYear();
    DateTime updateDay();
    DateTime updateHour();
    DateTime ResetClock();
    #endregion


}
