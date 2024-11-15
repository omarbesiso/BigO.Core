﻿using BigO.Core.Validation;

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
    /// <param name="random">
    ///     An instance of <see cref="Random" /> to use for shuffling.
    ///     If <c>null</c>, a new <see cref="Random" /> instance is created. Defaults to <c>null</c>.
    /// </param>
    /// <returns>
    ///     A shuffled list. This can be either a new list if <paramref name="preserveOriginal" /> is true, or the
    ///     original list otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the input list is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the Fisher–Yates shuffle algorithm for an efficient and unbiased shuffle.
    ///     Accepting a <see cref="Random" /> instance allows for better control over randomness, especially in testing
    ///     scenarios.
    ///     **Thread Safety:** Providing your own <see cref="Random" /> instance can improve thread safety since
    ///     <see cref="Random.Shared" /> is not thread-safe.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// IList<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
    /// IList<int> shuffledNumbers = numbers.Shuffle();
    /// IList<int> originalPreservedShuffle = numbers.Shuffle(preserveOriginal: true);
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<T> Shuffle<T>(this IList<T> list, bool preserveOriginal = false, Random? random = null)
    {
        Guard.NotNull(list);

        random ??= new Random();

        var shuffledList = preserveOriginal ? new List<T>(list) : list;

        if (shuffledList.Count <= 1)
        {
            return shuffledList;
        }

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
    ///     **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently
    ///     by multiple threads, ensure proper synchronization to avoid race conditions.
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
    ///     Thrown if either the collection or the predicate is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method iterates through the collection and removes each element that matches the conditions
    ///     defined by the specified predicate. For collections that implement <see cref="IList{T}" />,
    ///     it removes items using indexing for efficiency. For other collections, it collects items to remove
    ///     in a temporary list to avoid modifying the collection during enumeration.
    ///     **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///     ensure proper synchronization to avoid race conditions.
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

        switch (collection)
        {
            case List<T> list:
                return list.RemoveAll(predicate);
            case IList<T> internalList:
            {
                var count = 0;
                for (var i = internalList.Count - 1; i >= 0; i--)
                {
                    if (!predicate(internalList[i]))
                    {
                        continue;
                    }

                    internalList.RemoveAt(i);
                    count++;
                }

                return count;
            }
        }

        var itemsToRemove = collection.Where(item => predicate(item)).ToList();
        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }

        return itemsToRemove.Count;
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
    ///     Thrown if either the collection or the predicate is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks whether the provided value satisfies the predicate. If so, the value is added to the collection.
    ///     It's a convenient way to add elements conditionally without needing separate conditional logic outside the method
    ///     call.
    ///     **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///     ensure proper synchronization to avoid race conditions.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     ICollection<int> numbers = new List<int>();
    ///     bool added = numbers.AddIf(5, x => x > 0);
    ///     // added is true, numbers now contains 5
    ///     
    ///     added = numbers.AddIf(-1, x => x > 0);
    ///     // added is false, numbers still contains only 5
    ///     ]]></code>
    /// </example>
    public static bool AddIf<T>(this ICollection<T> collection, T value, Func<T, bool> predicate)
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
    ///     This method efficiently determines if any of the specified elements are present in the collection.
    ///     If the collection implements <see cref="ISet{T}" />, it uses the set's <c>Contains</c> method for efficient
    ///     lookups.
    ///     Otherwise, it adjusts the strategy based on the sizes of the collection and the values array.
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
    public static bool ContainsAny<T>([NoEnumeration] this ICollection<T> collection, params T[]? values)
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

        if (collection.Count <= 10 || values.Length > collection.Count)
        {
            return values.Any(collection.Contains);
        }

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
    ///     For collections that implement <see cref="ISet{T}" />, this operation is more efficient as
    ///     <see cref="ISet{T}.Add" /> is used, which handles uniqueness checks.
    ///     **Thread Safety:** This method is not thread-safe. If the collection is accessed concurrently,
    ///     ensure proper synchronization to avoid race conditions.
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
    public static int AddUniqueRange<T>([NoEnumeration] this ICollection<T> collection, IEnumerable<T>? values)
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
            counter += values.Count(set.Add);
        }
        else
        {
            var existingItems = new HashSet<T>(collection);
            foreach (var value in values)
            {
                if (!existingItems.Add(value))
                {
                    continue;
                }

                collection.Add(value);
                counter++;
            }
        }

        return counter;
    }
}