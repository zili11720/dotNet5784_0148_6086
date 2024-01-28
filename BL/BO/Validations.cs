//namespace BlImplementation;
using BlApi;
namespace BO;

internal static class Validations
{

   internal static void IsGreaterThenZero(this int value)
    {
        if (value < 0)
            throw new FormatException("negetive nuber");
    } //Cost<=0
    internal static void IsEmptuString(this string st)
    {
        if (st == "")
            throw new FormatException("Missing a string");
    }
    internal static void IsNotEmail(this string email)
    {
        if (email!.Contains("@gmail.com") == false)
            throw new FormatException("worng email adress");
    }
    
}
