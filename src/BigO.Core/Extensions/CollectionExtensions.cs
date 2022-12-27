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
    ///     Adds the specified value to the collection if it does not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the value to. Cannot be <c>null</c>.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>
    ///     <c>true</c> if the value was added to the collection, <c>false</c> if the value already exists in the
    ///     collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the <see cref="ICollection{T}" /> is read-only or if the
    ///     <see cref="ICollection{T}" /> has a fixed size.
    /// </exception>
    /// <remarks>
    ///     This method adds the specified value to the collection if it does not already exist in the collection using the
    ///     <see cref="ICollection{T}.Contains" /> method.
    ///     If the collection is empty, it adds the value to the collection and returns <c>true</c>.
    ///     If the collection is not empty and does not contain the value, it adds the value to the collection and returns
    ///     <c>true</c>.
    ///     If the collection is not empty and contains the value, it does not add the value to the collection and returns
    ///     <c>false</c>.
    /// </remarks>
    public static bool AddUnique<T>([NoEnumeration] this ICollection<T> collection, T value)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (collection.Count == 0)
        {
            collection.Add(value);
            return true;
        }

        if (collection.Contains(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }

    /// <summary>
    ///     Adds a range of values to the collection if they do not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the values to. Cannot be <c>null</c>.</param>
    /// <param name="values">The values to add to the collection. Can be <c>null</c>.</param>
    /// <returns>The number of values added to the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the <see cref="ICollection{T}" /> is read-only or if the
    ///     <see cref="ICollection{T}" /> has a fixed size.
    /// </exception>
    /// <remarks>
    ///     This method adds a range of values to the collection if they do not already exist in the collection using the
    ///     <see cref="AddUnique{T}" /> method.
    ///     If the <paramref name="collection" /> is <c>null</c>, it returns 0.
    ///     If the <paramref name="collection" /> is not <c>null</c> and the values are <c>null</c>, it returns 0.
    ///     If the <paramref name="collection" /> is not <c>null</c> and the values are not <c>null</c>, it adds the values to
    ///     the collection and returns the number of values added.
    /// </remarks>
    public static int AddUniqueRange<T>([NoEnumeration] this ICollection<T> collection, IEnumerable<T>? values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (values == null)
        {
            return 0;
        }

        var internalValues = values.ToList();

        return !internalValues.Any() ? 0 : internalValues.Count(collection.AddUnique);
    }

    /// <summary>
    ///     Removes all elements that match the conditions defined by the specified predicate from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove the elements from. Cannot be <c>null</c>.</param>
    /// <param name="predicate">The delegate that defines the conditions of the elements to remove. Cannot be <c>null</c>.</param>
    /// <returns>The number of elements removed from the collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> or <paramref name="predicate" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the <see cref="ICollection{T}" /> is read-only or if the
    ///     <see cref="ICollection{T}" /> has a fixed size.
    /// </exception>
    /// <remarks>
    ///     This method removes all elements that match the conditions defined by the specified <paramref name="predicate" />
    ///     from the <paramref name="collection" />.
    ///     If the <paramref name="collection" /> is <c>null</c> or empty, it returns 0.
    ///     If the <paramref name="collection" /> is not empty and the <paramref name="predicate" /> is <c>null</c>, it throws
    ///     an <see cref="ArgumentNullException" />.
    ///     If the <paramref name="collection" /> is not empty and the <paramref name="predicate" /> is not <c>null</c>, it
    ///     removes all elements that match the conditions defined by the predicate.
    ///     If the <paramref name="collection" /> is a <see cref="List{T}" />, it uses the <see cref="List{T}.RemoveAll" />
    ///     method to remove the elements.
    ///     If the <paramref name="collection" /> is not a <see cref="List{T}" />, it iterates through the collection, adds the
    ///     elements that match the predicate to a delete list, and then removes the elements from the delete list from the
    ///     collection.
    /// </remarks>
    public static int RemoveWhere<T>([NoEnumeration] this ICollection<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), $"The {nameof(predicate)} cannot be null.");
        }

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
    ///     Adds the value to the collection if the predicate is true.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the value to. Cannot be <c>null</c>.</param>
    /// <param name="predicate">
    ///     The delegate that defines the condition for adding the value to the collection. Cannot be
    ///     <c>null</c>.
    /// </param>
    /// <param name="value">The value to add to the collection if the predicate is true.</param>
    /// <returns>True if the value was added to the collection, false if not.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> or <paramref name="predicate" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the <see cref="ICollection{T}" /> is read-only or if the
    ///     <see cref="ICollection{T}" /> has a fixed size.
    /// </exception>
    /// <remarks>
    ///     This method adds the value to the collection if the predicate is true.
    ///     If the collection is <c>null</c>, it throws an <see cref="ArgumentNullException" />.
    ///     If the predicate is <c>null</c>, it throws an <see cref="ArgumentNullException" />.
    ///     If the predicate is not <c>null</c>, it adds the value to the collection if the predicate is true, and returns
    ///     true.
    ///     If the predicate is not <c>null</c> and is not true, it returns false.
    /// </remarks>
    public static bool AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), $"The {nameof(predicate)} cannot be null.");
        }

        // ReSharper disable once InvertIf
        if (predicate(value))
        {
            collection.Add(value);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the collection contains any of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for the presence of the values in. Cannot be <c>null</c>.</param>
    /// <param name="values">The values to check for in the collection.</param>
    /// <returns>True if the collection contains any of the values, false if not.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method determines whether the collection contains any of the specified values.
    ///     If the collection is <c>null</c>, it throws an <see cref="ArgumentNullException" />.
    ///     If the collection is not <c>null</c>, it checks if the collection is empty. If it is empty, it returns false.
    ///     If the collection is not empty, it checks if any of the values are contained in the collection using the
    ///     <see cref="ICollection{T}.Contains" /> method.
    ///     If any of the values are contained in the collection, it returns true.
    ///     If none of the values are contained in the collection, it returns false.
    /// </remarks>
    public static bool ContainsAny<T>(this ICollection<T> collection, params T[] values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        return !collection.IsEmpty() && values.Any(collection.Contains);
    }
}