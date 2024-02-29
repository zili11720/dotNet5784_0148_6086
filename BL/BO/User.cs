using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// A user entity (manager/employee)
/// </summary>
/// <param name="UserId">The user's id</param>
/// <param name="Name">The user's name</param>
/// <param name="Password">The user's password</param>
/// <param name="IsManager">A boolien variable that indicats wether the user is a manager or an employee</param>
/// </summary>
public class User
{
    public int UserId;

    public string? UserName;

    public  string? Password;

    public bool IsManager = false;

    public override string ToString() => this.ToStringProperty();
}