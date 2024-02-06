using System.Collections;
using System.Reflection;

namespace BO;
/// <summary>                                   
/// Various auxiliary functions for Bl 
/// </summary>
static internal class Tools
{
    internal static string ToStringProperty<T>(this T t)
    {
        var type = t!.GetType();
        string str = type.Name + Environment.NewLine;
        foreach (PropertyInfo item in type.GetProperties())
        {
            var value = item.GetValue(t);

            if (value is IEnumerable enumerable and not string)
                str += Environment.NewLine + item.Name + ":" + Environment.NewLine + string.Join(Environment.NewLine,
                    enumerable.Cast<object>().Select(o => o));
            else
                str += item.Name + ":" + item.GetValue(t, null) + "\n";//.tostring
        }
        return str;
    }
}