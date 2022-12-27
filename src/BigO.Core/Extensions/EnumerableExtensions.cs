using System.Collections;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IEnumerable" /> objects.
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    /// <summary>
    ///     Determines whether the specified collection is empty.
    /// </summary>
    /// <param name="collection">The collection to check for emptiness.</param>
    /// <returns>True if the collection is empty; otherwise, false.</returns>
    /// <remarks>
    ///     This method checks if the enumerator of the collection can move to the next element. If it cannot, then the
    ///     collection is considered empty.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    public static bool IsEmpty([NoEnumeration] this IEnumerable collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    ///     Determines whether the specified collection is not empty.
    /// </summary>
    /// <param name="collection">The collection to check for emptiness.</param>
    /// <returns><c>true</c> if the collection is not empty; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method checks if the enumerator of the collection can move to the next element. If it can, then the collection
    ///     is considered not empty.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection" /> is <c>null</c>.</exception>
    public static bool IsNotEmpty([NoEnumeration] this IEnumerable collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        var enumerator = collection.GetEnumerator();
        return enumerator.MoveNext();
    }

    /// <summary>
    ///     Determines whether the specified collection is null or has no items.
    /// </summary>
    /// <param name="collection">The collection to check for null or no items.</param>
    /// <returns>True if the collection is null or has no items; otherwise, false.</returns>
    /// <remarks>
    ///     If the collection is null, this method returns true. If the collection is not null, this method checks if the
    ///     enumerator of the collection can move to the next element. If it cannot, then the collection is considered to have
    ///     no items.
    /// </remarks>
    public static bool IsNullOrEmpty([NotNullWhen(false)] [NoEnumeration] this IEnumerable? collection)
    {
        if (collection == null)
        {
            return true;
        }

        var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    ///     Determines whether the specified collection is not null and has items.
    /// </summary>
    /// <param name="collection">The collection to check for not null and having items.</param>
    /// <returns>True if the collection is not null and has items; otherwise, false.</returns>
    /// <remarks>
    ///     If the collection is null, this method returns false. If the collection is not null, this method checks if the
    ///     enumerator of the collection can move to the next element. If it can, then the collection is considered to have
    ///     items.
    /// </remarks>
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] [NoEnumeration] this IEnumerable? collection)
    {
        if (collection == null)
        {
            return false;
        }

        var enumerator = collection.GetEnumerator();
        return enumerator.MoveNext();
    }

    /// <summary>
    ///     Divides the specified list into chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to divide into chunks.</param>
    /// <param name="chunkSize">The size of the chunks.</param>
    /// <returns>
    ///     A list of <see cref="IEnumerable" /> objects, where each inner enumerable represents a chunk of the original
    ///     list.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="list" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="chunkSize" /> is less than or equal to 0.</exception>
    /// <remarks>
    ///     This method divides the list into chunks by iterating through the list and taking a specified number of elements at
    ///     a time. The final chunk may have fewer elements if the list's size is not evenly divisible by the chunk size.
    /// </remarks>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> list, int chunkSize)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list), $"The {list} cannot be null.");
        }

        if (chunkSize <= 0)
        {
            throw new ArgumentException($"The {chunkSize} has to be greater than 0.");
        }

        var internalList = list.ToList();

        for (var i = 0; i < internalList.Count; i += chunkSize)
        {
            var chunk = internalList.Skip(i).Take(chunkSize);
            yield return chunk;
        }
    }
}