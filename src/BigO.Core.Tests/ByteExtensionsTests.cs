using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ByteExtensionsTests
{
    public static IEnumerable<object[]> ToMemoryStreamTestData()
    {
        yield return new object[] { new byte[] { 1, 2, 3, 4, 5 } };
        yield return new object[] { new byte[] { 6, 7, 8, 9, 10 } };
        yield return new object[] { new byte[] { 11, 12, 13, 14, 15 } };
    }

    [Theory]
    [MemberData(nameof(ToMemoryStreamTestData))]
    public void ToMemoryStream_BufferProvided_ReturnsMemoryStream(byte[] buffer)
    {
        // Act
        var result = buffer.ToMemoryStream();

        // Assert
        Assert.IsType<MemoryStream>(result);
        Assert.Equal(buffer.Length, result.Length);
        Assert.Equal(buffer, result.ToArray());
    }
}