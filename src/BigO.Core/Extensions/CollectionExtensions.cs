﻿namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds a value to the collection if it does not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the value will be added.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>
    ///     <c>true</c> if the value was added to the collection;
    ///     <c>false</c> if the value already exists in the collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the collection is <c>null</c>.</exception>
    /// <remarks>
    ///     This method checks if the collection already contains the given value. If not, the value is added.
    ///     For collections that implement <see cref="HashSet{T}" />, this operation is more efficient as
    ///     <see cref="HashSet{T}.Add" /> is used, which already handles uniqueness checks.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int>();
    ///     bool added = numbers.AddUnique(1);
    ///     // added is true, numbers now contains 1
    ///     
    ///     added = numbers.AddUnique(1);
    ///     // added is false, numbers still contains only 1
    ///     ]]></code>
    /// </example>
    public static bool AddUnique<T>([NoEnumeration] this ICollection<T> collection, T value)
    {
        switch (collection)
        {
            case null:
                throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
            // If collection is a HashSet<T>, this will be more efficient.
            case HashSet<T> hashSet:
                return hashSet.Add(value);
        }

        if (collection.Contains(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }

    /// <summary>
    ///     Adds a range of unique values to the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection and enumerable.</typeparam>
    /// <param name="collection">The collection to which the values will be added.</param>
    /// <param name="values">The values to add to the collection.</param>
    /// <returns>
    ///     The count of values successfully added to the collection.
    ///     If the values are already present in the collection, they are not added, and hence not counted.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the collection is <c>null</c>. The method returns 0 if the values are <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method iterates over the provided values and adds each one to the collection if it does not already exist.
    ///     For collections that implement <see cref="HashSet{T}" />, this operation is more efficient.
    ///     For other types of collections, the method uses a temporary <see cref="HashSet{T}" /> to buffer the values,
    ///     which helps in avoiding duplicate entries.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int> { 1, 2 };
    ///     int addedCount = numbers.AddUniqueRange(new int[] { 2, 3, 4 });
    ///     // addedCount is 2, numbers now contains { 1, 2, 3, 4 }
    ///     ]]></code>
    /// </example>
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

        var counter = 0;

        // Special case for HashSet<T>
        if (collection is HashSet<T> hashSet)
        {
            counter += values.Count(hashSet.Add);
            return counter;
        }

        // Buffer values for other collections
        var valueSet = new HashSet<T>(values);

        foreach (var value in valueSet.Where(value => !collection.Contains(value)))
        {
            collection.Add(value);
            counter++;
        }

        return counter;
    }

    /// <summary>
    ///     Removes all the elements from a collection that match the conditions defined by the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which elements will be removed.</param>
    /// <param name="predicate">The delegate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of elements removed from the collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either the collection or the predicate is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method iterates through the collection and removes each element that matches the conditions
    ///     defined by the specified predicate. If the collection implements <see cref="List{T}" />,
    ///     <see cref="List{T}.RemoveAll" /> is used for a more efficient removal process.
    ///     For other types of collections, the method iterates through the collection in reverse order to remove elements.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
    ///     int removedCount = numbers.RemoveWhere(x => x % 2 == 0);
    ///     // removedCount is 2, numbers now contains { 1, 3, 5 }
    ///     ]]></code>
    /// </example>
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

        if (collection.Count == 0)
        {
            return 0;
        }

        if (collection is List<T> list)
        {
            return list.RemoveAll(predicate);
        }

        var count = 0;
        for (var i = collection.Count - 1; i >= 0; i--)
        {
            var item = collection.ElementAt(i);
            if (!predicate(item))
            {
                continue;
            }

            collection.Remove(item);
            count++;
        }

        return count;
    }

    /// <summary>
    ///     Adds an element to the collection if it satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the element will be added.</param>
    /// <param name="predicate">The predicate that determines if the element should be added.</param>
    /// <param name="value">The element to add to the collection.</param>
    /// <returns>
    ///     <c>true</c> if the element is added to the collection;
    ///     <c>false</c> if the element does not satisfy the predicate and is not added.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either the collection or the predicate is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks whether the provided value satisfies the predicate. If so, the value is added to the collection.
    ///     It's a convenient way to add elements conditionally without needing separate conditional logic outside the method
    ///     call.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int>();
    ///     bool added = numbers.AddIf(x => x > 0, 5);
    ///     // added is true, numbers now contains 5
    ///     
    ///     added = numbers.AddIf(x => x > 0, -1);
    ///     // added is false, numbers still contains only 5
    ///     ]]></code>
    /// </example>
    public static bool AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate), $"The {nameof(predicate)} cannot be null.");
        }

        if (!predicate(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }

    /// <summary>
    ///     Checks if the collection contains any of the specified elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for the presence of elements.</param>
    /// <param name="values">The elements to check in the collection.</param>
    /// <returns>
    ///     <c>true</c> if the collection contains any of the specified elements;
    ///     <c>false</c> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the collection is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method provides an efficient way to determine if a collection contains any elements from a given set.
    ///     For small sets of values (less than or equal to 5), it directly iterates through the values to check for their
    ///     presence.
    ///     For larger sets of values, it uses a <see cref="HashSet{T}" /> for more efficient lookups.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
    ///     bool containsAny = numbers.ContainsAny(6, 7, 8);
    ///     // containsAny is false
    ///     
    ///     containsAny = numbers.ContainsAny(3, 6, 9);
    ///     // containsAny is true
    ///     ]]></code>
    /// </example>
    public static bool ContainsAny<T>([NoEnumeration] this ICollection<T> collection, params T[] values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        // For small collections, direct check might be faster
        if (values.Length <= 5)
        {
            return values.Any(collection.Contains);
        }

        // For larger collections, using HashSet can be more efficient
        var valueSet = new HashSet<T>(values);
        return collection.Any(valueSet.Contains);
    }
}