using System.Globalization;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void IsToday_Today_ReturnsTrue()
    {
        // Arrange
        var today = DateTime.Today;
        var dateOnly = new DateOnly(today.Year, today.Month, today.Day);

        // Act
        var result = dateOnly.IsToday();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsToday_Yesterday_ReturnsFalse()
    {
        // Arrange
        var yesterday = DateTime.Today.AddDays(-1);
        var dateOnly = new DateOnly(yesterday.Year, yesterday.Month, yesterday.Day);

        // Act
        var result = dateOnly.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsToday_Tomorrow_ReturnsFalse()
    {
        // Arrange
        var tomorrow = DateTime.Today.AddDays(1);
        var dateOnly = new DateOnly(tomorrow.Year, tomorrow.Month, tomorrow.Day);

        // Act
        var result = dateOnly.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsToday_DifferentYear_ReturnsFalse()
    {
        // Arrange
        var date = new DateTime(DateTime.Today.Year + 1, DateTime.Today.Month, DateTime.Today.Day);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);

        // Act
        var result = dateOnly.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetDatesInRange_FromDateEqualToToDate_ReturnsSingleDate()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 1);
        var fromDate = new DateOnly(date.Year, date.Month, date.Day);
        var toDate = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new[] { fromDate };

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDatesInRange_FromDateBeforeToDate_ReturnsDatesInRange()
    {
        // Arrange
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 3);
        var expected = new[] { fromDate, fromDate.AddDays(1), fromDate.AddDays(2) };

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDatesInRange_FromDateAfterToDate_ReturnsDatesInRange()
    {
        // Arrange
        var fromDate = new DateOnly(2020, 1, 3);
        var toDate = new DateOnly(2020, 1, 1);
        var expected = new[] { fromDate, fromDate.AddDays(-1), fromDate.AddDays(-2) };

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsLeapDay_LeapDay_ReturnsTrue()
    {
        // Arrange
        var date = new DateOnly(2020, 2, 29);

        // Act
        var result = date.IsLeapDay();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsLeapDay_NotLeapDay_ReturnsFalse()
    {
        // Arrange
        var date = new DateOnly(2020, 2, 28);

        // Act
        var result = date.IsLeapDay();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("2022-12-29", "2023-01-01")]
    [InlineData("2022-12-25", "2022-12-25")]
    [InlineData("2022-12-31", "2023-01-01")]
    public void GetLastDateOfWeek_ReturnsCorrectDate_ForGivenDate(string inputDateString, string expectedDateString)
    {
        // Arrange
        var inputDate = DateOnly.Parse(inputDateString);
        var expectedDate = DateOnly.Parse(expectedDateString);

        // Act
        var result = inputDate.GetLastDateOfWeek();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2020-12-31", "en-US", "2021-01-02")]
    [InlineData("2022-06-01", "en-US", "2022-06-04")]
    [InlineData("2020-01-01", "fr-FR", "2020-01-05")]
    public void GetLastDateOfWeek_ReturnsCorrectResult_ForDifferentCultures(string input, string culture,
        string expected)
    {
        // Arrange
        var date = DateOnly.Parse(input);
        var cultureInfo = CultureInfo.GetCultureInfo(culture);

        // Act
        var result = date.GetLastDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(expected, result.ToString("yyyy-MM-dd"));
    }

    public static IEnumerable<object[]> AddWeeksTestData()
    {
        yield return new object[] { new DateOnly(2020, 1, 1), 0, new DateOnly(2020, 1, 1) };
        yield return new object[] { new DateOnly(2020, 1, 1), 1, new DateOnly(2020, 1, 8) };
        yield return new object[] { new DateOnly(2020, 1, 1), 2, new DateOnly(2020, 1, 15) };
        yield return new object[] { new DateOnly(2020, 1, 1), 3, new DateOnly(2020, 1, 22) };
        yield return new object[] { new DateOnly(2020, 1, 1), -1, new DateOnly(2019, 12, 25) };
        yield return new object[] { new DateOnly(2020, 1, 1), -2, new DateOnly(2019, 12, 18) };
        yield return new object[] { new DateOnly(2020, 1, 1), -3, new DateOnly(2019, 12, 11) };
    }

    [Theory]
    [MemberData(nameof(AddWeeksTestData))]
    public void AddWeeks_ReturnsExpectedDate(DateOnly date, int numberOfWeeks, DateOnly expectedDate)
    {
        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Fact]
    public void PreviousDay()
    {
        var date = new DateOnly(2021, 1, 1);
        var previousDay = date.PreviousDay();
        Assert.Equal(new DateOnly(2020, 12, 31), previousDay);
    }

    [Fact]
    public void PreviousDay_Today_ReturnsYesterday()
    {
        // Arrange
        var today = DateTime.Today;
        var dateOnly = new DateOnly(today.Year, today.Month, today.Day);
        var expected = dateOnly.AddDays(-1);

        // Act
        var result = dateOnly.PreviousDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PreviousDay_FirstDayOfMonth_ReturnsLastDayOfPreviousMonth()
    {
        // Arrange
        var date = new DateTime(2020, 3, 1);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 2, 29); // leap year

        // Act
        var result = dateOnly.PreviousDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PreviousDay_FirstDayOfYear_ReturnsLastDayOfPreviousYear()
    {
        // Arrange
        var date = new DateTime(2021, 1, 1);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 12, 31);

        // Act
        var result = dateOnly.PreviousDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PreviousDay_NonFirstDayOfMonth_ReturnsPreviousDay()
    {
        // Arrange
        var date = new DateTime(2020, 2, 29); // leap year
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 2, 28);

        // Act
        var result = dateOnly.PreviousDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PreviousDay_NonFirstDayOfYear_ReturnsPreviousDay()
    {
        // Arrange
        var date = new DateTime(2020, 1, 2);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 1, 1);

        // Act
        var result = dateOnly.PreviousDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2020, 1, 31)]
    [InlineData(2020, 2, 29)]
    [InlineData(2019, 2, 28)]
    [InlineData(2021, 4, 30)]
    public void GetLastDateOfMonth_ReturnsLastDateOfMonth(int year, int month, int expectedDay)
    {
        // Arrange
        var date = new DateOnly(year, month, 1);

        // Act
        var lastDateOfMonth = date.GetLastDateOfMonth();

        // Assert
        Assert.Equal(expectedDay, lastDateOfMonth.Day);
    }

    [Theory]
    [InlineData(2020, 1, DayOfWeek.Sunday, 26)]
    [InlineData(2020, 2, DayOfWeek.Monday, 24)]
    [InlineData(2019, 2, DayOfWeek.Tuesday, 26)]
    [InlineData(2021, 4, DayOfWeek.Wednesday, 28)]
    public void GetLastDateOfMonth_WithDayOfWeek_ReturnsLastDateOfMonthOnSpecifiedDayOfWeek(int year, int month,
        DayOfWeek dayOfWeek, int expectedDay)
    {
        // Arrange
        var date = new DateOnly(year, month, 1);

        // Act
        var lastDateOfMonth = date.GetLastDateOfMonth(dayOfWeek);

        // Assert
        Assert.Equal(expectedDay, lastDateOfMonth.Day);
        Assert.Equal(dayOfWeek, lastDateOfMonth.DayOfWeek);
    }

    [Theory]
    [InlineData(2020, 1, 4, 2020, 1, 2, true)]
    [InlineData(2020, 1, 2, 2020, 1, 1, true)]
    [InlineData(2020, 1, 1, 2020, 1, 1, false)]
    public void IsAfter_ReturnsExpectedResult(int sourceYear, int sourceMonth, int sourceDay, int otherYear,
        int otherMonth, int otherDay, bool expectedResult)
    {
        // Arrange
        var source = new DateOnly(sourceYear, sourceMonth, sourceDay);
        var other = new DateOnly(otherYear, otherMonth, otherDay);

        // Act
        var result = source.IsAfter(other);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_Today_ReturnsTomorrow()
    {
        // Arrange
        var today = DateTime.Today;
        var dateOnly = new DateOnly(today.Year, today.Month, today.Day);
        var expected = dateOnly.AddDays(1);

        // Act
        var result = dateOnly.NextDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NextDay_LastDayOfMonth_ReturnsFirstDayOfNextMonth()
    {
        // Arrange
        var date = new DateTime(2020, 2, 29); // leap year
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 3, 1);

        // Act
        var result = dateOnly.NextDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NextDay_LastDayOfYear_ReturnsFirstDayOfNextYear()
    {
        // Arrange
        var date = new DateTime(2020, 12, 31);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2021, 1, 1);

        // Act
        var result = dateOnly.NextDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NextDay_NonLastDayOfMonth_ReturnsNextDay()
    {
        // Arrange
        var date = new DateTime(2020, 2, 28);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 2, 29);

        // Act
        var result = dateOnly.NextDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NextDay_NonLastDayOfYear_ReturnsNextDay()
    {
        // Arrange
        var date = new DateTime(2020, 12, 30);
        var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        var expected = new DateOnly(2020, 12, 31);

        // Act
        var result = dateOnly.NextDay();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    public void IsBefore_GivenTwoDates_ReturnsCorrectResult(int day1, int day2)
    {
        // Arrange
        var date1 = new DateOnly(2022, 1, day1);
        var date2 = new DateOnly(2022, 1, day2);

        // Act
        var result = date1.IsBefore(date2);

        // Assert
        Assert.Equal(day1 < day2, result);
    }

    [Fact]
    public void IsBetween_FromDateEqualToToDate_ReturnsTrue()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 1);
        var fromDate = new DateOnly(date.Year, date.Month, date.Day);
        var toDate = new DateOnly(date.Year, date.Month, date.Day);
        var expected = true;

        // Act
        var result = date.IsBetween(fromDate, toDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsBetween_FromDateBeforeToDate_ReturnsTrue()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 2);
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 3);
        var expected = true;

        // Act
        var result = date.IsBetween(fromDate, toDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 1, true, true)]
    [InlineData(2020, 1, 1, 2020, 1, 1, false, false)]
    [InlineData(2020, 1, 2, 2020, 1, 1, true, true)]
    [InlineData(2020, 1, 3, 2020, 1, 2, true, true)]
    [InlineData(2020, 1, 3, 2020, 1, 2, false, false)]
    public void IsBetween_DateIsInRange_ReturnsExpectedResult(
        int dtYear, int dtMonth, int dtDay,
        int rangeBegYear, int rangeBegMonth, int rangeBegDay,
        bool isInclusive, bool expected)
    {
        // Arrange
        var dt = new DateOnly(dtYear, dtMonth, dtDay);
        var rangeBeg = new DateOnly(rangeBegYear, rangeBegMonth, rangeBegDay);
        var rangeEnd = new DateOnly(rangeBegYear, rangeBegMonth, rangeBegDay + 1);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, isInclusive);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2020, 1, 2, true, false)]
    [InlineData(2020, 1, 1, 2020, 1, 2, false, false)]
    public void IsBetween_DateIsNotInRange_ReturnsExpectedResult(
        int dtYear, int dtMonth, int dtDay,
        int rangeBegYear, int rangeBegMonth, int rangeBegDay,
        bool isInclusive, bool expected)
    {
        // Arrange
        var dt = new DateOnly(dtYear, dtMonth, dtDay);
        var rangeBeg = new DateOnly(rangeBegYear, rangeBegMonth, rangeBegDay);
        var rangeEnd = new DateOnly(rangeBegYear, rangeBegMonth, rangeBegDay + 1);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, isInclusive);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Wednesday, 1)]
    [InlineData(2020, 1, 1, DayOfWeek.Thursday, 2)]
    [InlineData(2020, 1, 1, DayOfWeek.Friday, 3)]
    [InlineData(2020, 1, 1, DayOfWeek.Saturday, 4)]
    [InlineData(2020, 1, 1, DayOfWeek.Sunday, 5)]
    [InlineData(2020, 1, 1, DayOfWeek.Monday, 6)]
    [InlineData(2020, 1, 1, DayOfWeek.Tuesday, 7)]
    public void GetFirstDateOfMonth_GivenDateAndDayOfWeek_ReturnsCorrectDate(int year, int month, int day,
        DayOfWeek dayOfWeek, int expected)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = date.GetFirstDateOfMonth(dayOfWeek);

        // Assert
        Assert.Equal(dayOfWeek, result.DayOfWeek);
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(expected, result.Day);
    }

    [Theory]
    [InlineData(2020, 1, 1)]
    [InlineData(2021, 2, 1)]
    [InlineData(2022, 3, 1)]
    [InlineData(2023, 4, 1)]
    [InlineData(2024, 5, 1)]
    [InlineData(2025, 6, 1)]
    [InlineData(2026, 7, 1)]
    [InlineData(2027, 8, 1)]
    [InlineData(2028, 9, 1)]
    [InlineData(2029, 10, 1)]
    [InlineData(2030, 11, 1)]
    [InlineData(2031, 12, 1)]
    public void GetFirstDateOfMonth_GivenDate_ReturnsCorrectDate(int year, int month, int day)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = date.GetFirstDateOfMonth();

        // Assert
        Assert.Equal(year, result.Year);
        Assert.Equal(month, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Theory]
    [InlineData(2020, 1, 1, 2019, 12, 30)]
    public void GetFirstDateOfWeek_WithDefaultCulture_ReturnsExpectedResult(int year, int month, int day,
        int expectedYear,
        int expectedMonth, int expectedDay)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = date.GetFirstDateOfWeek();

        // Assert
        Assert.Equal(new DateOnly(expectedYear, expectedMonth, expectedDay), result);
    }

    [Theory]
    [InlineData(2020, 1, 1, DayOfWeek.Wednesday, 2020, 1, 1)]
    [InlineData(2020, 1, 1, DayOfWeek.Sunday, 2019, 12, 29)]
    [InlineData(2020, 1, 1, DayOfWeek.Monday, 2019, 12, 30)]
    [InlineData(2020, 1, 1, DayOfWeek.Tuesday, 2019, 12, 31)]
    public void GetFirstDateOfWeek_WithCustomCulture_ReturnsExpectedResult(int year, int month, int day,
        DayOfWeek firstDayOfWeek, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var date = new DateOnly(year, month, day);
        var cultureInfo = new CultureInfo("en-US") { DateTimeFormat = { FirstDayOfWeek = firstDayOfWeek } };

        // Act
        var result = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(new DateOnly(expectedYear, expectedMonth, expectedDay), result);
    }

    public static IEnumerable<object[]> AgeTestData()
    {
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2020, 1, 1), 20 };
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2022, 6, 1), 22 };
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2020, 1, 2), 20 };
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2020, 12, 31), 20 };
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2020, 12, 30), 20 };
        yield return new object[] { new DateOnly(2000, 1, 1), new DateOnly(2020, 12, 29), 20 };
    }

    [Theory]
    [MemberData(nameof(AgeTestData))]
    public void Age_MaturityDateIsAfterBirthDate_ReturnsAge(DateOnly dateOfBirth, DateOnly maturityDate,
        int expectedAge)
    {
        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(expectedAge, age);
    }

    [Fact]
    public void Age_MaturityDateIsBeforeBirthDate_ThrowsArgumentException()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var maturityDate = new DateOnly(1999, 1, 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => dateOfBirth.Age(maturityDate));
        Assert.Equal(nameof(maturityDate), exception.ParamName);
        Assert.Equal(
            $"The maturity date '1/01/1999' cannot occur before the birth date '1/01/2000' (Parameter '{nameof(maturityDate)}')",
            exception.Message);
    }

    [Fact]
    public void Age_MaturityDateIsNull_UsesCurrentDate()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var expectedAge = DateTime.Now.Year - dateOfBirth.Year;

        // Act
        var age = dateOfBirth.Age();

        // Assert
        Assert.Equal(expectedAge, age);
    }

    public static IEnumerable<object[]> DaysInMonthTestData()
    {
        yield return new object[] { new DateOnly(2020, 1, 1), 31 };
        yield return new object[] { new DateOnly(2020, 2, 1), 29 };
        yield return new object[] { new DateOnly(2019, 2, 1), 28 };
        yield return new object[] { new DateOnly(2020, 3, 1), 31 };
        yield return new object[] { new DateOnly(2020, 4, 1), 30 };
        yield return new object[] { new DateOnly(2020, 5, 1), 31 };
        yield return new object[] { new DateOnly(2020, 6, 1), 30 };
        yield return new object[] { new DateOnly(2020, 7, 1), 31 };
        yield return new object[] { new DateOnly(2020, 8, 1), 31 };
        yield return new object[] { new DateOnly(2020, 9, 1), 30 };
        yield return new object[] { new DateOnly(2020, 10, 1), 31 };
        yield return new object[] { new DateOnly(2020, 11, 1), 30 };
        yield return new object[] { new DateOnly(2020, 12, 1), 31 };
    }

    [Theory]
    [MemberData(nameof(DaysInMonthTestData))]
    public void DaysInMonth_DateIsInMonth_ReturnsDaysInMonth(DateOnly date, int expectedDaysInMonth)
    {
        // Act
        var daysInMonth = date.DaysInMonth();

        // Assert
        Assert.Equal(expectedDaysInMonth, daysInMonth);
    }

    [Theory]
    [InlineData(2022, 1, 1, 2022, 1, 1, 0)]
    [InlineData(2022, 1, 1, 2022, 1, 2, 1)]
    [InlineData(2022, 1, 1, 2022, 1, 3, 2)]
    [InlineData(2022, 1, 1, 2022, 2, 1, 31)]
    [InlineData(2022, 1, 1, 2023, 1, 1, 365)]
    [InlineData(2022, 2, 28, 2022, 3, 1, 1)]
    public void GetNumberOfDays_ReturnsCorrectNumberOfDays(
        int fromYear, int fromMonth, int fromDay,
        int toYear, int toMonth, int toDay,
        int expectedNumberOfDays)
    {
        // Arrange
        var fromDate = new DateOnly(fromYear, fromMonth, fromDay);
        var toDate = new DateOnly(toYear, toMonth, toDay);

        // Act
        var numberOfDays = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(expectedNumberOfDays, numberOfDays);
    }
}