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
    ///     <para>
    ///         Internally uses <see cref="Array.Clear(Array,int,int)" /> on older frameworks,
    ///         and <see cref="Span{T}.Clear()" /> for .NET 6 or later (via <see cref="MemoryExtensions.AsSpan{T}(T[])" />).
    ///     </para>
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    /// <param name="index">The starting index of the range to clear.</param>
    /// <param name="length">The number of elements to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> or <paramref name="length" /> are out of bounds
    ///     relative to the <paramref name="array" />.
    /// </exception>
    /// <remarks>
    ///     Use this method to efficiently reset a subsection of an array to default values.
    ///     <example>
    ///         <code><![CDATA[
    ///         int[] numbers = { 1, 2, 3, 4, 5 };
    ///         numbers.ClearRange(1, 3);
    ///         // After clearing, numbers = { 1, 0, 0, 0, 5 }
    ///         ]]></code>
    ///     </example>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearRange<T>(this T[] array, int index, int length)
    {
        Guard.NotNull(array);

        if (index < 0 || index >= array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        if (length < 0 || index + length > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length is out of range.");
        }

#if NET6_0_OR_GREATER
        // For .NET 6 or later, we can clear via spans:
        array.AsSpan(index, length).Clear();
#else
        // For earlier .NET versions, fallback to the standard Array.Clear approach:
        Array.Clear(array, index, length);
#endif
    }

    /// <summary>
    ///     Clears all elements in the array, setting each element to its default value.
    ///     <para>
    ///         Internally uses <see cref="Array.Clear(Array,int,int)" /> on older frameworks,
    ///         and <see cref="Span{T}.Clear" /> for .NET 6 or later (via <see cref="MemoryExtensions.AsSpan{T}(T[])" />).
    ///     </para>
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to clear.</param>
    /// <exception cref="ArgumentNullException">Thrown if the input array is <c>null</c>.</exception>
    /// <remarks>
    ///     This method resets all elements in the array to the type's default value.
    ///     <example>
    ///         <code><![CDATA[
    ///         int[] numbers = { 1, 2, 3, 4, 5 };
    ///         numbers.Clear();
    ///         // After clearing, numbers = { 0, 0, 0, 0, 0 }
    ///         ]]></code>
    ///     </example>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(this T[] array)
    {
        Guard.NotNull(array);

#if NET6_0_OR_GREATER
        array.AsSpan().Clear();
#else
        Array.Clear(array, 0, array.Length);
#endif
    }
}