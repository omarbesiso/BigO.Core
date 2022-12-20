using BigO.Core.Types;

namespace BigO.Core.Tests;

public class DateRangeTests
{
    [Fact]
    public void DateRange_Constructor_ThrowsArgumentException_WhenEndDateIsBeforeStartDate()
    {
        // Arrange
        var startDate = new DateOnly(2020, 1, 1);
        var endDate = new DateOnly(2019, 12, 31);

        // Assert
        Assert.Throws<ArgumentException>(() => new DateRange(startDate, endDate));
    }

    [Fact]
    public void DateRange_Equals_ReturnsTrue_WhenComparingEqualInstances()
    {
        // Arrange
        var dateRange1 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2));
        var dateRange2 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2));

        // Act
        var result = dateRange1.Equals(dateRange2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateRange_Equals_ReturnsFalse_WhenComparingNonEqualInstances()
    {
        // Arrange
        var dateRange1 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2));
        var dateRange2 = new DateRange(new DateOnly(2020, 1, 2), new DateOnly(2020, 1, 3));

        // Act
        var result = dateRange1.Equals(dateRange2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateRange_GetHashCode_ReturnsEqualValues_ForEqualInstances()
    {
        // Arrange
        var dateRange1 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2));
        var dateRange2 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2));

        // Act
        var hashCode1 = dateRange1.GetHashCode();
        var hashCode2 = dateRange2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void DateRange_Contains_ReturnsTrue_WhenDateIsWithinRange()
    {
        // Arrange
        var dateRange = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 3));
        var date = new DateOnly(2020, 1, 2);

        // Act
        var result = dateRange.Contains(date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateRange_Contains_ReturnsFalse_WhenDateIsBeforeRange()
    {
        // Arrange
        var dateRange = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 3));
        var date = new DateOnly(2019, 12, 31);

        // Act
        var result = dateRange.Contains(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateRange_Contains_ReturnsFalse_WhenDateIsAfterRange()
    {
        // Arrange
        var dateRange = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 3));
        var date = new DateOnly(2020, 1, 4);

        // Act
        var result = dateRange.Contains(date);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateRange_Overlaps_ReturnsTrue_WhenRangesOverlap()
    {
        // Arrange
        var dateRange1 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 3));
        var dateRange2 = new DateRange(new DateOnly(2020, 1, 2), new DateOnly(2020, 1, 4));

        // Act
        var result = dateRange1.Overlaps(dateRange2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateRange_Overlaps_ReturnsFalse_WhenRangesDoNotOverlap()
    {
        // Arrange
        var dateRange1 = new DateRange(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 3));
        var dateRange2 = new DateRange(new DateOnly(2020, 1, 4), new DateOnly(2020, 1, 5));

        // Act
        var result = dateRange1.Overlaps(dateRange2);

        // Assert
        Assert.False(result);
    }
}