namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Stream" /> objects.
/// </summary>
[PublicAPI]
public static class StreamExtensions
{
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
    ///     <see cref="MemoryStream" />, the method creates a new <see cref="MemoryStream" /> with an initial capacity matching
    ///     the length of the input <paramref name="stream" /> or a default size of 8192 bytes if the length is unknown or 0.
    ///     The method copies the contents of the input <paramref name="stream" /> to the new <see cref="MemoryStream" /> using
    ///     the <see cref="Stream.CopyTo(System.IO.Stream)" /> method. Finally, the method calls
    ///     <see cref="MemoryStream.ToArray" /> on the new <see cref="MemoryStream" /> to obtain the byte array representation
    ///     of the input <paramref name="stream" />.
    ///     Note that this method can throw an <see cref="ArgumentNullException" /> if the provided <paramref name="stream" />
    ///     is <c>null</c>. It is the responsibility of the caller to handle this exception as appropriate.
    /// </remarks>
    public static byte[] ToByteArray(this Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream), $"The {nameof(stream)} cannot be null.");
        }

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using (memoryStream = new MemoryStream((int)(stream.Length == 0 ? 8192 : stream.Length)))
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    /// <summary>
    ///     Asynchronously reads the bytes from the current stream and returns them as a byte array. If the stream is a
    ///     <see cref="MemoryStream" />, it returns the underlying buffer.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns>A byte array containing the contents of the stream.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stream" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     If <paramref name="stream" /> is a <see cref="MemoryStream" />, the underlying buffer is returned as a byte array.
    ///     If <paramref name="stream" /> is not a <see cref="MemoryStream" />, the contents of the stream are read
    ///     asynchronously into a new <see cref="MemoryStream" /> with an initial capacity of either 8192 or the length of
    ///     <paramref name="stream" />, whichever is greater, and returned as a byte array.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the ToByteArrayAsync method to read a stream and return its contents as a
    ///     byte array.
    ///     <code><![CDATA[
    /// using System.IO;
    /// using System.Text;
    /// 
    /// class Example
    /// {
    ///     static async Task Main()
    ///     {
    ///         string text = "This is a test string.";
    ///         byte[] bytes = Encoding.UTF8.GetBytes(text);
    ///         using (MemoryStream stream = new MemoryStream(bytes))
    ///         {
    ///             byte[] result = await stream.ToByteArrayAsync();
    ///             Console.WriteLine(Encoding.UTF8.GetString(result));
    ///         }
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream), $"The {nameof(stream)} cannot be null.");
        }

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using (memoryStream = new MemoryStream((int)(stream.Length == 0 ? 8192 : stream.Length)))
        {
            await stream.CopyToAsync(memoryStream);
            // ReSharper disable once MethodHasAsyncOverload
            return memoryStream.ToByteArray();
        }
    }
}