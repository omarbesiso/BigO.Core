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
}