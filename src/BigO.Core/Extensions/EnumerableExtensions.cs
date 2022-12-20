using System.Collections;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility methods when working with <see cref="IEnumerable" /> objects.
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    /// <summary>
    ///     Determines whether the given collection is empty.
    /// </summary>
    /// <param name="collection">The collection to check.</param>
    /// <returns>True if the collection is empty, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="collection" /> parameter is <c>null</c>.</exception>
    /// <remarks>
    ///     This method determines whether the given collection is empty by using the <see cref="IEnumerable.GetEnumerator" />
    ///     method to get an enumerator for the collection, and then calling the <see cref="IEnumerator.MoveNext" /> method on
    ///     the enumerator.
    ///     If the <see cref="IEnumerator.MoveNext" /> method returns false, the collection is considered empty.
    /// </remarks>
    public static bool IsEmpty([NoEnumeration] this IEnumerable collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    ///     Determines if the specified collection is not empty.
    /// </summary>
    /// <param name="collection">The collection to check.</param>
    /// <returns>True if the collection is not empty, false otherwise.</returns>
    /// <remarks>
    ///     This method returns the opposite of the <see cref="IsEmpty(IEnumerable)" /> method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collection" /> parameter is <c>null</c>, a <see cref="ArgumentNullException" /> is thrown.
    /// </exception>
    public static bool IsNotEmpty([NoEnumeration] this IEnumerable collection)
    {
        return !IsEmpty(collection);
    }

    /// <summary>
    ///     Determines whether the given collection is null or empty.
    /// </summary>
    /// <param name="collection">The collection to check for null or empty.</param>
    /// <returns>True if the collection is null or empty; otherwise, false.</returns>
    /// <remarks>
    ///     This method first checks whether the collection is <c>null</c>. If it is, it returns true.
    ///     If the collection is not null, it invokes the <see cref="IsEmpty" /> method on the collection to determine
    ///     whether it is empty. If the collection is empty, the method returns true; otherwise, it returns false.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if the collection is null.</exception>
    public static bool IsNullOrEmpty([NotNullWhen(false)] [NoEnumeration] this IEnumerable? collection)
    {
        return collection == null || collection.IsEmpty();
    }


    /// <summary>
    ///     Determines whether the given collection is not null and not empty.
    /// </summary>
    /// <param name="collection">The collection to check for not null and not empty.</param>
    /// <returns>True if the collection is not null and not empty; otherwise, false.</returns>
    /// <remarks>
    ///     This method invokes the <see cref="IsNullOrEmpty" /> method on the given collection to determine
    ///     whether it is null or empty. If the collection is not null and not empty, the method returns true;
    ///     otherwise, it returns false.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if the collection is null.</exception>
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] [NoEnumeration] this IEnumerable? collection)
    {
        return !IsNullOrEmpty(collection);
    }

    /// <summary>
    ///     Splits the given list into chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to split into chunks.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>An enumerable of chunks, where each chunk is an enumerable of elements of type <typeparamref name="T" />.</returns>
    /// <remarks>
    ///     The method first checks whether the <paramref name="list" /> is <c>null</c>. If it is, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    ///     The method then checks whether the <paramref name="chunkSize" /> is less than or equal to 0. If it is, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The method then converts the list to a list and iterates over it, yielding chunks of the specified size as it goes.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the chunk size is less than or equal to 0.</exception>
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