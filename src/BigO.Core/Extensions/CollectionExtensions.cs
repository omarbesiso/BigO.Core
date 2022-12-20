using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds the given value to the given collection if it is not already present.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the value to.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>True if the value was added to the collection, false if it was already present.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method adds the given value to the given collection if it is not already present. It uses the
    ///     <see cref="ICollection{T}.Contains(T)" /> method to check if the value is already present in the collection. If the
    ///     value is not present, it adds the value using the <see cref="ICollection{T}.Add(T)" /> method.
    /// </remarks>
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
    ///     Adds a range of values to the given collection if they are not already present.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the values to.</param>
    /// <param name="values">The values to add to the collection.</param>
    /// <returns>The number of values added to the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method adds a range of values to the given collection if they are not already present.
    ///     It uses the <see cref="ICollection{T}.Contains(T)" /> method to check if each value is already present in the
    ///     collection.
    ///     If a value is not present, it adds the value using the <see cref="ICollection{T}.Add(T)" /> method.
    ///     The method returns the number of values added to the collection.
    /// </remarks>
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
    ///     Removes all elements that match the conditions defined by the specified predicate from the given collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove the elements from.</param>
    /// <param name="predicate">The delegate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of elements removed from the collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> or <paramref name="predicate" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method removes all elements that match the conditions defined by the specified predicate from the given
    ///     collection.
    ///     If the collection is a <see cref="List{T}" />, it uses the <see cref="List{T}.RemoveAll(Predicate{T})" /> method to
    ///     remove the elements.
    ///     Otherwise, it iterates over the elements in the collection and adds the elements that match the predicate to a
    ///     temporary collection.
    ///     Then it removes the elements in the temporary collection from the original collection.
    ///     The method returns the number of elements removed from the collection.
    /// </remarks>
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
    ///     Adds the given value to the given collection if it meets the conditions defined by the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the value to.</param>
    /// <param name="predicate">The delegate that defines the conditions the value must meet to be added to the collection.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>
    ///     True if the value was added to the collection, false if it did not meet the conditions defined by the
    ///     predicate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> or <paramref name="predicate" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method adds the given value to the given collection if it meets the conditions defined by the specified
    ///     predicate.
    ///     If the value meets the conditions, it adds the value using the <see cref="ICollection{T}.Add(T)" /> method.
    ///     The method returns true if the value was added to the collection, false if it did not meet the conditions defined
    ///     by the predicate.
    /// </remarks>
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
    ///     Determines whether the given collection contains any of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to search for the values.</param>
    /// <param name="values">The values to search for in the collection.</param>
    /// <returns>True if the collection contains any of the specified values, false if it does not.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method determines whether the given collection contains any of the specified values.
    ///     It uses the <see cref="ICollection{T}.Contains(T)" /> method to check if the collection contains each value.
    ///     If the collection is empty, it returns false.
    /// </remarks>
    public static bool ContainsAny<T>(this ICollection<T> collection, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(collection);
        return !collection.IsEmpty() && values.Any(collection.Contains);
    }
}