using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>([NoEnumeration] this IEnumerable<T> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        using var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty<T>([NoEnumeration] this IEnumerable<T> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), $"The {nameof(collection)} cannot be null.");
        }

        using var enumerator = collection.GetEnumerator();
        return enumerator.MoveNext();
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] [NoEnumeration] this IEnumerable<T>? collection)
    {
        if (collection == null)
        {
            return true;
        }

        using var enumerator = collection.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="collection" /> is not null and contains at least one element.
    /// </summary>
    /// <param name="collection">The collection to check for non-nullness and non-emptiness.</param>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] [NoEnumeration] this IEnumerable<T>? collection)
    {
        if (collection == null)
        {
            return false;
        }

        using var enumerator = collection.GetEnumerator();
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