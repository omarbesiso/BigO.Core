using BigO.Core.Types;

namespace BigO.Core.Tests;

public class DateTimeWithTimeZoneRangeTests
{
    [Fact]
    public void DateTimeWithTimeZoneRange_Contains_ShouldReturnTrueIfDateTimeIsWithinRange()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithinRange = start.AddMinutes(30);
        var dateTimeOutsideRange = start.AddHours(2);

        // Act
        var resultWithinRange = dateTimeWithTimeZoneRange.Contains(dateTimeWithinRange);
        var resultOutsideRange = dateTimeWithTimeZoneRange.Contains(dateTimeOutsideRange);

        // Assert
        Assert.True(resultWithinRange);
        Assert.False(resultOutsideRange);
    }

    [Fact]
    public void DateTimeWithTimeZoneRange_Overlaps_ShouldReturnTrueIfDateTimeRangeOverlaps()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var overlappingStart = start.AddMinutes(-30);
        var overlappingEnd = start.AddHours(2);
        var nonOverlappingStart = start.AddHours(-1);
        var nonOverlappingEnd = start.AddMinutes(-1);

        // Act
        var resultOverlapping =
            dateTimeWithTimeZoneRange.Overlaps(new DateTimeWithTimeZoneRange(overlappingStart, overlappingEnd));
        var resultNonOverlapping =
            dateTimeWithTimeZoneRange.Overlaps(new DateTimeWithTimeZoneRange(nonOverlappingStart, nonOverlappingEnd));

        // Assert
        Assert.True(resultOverlapping);
        Assert.False(resultNonOverlapping);
    }

    [Fact]
    public void DateTimeWithTimeZoneRange_Equals_ShouldReturnTrueIfStartAndEndAreEqual()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeEqual = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeNonEqual = new DateTimeWithTimeZoneRange(end, start);

        // Act
        var resultEqual = dateTimeWithTimeZoneRange.Equals(dateTimeWithTimeZoneRangeEqual);
        var resultNonEqual = dateTimeWithTimeZoneRange.Equals(dateTimeWithTimeZoneRangeNonEqual);

        // Assert
        Assert.True(resultEqual);
        Assert.False(resultNonEqual);
    }

    [Fact]
    public void DateTimeWithTimeZoneRange_GetHashCode_ShouldReturnHashCodeBasedOnStartAndEnd()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeEqual = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeNonEqual = new DateTimeWithTimeZoneRange(end, start);

        // Act
        var hashCode = dateTimeWithTimeZoneRange.GetHashCode();
        var hashCodeEqual = dateTimeWithTimeZoneRangeEqual.GetHashCode();
        var hashCodeNonEqual = dateTimeWithTimeZoneRangeNonEqual.GetHashCode();

        // Assert
        Assert.Equal(hashCode, hashCodeEqual);
        Assert.NotEqual(hashCode, hashCodeNonEqual);
    }

    [Fact]
    public void DateTimeWithTimeZoneRange_OperatorEqual_ShouldReturnTrueIfStartAndEndAreEqual()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeEqual = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeNonEqual = new DateTimeWithTimeZoneRange(end, start);

        // Act
        var resultEqual = dateTimeWithTimeZoneRange == dateTimeWithTimeZoneRangeEqual;
        var resultNonEqual = dateTimeWithTimeZoneRange == dateTimeWithTimeZoneRangeNonEqual;

        // Assert
        Assert.True(resultEqual);
        Assert.False(resultNonEqual);
    }

    [Fact]
    public void DateTimeWithTimeZoneRange_OperatorNotEqual_ShouldReturnTrueIfStartOrEndAreNotEqual()
    {
        // Arrange
        var start = new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
        var end = start.AddHours(1);
        var dateTimeWithTimeZoneRange = new DateTimeWithTimeZoneRange(start, end);
        var dateTimeWithTimeZoneRangeNonEqual = new DateTimeWithTimeZoneRange(end, start);

        // Act
        // ReSharper disable once EqualExpressionComparison
        var resultEqual = dateTimeWithTimeZoneRange != dateTimeWithTimeZoneRange;
        var resultNonEqual = dateTimeWithTimeZoneRange != dateTimeWithTimeZoneRangeNonEqual;

        // Assert
        Assert.False(resultEqual);
        Assert.True(resultNonEqual);
    }
}