using System.Text;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public void ToByteArray_ReturnsCorrectByteArray_ForValidInput()
    {
        // Arrange
        const string input = "hello world";
        var expectedOutput = Encoding.UTF8.GetBytes(input);
        using var inputStream = new MemoryStream(expectedOutput);

        // Act
        var result = inputStream.ToByteArray();

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void ToByteArray_ThrowsArgumentNullException_ForNullStream()
    {
        // Arrange
        Stream stream = null!;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stream.ToByteArray());
    }

    [Fact]
    public void ToByteArrayAsync_ReturnsCorrectByteArray_ForValidInput()
    {
        // Arrange
        const string input = "hello world";
        var expectedOutput = Encoding.UTF8.GetBytes(input);
        using var inputStream = new MemoryStream(expectedOutput);

        // Act
        var result = inputStream.ToByteArrayAsync().Result;

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void ToByteArrayAsync_ThrowsArgumentNullException_ForNullStream()
    {
        // Arrange
        Stream stream = null!;

        // Act and assert
        Assert.ThrowsAsync<ArgumentNullException>(() => stream.ToByteArrayAsync());
    }
}