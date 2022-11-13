using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Array" /> objects.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Shuffles the items within the <paramref name="array" />.
    /// </summary>
    /// <typeparam name="T">The type of items in the <paramref name="array" />.</typeparam>
    /// <param name="array">The <paramref name="array" /> to be shuffled.</param>
    /// <returns>The shuffled array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="array" /> is <c>null</c>.</exception>
    public static T[] Shuffle<T>(this T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        for (var i = 0; i < array.Length; i++)
        {
            var j = Random.Shared.Next(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }

        return array;
    }

    /// <summary>
    ///     Sets a range of elements in the <see cref="Array" /> to zero, to false, or to null, depending on the element
    ///     type.
    /// </summary>
    /// <param name="array">The array whose elements are to be cleared.</param>
    /// <param name="index">The starting index of the range of elements to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="array" /> is <c>null</c>.</exception>
    /// <exception cref="IndexOutOfRangeException">
    ///     <para>
    ///         Thrown when <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
    ///     </para>
    ///     <para>-or-</para>
    ///     <para><paramref name="length" /> is less than zero.</para>
    ///     <para>-or-</para>
    ///     <para>The sum of <paramref name="index" /> and <paramref name="length" /> is greater than the size of array. </para>
    /// </exception>
    public static void Clear(this Array array, int index, int length)
    {
        ArgumentNullException.ThrowIfNull(array);
        Array.Clear(array, index, length);
    }
}