using BigO.Core.Types;

namespace BigO.Core.Tests;

public class TimeRangeTests
{
    [Fact]
    public void Constructor_EndTimeIsBeforeStartTime_ThrowsArgumentException()
    {
        // Arrange
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(9, 0);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new TimeRange(startTime, endTime));
    }

    [Fact]
    public void Constructor_EndTimeIsAfterStartTime_CreatesValidTimeRange()
    {
        // Arrange
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(11, 0);

        // Act
        var timeRange = new TimeRange(startTime, endTime);

        // Assert
        Assert.Equal(startTime, timeRange.StartTime);
        Assert.Equal(endTime, timeRange.EndTime);
    }

    [Fact]
    public void Duration_ReturnsCorrectDuration()
    {
        // Arrange
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(12, 0);
        var timeRange = new TimeRange(startTime, endTime);

        // Act
        var duration = timeRange.Duration;

        // Assert
        Assert.Equal(TimeSpan.FromHours(2), duration);
    }

    [Fact]
    public void Duration_WhenStartTimeEqualsEndTime_ReturnsZeroDuration()
    {
        // Arrange
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(10, 0);
        var timeRange = new TimeRange(startTime, endTime);

        // Act
        var duration = timeRange.Duration;

        // Assert
        Assert.Equal(TimeSpan.Zero, duration);
    }

    [Fact]
    public void Duration_WhenEndTimeIsBeforeStartTime_ThrowsArgumentException()
    {
        // Arrange
        var startTime = new TimeOnly(10, 0);
        var endTime = new TimeOnly(9, 0);

        // Act and assert
        Assert.Throws<ArgumentException>(() => new TimeRange(startTime, endTime));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenComparingEqualTimeRanges()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var timeRange2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));

        // Act
        var result = timeRange1.Equals(timeRange2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparingDifferentTimeRanges()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var timeRange2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 30));

        // Act
        var result = timeRange1.Equals(timeRange2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparingDifferentTypes()
    {
        // Arrange
        var timeRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var otherObject = new object();

        // Act
        var result = timeRange.Equals(otherObject);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_ReturnsTrue_WhenTimeIsWithinRange()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);
        var timeRange = new TimeRange(start, end);
        var time = new TimeOnly(11, 0);

        // Act
        var result = timeRange.Contains(time);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ReturnsFalse_WhenTimeIsBeforeRange()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);
        var timeRange = new TimeRange(start, end);
        var time = new TimeOnly(9, 0);

        // Act
        var result = timeRange.Contains(time);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_ReturnsFalse_WhenTimeIsAfterRange()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);
        var timeRange = new TimeRange(start, end);
        var time = new TimeOnly(13, 0);

        // Act
        var result = timeRange.Contains(time);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_ReturnsTrue_WhenTimeIsEqualToStartOfRange()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);
        var timeRange = new TimeRange(start, end);

        // Act
        var result = timeRange.Contains(start);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ReturnsTrue_WhenTimeIsEqualToEndOfRange()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(12, 0);
        var timeRange = new TimeRange(start, end);

        // Act
        var result = timeRange.Contains(end);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryParse_ValidString_ReturnsTrue()
    {
        // Arrange
        var input = "12:00 - 13:00";
        TimeRange result;

        // Act
        var success = TimeRange.TryParse(input, out result);

        // Assert
        Assert.True(success);
    }

    [Fact]
    public void TryParse_ValidString_SetsStartTime()
    {
        // Arrange
        var input = "12:00 - 13:00";
        TimeRange result;

        // Act
        TimeRange.TryParse(input, out result);

        // Assert
        Assert.Equal(new TimeOnly(12, 0), result.StartTime);
    }

    [Fact]
    public void TryParse_ValidString_SetsEndTime()
    {
        // Arrange
        var input = "12:00 - 13:00";
        TimeRange result;

        // Act
        TimeRange.TryParse(input, out result);

        // Assert
        Assert.Equal(new TimeOnly(13, 0), result.EndTime);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        // Arrange
        var input = "invalid string";

        // Act
        var success = TimeRange.TryParse(input, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryParse_EmptyString_ReturnsFalse()
    {
        // Arrange
        var input = "";

        // Act
        var success = TimeRange.TryParse(input, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryParse_InvalidStartTime_ReturnsFalse()
    {
        // Arrange
        var input = "25:00 - 13:00";

        // Act
        var success = TimeRange.TryParse(input, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryParse_InvalidEndTime_ReturnsFalse()
    {
        // Arrange
        var input = "12:00 - 25:00";

        // Act
        var success = TimeRange.TryParse(input, out _);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void Union_ReturnsUnionOfTwoRanges()
    {
        // Arrange
        var firstRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var secondRange = new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0));

        // Act
        var result = firstRange.Union(secondRange);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(10, 0), new TimeOnly(13, 0)), result);
    }

    [Fact]
    public void Union_ReturnsSelfWhenOverlappingRangesAreEqual()
    {
        // Arrange
        var firstRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var secondRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));

        // Act
        var result = firstRange.Union(secondRange);

        // Assert
        Assert.Equal(firstRange, result);
    }

    [Fact]
    public void Union_ReturnsUnionWithEarlierStartTime()
    {
        // Arrange
        var firstRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var secondRange = new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0));

        // Act
        var result = firstRange.Union(secondRange);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(9, 0), new TimeOnly(12, 0)), result);
    }

    [Fact]
    public void Union_ReturnsUnionWithLaterEndTime()
    {
        // Arrange
        var firstRange = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var secondRange = new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0));

        // Act
        var result = firstRange.Union(secondRange);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(10, 0), new TimeOnly(13, 0)), result);
    }

    [Fact]
    public void Merge_ReturnsExpectedResult_ForOverlappingRanges()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0));

        // Act
        var result = range1.Merge(range2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(10, 0), new TimeOnly(13, 0)), result);
    }

    [Fact]
    public void Merge_ThrowsInvalidOperationException_ForNonOverlappingRanges()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0));
        var range2 = new TimeRange(new TimeOnly(12, 0), new TimeOnly(13, 0));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => range1.Merge(range2));
    }

    [Fact]
    public void Merge_ReturnsExpectedResult_ForEqualRanges()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));

        // Act
        var result = range1.Merge(range2);

        // Assert
        Assert.Equal(range1, result);
    }

    [Fact]
    public void Merge_ReturnsExpectedResult_ForNestedRanges()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(13, 0));
        var range2 = new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Act
        var result = range1.Merge(range2);

        // Assert
        Assert.Equal(range1, result);
    }

    [Fact]
    public void Merge_ReturnsExpectedResult_ForAdjacentRanges()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0));
        var range2 = new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Act
        var result = range1.Merge(range2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0)), result);
    }

    [Fact]
    public void Intersect_Returns_Null_When_No_Overlap()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(9, 0));
        var timeRange2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Intersect_Returns_Overlapping_Range()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(10, 0));
        var timeRange2 = new TimeRange(new TimeOnly(9, 0), new TimeOnly(11, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), result);
    }

    [Fact]
    public void Intersect_Returns_Null_When_Touching_Ranges()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(10, 0));
        var timeRange2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(10, 0), new TimeOnly(10, 0)), result);
    }

    [Fact]
    public void Intersect_Returns_Null_When_Contained_Range()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(11, 0));
        var timeRange2 = new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), result);
    }

    [Fact]
    public void Intersect_Returns_Null_When_Overlap_Is_On_End()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(10, 0));
        var timeRange2 = new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), result);
    }

    [Fact]
    public void Intersect_Returns_Null_When_Overlap_Is_On_Start()
    {
        // Arrange
        var timeRange1 = new TimeRange(new TimeOnly(9, 0), new TimeOnly(10, 0));
        var timeRange2 = new TimeRange(new TimeOnly(8, 0), new TimeOnly(9, 0));

        // Act
        var result = timeRange1.Intersect(timeRange2);

        // Assert
        Assert.Equal(new TimeRange(new TimeOnly(9, 0), new TimeOnly(9, 0)), result);
    }

    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesOverlap()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(11, 0), new TimeOnly(13, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.True(overlaps);
    }

    [Fact]
    public void Overlaps_ReturnsFalse_WhenRangesDoNotOverlap()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(13, 0), new TimeOnly(14, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.False(overlaps);
    }

    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesHaveTheSameStartTime()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.True(overlaps);
    }

    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesHaveTheSameEndTime()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(11, 0), new TimeOnly(12, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.True(overlaps);
    }

    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesAreEqual()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.True(overlaps);
    }

    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesOverlapAtTheBoundary()
    {
        // Arrange
        var range1 = new TimeRange(new TimeOnly(10, 0), new TimeOnly(12, 0));
        var range2 = new TimeRange(new TimeOnly(12, 0), new TimeOnly(13, 0));

        // Act
        var overlaps = range1.Overlaps(range2);

        // Assert
        Assert.True(overlaps);
    }

    [Theory]
    [InlineData("10:00:00", true)]
    [InlineData("09:59:59", false)]
    [InlineData("12:00:00", true)]
    [InlineData("11:00:00", true)]
    [InlineData("11:30:00", true)]
    public void Contains_ReturnsExpectedResult(string timeString, bool expectedResult)
    {
        var startTime = new TimeOnly(10, 0, 0);
        var endTime = new TimeOnly(12, 0, 0);
        var timeRange = new TimeRange(startTime, endTime);

        var time = TimeOnly.Parse(timeString);

        var result = timeRange.Contains(time);

        Assert.Equal(expectedResult, result);
    }
}