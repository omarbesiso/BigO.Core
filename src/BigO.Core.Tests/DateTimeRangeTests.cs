using BigO.Core.Types;

namespace BigO.Core.Tests;

public class DateTimeRangeTests
{
    [Fact]
    public void DateTimeRange_Constructor_ThrowsArgumentException_WhenEndDateTimeIsBeforeStartDateTime()
    {
        // Arrange
        var startDateTime = new DateTime(2020, 1, 1, 12, 0, 0);
        var endDateTime = new DateTime(2020, 1, 1, 11, 59, 59);

        // Assert
        Assert.Throws<ArgumentException>(() => new DateTimeRange(startDateTime, endDateTime));
    }

    [Fact]
    public void DateTimeRange_Contains_ReturnsTrue_WhenDateTimeIsWithinRange()
    {
        // Arrange
        var dateTimeRange = new DateTimeRange(new DateTime(2020, 1, 1, 12, 0, 0), new DateTime(2020, 1, 1, 13, 0, 0));
        var dateTime = new DateTime(2020, 1, 1, 12, 30, 0);

        // Act
        var result = dateTimeRange.Contains(dateTime);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateTimeRange_Contains_ReturnsFalse_WhenDateTimeIsBeforeRange()
    {
        // Arrange
        var dateTimeRange = new DateTimeRange(new DateTime(2020, 1, 1, 12, 0, 0), new DateTime(2020, 1, 1, 13, 0, 0));
        var dateTime = new DateTime(2020, 1, 1, 11, 59, 59);

        // Act
        var result = dateTimeRange.Contains(dateTime);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeRange_Contains_ReturnsFalse_WhenDateTimeIsAfterRange()
    {
        // Arrange
        var dateTimeRange = new DateTimeRange(new DateTime(2020, 1, 1, 12, 0, 0), new DateTime(2020, 1, 1, 13, 0, 0));
        var dateTime = new DateTime(2020, 1, 1, 13, 0, 1);

        // Act
        var result = dateTimeRange.Contains(dateTime);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeRange_Overlaps_ReturnsTrue_WhenRangesOverlap()
    {
        // Arrange
        var dateTimeRange1 = new DateTimeRange(new DateTime(2020, 1, 1, 12, 0, 0), new DateTime(2020, 1, 1, 13, 0, 0));
        var dateTimeRange2 =
            new DateTimeRange(new DateTime(2020, 1, 1, 12, 30, 0), new DateTime(2020, 1, 1, 13, 30, 0));

        // Act
        var result = dateTimeRange1.Overlaps(dateTimeRange2);

        // Assert
        Assert.True(result);
    }
}