using System.Collections.ObjectModel;

namespace MassRename.ViewModels.Extensions;


/// <summary>
///     Provides extension methods for collection types
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds a collection of items to an <see cref="ObservableCollection{T}"/>
    /// </summary>
    /// <param name="observableCollection">The <see cref="ObservableCollection{T}"/> to add the items to</param>
    /// <param name="items">The items to be added</param>
    /// <typeparam name="T">The <see cref="ObservableCollection{T}"/> and <see cref="ICollection{T}"/>'s type</typeparam>
    public static void AddRange<T>(this ObservableCollection<T> observableCollection, ICollection<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        foreach (var item in items)
        {
            observableCollection.Add(item);
        }
    }
    
}