using System.Globalization;
using BigO.Core.Extensions;

// ReSharper disable PossibleMultipleEnumeration

namespace BigO.Core.Tests;

public class DateTimeExtensionsTests
{
    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenMaturityDateIsNotProvided()
    {
        // Arrange
        var dateOfBirth = new DateTime(2000, 1, 1);

        // Act
        var age = dateOfBirth.Age();

        // Assert
        Assert.Equal(DateTime.Today.Year - 2000, age);
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenMaturityDateIsProvided()
    {
        // Arrange
        var dateOfBirth = new DateTime(2000, 1, 1);
        var maturityDate = new DateTime(2020, 1, 1);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(2020 - 2000, age);
    }

    [Fact]
    public void Age_ShouldThrowArgumentException_WhenMaturityDateOccursBeforeDateOfBirth()
    {
        // Arrange
        var dateOfBirth = new DateTime(2000, 1, 1);
        var maturityDate = new DateTime(1999, 1, 1);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => dateOfBirth.Age(maturityDate));
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenMaturityDateOccursOnSameDayAsDateOfBirth()
    {
        // Arrange
        var dateOfBirth = new DateTime(2000, 1, 1);
        var maturityDate = new DateTime(2020, 1, 1);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(2020 - 2000, age);
    }

    [Fact]
    public void Age_ShouldReturnCorrectAge_WhenMaturityDateOccursAfterDateOfBirth()
    {
        // Arrange
        var dateOfBirth = new DateTime(2000, 1, 1);
        var maturityDate = new DateTime(2020, 2, 1);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(20, age);
    }

    [Fact]
    public void AddWeeks_ShouldAddCorrectNumberOfDays_WhenPositiveNumberOfWeeksIsPassed()
    {
        // Arrange
        var date = new DateTime(2000, 1, 1);
        var numberOfWeeks = 2.5;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateTime(2000, 1, 19), result);
    }

    [Fact]
    public void AddWeeks_ShouldAddCorrectNumberOfDays_WhenNegativeNumberOfWeeksIsPassed()
    {
        // Arrange
        var date = new DateTime(2000, 1, 8);
        var numberOfWeeks = -2.5;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateTime(1999, 12, 22), result);
    }

    [Fact]
    public void AddWeeks_ShouldAddCorrectNumberOfDays_WhenZeroNumberOfWeeksIsPassed()
    {
        // Arrange
        var date = new DateTime(2000, 1, 1);
        var numberOfWeeks = 0;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateTime(2000, 1, 1), result);
    }

    [Fact]
    public void AddWeeks_ShouldAddCorrectNumberOfDays_WhenNumberOfWeeksIsNotAnInteger()
    {
        // Arrange
        var date = new DateTime(2000, 1, 1);
        var numberOfWeeks = 1.4;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateTime(2000, 1, 11), result);
    }

    [Fact]
    public void DaysInMonth_ReturnsCorrectNumberOfDays()
    {
        // Arrange
        var date = new DateTime(2022, 2, 1); // February 2022 has 28 days

        // Act
        var daysInMonth = date.DaysInMonth();

        // Assert
        Assert.Equal(28, daysInMonth);
    }

    [Theory]
    [InlineData(1, 31)] // January has 31 days
    [InlineData(2, 28)] // February has 28 days (leap year exception handled by .NET framework)
    [InlineData(3, 31)] // March has 31 days
    [InlineData(4, 30)] // April has 30 days
    [InlineData(5, 31)] // May has 31 days
    [InlineData(6, 30)] // June has 30 days
    [InlineData(7, 31)] // July has 31 days
    [InlineData(8, 31)] // August has 31 days
    [InlineData(9, 30)] // September has 30 days
    [InlineData(10, 31)] // October has 31 days
    [InlineData(11, 30)] // November has 30 days
    [InlineData(12, 31)] // December has 31 days
    public void DaysInMonth_ReturnsCorrectNumberOfDays_ForVariousMonths(int month, int expectedDays)
    {
        // Arrange
        var date = new DateTime(2022, month, 1);

        // Act
        var daysInMonth = date.DaysInMonth();

        // Assert
        Assert.Equal(expectedDays, daysInMonth);
    }

    [Fact]
    public void GetFirstDateOfMonth_ReturnsFirstDateOfMonth_WhenDayOfWeekNotSpecified()
    {
        // Arrange
        var date = new DateTime(2022, 2, 15); // February 15, 2022

        // Act
        var firstDateOfMonth = date.GetFirstDateOfMonth();

        // Assert
        Assert.Equal(new DateTime(2022, 2, 1), firstDateOfMonth);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, 2022, 2, 7)] // First Monday in February 2022 is February 7
    [InlineData(DayOfWeek.Tuesday, 2022, 2, 1)] // First Tuesday in February 2022 is February 1
    [InlineData(DayOfWeek.Wednesday, 2022, 2, 2)] // First Wednesday in February 2022 is February 2
    [InlineData(DayOfWeek.Thursday, 2022, 2, 3)] // First Thursday in February 2022 is February 3
    [InlineData(DayOfWeek.Friday, 2022, 2, 4)] // First Friday in February 2022 is February 4
    [InlineData(DayOfWeek.Saturday, 2022, 2, 5)] // First Saturday in February 2022 is February 5
    [InlineData(DayOfWeek.Sunday, 2022, 2, 6)] // First Sunday in February 2022 is February 6
    public void GetFirstDateOfMonth_ReturnsFirstSpecifiedDayOfWeekOfMonth(DayOfWeek dayOfWeek, int year, int month,
        int day)
    {
        // Arrange
        var date = new DateTime(2022, 2, 15); // February 15, 2022

        // Act
        var firstDateOfMonth = date.GetFirstDateOfMonth(dayOfWeek);

        // Assert
        var expectedDate = new DateTime(year, month, day);
        Assert.Equal(expectedDate, firstDateOfMonth);
    }

    [Fact]
    public void GetFirstDateOfWeek_ReturnsFirstDateOfWeek_UsingDefaultCulture()
    {
        // Arrange
        var date = new DateTime(2022, 2,
            15); // Tuesday, February 15, 2022 (assume default culture has Monday as first day of week)

        // Act
        var firstDateOfWeek = date.GetFirstDateOfWeek();

        // Assert
        Assert.Equal(new DateTime(2022, 2, 14), firstDateOfWeek);
    }

    [Fact]
    public void GetFirstDateOfWeek_ReturnsFirstDateOfWeek_UsingSpecifiedCulture()
    {
        // Arrange
        var date = new DateTime(2022, 2, 15); // Tuesday, February 15, 2022
        var cultureInfo = new CultureInfo("ar-SA"); // First day of week is Sunday in ar-SA culture

        // Act
        var firstDateOfWeek = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(new DateTime(2022, 2, 13), firstDateOfWeek);
    }

    [Fact]
    public void GetLastDateOfMonth_ReturnsLastDateOfMonth_WhenDayOfWeekNotSpecified()
    {
        // Arrange
        var date = new DateTime(2022, 2, 15); // February 15, 2022

        // Act
        var lastDateOfMonth = date.GetLastDateOfMonth();

        // Assert
        Assert.Equal(new DateTime(2022, 2, 28), lastDateOfMonth);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, 2022, 2, 28)] // Last Monday in February 2022 is February 28
    [InlineData(DayOfWeek.Tuesday, 2022, 2, 22)] // Last Tuesday in February 2022 is February 22
    [InlineData(DayOfWeek.Wednesday, 2022, 2, 23)] // Last Wednesday in February 2022 is February 23
    [InlineData(DayOfWeek.Thursday, 2022, 2, 24)] // Last Thursday in February 2022 is February 24
    [InlineData(DayOfWeek.Friday, 2022, 2, 25)] // Last Friday in February 2022 is February 25
    [InlineData(DayOfWeek.Saturday, 2022, 2, 26)] // Last Saturday in February 2022 is February 26
    [InlineData(DayOfWeek.Sunday, 2022, 2, 27)] // Last Sunday in February 2022 is February 27
    public void GetLastDateOfMonth_ReturnsLastSpecifiedDayOfWeekOfMonth(DayOfWeek dayOfWeek,
        int year, int month, int day)
    {
        // Arrange
        var date = new DateTime(2022, 2, 15); // February 15, 2022

        // Act
        var lastDateOfMonth = date.GetLastDateOfMonth(dayOfWeek);

        // Assert
        var expectedDate = new DateTime(year, month, day);
        Assert.Equal(expectedDate, lastDateOfMonth);
    }

    [Fact]
    public void TestGetLastDateOfWeek_DefaultCulture()
    {
        // Arrange
        var inputDate = new DateTime(2022, 12, 18);

        // Act
        var result = inputDate.GetLastDateOfWeek();

        // Assert
        var expectedResult = new DateTime(2022, 12, 18);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestGetLastDateOfWeek_SpecificCulture()
    {
        // Arrange
        var inputDate = new DateTime(2022, 12, 18);
        var culture = CultureInfo.GetCultureInfo("en-US");

        // Act
        var result = inputDate.GetLastDateOfWeek(culture);

        // Assert
        var expectedResult = new DateTime(2022, 12, 24);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestGetLastDateOfWeek_BeginningOfWeek()
    {
        // Arrange
        var inputDate = new DateTime(2022, 12, 18);

        // Act
        var result = inputDate.GetLastDateOfWeek();

        // Assert
        var expectedResult = new DateTime(2022, 12, 18);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestGetLastDateOfWeek_EndOfWeek()
    {
        // Arrange
        var inputDate = new DateTime(2022, 12, 25);

        // Act
        var result = inputDate.GetLastDateOfWeek();

        // Assert
        var expectedResult = new DateTime(2022, 12, 25);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestGetLastDateOfWeek_MiddleOfWeek()
    {
        // Arrange
        var inputDate = new DateTime(2022, 12, 20);

        // Act
        var result = inputDate.GetLastDateOfWeek();

        // Assert
        var expectedResult = new DateTime(2022, 12, 25);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TestGetNumberOfDays_SameDates()
    {
        // Arrange
        var fromDate = new DateTime(2022, 12, 18);
        var toDate = new DateTime(2022, 12, 18);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void TestGetNumberOfDays_OneDayApart()
    {
        // Arrange
        var fromDate = new DateTime(2022, 12, 18);
        var toDate = new DateTime(2022, 12, 19);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void TestGetNumberOfDays_MultipleDaysApart()
    {
        // Arrange
        var fromDate = new DateTime(2022, 12, 18);
        var toDate = new DateTime(2022, 12, 24);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void TestGetNumberOfDays_LeapYears()
    {
        // Arrange
        var fromDate = new DateTime(2020, 2, 28);
        var toDate = new DateTime(2024, 2, 29);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(1462, result);
    }


    [Fact]
    public void TestIsAfter_SameDates()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2022, 12, 18);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsAfter_SourceAfterOther()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2022, 12, 17);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsAfter_SourceBeforeOther()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2022, 12, 19);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsAfter_MultipleMonths()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2023, 1, 17);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsAfter_MultipleYears()
    {
// Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2020, 12, 19);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsAfter_LeapYears()
    {
        // Arrange
        var sourceDate = new DateTime(2020, 2, 29);
        var otherDate = new DateTime(2019, 3, 1);

        // Act
        var result = sourceDate.IsAfter(otherDate);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public void TestIsBefore_SameDates()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2022, 12, 18);

        // Act
        var result = sourceDate.IsBefore(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBefore_SourceBeforeOther()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2022, 12, 17);

        // Act
        var result = sourceDate.IsBefore(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBefore_MultipleMonths()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2023, 1, 17);

        // Act
        var result = sourceDate.IsBefore(otherDate);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public void TestIsBefore_MultipleYears()
    {
        // Arrange
        var sourceDate = new DateTime(2022, 12, 18);
        var otherDate = new DateTime(2021, 12, 19);

        // Act
        var result = sourceDate.IsBefore(otherDate);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBefore_LeapYears()
    {
        // Arrange
        var sourceDate = new DateTime(2020, 2, 29);
        var otherDate = new DateTime(2019, 3, 1);

        // Act
        var result = sourceDate.IsBefore(otherDate);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void TestIsBetween_SameDates_Inclusive()
    {
        // Arrange
        var dt = new DateTime(2022, 12, 18);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2022, 12, 18);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsBetween_SameDates_NotInclusive()
    {
        // Arrange
        var dt = new DateTime(2022, 12, 18);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2022, 12, 18);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, false);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBetween_SourceBetweenDates_Inclusive()
    {
        // Arrange
        var dt = new DateTime(2022, 12, 18);
        var rangeBeg = new DateTime(2022, 12, 17);
        var rangeEnd = new DateTime(2022, 12, 19);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsBetween_SourceBeforeRangeBeg()
    {
        // Arrange
        var dt = new DateTime(2022, 12, 17);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2022, 12, 19);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBetween_SourceAfterRangeEnd()
    {
        // Arrange
        var dt = new DateTime(2022, 12, 20);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2022, 12, 19);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsBetween_MultipleMonths()
    {
        // Arrange
        var dt = new DateTime(2023, 1, 17);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2023, 1, 17);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsBetween_MultipleYears()
    {
        // Arrange
        var dt = new DateTime(2023, 12, 19);
        var rangeBeg = new DateTime(2022, 12, 18);
        var rangeEnd = new DateTime(2023, 12, 19);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsBetween_LeapYears()
    {
        // Arrange
        var dt = new DateTime(2020, 2, 29);
        var rangeBeg = new DateTime(2020, 2, 28);
        var rangeEnd = new DateTime(2020, 3, 1);

        // Act
        var result = dt.IsBetween(rangeBeg, rangeEnd, true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsDateEqual_SameDates()
    {
        // Arrange
        var date = new DateTime(2022, 12, 18);
        var dateToCompare = new DateTime(2022, 12, 18);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsDateEqual_SourceBeforeOther()
    {
        // Arrange
        var date = new DateTime(2022, 12, 17);
        var dateToCompare = new DateTime(2022, 12, 18);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsDateEqual_SourceAfterOther()
    {
        // Arrange
        var date = new DateTime(2022, 12, 19);
        var dateToCompare = new DateTime(2022, 12, 18);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsDateEqual_MultipleMonths()
    {
        // Arrange
        var date = new DateTime(2023, 1, 17);
        var dateToCompare = new DateTime(2022, 12, 17);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsDateEqual_MultipleYears()
    {
        // Arrange
        var date = new DateTime(2023, 12, 19);
        var dateToCompare = new DateTime(2022, 12, 19);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsDateEqual_LeapYears()
    {
        // Arrange
        var date = new DateTime(2020, 2, 29);
        var dateToCompare = new DateTime(2020, 3, 1);

        // Act
        var result = date.IsDateEqual(dateToCompare);

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void TestIsTimeEqual_SameTimes()
    {
        // Arrange
        var time = new DateTime(2022, 12, 18, 12, 0, 0);
        var timeToCompare = new DateTime(2022, 12, 18, 12, 0, 0);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsTimeEqual_SourceBeforeOther()
    {
        // Arrange
        var time = new DateTime(2022, 12, 18, 11, 0, 0);
        var timeToCompare = new DateTime(2022, 12, 18, 12, 0, 0);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsTimeEqual_SourceAfterOther()
    {
        // Arrange
        var time = new DateTime(2022, 12, 18, 13, 0, 0);
        var timeToCompare = new DateTime(2022, 12, 18, 12, 0, 0);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsTimeEqual_MultipleHours()
    {
        // Arrange
        var time = new DateTime(2022, 12, 18, 13, 0, 0);
        var timeToCompare = new DateTime(2022, 12, 18, 12, 0, 0);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsTimeEqual_MultipleDays()
    {
        // Arrange
        var time = new DateTime(2022, 12, 19, 0, 0, 0);
        var timeToCompare = new DateTime(2022, 12, 18, 23, 59, 59);

        // Act
        var result = time.IsTimeEqual(timeToCompare);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsToday_CurrentDate()
    {
        // Arrange
        var date = DateTime.Today;

        // Act
        var result = date.IsToday();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsToday_PastDate()
    {
        // Arrange
        var date = DateTime.Today.AddDays(-1);

        // Act
        var result = date.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsToday_FutureDate()
    {
        // Arrange
        var date = DateTime.Today.AddDays(1);

        // Act
        var result = date.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsToday_MultipleDays()
    {
        // Arrange
        var date = DateTime.Today.AddDays(2);

        // Act
        var result = date.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsToday_MultipleMonths()
    {
        // Arrange
        var date = DateTime.Today.AddMonths(1);

        // Act
        var result = date.IsToday();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TestIsToday_MultipleYears()
    {
        // Arrange
        var date = DateTime.Today.AddYears(1);

        // Act
        var result = date.IsToday();

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void TestGetNumberOfDaysInYear_NormalYear()
    {
        // Arrange
        var year = 2022;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(365, result);
    }

    [Fact]
    public void TestGetNumberOfDaysInYear_LeapYear()
    {
        // Arrange
        var year = 2020;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(366, result);
    }

    [Fact]
    public void TestGetNumberOfDaysInYear_MultipleOf100NotMultipleOf400()
    {
        // Arrange
        var year = 2100;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(365, result);
    }

    [Fact]
    public void TestGetNumberOfDaysInYear_MultipleOf400()
    {
        // Arrange
        var year = 2000;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(366, result);
    }

    [Fact]
    public void TestGetNumberOfDaysInYear_BeforeGregorianCalendar()
    {
        // Arrange
        var year = 1582;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(365, result);
    }

    [Fact]
    public void TestGetNumberOfDaysInYear_AfterCurrentYear()
    {
        // Arrange
        var year = 2023;

        // Act
        var result = DateTimeExtensions.GetNumberOfDaysInYear(year);

        // Assert
        Assert.Equal(365, result);
    }

    [Fact]
    public void TestIsLeapDay_LeapDay()
    {
        // Arrange
        var date = new DateTime(2020, 2, 29);

        // Act
        var result = date.IsLeapDay();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestIsLeapDay_NotLeapDay()
    {
        // Arrange
        var date = new DateTime(2021, 2, 28);

        // Act
        var result = date.IsLeapDay();

        // Assert
        Assert.False(result);
    }


    [Fact]
    public void TestElapsed_PastDate()
    {
        // Arrange
        var startDate = DateTime.Now.AddMinutes(-5);

        // Act
        var result = startDate.Elapsed();

        // Assert
        Assert.True(result.TotalMinutes > 0);
    }

    [Fact]
    public void TestElapsed_FutureDate()
    {
        // Arrange
        var startDate = DateTime.Now.AddMinutes(5);

        // Act
        var result = startDate.Elapsed();

        // Assert
        Assert.True(result.TotalMinutes < 0);
    }


    [Fact]
    public void TestMidnight_NotMidnight()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 10, 0, 0);

        // Act
        var result = time.Midnight();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 0, 0, 0), result);
    }

    [Fact]
    public void TestMidnight_Midnight()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 0, 0, 0);

        // Act
        var result = time.Midnight();

        // Assert
        Assert.Equal(time, result);
    }

    [Fact]
    public void TestMidnight_BeforeMidnight()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 22, 0, 0);

        // Act
        var result = time.Midnight();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 0, 0, 0), result);
    }

    [Fact]
    public void TestMidnight_AfterMidnight()
    {
        // Arrange
        var time = new DateTime(2022, 1, 2, 2, 0, 0);

        // Act
        var result = time.Midnight();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 2, 0, 0, 0), result);
    }

    [Fact]
    public void TestMidnight_Noon()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 12, 0, 0);

        // Act
        var result = time.Midnight();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 0, 0, 0), result);
    }


    [Fact]
    public void TestNoon_NotNoon()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 10, 0, 0);

        // Act
        var result = time.Noon();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), result);
    }

    [Fact]
    public void TestNoon_Noon()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 12, 0, 0);

        // Act
        var result = time.Noon();

        // Assert
        Assert.Equal(time, result);
    }

    [Fact]
    public void TestNoon_BeforeNoon()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 10, 0, 0);

        // Act
        var result = time.Noon();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), result);
    }

    [Fact]
    public void TestNoon_AfterNoon()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 14, 0, 0);

        // Act
        var result = time.Noon();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), result);
    }

    [Fact]
    public void TestNoon_Midnight()
    {
        // Arrange
        var time = new DateTime(2022, 1, 1, 0, 0, 0);

        // Act
        var result = time.Noon();

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), result);
    }


    [Fact]
    public void TestSetTime_DifferentTime()
    {
        // Arrange
        var date = new DateTime(2022, 1, 1);

        // Act
        var result = date.SetTime(12, 30, 45);

        // Assert
        Assert.Equal(new DateTime(2022, 1, 1, 12, 30, 45), result);
    }

    [Fact]
    public void TestSetTime_SameTime()
    {
        // Arrange
        var date = new DateTime(2022, 1, 1, 12, 30, 45);
        var time = new TimeSpan(12, 30, 45);

        // Act
        var result = date.SetTime(time);

        // Assert
        Assert.Equal(date, result);
    }

    [Fact]
    public void SetTime_ReturnsCorrectDateTime_ForMidnight()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);
        var timeOnly = new TimeOnly(0, 0, 0);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        var expectedResult = new DateTime(2020, 1, 1, 0, 0, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SetTime_ReturnsCorrectDateTime_ForNoon()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);
        var timeOnly = new TimeOnly(12, 0, 0);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        var expectedResult = new DateTime(2020, 1, 1, 12, 0, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SetTime_ReturnsCorrectDateTime_ForEvening()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);
        var timeOnly = new TimeOnly(18, 30, 0);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        var expectedResult = new DateTime(2020, 1, 1, 18, 30, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SetTime_ReturnsCorrectDateTime_ForNonMidnightTime()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);
        var timeOnly = new TimeOnly(3, 15, 0);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        var expectedResult = new DateTime(2020, 1, 1, 3, 15, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SetTime_ReturnsCorrectDateTime_ForDifferentDate()
    {
        // Arrange
        var date = new DateTime(2020, 2, 14);
        var timeOnly = new TimeOnly(12, 0, 0);

        // Act
        var result = date.SetTime(timeOnly);

        // Assert
        var expectedResult = new DateTime(2020, 2, 14, 12, 0, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_ReturnsCorrectDate_ForEndOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 1, 31);

        // Act
        var result = date.NextDay();

        // Assert
        var expectedResult = new DateTime(2020, 2, 1);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_ReturnsCorrectDate_ForEndOfYear()
    {
        // Arrange
        var date = new DateTime(2020, 12, 31);

        // Act
        var result = date.NextDay();

        // Assert
        var expectedResult = new DateTime(2021, 1, 1);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_ReturnsCorrectDate_ForLeapYear()
    {
        // Arrange
        var date = new DateTime(2020, 2, 28);

        // Act
        var result = date.NextDay();

        // Assert
        var expectedResult = new DateTime(2020, 2, 29);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_ReturnsCorrectDate_ForNonLeapYear()
    {
        // Arrange
        var date = new DateTime(2021, 2, 28);

        // Act
        var result = date.NextDay();

        // Assert
        var expectedResult = new DateTime(2021, 3, 1);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void NextDay_ReturnsCorrectDate_ForNonEndOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 1, 15);

        // Act
        var result = date.NextDay();

        // Assert
        var expectedResult = new DateTime(2020, 1, 16);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PreviousDay_ReturnsCorrectDate_ForBeginningOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        var expectedResult = new DateTime(2019, 12, 31);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PreviousDay_ReturnsCorrectDate_ForBeginningOfYear()
    {
        // Arrange
        var date = new DateTime(2020, 1, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        var expectedResult = new DateTime(2019, 12, 31);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PreviousDay_ReturnsCorrectDate_ForLeapYear()
    {
        // Arrange
        var date = new DateTime(2020, 3, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        var expectedResult = new DateTime(2020, 2, 29);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PreviousDay_ReturnsCorrectDate_ForNonLeapYear()
    {
        // Arrange
        var date = new DateTime(2021, 3, 1);

        // Act
        var result = date.PreviousDay();

        // Assert
        var expectedResult = new DateTime(2021, 2, 28);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PreviousDay_ReturnsCorrectDate_ForNonBeginningOfMonth()
    {
        // Arrange
        var date = new DateTime(2020, 1, 15);

        // Act
        var result = date.PreviousDay();

        // Assert
        var expectedResult = new DateTime(2020, 1, 14);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GenerateTimestamp_ReturnsCorrectTimestamp()
    {
        // Arrange
        var dateTime = new DateTime(2020, 1, 1, 12, 0, 0);

        // Act
        var result = dateTime.GenerateTimestamp();

        // Assert
        var expectedResult = "202001011200000000";
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetDatesInRange_FromDateEqualsToDate_ReturnsSingleDate()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 1);
        var toDate = new DateTime(2022, 1, 1);

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(fromDate, result.Single());
    }

    [Fact]
    public void GetDatesInRange_FromDateBeforeToDate_ReturnsDatesInRange()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 1);
        var toDate = new DateTime(2022, 1, 3);

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Equal(new[] { fromDate, fromDate.AddDays(1), fromDate.AddDays(2) }, result);
    }

    [Fact]
    public void GetDatesInRange_FromDateAfterToDate_ReturnsDatesInRange()
    {
        // Arrange
        var fromDate = new DateTime(2022, 1, 3);
        var toDate = new DateTime(2022, 1, 1);

        // Act
        var result = fromDate.GetDatesInRange(toDate);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Equal(new[] { fromDate, fromDate.AddDays(-1), fromDate.AddDays(-2) }, result);
    }

    [Fact]
    public void ToDateOnly_ReturnsCorrectDateOnly()
    {
        // Arrange
        var dateTime = new DateTime(2022, 12, 18, 10, 15, 30);

        // Act
        var dateOnly = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(2022, dateOnly.Year);
        Assert.Equal(12, dateOnly.Month);
        Assert.Equal(18, dateOnly.Day);
    }

    [Fact]
    public void ToDateOnly_ReturnsCorrectDateOnly_ForMinDate()
    {
        // Arrange
        var dateTime = DateTime.MinValue;

        // Act
        var dateOnly = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(1, dateOnly.Year);
        Assert.Equal(1, dateOnly.Month);
        Assert.Equal(1, dateOnly.Day);
    }

    [Fact]
    public void ToDateOnly_ReturnsCorrectDateOnly_ForMaxDate()
    {
        // Arrange
        var dateTime = DateTime.MaxValue;

        // Act
        var dateOnly = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(9999, dateOnly.Year);
        Assert.Equal(12, dateOnly.Month);
        Assert.Equal(31, dateOnly.Day);
    }

    [Fact]
    public void ToDateOnly_ReturnsCorrectDateOnly_ForLeapYear()
    {
        // Arrange
        var dateTime = new DateTime(2020, 2, 29, 10, 15, 30);

        // Act
        var dateOnly = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(2020, dateOnly.Year);
        Assert.Equal(2, dateOnly.Month);
        Assert.Equal(29, dateOnly.Day);
    }

    [Fact]
    public void ToDateOnly_ReturnsCorrectDateOnly_ForNonLeapYear()
    {
        // Arrange
        var dateTime = new DateTime(2021, 2, 28, 10, 15, 30);

        // Act
        var dateOnly = dateTime.ToDateOnly();

        // Assert
        Assert.Equal(2021, dateOnly.Year);
        Assert.Equal(2, dateOnly.Month);
        Assert.Equal(28, dateOnly.Day);
    }

    [Fact]
    public void ToTimeOnly_ConvertsDateTimeToTimeOnly()
    {
        // Arrange
        var dateTime = new DateTime(2022, 12, 18, 13, 24, 53, 123, DateTimeKind.Utc);

        // Act
        var timeOnly = dateTime.ToTimeOnly();

        // Assert
        Assert.Equal(13, timeOnly.Hour);
        Assert.Equal(24, timeOnly.Minute);
        Assert.Equal(53, timeOnly.Second);
        Assert.Equal(123, timeOnly.Millisecond);
        Assert.Equal(0, timeOnly.Microsecond);
    }


    [Fact]
    public void ToTimeOnly_HandlesMinimumDateTime()
    {
        // Arrange
        var dateTime = DateTime.MinValue;

        // Act
        var timeOnly = dateTime.ToTimeOnly();

        // Assert
        Assert.Equal(0, timeOnly.Hour);
        Assert.Equal(0, timeOnly.Minute);
        Assert.Equal(0, timeOnly.Second);
        Assert.Equal(0, timeOnly.Millisecond);
        Assert.Equal(0, timeOnly.Microsecond);
    }

    [Fact]
    public void ToTimeOnly_HandlesMaximumDateTime()
    {
        // Arrange
        var dateTime = DateTime.MaxValue;

        // Act
        var timeOnly = dateTime.ToTimeOnly();

        // Assert
        Assert.Equal(DateTime.MaxValue.Hour, timeOnly.Hour);
        Assert.Equal(DateTime.MaxValue.Minute, timeOnly.Minute);
        Assert.Equal(DateTime.MaxValue.Second, timeOnly.Second);
        Assert.Equal(DateTime.MaxValue.Millisecond, timeOnly.Millisecond);
        Assert.Equal(DateTime.MaxValue.Microsecond, timeOnly.Microsecond);
    }
}