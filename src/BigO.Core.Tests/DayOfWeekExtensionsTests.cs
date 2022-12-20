using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class DayOfWeekExtensionsTests
{
    [Fact]
    public void Increment_Sunday_ReturnsMonday()
    {
        // Arrange
        var input = DayOfWeek.Sunday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Monday, result);
    }

    [Fact]
    public void Increment_Monday_ReturnsTuesday()
    {
        // Arrange
        var input = DayOfWeek.Monday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Tuesday, result);
    }

    [Fact]
    public void Increment_Tuesday_ReturnsWednesday()
    {
        // Arrange
        var input = DayOfWeek.Tuesday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Wednesday, result);
    }

    [Fact]
    public void Increment_Wednesday_ReturnsThursday()
    {
        // Arrange
        var input = DayOfWeek.Wednesday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Thursday, result);
    }

    [Fact]
    public void Increment_Thursday_ReturnsFriday()
    {
        // Arrange
        var input = DayOfWeek.Thursday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Friday, result);
    }

    [Fact]
    public void Increment_Friday_ReturnsSaturday()
    {
        // Arrange
        var input = DayOfWeek.Friday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Saturday, result);
    }

    [Fact]
    public void Increment_Saturday_ReturnsSunday()
    {
        // Arrange
        var input = DayOfWeek.Saturday;

        // Act
        var result = input.Increment();

        // Assert
        Assert.Equal(DayOfWeek.Sunday, result);
    }

    [Theory]
    [InlineData(DayOfWeek.Sunday, 2, DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Monday, 3, DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Tuesday, 4, DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Wednesday, 5, DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Thursday, 6, DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Friday, 7, DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday, 8, DayOfWeek.Sunday)]
    public void Increment_MultipleDays_ReturnsCorrectDay(DayOfWeek input, int numberOfDays, DayOfWeek expected)
    {
        // Act
        var result = input.Increment(numberOfDays);

        // Assert
        Assert.Equal(expected, result);
    }
}