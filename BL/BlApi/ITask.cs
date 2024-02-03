﻿namespace BlApi;
/// <summary>
/// Interface of a task
/// All availabe operations for a logic task 
/// </summary>
public interface ITask
{
    int Create(BO.Task boTask);
    void Delete(int id);
    BO.Task? Read(int id);
    IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null);
    void Update(BO.Task boTask);
    void UpdateScheduledStartDate(int taskId, DateTime? start);

    //להוסיף הקצאת משימה לסוכן כדי להשתמש בתוך העדכון של משימה לשדה סוכן
}
