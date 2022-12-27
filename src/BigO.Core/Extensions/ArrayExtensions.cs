using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Array" /> objects.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Shuffles the elements of the specified array in random order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="array">The array to shuffle. Cannot be <c>null</c>.</param>
    /// <returns>The shuffled array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method shuffles the elements of the specified array in random order using the <see cref="Random.Shared" />
    ///     instance.
    ///     It iterates over the elements of the array and for each element, it swaps it with a random element from the rest of
    ///     the array using deconstructing assignment.
    ///     The <see cref="Random.Shared" /> instance is a thread-safe, shared instance of the <see cref="Random" /> class that
    ///     can be used to generate random numbers.
    /// </remarks>
    public static T[] Shuffle<T>(this T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), $"The {nameof(array)} cannot be null.");
        }

        for (var i = 0; i < array.Length; i++)
        {
            var j = Random.Shared.Next(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }

        return array;
    }

    /// <summary>
    ///     Clears the elements of the specified array.
    /// </summary>
    /// <param name="array">The array to clear. Cannot be <c>null</c>.</param>
    /// <param name="index">The starting index of the range of elements to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> is less than the lower bound of the
    ///     array or if <paramref name="length" /> is less than zero.
    /// </exception>
    /// <remarks>
    ///     This method clears the elements of the specified array using the <see cref="Array.Clear(System.Array)" /> method.
    ///     The <paramref name="index" /> parameter specifies the starting index of the range of elements to clear, and the
    ///     <paramref name="length" /> parameter specifies the number of elements to clear.
    ///     If the <paramref name="index" /> parameter is less than the lower bound of the array or if the
    ///     <paramref name="length" /> parameter is less than zero, it throws an <see cref="ArgumentOutOfRangeException" />.
    ///     The <see cref="Array.Clear(System.Array)" /> method sets the elements of the specified array to their default
    ///     values. For reference types, this means setting the elements to <c>null</c>. For value types, this means setting
    ///     the elements to the default value for the type, such as 0 for integers or false for booleans.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(this Array array, int index, int length)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), $"The {nameof(array)} cannot be null.");
        }

        Array.Clear(array, index, length);
    }
}