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
    ///     It initializes a new instance of the <see cref="MemoryStream" /> class with the specified byte array.
    ///     The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option,
    ///     allowing the JIT compiler to inline the method's body at the call site for improved performance.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// byte[] data = { 0x1, 0x2, 0x3, 0x4 };
    /// using (MemoryStream stream = data.ToMemoryStream())
    /// {
    ///     // Use the MemoryStream instance.
    /// }
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] buffer, bool writable = false)
    {
        Guard.NotNull(buffer);
        return new MemoryStream(buffer, writable);
    }
}