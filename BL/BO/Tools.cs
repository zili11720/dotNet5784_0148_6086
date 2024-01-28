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

    internal static TaskStatus CalcStatus(BO.Task boTask)
    {
        if (boTask.SchedualedDate == null)
            return TaskStatus.Unscheduled;
        if (DateTime.Today>boTask.SchedualedDate)
           return TaskStatus.Scheduled;
        //if(boTask)
            return TaskStatus.OnTrack;
        if (boTask.CompleteDate >= DateTime.Now)
            return TaskStatus.Done;
       

    }





}
