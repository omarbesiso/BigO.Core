using System.Globalization;
using BigO.Core.Extensions;

// ReSharper disable PossibleMultipleEnumeration

namespace BigO.Core.Tests;

public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData(1990, 1, 1, 2020, 1, 1, 30)]
    [InlineData(1990, 1, 1, 2020, 6, 30, 30)]
    [InlineData(1990, 1, 1, 2020, 12, 31, 30)]
    [InlineData(1990, 1, 1, 2021, 1, 1, 31)]
    [InlineData(1990, 1, 1, 2021, 1, 2, 31)]
    public void Age_CorrectAgeInYears(int birthYear, int birthMonth, int birthDay, int maturityYear, int maturityMonth,
        int maturityDay, int expectedAge)
    {
        // Arrange
        var dateOfBirth = new DateTime(birthYear, birthMonth, birthDay);
        var maturityDate = new DateTime(maturityYear, maturityMonth, maturityDay);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(expectedAge, age);
    }

    [Fact]
    public void Age_NoMaturityDate_UsesCurrentDate()
    {
        // Arrange
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;
        var currentDay = DateTime.Now.Day;
        var dateOfBirth = new DateTime(currentYear - 30, currentMonth, currentDay);

        // Act
        var age = dateOfBirth.Age();

        // Assert
        Assert.Equal(30, age);
    }

    [Theory]
    [InlineData(2022, 12, 24, 1, 2022, 12, 31)]
    [InlineData(2022, 12, 24, 2, 2023, 1, 7)]
    [InlineData(2022, 12, 24, 3, 2023, 1, 14)]
    [InlineData(2022, 12, 24, 0.5, 2022, 12, 28)]
    [InlineData(2022, 12, 24, 0.25, 2022, 12, 26)]
    [InlineData(2022, 12, 24, 0.125, 2022, 12, 25)]
    public void AddWeeks_AddsCorrectNumberOfWeeks(int year, int month, int day, double weeks, int expectedYear,
        int expectedMonth, int expectedDay)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.AddWeeks(weeks);

        // Assert
        Assert.Equal(expectedYear, result.Year);
        Assert.Equal(expectedMonth, result.Month);
        Assert.Equal(expectedDay, result.Day);
    }

    [Fact]
    public void AddWeeks_AddsCorrectNumberOfWeeks_WhenNumberOfWeeksIsFractional()
    {
        // Arrange
        var date = new DateTime(2022, 12, 24);

        // Act
        var result = date.AddWeeks(1.5);

        // Assert
        Assert.Equal(2023, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(4, result.Day);
    }

    [Fact]
    public void AddWeeks_AddsCorrectNumberOfWeeks_WhenNumberOfWeeksIsNegative()
    {
        // Arrange
        var date = new DateTime(2022, 12, 24);

        // Act
        var result = date.AddWeeks(-1);

        // Assert
        Assert.Equal(2022, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(17, result.Day);
    }

    [Fact]
    public void AddWeeks_DoesNotChangeOriginalDate()
    {
        // Arrange
        var date = new DateTime(2022, 12, 24);
        var originalDate = date;

        // Act
        date.AddWeeks(1);

        // Assert
        Assert.Equal(originalDate, date);
    }

    [Theory]
    [InlineData(2020, 2, 29)]
    [InlineData(2021, 2, 28)]
    [InlineData(2022, 2, 28)]
    [InlineData(2020, 4, 30)]
    [InlineData(2021, 4, 30)]
    [InlineData(2022, 4, 30)]
    [InlineData(2020, 6, 30)]
    [InlineData(2021, 6, 30)]
    [InlineData(2022, 6, 30)]
    [InlineData(2020, 9, 30)]
    [InlineData(2021, 9, 30)]
    [InlineData(2022, 9, 30)]
    [InlineData(2020, 11, 30)]
    [InlineData(2021, 11, 30)]
    [InlineData(2022, 11, 30)]
    [InlineData(2020, 1, 31)]
    [InlineData(2021, 1, 31)]
    [InlineData(2022, 1, 31)]
    [InlineData(2020, 3, 31)]
    [InlineData(2021, 3, 31)]
    [InlineData(2022, 3, 31)]
    [InlineData(2020, 5, 31)]
    [InlineData(2021, 5, 31)]
    [InlineData(2022, 5, 31)]
    [InlineData(2020, 7, 31)]
    [InlineData(2021, 7, 31)]
    [InlineData(2022, 7, 31)]
    [InlineData(2020, 8, 31)]
    [InlineData(2021, 8, 31)]
    [InlineData(2022, 8, 31)]
    [InlineData(2020, 10, 31)]
    [InlineData(2021, 10, 31)]
    [InlineData(2022, 10, 31)]
    [InlineData(2020, 12, 31)]
    [InlineData(2021, 12, 31)]
    [InlineData(2022, 12, 31)]
    public void DaysInMonth_ReturnsCorrectNumberOfDays(int year, int month, int expectedDays)
    {
        // Arrange
        var date = new DateTime(year, month, 1);

        // Act
        var result = date.DaysInMonth();

        // Assert
        Assert.Equal(expectedDays, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Monday, 2020, 1, 6)]
    [InlineData(2020, 1, 31, DayOfWeek.Monday, 2020, 1, 6)]
    [InlineData(2020, 2, 29, DayOfWeek.Sunday, 2020, 2, 2)]
    [InlineData(2020, 2, 29, DayOfWeek.Friday, 2020, 2, 7)]
    public void GetFirstDateOfMonth_WithDayOfWeek_ReturnsCorrectDate(int year, int month, int day, DayOfWeek dayOfWeek,
        int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetFirstDateOfMonth(dayOfWeek);

        // Assert
        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay), result);
    }

    [Theory]
    [InlineData(2020, 1, 1)]
    [InlineData(2020, 1, 31)]
    [InlineData(2020, 2, 29)]
    public void GetFirstDateOfMonth_WithoutDayOfWeek_ReturnsFirstDateOfMonth(int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetFirstDateOfMonth();

        // Assert
        Assert.Equal(new DateTime(year, month, 1), result);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Monday, 2019, 12, 30)]
    [InlineData(2020, 1, 1, DayOfWeek.Sunday, 2019, 12, 29)]
    [InlineData(2020, 1, 1, DayOfWeek.Tuesday, 2019, 12, 31)]
    [InlineData(2020, 1, 5, DayOfWeek.Monday, 2019, 12, 30)]
    [InlineData(2020, 1, 5, DayOfWeek.Sunday, 2020, 1, 5)]
    [InlineData(2020, 1, 5, DayOfWeek.Tuesday, 2019, 12, 31)]
    public void GetFirstDateOfWeek_ReturnsCorrectDate(int year, int month, int day, DayOfWeek firstDayOfWeek,
        int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var cultureInfo = new CultureInfo("en-US")
        {
            DateTimeFormat = { FirstDayOfWeek = firstDayOfWeek }
        };
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay), result);
    }

    [Fact]
    public void GetFirstDateOfWeek_UsesCurrentCulture_IfCultureInfoNotProvided()
    {
        // Arrange
        var cultureInfo = CultureInfo.CurrentCulture;
        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        var date = new DateTime(2020, 1, 1);

        // Act
        var result = date.GetFirstDateOfWeek();

        // Assert
        Assert.Equal(firstDayOfWeek, result.DayOfWeek);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Sunday, 2020, 1, 26)]
    [InlineData(2020, 1, 31, DayOfWeek.Sunday, 2020, 1, 26)]
    [InlineData(2020, 2, 29, DayOfWeek.Sunday, 2020, 2, 23)]
    [InlineData(2020, 2, 29, DayOfWeek.Friday, 2020, 2, 28)]
    public void GetLastDateOfMonth_WithDayOfWeek_ReturnsCorrectDate(int year, int month, int day, DayOfWeek dayOfWeek,
        int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetLastDateOfMonth(dayOfWeek);

        // Assert
        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay), result);
    }

    [Theory]
    [InlineData(2020, 1, 1)]
    [InlineData(2020, 1, 31)]
    [InlineData(2020, 2, 29)]
    public void GetLastDateOfMonth_WithoutDayOfWeek_ReturnsLastDateOfMonth(int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetLastDateOfMonth();

        // Assert
        Assert.Equal(new DateTime(year, month, DateTime.DaysInMonth(year, month)), result);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Monday, 2020, 1, 5)]
    [InlineData(2020, 1, 1, DayOfWeek.Sunday, 2020, 1, 4)]
    [InlineData(2020, 1, 1, DayOfWeek.Tuesday, 2020, 1, 6)]
    [InlineData(2020, 1, 5, DayOfWeek.Monday, 2020, 1, 5)]
    [InlineData(2020, 1, 5, DayOfWeek.Sunday, 2020, 1, 11)]
    [InlineData(2020, 1, 5, DayOfWeek.Tuesday, 2020, 1, 6)]
    public void GetLastDateOfWeek_ReturnsCorrectDate(int year, int month, int day, DayOfWeek firstDayOfWeek,
        int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var cultureInfo = new CultureInfo("en-US")
        {
            DateTimeFormat = { FirstDayOfWeek = firstDayOfWeek }
        };
        var date = new DateTime(year, month, day);

        // Act
        var result = date.GetLastDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(new DateTime(expectedYear, expectedMonth, expectedDay), result);
    }

    [Fact]
    public void GetLastDateOfWeek_UsesCurrentCulture_IfCultureInfoNotProvided()
    {
        // Arrange
        var cultureInfo = CultureInfo.CurrentCulture;
        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        var date = new DateTime(2020, 1, 1);

        // Act
        var result = date.GetLastDateOfWeek();

        // Assert
        Assert.Equal(firstDayOfWeek.Increment(6), result.DayOfWeek);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 1, 0)]
    [InlineData(2020, 1, 1, 2020, 1, 2, 1)]
    [InlineData(2020, 1, 1, 2020, 2, 1, 31)]
    [InlineData(2020, 1, 1, 2021, 1, 1, 366)]
    [InlineData(2020, 1, 1, 2021, 1, 2, 367)]
    public void GetNumberOfDays_ReturnsCorrectNumberOfDays(int fromYear, int fromMonth, int fromDay, int toYear,
        int toMonth, int toDay, int expectedDays)
    {
        // Arrange
        var fromDate = new DateTime(fromYear, fromMonth, fromDay);
        var toDate = new DateTime(toYear, toMonth, toDay);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(expectedDays, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 1, false)]
    [InlineData(2020, 1, 2, 2020, 1, 1, true)]
    [InlineData(2020, 2, 1, 2020, 1, 1, true)]
    [InlineData(2021, 1, 1, 2020, 1, 1, true)]
    [InlineData(2020, 1, 1, 2021, 1, 1, false)]
    public void IsAfter_ReturnsCorrectResult(int sourceYear, int sourceMonth, int sourceDay, int otherYear,
        int otherMonth, int otherDay, bool expectedResult)
    {
        // Arrange
        var source = new DateTime(sourceYear, sourceMonth, sourceDay);
        var other = new DateTime(otherYear, otherMonth, otherDay);

        // Act
        var result = source.IsAfter(other);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 1, false)]
    [InlineData(2020, 1, 1, 2020, 1, 2, true)]
    [InlineData(2020, 1, 1, 2020, 2, 1, true)]
    [InlineData(2020, 1, 1, 2021, 1, 1, true)]
    [InlineData(2021, 1, 1, 2020, 1, 1, false)]
    public void IsBefore_ReturnsCorrectResult(int sourceYear, int sourceMonth, int sourceDay, int otherYear,
        int otherMonth, int otherDay, bool expectedResult)
    {
        // Arrange
        var source = new DateTime(sourceYear, sourceMonth, sourceDay);
        var other = new DateTime(otherYear, otherMonth, otherDay);

        // Act
        var result = source.IsBefore(other);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 2, 1, 2020, 1, 15, true)]
    [InlineData(2020, 1, 1, 2020, 2, 1, 2020, 2, 1, true)]
    [InlineData(2020, 1, 1, 2020, 2, 1, 2020, 2, 2, false)]
    [InlineData(2020, 1, 1, 2020, 2, 1, 2019, 12, 31, false)]
    public void IsBetween_GivenInclusiveRange_ReturnsExpectedResult(int rangeBegYear, int rangeBegMonth,
        int rangeBegDay, int rangeEndYear, int rangeEndMonth, int rangeEndDay, int sourceYear, int sourceMonth,
        int sourceDay, bool expectedResult)
    {
        // Arrange
        var rangeBeg = new DateTime(rangeBegYear, rangeBegMonth, rangeBegDay);
        var rangeEnd = new DateTime(rangeEndYear, rangeEndMonth, rangeEndDay);
        var source = new DateTime(sourceYear, sourceMonth, sourceDay);

        // Act
        var result = source.IsBetween(rangeBeg, rangeEnd);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("2022-12-24", "2022-12-24", true)]
    [InlineData("2022-12-24 01:00", "2022-12-24 23:00", true)]
    [InlineData("2022-12-25", "2022-12-24", false)]
    [InlineData("2022-12-24", "2022-12-23", false)]
    public void IsDateEqual_ReturnsExpectedResult(string date, string dateToCompare, bool expected)
    {
        // Arrange
        var dt = DateTime.Parse(date);
        var dtCompare = DateTime.Parse(dateToCompare);

        // Act
        var result = dt.IsDateEqual(dtCompare);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("01/01/2022 12:00:00", "02/02/2022 12:01:00")]
    [InlineData("01/01/2022 12:00:00", "01/01/2022 11:00:00")]
    [InlineData("01/01/2022 12:00:00", "01/01/2022 13:00:00")]
    public void IsTimeEqual_WhenTimeIsNotEqual_ReturnsFalse(string timeString, string timeToCompareString)
    {
        // Arrange
        var time = DateTime.Parse(timeString);
        var timeToCompare = DateTime.Parse(timeToCompareString);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("01/01/2022 12:00:00", "01/01/2022 12:00:00")]
    [InlineData("01/01/2022 12:00:01", "01/01/2022 12:00:01")]
    [InlineData("01/01/2022 12:01:00", "01/01/2022 12:01:00")]
    public void IsTimeEqual_WhenTimeIsEqual_ReturnsTrue(string timeString, string timeToCompareString)
    {
        // Arrange
        var time = DateTime.Parse(timeString);
        var timeToCompare = DateTime.Parse(timeToCompareString);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsToday_GivenDate_ReturnsExpectedResult()
    {
        // Arrange
        var date = DateTime.Today;

        // Act
        var result = date.IsToday();

        // Assert
        Assert.True(result);

        // Arrange
        date = DateTime.Today.AddDays(1);

        // Act
        result = date.IsToday();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 365)]
    [InlineData(4, 366)]
    [InlineData(100, 365)]
    [InlineData(400, 366)]
    [InlineData(800, 366)]
    [InlineData(1900, 365)]
    [InlineData(2000, 366)]
    [InlineData(2100, 365)]
    public void GetNumberOfDaysInYear_GivenYear_ReturnsExpectedResult(int year, int expectedResult)
    {
        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNumberOfDaysInYear_GivenCultureInfo_UsesCorrectCalendar()
    {
        // Arrange
        var cultureInfo = new CultureInfo("en-US");
        var year = 2000;
        var expectedResult = 366;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year, cultureInfo);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetNumberOfDaysInYear_GivenNullCultureInfo_UsesCurrentCulture()
    {
        // Arrange
        var year = 2000;
        var expectedResult = 366;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(2024, 2, 29, true)]
    [InlineData(2000, 2, 29, true)]
    [InlineData(2022, 3, 29, false)]
    [InlineData(2022, 2, 28, false)]
    public void IsLeapDay_GivenDate_ReturnsExpectedResult(int year, int month, int day, bool expectedResult)
    {
        // Arrange
        var date = new DateTime(year, month, day);

        // Act
        var result = date.IsLeapDay();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(2022, 12, 24, 12, 0, 0, 2022, 12, 24, 12, 0, 0)]
    [InlineData(2022, 12, 24, 0, 0, 0, 2022, 12, 24, 0, 0, 0)]
    [InlineData(2022, 12, 24, 23, 59, 59, 2022, 12, 24, 23, 59, 59)]
    [InlineData(2022, 12, 24, 12, 30, 45, 2022, 12, 24, 12, 30, 45)]
    [InlineData(2022, 12, 24, 0, 30, 45, 2022, 12, 24, 0, 30, 45)]
    [InlineData(2022, 12, 24, 23, 30, 45, 2022, 12, 24, 23, 30, 45)]
    public void SetTime_GivenDateAndTime_ReturnsExpectedResult(int year, int month, int day, int hour, int minute,
        int second, int expectedYear, int expectedMonth, int expectedDay, int expectedHour, int expectedMinute,
        int expectedSecond)
    {
        // Arrange
        var date = new DateTime(year, month, day, hour, minute, second);
        var time = new TimeSpan(hour, minute, second);

        // Act
        var result = date.SetTime(time);

        // Assert
        Assert.Equal(expectedYear, result.Year);
        Assert.Equal(expectedMonth, result.Month);
        Assert.Equal(expectedDay, result.Day);
        Assert.Equal(expectedHour, result.Hour);
        Assert.Equal(expectedMinute, result.Minute);
        Assert.Equal(expectedSecond, result.Second);
    }

    [Theory]
    [InlineData(2022, 12, 24, 12, 0, 0, 0, 2022, 12, 24, 12, 0, 0, 0)]
    [InlineData(2022, 12, 24, 0, 0, 0, 0, 2022, 12, 24, 0, 0, 0, 0)]
    [InlineData(2022, 12, 24, 23, 59, 59, 999, 2022, 12, 24, 23, 59, 59, 999)]
    [InlineData(2022, 12, 24, 12, 30, 45, 123, 2022, 12, 24, 12, 30, 45, 123)]
    [InlineData(2022, 12, 24, 0, 30, 45, 123, 2022, 12, 24, 0, 30, 45, 123)]
    [InlineData(2022, 12, 24, 23, 30, 45, 123, 2022, 12, 24, 23, 30, 45, 123)]
    public void SetTime_GivenDateAndTimeComponents_ReturnsExpectedResult(int year, int month, int day, int hour,
        int minute, int second, int millisecond, int expectedYear, int expectedMonth, int expectedDay, int expectedHour,
        int expectedMinute, int expectedSecond, int expectedMillisecond)
    {
        // Arrange
        var date = new DateTime(year, month, day, hour, minute, second, millisecond);

        // Act
        var result = date.SetTime(hour, minute, second, millisecond);

        // Assert
        Assert.Equal(expectedYear, result.Year);
        Assert.Equal(expectedMonth, result.Month);
        Assert.Equal(expectedDay, result.Day);
        Assert.Equal(expectedHour, result.Hour);
        Assert.Equal(expectedMinute, result.Minute);
        Assert.Equal(expectedSecond, result.Second);
        Assert.Equal(expectedMillisecond, result.Millisecond);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
    [InlineData(1, 2, 3, 4, 5, 6, 7, 8, 5, 6)]
    public void SetTime_SetsTimeComponentToSpecifiedTime(
        int dateHour, int dateMinute, int dateSecond, int dateMillisecond,
        int timeHour, int timeMinute, int timeSecond, int timeMillisecond,
        int expectedHour, int expectedMinute)
    {
        // Arrange
        var date = new DateTime(2020, 1, 1, dateHour, dateMinute, dateSecond, dateMillisecond);
        var timeOnly = new TimeOnly(timeHour, timeMinute, timeSecond, timeMillisecond);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        Assert.Equal(expectedHour, result.Hour);
        Assert.Equal(expectedMinute, result.Minute);
        Assert.Equal(timeSecond, result.Second);
        Assert.Equal(timeMillisecond, result.Millisecond);
    }

    [Fact]
    public void SetTime_ReturnsDateWithSameDateAsGivenDate()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1, 12, 0, 0, 0);
        var timeOnly = new TimeOnly(1, 2, 3, 4);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void NextDay_ReturnsDateThatIsOneDayAfterGivenDate()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);

        // Act
        var result = date.NextDay();

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(2, result.Day);
    }

    [Fact]
    public void NextDay_HandlesLastDayOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 1, 31);

        // Act
        var result = date.NextDay();

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(2, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void NextDay_HandlesLastDayOfYear()
    {
        // Arrange
        var date = new DateTime(2020, 12, 31);

        // Act
        var result = date.NextDay();

        // Assert
        Assert.Equal(2021, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void NextDay_PreservesTimeComponentOfGivenDate()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1, 12, 34, 56, 78);

        // Act
        var result = date.NextDay();

        // Assert
        Assert.Equal(12, result.Hour);
        Assert.Equal(34, result.Minute);
        Assert.Equal(56, result.Second);
        Assert.Equal(78, result.Millisecond);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void NextDay_HandlesEdgeCases(int ticks)
    {
        // Arrange
        var date = new DateTime(ticks);

        // Act
        var result = date.NextDay();

        // Assert
        Assert.Equal(ticks + TimeSpan.TicksPerDay, result.Ticks);
    }

    [Fact]
    public void PreviousDay_ReturnsDateThatIsOneDayBeforeGivenDate()
    {
        // Arrange
        var date = new DateTime(2020, 1, 2);

        // Act
        var result = date.PreviousDay();

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void PreviousDay_HandlesFirstDayOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 2, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(31, result.Day);
    }

    [Fact]
    public void PreviousDay_HandlesFirstDayOfYear()
    {
        // Arrange
        var date = new DateTime(2021, 1, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        Assert.Equal(2020, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(31, result.Day);
    }

    [Fact]
    public void PreviousDay_PreservesTimeComponentOfGivenDate()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1, 12, 34, 56, 78);

        // Act
        var result = date.PreviousDay();

        // Assert
        Assert.Equal(12, result.Hour);
        Assert.Equal(34, result.Minute);
        Assert.Equal(56, result.Second);
        Assert.Equal(78, result.Millisecond);
    }

    [Fact]
    public void GenerateTimestamp_DoesNotModifyOriginalDateTime()
    {
        // Arrange
        var dateTime = new DateTime(2020, 1, 1, 0, 0, 0, 0);
        var expected = dateTime;

        // Act
        dateTime.GetTimestamp();

        // Assert
        Assert.Equal(expected, dateTime);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 1)]
    [InlineData(2020, 1, 1, 2020, 1, 2)]
    [InlineData(2020, 1, 2, 2020, 1, 1)]
    public void GetDatesInRange_ReturnsExpectedDates(int fromYear, int fromMonth, int fromDay, int toYear, int toMonth,
        int toDay)
    {
        // Arrange
        var fromDate = new DateTime(fromYear, fromMonth, fromDay);
        var toDate = new DateTime(toYear, toMonth, toDay);

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(Math.Abs((toDate - fromDate).Days) + 1, result.Count());
        Assert.Equal(fromDate, result.First());
        Assert.Equal(toDate, result.Last());
    }

    [Theory]
    [InlineData(2020, 1, 1, 0, 0, 0, 0, 2020, 1, 1)]
    [InlineData(2020, 12, 31, 23, 59, 59, 999, 2020, 12, 31)]
    [InlineData(2020, 2, 29, 12, 34, 56, 789, 2020, 2, 29)]
    public void ToDateOnly_ReturnsExpectedDate(int year, int month, int day, int hour, int minute, int second,
        int millisecond, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);

        // Act
        var result = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(expectedYear, result.Year);
        Assert.Equal(expectedMonth, result.Month);
        Assert.Equal(expectedDay, result.Day);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)]
    [InlineData(23, 59, 59, 999, 999, 23, 59, 59, 999, 999)]
    [InlineData(12, 34, 56, 789, 123, 12, 34, 56, 789, 123)]
    public void ToTimeOnly_ReturnsExpectedTime(int hour, int minute, int second, int millisecond, int microsecond,
        int expectedHour, int expectedMinute, int expectedSecond, int expectedMillisecond, int expectedMicrosecond)
    {
        // Arrange
        var dateTime = new DateTime(2020, 1, 1, hour, minute, second, millisecond, microsecond);

        // Act
        var result = dateTime.ToTimeOnly();

        // Assert
        Assert.Equal(expectedHour, result.Hour);
        Assert.Equal(expectedMinute, result.Minute);
        Assert.Equal(expectedSecond, result.Second);
        Assert.Equal(expectedMillisecond, result.Millisecond);
        Assert.Equal(expectedMicrosecond, result.Microsecond);
    }
}