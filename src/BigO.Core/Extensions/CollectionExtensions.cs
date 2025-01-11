using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="ICollection{T}" /> objects.
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    ///     Shuffles the elements of the specified list using the Fisher–Yates shuffle algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    /// <param name="preserveOriginal">
    ///     Specifies whether to preserve the original list. If <c>true</c>, the shuffle is performed on a copy of the list;
    ///     otherwise, the shuffle is performed on the original list. Defaults to <c>false</c>.
    /// </param>
    /// <param name="random">
    ///     An instance of <see cref="Random" /> to use for shuffling.
    ///     If <c>null</c>, a shared (thread-safe in .NET 6+) or new <see cref="Random" /> instance is used, depending on
    ///     target
    ///     framework. Defaults to <c>null</c>.
    /// </param>
    /// <returns>
    ///     A shuffled list. This can be either a new list if <paramref name="preserveOriginal" /> is true, or the
    ///     original list otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the input list is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the Fisher–Yates shuffle algorithm for an efficient and unbiased shuffle.
    ///     <para>
    ///         **Thread Safety:**
    ///         - Starting with .NET 6, <see cref="Random.Shared" /> is thread-safe.
    ///         - If you pass a custom <see cref="Random" /> instance, ensure it is safe to use across threads if accessing
    ///         concurrently.
    ///     </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<T> Shuffle<T>(
        this IList<T> list,
        bool preserveOriginal = false,
        Random? random = null)
    {
        Guard.NotNull(list);

        // Use Random.Shared if .NET 6 or later; otherwise fall back to new Random().
#if NET6_0_OR_GREATER
        random ??= Random.Shared;
#else
        random ??= new Random();
#endif

        var shuffledList = preserveOriginal ? new List<T>(list) : list;

        if (shuffledList.Count <= 1)
        {
            return shuffledList;
        }

        // Fisher-Yates shuffle
        for (var i = shuffledList.Count - 1; i > 0; i--)
        {
            var j = random.Next(0, i + 1);
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
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="NotSupportedException">Thrown if the collection is read-only.</exception>
    /// <remarks>
    ///     This method checks if the collection already contains the given value. If not, the value is added.
    ///     For collections that implement <see cref="ISet{T}" />, <see cref="ISet{T}.Add" /> is used for efficiency.
    ///     <para>
    ///         **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently
    ///         by multiple threads, ensure proper synchronization to avoid race conditions.
    ///     </para>
    /// </remarks>
    public static bool AddUnique<T>([NoEnumeration] this ICollection<T> collection, T value)
    {
        Guard.NotNull(collection, nameof(collection));

        if (collection is ISet<T> set)
        {
            return set.Add(value);
        }

        if (collection.IsReadOnly)
        {
            throw new NotSupportedException("The collection is read-only.");
        }

        if (collection.Contains(value))
        {
            return false;
        }

        collection.Add(value);
        return true;
    }

    /// <summary>
    ///     Removes all the elements from a collection that match the conditions defined by the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection from which elements will be removed.</param>
    /// <param name="predicate">The delegate that defines the conditions of the elements to remove.</param>
    /// <returns>The number of elements removed from the collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either the <paramref name="collection" /> or the <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method iterates through the collection and removes each element that matches the conditions
    ///     defined by the specified predicate.
    ///     <para>
    ///         For collections that implement <see cref="List{T}" />, it removes items using <see cref="List{T}.RemoveAll" />
    ///         for efficiency. For <see cref="IList{T}" /> (non-list-based), items are removed via
    ///         <see cref="IList{T}.RemoveAt" />
    ///         from the back to avoid reindexing overhead. For <see cref="ISet{T}" />, items are removed directly.
    ///     </para>
    ///     <para>
    ///         **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///         ensure proper synchronization to avoid race conditions.
    ///     </para>
    /// </remarks>
    public static int RemoveWhere<T>(
        [NoEnumeration] this ICollection<T> collection,
        Predicate<T> predicate)
    {
        Guard.NotNull(collection);
        Guard.NotNull(predicate);

        if (collection.Count == 0)
        {
            return 0;
        }

        // If it's a List<T>, use RemoveAll for speed
        if (collection is List<T> list)
        {
            return list.RemoveAll(predicate);
        }

        // If it's an IList<T> but not a List<T>
        if (collection is IList<T> internalList)
        {
            var count = 0;
            for (var i = internalList.Count - 1; i >= 0; i--)
            {
                if (predicate(internalList[i]))
                {
                    internalList.RemoveAt(i);
                    count++;
                }
            }

            return count;
        }

        // If it's an ISet<T>, remove items directly
        if (collection is ISet<T> set)
        {
            var itemsToRemove = set.Where(x => predicate(x)).ToList();
            foreach (var item in itemsToRemove)
            {
                set.Remove(item);
            }

            return itemsToRemove.Count;
        }

        // Fallback for other ICollection<T> implementations
        var toRemove = collection.Where(item => predicate(item)).ToList();
        foreach (var item in toRemove)
        {
            collection.Remove(item);
        }

        return toRemove.Count;
    }

    /// <summary>
    ///     Adds an element to the collection if it satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to which the element will be added.</param>
    /// <param name="value">The element to add to the collection.</param>
    /// <param name="predicate">The predicate that determines if the element should be added.</param>
    /// <returns>
    ///     <c>true</c> if the element is added to the collection;
    ///     <c>false</c> if the element does not satisfy the predicate and is not added.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="collection" /> or the <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks whether the provided value satisfies the predicate. If so, the value is added to the collection.
    ///     <para>
    ///         **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///         ensure proper synchronization to avoid race conditions.
    ///     </para>
    /// </remarks>
    public static bool AddIf<T>(
        this ICollection<T> collection,
        T value,
        Func<T, bool> predicate)
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
    ///     Thrown if <paramref name="collection" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method efficiently determines if any of the specified elements are present in the collection.
    ///     <para>
    ///         If the collection implements <see cref="ISet{T}" />, it uses the set's <c>Contains</c> method for efficient
    ///         lookups.
    ///         Otherwise, it adjusts the strategy based on the sizes of the collection and the <paramref name="values" />
    ///         array.
    ///     </para>
    /// </remarks>
    public static bool ContainsAny<T>(
        [NoEnumeration] this ICollection<T> collection,
        params T[]? values)
    {
        Guard.NotNull(collection);

        if (values == null || values.Length == 0)
        {
            return false;
        }

        if (collection is ISet<T> set)
        {
            return values.Any(set.Contains);
        }

        // If the collection is small or the values array is larger than the collection,
        // we do a simple "any contains" check.
        if (collection.Count <= 10 || values.Length > collection.Count)
        {
            return values.Any(collection.Contains);
        }

        // Otherwise, for moderate/large collections, we build a HashSet from the values and check membership in O(n).
        var valueSet = new HashSet<T>(values);
        return collection.Any(valueSet.Contains);
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
    ///     Thrown if the <paramref name="collection" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the collection is read-only.
    /// </exception>
    /// <remarks>
    ///     This method iterates over the provided values and adds each one to the collection if it does not already exist.
    ///     <para>
    ///         For collections that implement <see cref="ISet{T}" />, this operation uses <see cref="ISet{T}.Add" />,
    ///         which handles uniqueness checks more efficiently.
    ///     </para>
    ///     <para>
    ///         **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///         ensure proper synchronization to avoid race conditions.
    ///     </para>
    /// </remarks>
    public static int AddUniqueRange<T>(
        [NoEnumeration] this ICollection<T> collection,
        IEnumerable<T>? values)
    {
        Guard.NotNull(collection, nameof(collection));

        if (values == null)
        {
            return 0;
        }

        if (collection.IsReadOnly)
        {
            throw new NotSupportedException("Cannot add items to a read-only collection.");
        }

        var counter = 0;

        if (collection is ISet<T> set)
        {
            // We can count how many true "Add" operations happened.
            counter += values.Count(set.Add);
        }
        else
        {
            // For other ICollection<T> types, maintain a local set of existing items to avoid repeated Contains checks.
            var existingItems = new HashSet<T>(collection);
            foreach (var value in values)
            {
                if (!existingItems.Add(value))
                {
                    continue; // Already in set, skip
                }

                collection.Add(value);
                counter++;
            }
        }

        return counter;
    }
}