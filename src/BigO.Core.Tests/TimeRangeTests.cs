using BigO.Core.Types;

namespace BigO.Core.Tests;

public class TimeRangeTests
{
    [Fact]
    public void TimeRange_Equals_ReturnsFalse_WhenCalledWithUnequalTimeRange()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(9, 0, 0), new TimeOnly(17, 0, 0));
        var timeRange2 = new TimeRange(new TimeOnly(10, 0, 0), new TimeOnly(17, 0, 0));

        // Act
        var result = timeRange1.Equals(timeRange2);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void TimeRange_Equals_ReturnsFalse_WhenCalledWithDifferentTimeRange()
    {
        // Arrange
        var startTime1 = new TimeOnly(8, 0);
        var endTime1 = new TimeOnly(17, 0);
        var timeRange1 = new TimeRange(startTime1, endTime1);

        var startTime2 = new TimeOnly(9, 0);
        var endTime2 = new TimeOnly(17, 0);
        var timeRange2 = new TimeRange(startTime2, endTime2);

        // Act
        var result = timeRange1.Equals(timeRange2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TimeRange_Equals_ReturnsTrue_WhenCalledWithSameTimeRange()
    {
        // Arrange
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(17, 0);
        var timeRange = new TimeRange(startTime, endTime);

        // Act
        var result = timeRange.Equals(timeRange);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TimeRange_Equals_ReturnsFalse_WhenCalledWithNull()
    {
        // Arrange
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(17, 0);
        var timeRange = new TimeRange(startTime, endTime);

        // Act
        var result = timeRange.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TimeRange_GetHashCode_ReturnsDifferentHashCode_WhenCalledWithDifferentTimeRange()
    {
        // Arrange
        var startTime1 = new TimeOnly(8, 0);
        var endTime1 = new TimeOnly(17, 0);
        var timeRange1 = new TimeRange(startTime1, endTime1);

        var startTime2 = new TimeOnly(9, 0);
        var endTime2 = new TimeOnly(17, 0);
        var timeRange2 = new TimeRange(startTime2, endTime2);

        // Act
        var hashCode1 = timeRange1.GetHashCode();
        var hashCode2 = timeRange2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void TimeRange_OperatorEqual_ReturnsTrue_WhenCalledWithEqualTimeRange()
    {
        // Arrange
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(17, 0);
        var timeRange1 = new TimeRange(startTime, endTime);
        var timeRange2 = new TimeRange(startTime, endTime);

        // Act
        var result = timeRange1 == timeRange2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TimeRange_OperatorEqual_ReturnsFalse_WhenCalledWithDifferentTimeRange()
    {
        // Arrange
        var startTime1 = new TimeOnly(8, 0);
        var endTime1 = new TimeOnly(17, 0);
        var timeRange1 = new TimeRange(startTime1, endTime1);

        var startTime2 = new TimeOnly(9, 0);
        var endTime2 = new TimeOnly(17, 0);
        var timeRange2 = new TimeRange(startTime2, endTime2);

        // Act
        var result = timeRange1 == timeRange2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TimeRange_OperatorNotEqual_ReturnsTrue_WhenCalledWithDifferentTimeRange()
    {
        // Arrange
        var startTime1 = new TimeOnly(8, 0);
        var endTime1 = new TimeOnly(17, 0);
        var timeRange1 = new TimeRange(startTime1, endTime1);

        var startTime2 = new TimeOnly(9, 0);
        var endTime2 = new TimeOnly(17, 0);
        var timeRange2 = new TimeRange(startTime2, endTime2);

        // Act
        var result = timeRange1 != timeRange2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TimeRange_Contains_ReturnsTrue_WhenCalledWithTimeWithinRange()
    {
        // Arrange
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(17, 0);
        var timeRange = new TimeRange(startTime, endTime);
        var time = new TimeOnly(12, 0);

        // Act
        var result = timeRange.Contains(time);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TimeRange_Contains_ReturnsFalse_WhenCalledWithTimeOutsideRange()
    {
        // Arrange
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(17, 0);
        var timeRange = new TimeRange(startTime, endTime);
        var time = new TimeOnly(18, 0);

        // Act
        var result = timeRange.Contains(time);

        // Assert
        Assert.False(result);
    }
}