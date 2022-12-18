using System.Globalization;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class DateOnlyExtensionsTests
{
    [Fact]
    public void Age_MaturityDateIsBeforeBirthDate_ThrowsArgumentException()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var maturityDate = new DateOnly(1999, 12, 31);

        // Act
        void Act()
        {
            dateOfBirth.Age(maturityDate);
        }

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }

    [Fact]
    public void Age_MaturityDateIsOnBirthday_ReturnsAge()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var maturityDate = new DateOnly(2020, 1, 1);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(20, age);
    }

    [Fact]
    public void Age_MaturityDateIsAfterBirthday_ReturnsAge()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var maturityDate = new DateOnly(2020, 1, 2);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(20, age);
    }

    [Fact]
    public void Age_MaturityDateIsBeforeBirthday_ReturnsAgeMinusOne()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var maturityDate = new DateOnly(2019, 12, 31);

        // Act
        var age = dateOfBirth.Age(maturityDate);

        // Assert
        Assert.Equal(19, age);
    }

    [Fact]
    public void Age_MaturityDateIsNotProvided_UsesCurrentDate()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);

        // Act
        var age = dateOfBirth.Age();

        // Assert
        Assert.Equal(DateTime.Today.Year - dateOfBirth.Year, age);
    }

    [Fact]
    public void AddWeeks_PositiveNumberOfWeeks_ReturnsExpectedDate()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 1);
        var numberOfWeeks = 2;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateOnly(2020, 1, 15), result);
    }

    [Fact]
    public void AddWeeks_NegativeNumberOfWeeks_ReturnsExpectedDate()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 15);
        var numberOfWeeks = -2;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(new DateOnly(2020, 1, 1), result);
    }

    [Fact]
    public void AddWeeks_ZeroWeeks_ReturnsSameDate()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 1);
        var numberOfWeeks = 0;

        // Act
        var result = date.AddWeeks(numberOfWeeks);

        // Assert
        Assert.Equal(date, result);
    }

    [Fact]
    public void GetCountOfDaysInMonth_MonthWith30Days_Returns30()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.DaysInMonth();

        // Assert
        Assert.Equal(30, result);
    }

    [Fact]
    public void GetCountOfDaysInMonth_MonthWith31Days_Returns31()
    {
        // Arrange
        var date = new DateOnly(2020, 1, 15);

        // Act
        var result = date.DaysInMonth();

        // Assert
        Assert.Equal(31, result);
    }

    [Fact]
    public void GetCountOfDaysInMonth_FebruaryInLeapYear_Returns29()
    {
        // Arrange
        var date = new DateOnly(2020, 2, 15);

        // Act
        var result = date.DaysInMonth();

        // Assert
        Assert.Equal(29, result);
    }

    [Fact]
    public void GetCountOfDaysInMonth_FebruaryInNonLeapYear_Returns28()
    {
        // Arrange
        var date = new DateOnly(2021, 2, 15);

        // Act
        var result = date.DaysInMonth();

        // Assert
        Assert.Equal(28, result);
    }

    [Fact]
    public void GetFirstDateOfMonth_DayOfWeekNotSpecified_ReturnsFirstDateOfMonth()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.GetFirstDateOfMonth();

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 1), result);
    }

    [Fact]
    public void GetFirstDateOfMonth_DayOfWeekSpecified_ReturnsFirstDateOfMonthOnThatDay()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.GetFirstDateOfMonth(DayOfWeek.Tuesday);

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 7), result);
    }

    [Fact]
    public void GetFirstDateOfWeek_CultureInfoNotSpecified_UsesCurrentCulture()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act
        var result = date.GetFirstDateOfWeek();

        // Assert
        Assert.Equal(DayOfWeek.Sunday, result.DayOfWeek);
    }

    [Fact]
    public void GetFirstDateOfWeek_CultureInfoSpecified_UsesSpecifiedCulture()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);
        var cultureInfo = new CultureInfo("fr-FR");

        // Act
        var result = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(DayOfWeek.Monday, result.DayOfWeek);
    }

    [Fact]
    public void GetFirstDateOfWeek_DateIsOnFirstDayOfWeek_ReturnsSameDate()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 12);
        var cultureInfo = new CultureInfo("en-US");

        // Act
        var result = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(date, result);
    }

    [Fact]
    public void GetFirstDateOfWeek_DateIsAfterFirstDayOfWeek_ReturnsFirstDayOfWeek()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);
        var cultureInfo = new CultureInfo("en-US");

        // Act
        var result = date.GetFirstDateOfWeek(cultureInfo);

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 12), result);
    }

    [Fact]
    public void GetLastDateOfMonth_DayOfWeekNotSpecified_ReturnsLastDateOfMonth()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.GetLastDateOfMonth();

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 30), result);
    }

    [Fact]
    public void GetLastDateOfMonth_DayOfWeekSpecified_ReturnsLastDateOfMonthOnThatDay()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.GetLastDateOfMonth(DayOfWeek.Tuesday);

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 28), result);
    }

    [Fact]
    public void GetLastDateOfMonth_DayOfWeekSpecifiedButNotInMonth_ReturnsLastDateOfPreviousMonthOnThatDay()
    {
        // Arrange
        var date = new DateOnly(2020, 4, 15);

        // Act
        var result = date.GetLastDateOfMonth(DayOfWeek.Sunday);

        // Assert
        Assert.Equal(new DateOnly(2020, 4, 26), result);
    }

    [Fact]
    public void GetNumberOfDays_FromDateBeforeToDate_ReturnsPositiveNumber()
    {
        // Arrange
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 2);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetNumberOfDays_FromDateAfterToDate_ReturnsNegativeNumber()
    {
        // Arrange
        var fromDate = new DateOnly(2020, 1, 2);
        var toDate = new DateOnly(2020, 1, 1);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void GetNumberOfDays_FromDateEqualToToDate_ReturnsZero()
    {
        // Arrange
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 1);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetNumberOfDays_FromDateOneYearBeforeToDate_Returns365()
    {
        // Arrange
        var fromDate = new DateOnly(2019, 1, 1);
        var toDate = new DateOnly(2020, 1, 1);

        // Act
        var result = fromDate.GetNumberOfDays(toDate);

        // Assert
        Assert.Equal(365, result);
    }

    [Fact]
    public void IsAfter_SameYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 2, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.True(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsAfter_DifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2019, 12, 31);

        Assert.True(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsAfter_SameMonthDifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2019, 1, 1);

        Assert.True(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsAfter_SameDayDifferentMonth_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 2, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.True(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsAfter_SameDaySameMonthDifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2019, 1, 1);

        Assert.True(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsAfter_SameDaySameMonthSameYear_ReturnsFalse()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.False(date1.IsAfter(date2));
        Assert.False(date2.IsAfter(date1));
    }

    [Fact]
    public void IsBefore_SameYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2020, 2, 1);

        Assert.True(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBefore_DifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2019, 12, 31);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.True(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBefore_SameMonthDifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2019, 1, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.True(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBefore_SameDayDifferentMonth_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2020, 2, 1);

        Assert.True(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBefore_SameDaySameMonthDifferentYear_ReturnsCorrectResult()
    {
        var date1 = new DateOnly(2019, 1, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.True(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBefore_SameDaySameMonthSameYear_ReturnsFalse()
    {
        var date1 = new DateOnly(2020, 1, 1);
        var date2 = new DateOnly(2020, 1, 1);

        Assert.False(date1.IsBefore(date2));
        Assert.False(date2.IsBefore(date1));
    }

    [Fact]
    public void IsBetween_StartAndEndDatesAreTheSame_ReturnsTrue()
    {
        var date = new DateOnly(2020, 1, 1);
        var rangeBeg = new DateOnly(2020, 1, 1);
        var rangeEnd = new DateOnly(2020, 1, 1);

        Assert.True(date.IsBetween(rangeBeg, rangeEnd));
        Assert.False(date.IsBetween(rangeBeg, rangeEnd, false));
    }

    [Fact]
    public void IsBetween_DateIsBeforeStartDate_ReturnsFalse()
    {
        var date = new DateOnly(2019, 12, 31);
        var rangeBeg = new DateOnly(2020, 1, 1);
        var rangeEnd = new DateOnly(2020, 1, 2);

        Assert.False(date.IsBetween(rangeBeg, rangeEnd));
        Assert.False(date.IsBetween(rangeBeg, rangeEnd, false));
    }

    [Fact]
    public void IsBetween_DateIsAfterEndDate_ReturnsFalse()
    {
        var date = new DateOnly(2020, 1, 3);
        var rangeBeg = new DateOnly(2020, 1, 1);
        var rangeEnd = new DateOnly(2020, 1, 2);

        Assert.False(date.IsBetween(rangeBeg, rangeEnd));
        Assert.False(date.IsBetween(rangeBeg, rangeEnd, false));
    }

    [Fact]
    public void IsBetween_DateIsInclusive_ReturnsTrue()
    {
        var date = new DateOnly(2020, 1, 2);
        var rangeBeg = new DateOnly(2020, 1, 1);
        var rangeEnd = new DateOnly(2020, 1, 2);

        Assert.True(date.IsBetween(rangeBeg, rangeEnd));
    }

    [Fact]
    public void IsBetween_DateIsExclusive_ReturnsFalse()
    {
        var date = new DateOnly(2020, 1, 2);
        var rangeBeg = new DateOnly(2020, 1, 1);
        var rangeEnd = new DateOnly(2020, 1, 2);

        Assert.False(date.IsBetween(rangeBeg, rangeEnd, false));
    }

    [Fact]
    public void IsBetween_DateIsBetweenStartAndEndDates_ReturnsTrue()
    {
        var date = new DateOnly(2020, 1, 1);
        var rangeBeg = new DateOnly(2019, 12, 31);
        var rangeEnd = new DateOnly(2020, 1, 2);

        Assert.True(date.IsBetween(rangeBeg, rangeEnd));
        Assert.True(date.IsBetween(rangeBeg, rangeEnd, false));
    }

    [Fact]
    public void IsToday_DateIsToday_ReturnsTrue()
    {
        var today = DateTime.Today.ToDateOnly();
        Assert.True(today.IsToday());
    }

    [Fact]
    public void IsToday_DateIsYesterday_ReturnsFalse()
    {
        var yesterday = DateTime.Today.AddDays(-1).ToDateOnly();
        Assert.False(yesterday.IsToday());
    }

    [Fact]
    public void IsToday_DateIsTomorrow_ReturnsFalse()
    {
        var tomorrow = DateTime.Today.AddDays(1).ToDateOnly();
        Assert.False(tomorrow.IsToday());
    }

    [Fact]
    public void IsToday_DateIsNotToday_ReturnsFalse()
    {
        var notToday = new DateOnly(2020, 1, 1);
        Assert.False(notToday.IsToday());
    }

    [Fact]
    public void IsLeapDay_DateIsLeapDay_ReturnsTrue()
    {
        var leapDay = new DateOnly(2020, 2, 29);
        Assert.True(leapDay.IsLeapDay());
    }

    [Fact]
    public void IsLeapDay_DateIsNotLeapDay_ReturnsFalse()
    {
        var notLeapDay = new DateOnly(2020, 2, 28);
        Assert.False(notLeapDay.IsLeapDay());
    }

    [Fact]
    public void IsLeapDay_DateIsNotInFebruary_ReturnsFalse()
    {
        var notInFebruary = new DateOnly(2020, 1, 29);
        Assert.False(notInFebruary.IsLeapDay());
    }

    [Fact]
    public void NextDay_DateIsNotEndOfMonth_ReturnsCorrectDate()
    {
        var date = new DateOnly(2020, 1, 1);
        var expected = new DateOnly(2020, 1, 2);
        Assert.Equal(expected, date.NextDay());
    }

    [Fact]
    public void NextDay_DateIsEndOfMonth_ReturnsCorrectDate()
    {
        var date = new DateOnly(2020, 1, 31);
        var expected = new DateOnly(2020, 2, 1);
        Assert.Equal(expected, date.NextDay());
    }

    [Fact]
    public void NextDay_DateIsEndOfYear_ReturnsCorrectDate()
    {
        var date = new DateOnly(2020, 12, 31);
        var expected = new DateOnly(2021, 1, 1);
        Assert.Equal(expected, date.NextDay());
    }

    [Fact]
    public void PreviousDay_DateIsNotStartOfMonth_ReturnsCorrectDate()
    {
        var date = new DateOnly(2020, 1, 2);
        var expected = new DateOnly(2020, 1, 1);
        Assert.Equal(expected, date.PreviousDay());
    }

    [Fact]
    public void PreviousDay_DateIsStartOfMonth_ReturnsCorrectDate()
    {
        var date = new DateOnly(2020, 2, 1);
        var expected = new DateOnly(2020, 1, 31);
        Assert.Equal(expected, date.PreviousDay());
    }

    [Fact]
    public void PreviousDay_DateIsStartOfYear_ReturnsCorrectDate()
    {
        var date = new DateOnly(2021, 1, 1);
        var expected = new DateOnly(2020, 12, 31);
        Assert.Equal(expected, date.PreviousDay());
    }

    [Fact]
    public void GetDatesInRange_FromDateEqualsToDate_ReturnsCorrectDates()
    {
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 1);
        var expected = new[] { new DateOnly(2020, 1, 1) };

        Assert.Equal(expected, fromDate.GetDatesInRange(toDate));
    }

    [Fact]
    public void GetDatesInRange_FromDateBeforeToDate_ReturnsCorrectDates()
    {
        var fromDate = new DateOnly(2020, 1, 1);
        var toDate = new DateOnly(2020, 1, 3);
        var expected = new[]
        {
            new DateOnly(2020, 1, 1),
            new DateOnly(2020, 1, 2),
            new DateOnly(2020, 1, 3)
        };

        Assert.Equal(expected, fromDate.GetDatesInRange(toDate));
    }

    [Fact]
    public void GetDatesInRange_FromDateAfterToDate_ReturnsCorrectDates()
    {
        var fromDate = new DateOnly(2020, 1, 3);
        var toDate = new DateOnly(2020, 1, 1);
        var expected = new[]
        {
            new DateOnly(2020, 1, 3),
            new DateOnly(2020, 1, 2),
            new DateOnly(2020, 1, 1)
        };

        Assert.Equal(expected, fromDate.GetDatesInRange(toDate));
    }

    [Fact]
    public void GetDatesInRange_FromDateAndToDateSpanMultipleMonths_ReturnsCorrectDates()
    {
        var fromDate = new DateOnly(2020, 1, 31);
        var toDate = new DateOnly(2020, 2, 2);
        var expected = new[]
        {
            new DateOnly(2020, 1, 31),
            new DateOnly(2020, 2, 1),
            new DateOnly(2020, 2, 2)
        };

        Assert.Equal(expected, fromDate.GetDatesInRange(toDate));
    }
}