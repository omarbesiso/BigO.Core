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
    ///     Indicates whether the specified <see cref="IEnumerable" /> is empty; i.e. instance does not have any items.
    /// </summary>
    /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
    /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>; otherwise <c>false</c>. </exception>
    public static bool IsEmpty([NoEnumeration] this IEnumerable collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    ///     Indicates whether the specified <see cref="IEnumerable" /> is not empty; i.e. instance has at least one item.
    /// </summary>
    /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
    /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is not empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>; otherwise <c>false</c>. </exception>
    public static bool IsNotEmpty([NoEnumeration] this IEnumerable collection)
    {
        return !IsEmpty(collection);
    }

    /// <summary>
    ///     Indicates whether the specified <see cref="IEnumerable" /> is null or empty; i.e. instance does not have has any
    ///     items.
    /// </summary>
    /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
    /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is null or has no items.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>; otherwise <c>false</c>. </exception>
    public static bool IsNullOrEmpty([NotNullWhen(false)] [NoEnumeration] this IEnumerable? collection)
    {
        return collection == null || collection.IsEmpty();
    }


    /// <summary>
    ///     Indicates whether the specified <see cref="IEnumerable" /> is not null or empty; i.e. instance has at least one
    ///     item.
    /// </summary>
    /// <param name="collection">The <see cref="IEnumerable" /> instance to be checked.</param>
    /// <returns><c>true</c> if the <see cref="IEnumerable" /> object is not null or has no items; otherwise <c>false</c>.</returns>
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] [NoEnumeration] this IEnumerable? collection)
    {
        return !IsNullOrEmpty(collection);
    }

    /// <summary>
    ///     Chunks the specified list into chunks of the specified <paramref name="chunkSize" />.
    /// </summary>
    /// <typeparam name="T">The type of element in the list.</typeparam>
    /// <param name="list">The list to be chunked.</param>
    /// <param name="chunkSize">Size of the chunk.</param>
    /// <returns>The list of the chunks created.</returns>
    /// <exception cref="System.ArgumentNullException">The <paramref name="list" /> cannot be null.</exception>
    /// <exception cref="System.ArgumentException">The <paramref name="chunkSize" /> has to be greater than 0.</exception>
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