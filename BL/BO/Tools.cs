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
            if (item.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) //if the item is a list type
            {
                ToStringProperty(value);
            }
            str +="/n"+item.Name + ":" + item.GetValue(t, null);//.tostring

        }
        return str;
   }

    internal static TaskStatus CalcStatus(DO.Task task)
    {
        if (task.SchedualedDate == null)
            return TaskStatus.Unscheduled;
        if (task.SchedualedDate!=null&& task.StartDate< DateTime.Now)
           return TaskStatus.Scheduled;
        if(task.StartDate>=DateTime.Now &&task.CompleteDate<DateTime.Now)
            return TaskStatus.OnTrack;
        if (task.CompleteDate >= DateTime.Now)
            return TaskStatus.Done;
        else
            throw new BlWrongDateOrderException("Task's dates are impossible");
    }





}
