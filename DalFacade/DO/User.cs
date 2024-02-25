namespace DO;
/// <summary>
/// A user entity (manager/employee)
/// </summary>
/// <param name="Name">The user's name</param>
/// <param name="Password">The user's password(id)</param>
/// <param name="IsManager">A boolien variable that indicats wether the user is a manager or an employee</param>
/// </summary>
public  record User
(
    string? Name=null,
    
    int? Password=null,

    bool IsManager=false
);
