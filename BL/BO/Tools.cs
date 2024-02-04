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
        string str = "";
        foreach(PropertyInfo item in t!.GetType().GetProperties())
        {  
            var value = item.GetValue(t);

           //if (item.PropertyType.IsGenericType &&
           //item.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
           // {
           //     // If the property is a generic list type, call ToStringProperty recursively
           //     str += $"\n{item.Name}: {ToStringProperty(value)}";
           // }
           // //if (item.PropertyType() == typeof(IEnumerable<>)) //if the item is a list type
            //{
            //    ToStringProperty(value);
            //}
            str +="\n"+item.Name + ":" + item.GetValue(t, null);//.tostring
        }
        return str;
   }

    internal static TaskStatus CalcStatus(this DO.Task task)
    {
        if (task.ScheduledDate == null)
            return TaskStatus.Unscheduled;
        if (task.ScheduledDate != null && task.StartDate < DateTime.Now|| task.StartDate==null)
            return TaskStatus.Scheduled;
        if (task.StartDate >= DateTime.Now && task.CompleteDate < DateTime.Now||task.CompleteDate==null)
            return TaskStatus.OnTrack;
        if (task.CompleteDate >= DateTime.Now)
            return TaskStatus.Done;
        else
            throw new BlWrongDateException("Task's dates are impossible");
    }
}