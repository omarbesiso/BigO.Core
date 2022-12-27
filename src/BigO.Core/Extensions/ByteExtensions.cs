using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="byte" /> objects.
/// </summary>
[PublicAPI]
public static class ByteExtensions
{
    /// <summary>
    ///     Converts the specified byte array to a <see cref="MemoryStream" />.
    /// </summary>
    /// <param name="buffer">The byte array to convert. Cannot be <c>null</c>.</param>
    /// <returns>A <see cref="MemoryStream" /> that represents the byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method converts the specified byte array to a <see cref="MemoryStream" /> using the
    ///     <see cref="MemoryStream" /> constructor.
    ///     The <see cref="MemoryStream" /> class represents a stream of in-memory data, and provides methods and properties
    ///     for reading and writing to the stream.
    ///     The <see cref="MemoryStream" /> constructor takes a byte array as an input parameter and creates a stream that is
    ///     backed by the byte array.
    ///     The <c>Position</c> property of the <see cref="MemoryStream" /> is set to 0, which means that the stream is
    ///     positioned at the beginning of the data.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] buffer)
    {
        return new MemoryStream(buffer) { Position = 0 };
    }
}