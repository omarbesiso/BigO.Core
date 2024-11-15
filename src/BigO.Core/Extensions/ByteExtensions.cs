using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="byte" /> arrays.
/// </summary>
[PublicAPI]
public static class ByteExtensions
{
    /// <summary>
    ///     Converts a byte array to a <see cref="MemoryStream" />.
    /// </summary>
    /// <param name="buffer">The byte array to convert into a <see cref="MemoryStream" />.</param>
    /// <param name="writable">
    ///     A value indicating whether the <see cref="MemoryStream" /> can be written to.
    ///     Defaults to <c>false</c>.
    /// </param>
    /// <returns>A <see cref="MemoryStream" /> created from the byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input byte array is <c>null</c>.</exception>
    /// <remarks>
    ///     This extension method provides a convenient way to convert a byte array into a <see cref="MemoryStream" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] buffer, bool writable = false)
    {
        Guard.NotNull(buffer);
        return new MemoryStream(buffer, writable);
    }

    /// <summary>
    ///     Converts a subset of a byte array to a <see cref="MemoryStream" />.
    /// </summary>
    /// <param name="buffer">The byte array containing the data to create a <see cref="MemoryStream" /> from.</param>
    /// <param name="index">The zero-based byte offset in <paramref name="buffer" /> at which to begin using data.</param>
    /// <param name="count">The number of bytes to use from <paramref name="buffer" />.</param>
    /// <param name="writable">
    ///     A value indicating whether the <see cref="MemoryStream" /> can be written to.
    ///     Defaults to <c>false</c>.
    /// </param>
    /// <returns>A <see cref="MemoryStream" /> created from the specified subset of the byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input byte array is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index" /> or <paramref name="count" /> are out of bounds.
    /// </exception>
    /// <remarks>
    ///     This extension method allows for creating a <see cref="MemoryStream" /> from a subset of a byte array.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// byte[] data = { 0x1, 0x2, 0x3, 0x4, 0x5 };
    /// using (MemoryStream stream = data.ToMemoryStream(1, 3))
    /// {
    ///     // Use the MemoryStream instance.
    /// }
    /// // This will create a MemoryStream with bytes { 0x2, 0x3, 0x4 }
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] buffer, int index, int count, bool writable = false)
    {
        Guard.NotNull(buffer);

        if (index < 0 || index > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        if (count < 0 || index + count > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count is out of range.");
        }

        return new MemoryStream(buffer, index, count, writable);
    }
}