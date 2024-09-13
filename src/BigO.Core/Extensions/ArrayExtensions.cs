using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides extension methods for arrays.
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    ///     Clears a range of elements in the array, setting each element within the range to its default value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    /// <param name="index">The starting index of the range to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> or <paramref name="length" /> are out of bounds.
    /// </exception>
    /// <remarks>
    ///     This method is an extension that internally calls <see cref="Array.Clear(Array, int, int)" />,
    ///     providing a more intuitive way to clear a portion of an array.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int[] numbers = { 1, 2, 3, 4, 5 };
    ///     numbers.ClearRange(1, 3);
    ///     // After clearing, numbers = { 1, 0, 0, 0, 5 }
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearRange<T>(this T[] array, int index, int length)
    {
        Guard.NotNull(array);
        Array.Clear(array, index, length);
    }

    /// <summary>
    ///     Clears all elements in the array, setting each element to its default value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <remarks>
    ///     This method sets all elements in the array to the type's default value.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int[] numbers = { 1, 2, 3, 4, 5 };
    ///     numbers.Clear();
    ///     // After clearing, numbers = { 0, 0, 0, 0, 0 }
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(this T[] array)
    {
        Guard.NotNull(array);
        Array.Clear(array, 0, array.Length);
    }
}