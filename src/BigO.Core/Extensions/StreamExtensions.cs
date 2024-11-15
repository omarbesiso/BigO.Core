using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Stream" /> objects.
/// </summary>
[PublicAPI]
public static class StreamExtensions
{
    private const int DefaultBufferSize = 81920; // 80 KB

    /// <summary>
    ///     Converts a <see cref="Stream" /> to a byte array.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> to be converted to a byte array.</param>
    /// <returns>A byte array representing the contents of the given <paramref name="stream" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
    /// byte[] byteArray = stream.ToByteArray();
    /// 
    /// // byteArray now contains the binary representation of "Hello, World!".
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method is useful for converting a <see cref="Stream" /> to a byte array. If the input
    ///     <paramref name="stream" /> is already a <see cref="MemoryStream" />, it directly calls the
    ///     <see cref="MemoryStream.ToArray" /> method for efficiency. If the input <paramref name="stream" /> is not a
    ///     <see cref="MemoryStream" />, the method uses a buffer to copy the contents to a byte array.
    ///     Note that this method can throw an <see cref="ArgumentNullException" /> if the provided <paramref name="stream" />
    ///     is <c>null</c>. It is the responsibility of the caller to handle this exception as appropriate.
    /// </remarks>
    public static byte[] ToByteArray(this Stream stream)
    {
        Guard.NotNull(stream);

        // Optimization for MemoryStream
        if (stream is MemoryStream sourceMemoryStream)
        {
            return sourceMemoryStream.ToArray();
        }

        // Optimization for seekable streams with known length
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];

            var totalBytesRead = 0;
            while (totalBytesRead < buffer.Length)
            {
                var bytesRead = stream.Read(
                    buffer,
                    totalBytesRead,
                    buffer.Length - totalBytesRead);

                if (bytesRead == 0)
                {
                    break; // End of stream reached
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead == buffer.Length ? buffer : buffer[..totalBytesRead];
        }

        // Handle non-seekable streams
        using var targetMemoryStream = new MemoryStream();
        stream.CopyTo(targetMemoryStream, DefaultBufferSize);
        return targetMemoryStream.ToArray();
    }

    /// <summary>
    ///     Asynchronously converts a <see cref="Stream" /> to a byte array.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> to be converted to a byte array.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a byte array representing the
    ///     contents of the given <paramref name="stream" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
    /// byte[] byteArray = await stream.ToByteArrayAsync();
    /// 
    /// // byteArray now contains the binary representation of "Hello, World!".
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method is useful for converting a <see cref="Stream" /> to a byte array. If the input
    ///     <paramref name="stream" /> is already a <see cref="MemoryStream" />, it directly calls the
    ///     <see cref="MemoryStream.ToArray" /> method for efficiency. If the input <paramref name="stream" /> is not a
    ///     <see cref="MemoryStream" />, the method uses a buffer to copy the contents to a byte array asynchronously.
    ///     Note that this method can throw an <see cref="ArgumentNullException" /> if the provided <paramref name="stream" />
    ///     is <c>null</c>. It is the responsibility of the caller to handle this exception as appropriate.
    /// </remarks>
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(stream);

        // Optimization for MemoryStream
        if (stream is MemoryStream sourceMemoryStream)
        {
            return sourceMemoryStream.ToArray();
        }

        // Optimization for seekable streams with known length
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];

            var totalBytesRead = 0;
            while (totalBytesRead < buffer.Length)
            {
                var bytesRead = await stream.ReadAsync(
                    buffer.AsMemory(totalBytesRead, buffer.Length - totalBytesRead),
                    cancellationToken).ConfigureAwait(false);

                if (bytesRead == 0)
                {
                    break; // End of stream reached
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead == buffer.Length ? buffer : buffer[..totalBytesRead];
        }

        // Handle non-seekable streams
        using var targetMemoryStream = new MemoryStream();
        await stream.CopyToAsync(targetMemoryStream, cancellationToken).ConfigureAwait(false);
        return targetMemoryStream.ToArray();
    }
}