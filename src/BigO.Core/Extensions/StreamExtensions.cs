using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Stream" /> objects.
/// </summary>
[PublicAPI]
public static class StreamExtensions
{
    /// <summary>
    ///     Converts the specified <see cref="Stream" /> to a byte array.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> to convert.</param>
    /// <returns>
    ///     A byte array that contains the contents of the input <paramref name="stream" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="stream" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method uses a <see cref="MemoryStream" /> to copy the contents of the input <paramref name="stream" /> and
    ///     then calls the
    ///     <see cref="MemoryStream.ToArray" /> method to convert the contents to a byte array.
    /// </remarks>
    public static byte[] ToByteArray(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    ///     Asynchronously converts the specified <see cref="Stream" /> to a byte array.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> to convert.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a byte array that contains the contents
    ///     of the
    ///     input <paramref name="stream" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="stream" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method uses a <see cref="MemoryStream" /> to asynchronously copy the contents of the input
    ///     <paramref name="stream" /> and then calls the
    ///     <see cref="MemoryStream.ToArray" /> method to convert the contents to a byte array.
    /// </remarks>
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}