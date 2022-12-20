using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="byte" /> objects.
/// </summary>
[PublicAPI]
public static class ByteExtensions
{
    /// <summary>
    ///     Converts the given byte array to a memory stream.
    /// </summary>
    /// <param name="buffer">The byte array to convert.</param>
    /// <returns>A memory stream containing the contents of the byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method converts the given byte array to a memory stream using the <see cref="MemoryStream(byte[])" />
    ///     constructor.
    ///     The position of the returned memory stream is set to 0.
    /// </remarks>
    public static MemoryStream ToMemoryStream(this byte[] buffer)
    {
        return new MemoryStream(buffer) { Position = 0 };
    }
}