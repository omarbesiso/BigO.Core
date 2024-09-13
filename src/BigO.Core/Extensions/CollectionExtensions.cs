using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Shuffles the elements of the specified list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    /// <param name="preserveOriginal">
    ///     Specifies whether to preserve the original list. If <c>true</c>, the shuffle is performed on a copy of the list;
    ///     otherwise, the shuffle is performed on the original list. Defaults to <c>false</c>.
    /// </param>
    /// <returns>
    ///     A shuffled list. This can be either a new list if <paramref name="preserveOriginal" /> is true, or the
    ///     original list otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the input list is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the Fisher–Yates shuffle algorithm for an efficient and unbiased shuffle.
    ///     The algorithm has a time complexity of O(n), where n is the number of elements.
    ///     When <paramref name="preserveOriginal" /> is set to <c>true</c>, a copy of the list is made to ensure that the
    ///     original
    ///     list's order is not altered.
    ///     The shuffle operation uses <see cref="Random.Shared" />, which is suitable for general-purpose use but not for
    ///     cryptographic purposes.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// IList<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
    /// IList<int> shuffledNumbers = numbers.Shuffle();
    /// IList<int> originalPreservedShuffle = numbers.Shuffle(true);
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<T> Shuffle<T>(this IList<T> list, bool preserveOriginal = false)
    {
        Guard.NotNull(list);

        var shuffledList = preserveOriginal ? new List<T>(list) : list;

        if (shuffledList.Count <= 1)
        {
            return shuffledList;
        }

        for (var i = shuffledList.Count - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(0, i + 1);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList;
    }

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
        Guard.NotNull(collection);

        if (collection is HashSet<T> hashSet)
        {
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
    ///     If the provided values are already a <see cref="HashSet{T}" />, the method uses it directly for better performance.
    ///     Otherwise, for other types of collections, the method uses a temporary <see cref="HashSet{T}" /> to buffer the
    ///     values,
    ///     which helps in avoiding duplicate entries.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int> { 1, 2 };
    ///     int addedCount = numbers.AddUniqueRange(new int[] { 2, 3, 4 });
    ///     // addedCount is 2, numbers now contains { 1, 2, 3, 4 }
    /// 
    ///     // Using HashSet<T>
    ///     HashSet<int> moreNumbers = new HashSet<int> { 5, 6 };
    ///     addedCount = numbers.AddUniqueRange(moreNumbers);
    ///     // addedCount is 2, numbers now contains { 1, 2, 3, 4, 5, 6 }
    ///     ]]></code>
    /// </example>
    public static int AddUniqueRange<T>(this ICollection<T> collection, IEnumerable<T>? values)
    {
        Guard.NotNull(collection);

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

        // Check if values is already a HashSet<T>
        if (values is HashSet<T> valueHashSet)
        {
            foreach (var value in valueHashSet.Where(value => !collection.Contains(value)))
            {
                collection.Add(value);
                counter++;
            }
        }
        else
        {
            // Buffer values for other collections using a HashSet<T>
            var valueSet = new HashSet<T>(values);
            foreach (var value in valueSet.Where(value => !collection.Contains(value)))
            {
                collection.Add(value);
                counter++;
            }
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
        Guard.NotNull(collection);
        Guard.NotNull(predicate);

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
        Guard.NotNull(collection);
        Guard.NotNull(predicate);

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
    ///     For small sets of values (less than or equal to 10), it directly iterates through the values to check for their
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
        Guard.NotNull(collection);

        // For small collections, direct check might be faster
        if (values.Length <= 10)
        {
            return values.Any(collection.Contains);
        }

        // For larger collections, using HashSet can be more efficient
        var valueSet = new HashSet<T>(values);
        return collection.Any(valueSet.Contains);
    }
}