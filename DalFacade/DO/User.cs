namespace DO;
/// <summary>
/// A user entity (manager/employee)
/// </summary>
/// <param name="UserId">The user's id</param>
/// <param name="Name">The user's name</param>
/// <param name="Password">The user's password</param>
/// <param name="IsManager">A boolien variable that indicats wether the user is a manager or an employee</param>
/// </summary>
public record User
(
    int UserId ,

    string UserName,
    
    string Password,

    bool IsManager=false
);
