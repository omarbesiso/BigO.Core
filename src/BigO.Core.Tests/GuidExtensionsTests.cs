using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class GuidExtensionsTests
{
    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyGuid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act
        var result = emptyGuid.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForNonEmptyGuid()
    {
        // Arrange
        var nonEmptyGuid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var result = nonEmptyGuid.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_ReturnsTrue_ForNonEmptyGuid()
    {
        // Arrange
        var nonEmptyGuid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var result = nonEmptyGuid.IsNotEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNotEmpty_ReturnsFalse_ForEmptyGuid()
    {
        // Arrange
        var emptyGuid = Guid.Empty;

        // Act
        var result = emptyGuid.IsNotEmpty();

        // Assert
        Assert.False(result);
    }
}