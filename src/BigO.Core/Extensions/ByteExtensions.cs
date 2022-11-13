using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with byte objects.
/// </summary>
[PublicAPI]
public static class ByteExtensions
{
    /// <summary>
    ///     Gets a new <see cref="MemoryStream" /> instance created from the specified <see cref="T:byte[]" /> object.
    /// </summary>
    /// <param name="buffer">The <see cref="T:byte[]" /> object used to create the new <see cref="MemoryStream" /> instance.</param>
    /// <returns>A new <see cref="MemoryStream" /> instance created from the specified <see cref="T:byte[]" /> object.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="buffer" /> is <c>null</c>.</exception>
    public static MemoryStream ToMemoryStream(this byte[] buffer)
    {
        return new MemoryStream(buffer) { Position = 0 };
    }
}