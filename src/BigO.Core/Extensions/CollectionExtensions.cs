using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds an item to the <paramref name="collection" /> if it doesn't already exist.
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="collection" />.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> instance.</param>
    /// <param name="value">The <paramref name="value" /> to be added.</param>
    /// <returns><c>true</c> if the <paramref name="value" /> was added, otherwise <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="collection" /> is read-only.</exception>
    public static bool AddUnique<T>(this ICollection<T> collection, T value)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count != 0 && collection.Contains(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }

    /// <summary>
    ///     Adds items to the <paramref name="collection" />. Only items that do not exist are added to the list.
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="collection" />.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> instance.</param>
    /// <param name="values">The <paramref name="values" /> to be added.</param>
    /// <returns>The number of unique items added.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="collection" /> is read-only.</exception>
    public static int AddUniqueRange<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var numbersOfItemsAdded = 0;

        if (values == null || values.IsEmpty())
        {
            return numbersOfItemsAdded;
        }

        return values.Count(collection.AddUnique);
    }

    /// <summary>
    ///     Removes items from the <paramref name="collection" /> where the child item satisfies the passed in
    ///     <paramref name="predicate" />.
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="collection" />.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> instance.</param>
    /// <param name="predicate">The <see cref="Predicate{T}" /> object used to filter the items to be removed.</param>
    /// <returns>The number of items removed.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="collection" /> or <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    public static int RemoveWhere<T>([NoEnumeration] this ICollection<T> collection, Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        if (collection.IsEmpty())
        {
            return 0;
        }

        if (collection is List<T> list)
        {
            return list.RemoveAll(predicate);
        }

        var deleteList = new Collection<T>();

        foreach (var item in collection)
        {
            if (predicate(item))
            {
                deleteList.Add(item);
            }
        }

        var count = deleteList.Count;

        foreach (var item in deleteList)
        {
            collection.Remove(item);
        }

        return count;
    }

    /// <summary>
    ///     Adds an item to the <paramref name="collection" /> if the item satisfies the passed in
    ///     <paramref name="predicate" />
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="collection" />.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> instance.</param>
    /// <param name="predicate">The <see cref="Predicate{T}" /> object used to filter the item to be added.</param>
    /// <param name="value">The value to be checked against the predicate.</param>
    /// <returns><c>true</c> if the <paramref name="value" /> is added, otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="collection" /> or <paramref name="predicate" />
    ///     is is <c>null</c>.
    /// </exception>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="collection" /> is read-only.</exception>
    public static bool AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        // ReSharper disable once InvertIf
        if (predicate(value))
        {
            collection.Add(value);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether any item in <paramref name="values" /> exists in the <paramref name="collection" />.
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="collection" />.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> instance.</param>
    /// <param name="values">A variable-length parameters list containing value to be checked.</param>
    /// <returns>
    ///     <c>true</c> is any of the items in <paramref name="values" /> exists in the <paramref name="collection" />,
    ///     otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="collection" /> is <c>null</c>.
    /// </exception>
    public static bool ContainsAny<T>(this ICollection<T> collection, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return !collection.IsEmpty() && values.Any(collection.Contains);
    }
}