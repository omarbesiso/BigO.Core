namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Array" /> objects.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Shuffles the elements of the specified array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to shuffle.</param>
    /// <param name="preserveOriginal">
    ///     Specifies whether to preserve the original array. If <c>true</c>, the shuffle is performed on a copy of the array;
    ///     otherwise, the shuffle is performed on the original array.
    /// </param>
    /// <returns>
    ///     A shuffled array. This can be either a new array if <paramref name="preserveOriginal" /> is true, or the
    ///     original array otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the Fisher–Yates shuffle algorithm for an efficient and unbiased shuffle.
    ///     When <paramref name="preserveOriginal" /> is set to true, a copy of the array is made to ensure that the original
    ///     array's order is not altered.
    ///     The shuffle operation is randomized using <see cref="Random.Shared" />.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int[] numbers = { 1, 2, 3, 4, 5 };
    ///     int[] shuffledNumbers = numbers.Shuffle();
    ///     ]]></code>
    /// </example>
    public static T[] Shuffle<T>(this T[] array, bool preserveOriginal = false)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), $"The {nameof(array)} cannot be null.");
        }

        var shuffledArray = preserveOriginal ? (T[])array.Clone() : array;

        for (var i = 0; i < shuffledArray.Length - 1; i++)
        {
            var j = Random.Shared.Next(i, shuffledArray.Length);
            (shuffledArray[i], shuffledArray[j]) = (shuffledArray[j], shuffledArray[i]);
        }

        return shuffledArray;
    }

    /// <summary>
    ///     Clears a range of elements in the array, setting each element within the range to its default value.
    /// </summary>
    /// <param name="array">The array to clear.</param>
    /// <param name="index">The starting index of the range to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> is less than the lower bound of the array,
    ///     or if <paramref name="length" /> is less than zero,
    ///     or if <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in the array.
    /// </exception>
    /// <remarks>
    ///     This method is an extension that internally calls <see cref="Array.Clear(Array, int, int)" />,
    ///     providing a more intuitive way to clear a portion of an array.
    ///     It sets a range of elements in the array to the type's default value (for example, null for reference types, 0 for
    ///     numeric types).
    ///     The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option, allowing the JIT compiler to inline the method's body
    ///     at the call site for improved performance.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int[] numbers = { 1, 2, 3, 4, 5 };
    ///     numbers.Clear(1, 3);
    ///     // After clearing, numbers = { 1, 0, 0, 0, 5 }
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(this Array array, int index, int length)
    {
        Array.Clear(array, index, length);
    }
}