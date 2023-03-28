using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Adds a value to a collection only if it does not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <remarks>
    ///     This method is an extension method for ICollection types. It adds a value to the collection only if it does not
    ///     already exist in the collection.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     var collection = new List<int>() { 1, 2, 3 };
    ///     collection.AddUnique(4);
    ///     collection.AddUnique(3);
    ///     Console.WriteLine(string.Join(",", collection));  // Output: "1,2,3,4"
    /// ]]></code>
    /// </example>
    /// <param name="collection">The collection to add the value to.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>True if the value was added to the collection; otherwise, false.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when collection is null.</exception>
    public static bool AddUnique<T>([NoEnumeration] this ICollection<T> collection, T value)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        if (collection.Contains(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }


    /// <summary>
    ///     Adds a range of values to a collection, only adding values that do not already exist in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <remarks>
    ///     This method is an extension method for ICollection types. It adds a range of values to the collection, only adding
    ///     values that do not already exist in the collection.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     var collection = new List<int>() { 1, 2, 3 };
    ///     var valuesToAdd = new List<int>() { 2, 3, 4 };
    ///     int count = collection.AddUniqueRange(valuesToAdd);
    ///     Console.WriteLine(string.Join(",", collection));  // Output: "1,2,3,4"
    ///     Console.WriteLine(count);  // Output: 1
    /// ]]></code>
    /// </example>
    /// <param name="collection">The collection to add the values to.</param>
    /// <param name="values">The values to add to the collection.</param>
    /// <returns>The number of values that were added to the collection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when collection is null.</exception>
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

        foreach (var value in values)
        {
            if (collection.Contains(value))
            {
                continue;
            }

            collection.Add(value);
            counter++;
        }

        return counter;
    }

    /// <summary>
    ///     Removes all the elements that match the conditions defined by the specified predicate from the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove elements from.</param>
    /// <param name="predicate">The predicate used to determine if an element should be removed.</param>
    /// <returns>The number of elements removed from the collection.</returns>
    /// <remarks>
    ///     This method will remove all the elements from the collection that match the specified <paramref name="predicate" />
    ///     .
    ///     The elements are removed in reverse order to avoid invalidating the index of the remaining elements. If the
    ///     <paramref name="collection" />
    ///     is a <see cref="List{T}" />, then the <see cref="List{T}.RemoveAll" /> method is used instead to improve
    ///     performance.
    /// </remarks>
    /// <example>
    ///     The following code example removes all the negative integers from a list:
    ///     <code><![CDATA[
    /// var list = new List<int> { 1, -2, 3, -4, 5 };
    /// int count = list.RemoveWhere(i => i < 0);
    /// Console.WriteLine($"Removed {count} elements from the list.");
    /// foreach (int i in list)
    /// {
    ///     Console.WriteLine(i);
    /// }
    /// // Output:
    /// // Removed 2 elements from the list.
    /// // 1
    /// // 3
    /// // 5
    /// ]]></code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="collection" /> or <paramref name="predicate" /> is null.
    /// </exception>
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
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to add the element to.</param>
    /// <param name="predicate">The predicate used to determine if the element should be added.</param>
    /// <param name="value">The value to add to the collection.</param>
    /// <returns>True if the element was added to the collection, otherwise false.</returns>
    /// <remarks>
    ///     This method will add the specified <paramref name="value" /> to the <paramref name="collection" /> if it satisfies
    ///     the specified <paramref name="predicate" />.
    ///     If the <paramref name="collection" /> is null, an <see cref="ArgumentNullException" /> is thrown.
    ///     If the <paramref name="predicate" /> is null, an <see cref="ArgumentNullException" /> is thrown.
    /// </remarks>
    /// <example>
    ///     The following code example adds a string to a list if it starts with the letter 'A':
    ///     <code><![CDATA[
    /// var list = new List<string> { "Apple", "Banana", "Cherry" };
    /// bool added = list.AddIf(s => s.StartsWith("A"), "Apricot");
    /// Console.WriteLine(added); // True
    /// foreach (string s in list)
    /// {
    ///     Console.WriteLine(s);
    /// }
    /// // Output:
    /// // Apple
    /// // Banana
    /// // Cherry
    /// // Apricot
    /// ]]></code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="collection" /> or <paramref name="predicate" /> is null.
    /// </exception>
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
    /// <param name="collection">The collection to search for the values.</param>
    /// <param name="values">The values to search for in the collection.</param>
    /// <returns>True if any of the specified values are found in the collection, otherwise false.</returns>
    /// <remarks>
    ///     This method will search the <paramref name="collection" /> for any of the specified <paramref name="values" />.
    ///     If the <paramref name="collection" /> is null, an <see cref="ArgumentNullException" /> is thrown.
    /// </remarks>
    /// <example>
    ///     The following code example checks if a list contains any of the specified integers:
    ///     <code><![CDATA[
    /// var list = new List<int> { 1, 2, 3 };
    /// bool containsAny = list.ContainsAny(3, 4, 5);
    /// Console.WriteLine(containsAny); // True
    /// ]]></code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="collection" /> is null.
    /// </exception>
    public static bool ContainsAny<T>(this ICollection<T> collection, params T[] values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        return !collection.IsEmpty() && values.Any(collection.Contains);
    }
}