using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Stream" /> objects.
/// </summary>
[PublicAPI]
public static class StreamExtensions
{
    /// <summary>
    ///     Writes the stream contents to a byte array.
    /// </summary>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>The byte array representing contents read from the specified stream.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream" /> is <c>null</c>.</exception>
    public static byte[] ToByteArray(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    ///     Asynchronously writes the stream contents to a byte array.
    /// </summary>
    /// <param name="stream">The stream to be written.</param>
    /// <returns>A task that represents the asynchronous writing operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream" /> is <c>null</c>.</exception>
    public static async Task<byte[]> ToByteArrayAsync(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}