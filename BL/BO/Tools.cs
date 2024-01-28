using System.Reflection;

namespace BO;
/// <summary>
/// Various auxiliary functions for Bl 
/// </summary>
static internal class Tools
{

   internal static string ToStringProperty<T>(this T t)
   {
        string str = "";
        foreach(PropertyInfo item in t!.GetType().GetProperties())
        {  ///////להוסיף אופציה שהשדה יהיה רשימה/tuple
            str +="/n"+item.Name + ":" + item.GetValue(t, null);
        }
        return str;
   }





}
