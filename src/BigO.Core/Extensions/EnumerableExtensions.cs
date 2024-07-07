﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IEnumerable" /> objects.
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is empty.
    /// </summary>
    /// <param name="collection">The collection to check for emptiness.</param>
    /// <returns>true if the specified <paramref name="collection" /> is empty; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
    /// <remarks>
    ///     This extension method can be used on any type that implements the <see cref="System.Collections.IEnumerable" />
    ///     interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsEmpty" /> method to check if an array is empty.
    ///     <code><![CDATA[
    /// int[] emptyArray = new int[0];
    /// bool isEmpty = emptyArray.IsEmpty();
    /// Console.WriteLine(isEmpty); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    public static bool IsEmpty([NoEnumeration] this IEnumerable collection)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        Guard.NotNull(collection);

        switch (collection)
        {
            case Array a:
                return a.Length == 0;
            case ICollection c:
                return c.Count == 0;
            case IReadOnlyCollection<object> rc:
                return rc.Count == 0;
            default:
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var enumerator = collection.GetEnumerator();
                try
                {
                    return !enumerator.MoveNext();
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for emptiness.</param>
    /// <returns>true if the specified <paramref name="collection" /> is empty; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
    /// <remarks>
    ///     This extension method can be used on any type that implements the
    ///     <see cref="System.Collections.Generic.IEnumerable{T}" /> interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsEmpty{T}(IEnumerable{T})" /> method to check if a list
    ///     is empty.
    ///     <code><![CDATA[
    /// List<int> emptyList = new List<int>();
    /// bool isEmpty = emptyList.IsEmpty();
    /// Console.WriteLine(isEmpty); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>([NoEnumeration] this IEnumerable<T> collection)
    {
        Guard.NotNull(collection);
        return !collection.Any();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is not empty.
    /// </summary>
    /// <param name="collection">The collection to check for non-emptiness.</param>
    /// <returns>true if the specified <paramref name="collection" /> is not empty; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
    /// <remarks>
    ///     This extension method can be used on any type that implements the <see cref="System.Collections.IEnumerable" />
    ///     interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNotEmpty" /> method to check if an array is not empty.
    ///     <code><![CDATA[
    /// int[] nonEmptyArray = new int[] { 1, 2, 3 };
    /// bool isNotEmpty = nonEmptyArray.IsNotEmpty();
    /// Console.WriteLine(isNotEmpty); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty([NoEnumeration] this IEnumerable collection)
    {
        Guard.NotNull(collection);
        return !collection.IsEmpty();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for non-emptiness.</param>
    /// <returns>true if the specified <paramref name="collection" /> is not empty; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
    /// <remarks>
    ///     This extension method can be used on any type that implements the
    ///     <see cref="System.Collections.Generic.IEnumerable{T}" /> interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNotEmpty(System.Collections.IEnumerable)" /> method to
    ///     check if a list is not empty.
    ///     <code><![CDATA[
    /// List<int> nonEmptyList = new List<int>() { 1, 2, 3 };
    /// bool isNotEmpty = nonEmptyList.IsNotEmpty();
    /// Console.WriteLine(isNotEmpty); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty<T>([NoEnumeration] this IEnumerable<T> collection)
    {
        Guard.NotNull(collection);
        return !collection.IsEmpty();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is null, empty, or contains no elements.
    /// </summary>
    /// <param name="collection">The collection to check for null or emptiness.</param>
    /// <returns>
    ///     true if the specified <paramref name="collection" /> is null, empty, or contains no elements; otherwise,
    ///     false.
    /// </returns>
    /// <remarks>
    ///     This extension method can be used on any type that implements the <see cref="System.Collections.IEnumerable" />
    ///     interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNullOrEmpty" /> method to check if an array is null or
    ///     empty.
    ///     <code><![CDATA[
    /// int[] emptyArray = new int[0];
    /// int[] nullArray = null;
    /// bool isNullOrEmpty1 = emptyArray.IsNullOrEmpty();
    /// bool isNullOrEmpty2 = nullArray.IsNullOrEmpty();
    /// Console.WriteLine(isNullOrEmpty1); // Output: True
    /// Console.WriteLine(isNullOrEmpty2); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] [NoEnumeration] this IEnumerable? collection)
    {
        return collection == null || collection.IsEmpty();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is null, empty, or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for null or emptiness.</param>
    /// <returns>
    ///     true if the specified <paramref name="collection" /> is null, empty, or contains no elements; otherwise,
    ///     false.
    /// </returns>
    /// <remarks>
    ///     This extension method can be used on any type that implements the
    ///     <see cref="System.Collections.Generic.IEnumerable{T}" /> interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNullOrEmpty{T}(IEnumerable{T})" /> method to check if a
    ///     list is null or empty.
    ///     <code><![CDATA[
    /// List<int> emptyList = new List<int>();
    /// List<int> nullList = null;
    /// bool isNullOrEmpty1 = emptyList.IsNullOrEmpty();
    /// bool isNullOrEmpty2 = nullList.IsNullOrEmpty();
    /// Console.WriteLine(isNullOrEmpty1); // Output: True
    /// Console.WriteLine(isNullOrEmpty2); // Output: True
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] [NoEnumeration] this IEnumerable<T>? collection)
    {
        return collection == null || collection.IsEmpty();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is not null and contains at least one element.
    /// </summary>
    /// <param name="collection">The collection to check for non-null-ness and non-emptiness.</param>
    /// <returns>
    ///     true if the specified <paramref name="collection" /> is not null and contains at least one element; otherwise,
    ///     false.
    /// </returns>
    /// <remarks>
    ///     This extension method can be used on any type that implements the <see cref="System.Collections.IEnumerable" />
    ///     interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNotNullOrEmpty" /> method to check if an array is not
    ///     null and not empty.
    ///     <code><![CDATA[
    /// int[] nonEmptyArray = new int[] { 1, 2, 3 };
    /// int[] nullArray = null;
    /// bool isNotNullOrEmpty1 = nonEmptyArray.IsNotNullOrEmpty();
    /// bool isNotNullOrEmpty2 = nullArray.IsNotNullOrEmpty();
    /// Console.WriteLine(isNotNullOrEmpty1); // Output: True
    /// Console.WriteLine(isNotNullOrEmpty2); // Output: False
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] [NoEnumeration] this IEnumerable? collection)
    {
        return collection != null && collection.IsNotEmpty();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is not null and contains at least one element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check for non-nullness and non-emptiness.</param>
    /// <returns>
    ///     true if the specified <paramref name="collection" /> is not null and contains at least one element; otherwise,
    ///     false.
    /// </returns>
    /// <remarks>
    ///     This extension method can be used on any type that implements the
    ///     <see cref="System.Collections.Generic.IEnumerable{T}" /> interface, including arrays and lists.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsNotNullOrEmpty{T}(IEnumerable{T})" /> method to check
    ///     if a list is not null and not empty.
    ///     <code><![CDATA[
    /// List<int> nonEmptyList = new List<int> { 1, 2, 3 };
    /// List<int> nullList = null;
    /// bool isNotNullOrEmpty1 = nonEmptyList.IsNotNullOrEmpty();
    /// bool isNotNullOrEmpty2 = nullList.IsNotNullOrEmpty();
    /// Console.WriteLine(isNotNullOrEmpty1); // Output: True
    /// Console.WriteLine(isNotNullOrEmpty2); // Output: False
    /// ]]></code>
    /// </example>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] [NoEnumeration] this IEnumerable<T>? collection)
    {
        return collection != null && collection.IsNotEmpty();
    }

    /// <summary>
    ///     Splits the input collection into smaller chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be split into chunks.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> of chunks, where each chunk is an <see cref="IEnumerable{T}" /> of elements
    ///     from the input collection.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="chunkSize" /> is less than or equal to 0.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// var inputList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    /// int chunkSize = 3;
    /// 
    /// var chunks = inputList.Chunk(chunkSize);
    /// 
    /// // The chunks variable now contains the following chunks: [1, 2, 3], [4, 5, 6], and [7, 8, 9].
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method can be used to divide a large collection into smaller chunks of the specified size. The last chunk may
    ///     contain fewer elements if the input collection's size is not evenly divisible by the specified chunk size. Note
    ///     that the
    ///     input collection remains unchanged and the returned chunks are new <see cref="IEnumerable{T}" /> instances.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    public static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> collection, int chunkSize)
    {
        // Validate parameters
        Guard.NotNull(collection);

        if (chunkSize <= 0)
        {
            throw new ArgumentException($"The chunk size must be greater than 0. Provided value: {chunkSize}",
                nameof(chunkSize));
        }

        // Use an enumerator to yield chunks
        using var enumerator = collection.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return GetChunk(enumerator, chunkSize);
        }

        yield break;

        // Local static method to create a chunk
        static IEnumerable<TK> GetChunk<TK>(IEnumerator<TK> enumerator, int chunkSize)
        {
            do
            {
                yield return enumerator.Current;
            } while (--chunkSize > 0 && enumerator.MoveNext());
        }
    }
}