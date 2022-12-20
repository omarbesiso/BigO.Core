using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Array" /> objects.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Shuffles the elements in the given array randomly.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="array">The array to shuffle.</param>
    /// <returns>The shuffled array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method shuffles the elements in the given array randomly and returns the shuffled array.
    ///     It uses the <see cref="Random.Shared" /> object to generate random indices for the shuffle.
    /// </remarks>
    public static T[] Shuffle<T>(this T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        for (var i = 0; i < array.Length; i++)
        {
            var j = Random.Shared.Next(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }

        return array;
    }

    /// <summary>
    ///     Clears a range of elements in the given array.
    /// </summary>
    /// <param name="array">The array to clear.</param>
    /// <param name="index">The starting index of the range to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> or <paramref name="length" /> is
    ///     negative or if <paramref name="index" /> and <paramref name="length" /> do not denote a valid range in
    ///     <paramref name="array" />.
    /// </exception>
    /// <remarks>
    ///     This method clears a range of elements in the given array by setting their values to zero (for numeric types),
    ///     false (for boolean types), or null (for reference types).
    ///     It uses the <see cref="Array.Clear(Array, int, int)" /> method to perform the clear operation.
    /// </remarks>
    public static void Clear(this Array array, int index, int length)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        Array.Clear(array, index, length);
    }
}