using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace PL.Tools;

internal static class ToObservableCollectionExtansion
{
    internal static ObservableCollection<Item> ToObservableCollection<Item>(this IEnumerable<Item> items) 
        => new ObservableCollection<Item>(items);
}
