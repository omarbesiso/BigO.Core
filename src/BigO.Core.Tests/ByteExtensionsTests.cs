using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ByteExtensionsTests
{
    [Fact]
    public void ToMemoryStream_OnNullBuffer_ReturnsNull()
    {
        // Arrange
        byte[] buffer = null!;

        // Act
        Assert.Throws<ArgumentNullException>(() => buffer.ToMemoryStream());
    }

    [Theory]
    [InlineData(new byte[] { 1, 2, 3 })]
    [InlineData(new byte[] { 4, 5, 6, 7 })]
    public void ToMemoryStream_OnBuffer_ReturnsMemoryStream(byte[] buffer)
    {
        // Act
        var result = buffer.ToMemoryStream();

        // Assert
        Assert.IsType<MemoryStream>(result);
        Assert.Equal(buffer, result.ToArray());
        Assert.Equal(0, result.Position);
    }
}