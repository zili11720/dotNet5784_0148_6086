using System.Collections.ObjectModel;

namespace PL.Tools;

internal static class ToObservableCollectionExtansion
{
    internal static ObservableCollection<Item> ToObservableCollection<Item>(this IEnumerable<Item> items) 
        => new ObservableCollection<Item>(items);
}
