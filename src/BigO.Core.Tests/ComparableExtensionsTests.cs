using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ComparableExtensionsTests
{
    [Theory]
    [InlineData(1, 0, 2, true)]
    [InlineData(1, 1, 2, true)]
    [InlineData(1, 2, 3, false)]
    public void IsBetween_ReturnsExpectedResult_ForIntValues(int value, int lowerBoundary, int upperBoundary,
        bool expectedResult)
    {
        // Act
        var result = value.IsBetween(lowerBoundary, upperBoundary);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("a", "a", "z", true)]
    [InlineData("b", "a", "z", true)]
    [InlineData("z", "a", "z", true)]
    [InlineData("a", "d", "z", false)]
    public void IsBetween_ReturnsExpectedResult_ForStringValues(string value, string lowerBoundary,
        string upperBoundary, bool expectedResult)
    {
        // Act
        var result = value.IsBetween(lowerBoundary, upperBoundary);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1.0, 0.0, 2.0, true)]
    [InlineData(1.5, 1.0, 2.0, true)]
    [InlineData(1.0, 2.0, 3.0, false)]
    public void IsBetween_ReturnsExpectedResult_ForDoubleValues(double value, double lowerBoundary,
        double upperBoundary, bool expectedResult)
    {
        // Act
        var result = value.IsBetween(lowerBoundary, upperBoundary);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Limit_ValueLessThanMaximum_ReturnsValue()
    {
        // Arrange
        var value = 10;
        var maximum = 20;

        // Act
        var result = value.Limit(maximum);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Limit_ValueGreaterThanMaximum_ReturnsMaximum()
    {
        // Arrange
        var value = 30;
        var maximum = 20;

        // Act
        var result = value.Limit(maximum);

        // Assert
        Assert.Equal(maximum, result);
    }

    [Fact]
    public void Limit_ValueEqualToMaximum_ReturnsMaximum()
    {
        // Arrange
        var value = 20;
        var maximum = 20;

        // Act
        var result = value.Limit(maximum);

        // Assert
        Assert.Equal(maximum, result);
    }

    [Fact]
    public void Limit_ValueLessThanMinimum_ReturnsMinimum()
    {
        // Arrange
        var value = 10;
        var minimum = 20;
        var maximum = 30;

        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(minimum, result);
    }

    [Fact]
    public void Limit2_ValueGreaterThanMaximum_ReturnsMaximum()
    {
        // Arrange
        var value = 40;
        var minimum = 20;
        var maximum = 30;

        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(maximum, result);
    }

    [Fact]
    public void Limit_ValueBetweenMinimumAndMaximum_ReturnsValue()
    {
        // Arrange
        var value = 25;
        var minimum = 20;
        var maximum = 30;

        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(value, result);
    }
}