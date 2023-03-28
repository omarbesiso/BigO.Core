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
    ///     Converts a byte array to a MemoryStream.
    /// </summary>
    /// <remarks>
    ///     This method is an extension method for byte array types. It creates a new MemoryStream using the provided byte
    ///     array and sets the initial position to 0.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///         byte[] data = new byte[] { 0x01, 0x02, 0x03 };
    ///         MemoryStream stream = data.ToMemoryStream();
    /// ]]></code>
    /// </example>
    /// <param name="buffer">The byte array to convert to a MemoryStream.</param>
    /// <returns>A MemoryStream containing the data from the byte array.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when buffer is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] buffer)
    {
        return new MemoryStream(buffer) { Position = 0 };
    }
}