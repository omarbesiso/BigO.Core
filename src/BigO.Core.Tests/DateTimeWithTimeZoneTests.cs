using BigO.Core.Types;

namespace BigO.Core.Tests;

public class DateTimeWithTimeZoneTests
{
    [Fact]
    public void DateTimeWithTimeZone_Constructor_SetsValue()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var dateTimeWithTimeZone = new DateTimeWithTimeZone(value, timeZone);

        // Assert
        Assert.Equal(value, dateTimeWithTimeZone.Value);
    }

    [Fact]
    public void DateTimeWithTimeZone_Constructor_SetsTimeZone()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var dateTimeWithTimeZone = new DateTimeWithTimeZone(value, timeZone);

        // Assert
        Assert.Equal(timeZone, dateTimeWithTimeZone.TimeZone);
    }

    [Fact]
    public void DateTimeWithTimeZone_DateTimeOffset_ReturnsCorrectValue()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var dateTimeOffset = dateTimeWithTimeZone.DateTimeOffset;

        // Assert
        Assert.Equal(value, dateTimeOffset.DateTime);
        Assert.Equal(timeZone.GetUtcOffset(value), dateTimeOffset.Offset);
    }

    [Fact]
    public void DateTimeWithTimeZone_ToUtcDateTime_ReturnsCorrectValue()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var dateTimeWithTimeZone = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var utcDateTime = dateTimeWithTimeZone.ToUtcDateTime();

        // Assert
        Assert.Equal(TimeZoneInfo.ConvertTimeToUtc(value, timeZone), utcDateTime);
    }

    [Fact]
    public void DateTimeWithTimeZone_CompareTo_ReturnsCorrectValue()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);

        var value2 = new DateTime(2022, 1, 2);
        var timeZone2 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var result = dateTimeWithTimeZone1.CompareTo(dateTimeWithTimeZone2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void DateTimeWithTimeZone_Equals_ReturnsTrueForEqualInstances()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value, timeZone);
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var result = dateTimeWithTimeZone1.Equals(dateTimeWithTimeZone2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_Equals_ReturnsFalseForDifferentValues()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);

        var value2 = new DateTime(2022, 1, 2);
        var timeZone2 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var result = dateTimeWithTimeZone1.Equals(dateTimeWithTimeZone2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_Equals_ReturnsFalseForDifferentTimeZones()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);

        var value2 = new DateTime(2022, 1, 1);
        var timeZone2 = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var result = dateTimeWithTimeZone1.Equals(dateTimeWithTimeZone2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_GetHashCode_ReturnsSameValueForEqualInstances()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value, timeZone);
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var hashCode1 = dateTimeWithTimeZone1.GetHashCode();
        var hashCode2 = dateTimeWithTimeZone2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void DateTimeWithTimeZone_GetHashCode_ReturnsDifferentValuesForDifferentInstances()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);
        var value2 = new DateTime(2022, 1, 2);
        var timeZone2 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var hashCode1 = dateTimeWithTimeZone1.GetHashCode();
        var hashCode2 = dateTimeWithTimeZone2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void DateTimeWithTimeZone_OperatorEquals_ReturnsTrueForEqualInstances()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value, timeZone);
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var result = dateTimeWithTimeZone1 == dateTimeWithTimeZone2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_OperatorEquals_ReturnsFalseForDifferentValues()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);

        var value2 = new DateTime(2022, 1, 2);
        var timeZone2 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var result = dateTimeWithTimeZone1 == dateTimeWithTimeZone2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_OperatorEquals_ReturnsFalseForDifferentTimeZones()
    {
        // Arrange
        var value1 = new DateTime(2022, 1, 1);
        var timeZone1 = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value1, timeZone1);

        var value2 = new DateTime(2022, 1, 1);
        var timeZone2 = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value2, timeZone2);

        // Act
        var result = dateTimeWithTimeZone1 == dateTimeWithTimeZone2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateTimeWithTimeZone_OperatorNotEquals_ReturnsFalseForEqualInstances()
    {
        // Arrange
        var value = new DateTime(2022, 1, 1);
        var timeZone = TimeZoneInfo.Utc;
        var dateTimeWithTimeZone1 = new DateTimeWithTimeZone(value, timeZone);
        var dateTimeWithTimeZone2 = new DateTimeWithTimeZone(value, timeZone);

        // Act
        var result = dateTimeWithTimeZone1 != dateTimeWithTimeZone2;

        // Assert
        Assert.False(result);
    }
}