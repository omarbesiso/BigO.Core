using System.Globalization;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
{
    /// <summary>
    /// Calculates the age based on the specified date of birth and maturity date.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="maturityDate">The maturity date. If not specified, the current date is used.</param>
    /// <returns>The age in years.</returns>
    /// <exception cref="ArgumentException">Thrown if the maturity date occurs before the birth date.</exception>
    /// <remarks>
    /// This method calculates the age by subtracting the year of the birth date from the year of the maturity date. If the maturity date is before the birthday in the current year, the age is reduced by 1.
    /// </remarks>
    public static int Age(this DateOnly dateOfBirth, DateOnly? maturityDate = null)
    {
        maturityDate ??= DateTime.Today.ToDateOnly();

        if (maturityDate < dateOfBirth)
        {
            throw new ArgumentException(
                $"The maturity date '{maturityDate}' cannot occur before the birth date '{dateOfBirth}'");
        }

        if (maturityDate.Value.Month < dateOfBirth.Month ||
            (maturityDate.Value.Month == dateOfBirth.Month &&
             maturityDate.Value.Day < dateOfBirth.Day))
        {
            return maturityDate.Value.Year - dateOfBirth.Year - 1;
        }

        return maturityDate.Value.Year - dateOfBirth.Year;
    }

    /// <summary>
    /// Adds a specified number of weeks to the given date.
    /// </summary>
    /// <param name="date">The date to add weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add. Can be positive or negative.</param>
    /// <returns>The resulting date after the specified number of weeks have been added.</returns>
    /// <remarks>
    /// This method can be used to easily add or subtract a certain number of weeks from a given date.
    /// </remarks>
    public static DateOnly AddWeeks(this DateOnly date, int numberOfWeeks)
    {
        return date.AddDays(numberOfWeeks * 7);
    }

    /// <summary>
    /// Determines the number of days in the month of the given date.
    /// </summary>
    /// <param name="date">The date to determine the number of days in the month for.</param>
    /// <returns>The number of days in the month of the given date.</returns>
    /// <remarks>
    /// This method can be used to easily determine the number of days in the month of a given date.
    /// </remarks>
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    /// Gets the first date of the month of the given date, optionally filtered by day of week.
    /// </summary>
    /// <param name="date">The date to get the first date of the month for.</param>
    /// <param name="dayOfWeek">The day of week to filter the first date of the month by. If not provided, the first date of the month will be returned without filtering by day of week.</param>
    /// <returns>The first date of the month of the given date, optionally filtered by the specified day of week.</returns>
    /// <remarks>
    /// This method can be used to easily get the first date of the month of a given date, and optionally filter the result by day of week.
    /// </remarks>
    public static DateOnly GetFirstDateOfMonth(this DateOnly date, DayOfWeek? dayOfWeek = null)
    {
        var firstDateOfMonth = new DateOnly(date.Year, date.Month, 1);

        if (!dayOfWeek.HasValue)
        {
            return firstDateOfMonth;
        }

        while (firstDateOfMonth.DayOfWeek != dayOfWeek)
        {
            firstDateOfMonth = firstDateOfMonth.AddDays(1);
        }

        return firstDateOfMonth;
    }

    /// <summary>
    /// Gets the first date of the week of the given date, based on the culture-specific first day of the week.
    /// </summary>
    /// <param name="date">The date to get the first date of the week for.</param>
    /// <param name="cultureInfo">The culture to use to determine the first day of the week. If not provided, the current culture will be used.</param>
    /// <returns>The first date of the week of the given date, based on the culture-specific first day of the week.</returns>
    /// <remarks>
    /// This method can be used to easily get the first date of the week of a given date, based on the culture-specific first day of the week.
    /// </remarks>

    public static DateOnly GetFirstDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;

        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        while (date.DayOfWeek != firstDayOfWeek)
        {
            date = date.AddDays(-1);
        }

        return date;
    }

    /// <summary>
    /// Gets the last date of the month of the given date, optionally filtered by day of week.
    /// </summary>
    /// <param name="date">The date to get the last date of the month for.</param>
    /// <param name="dayOfWeek">The day of week to filter the last date of the month by. If not provided, the last date of the month will be returned without filtering by day of week.</param>
    /// <returns>The last date of the month of the given date, optionally filtered by the specified day of week.</returns>
    /// <remarks>
    /// This method can be used to easily get the last date of the month of a given date, and optionally filter the result by day of week.
    /// </remarks>

    public static DateOnly GetLastDateOfMonth(this DateOnly date, DayOfWeek? dayOfWeek = null)
    {
        var lastDateOfMonth = new DateOnly(date.Year, date.Month, DaysInMonth(date));

        if (!dayOfWeek.HasValue)
        {
            return lastDateOfMonth;
        }

        while (lastDateOfMonth.DayOfWeek != dayOfWeek)
        {
            lastDateOfMonth = lastDateOfMonth.AddDays(-1);
        }

        return lastDateOfMonth;
    }

    /// <summary>
    /// Gets the last date of the week of the given date, based on the culture-specific first day of the week.
    /// </summary>
    /// <param name="date">The date to get the last date of the week for.</param>
    /// <param name="cultureInfo">The culture to use to determine the first day of the week. If not provided, the current culture will be used.</param>
    /// <returns>The last date of the week of the given date, based on the culture-specific first day of the week.</returns>
    /// <remarks>
    /// This method can be used to easily get the last date of the week of a given date, based on the culture-specific first day of the week.
    /// </remarks>
    public static DateOnly GetLastDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    /// Calculates the number of days between two dates.
    /// </summary>
    /// <param name="fromDate">The start date.</param>
    /// <param name="toDate">The end date.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <remarks>
    /// This method converts both <paramref name="fromDate"/> and <paramref name="toDate"/> to DateTime objects using the minimum time value (midnight).
    /// The resulting time span is then converted to an integer number of days.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// If either <paramref name="fromDate"/> or <paramref name="toDate"/> is <c>null</c>, an <c>ArgumentNullException</c> is thrown.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the number of days in the time span is too large to fit in an <c>int</c>, an <c>OverflowException</c> is thrown.
    /// </exception>
    public static int GetNumberOfDays(this DateOnly fromDate, DateOnly toDate)
    {
        var timeSpan = toDate.ToDateTime(TimeOnly.MinValue).Subtract(fromDate.ToDateTime(TimeOnly.MinValue));
        return Convert.ToInt32(timeSpan.TotalDays);
    }

    /// <summary>
    /// Determines whether the source date is after the other date.
    /// </summary>
    /// <param name="source">The source date.</param>
    /// <param name="other">The other date.</param>
    /// <returns><c>true</c> if the source date is after the other date; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method compares the source date to the other date using the <see cref="DateOnly.CompareTo(DateOnly)"/> method and returns <c>true</c> if the result is greater than 0.
    /// </remarks>
    public static bool IsAfter(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    /// Determines whether the source date is before the other date.
    /// </summary>
    /// <param name="source">The source date.</param>
    /// <param name="other">The other date.</param>
    /// <returns><c>true</c> if the source date is before the other date; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method compares the source date to the other date using the <see cref="DateOnly.CompareTo(DateOnly)"/> method and returns <c>true</c> if the result is less than 0.
    /// </remarks>
    public static bool IsBefore(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    /// Determines whether the specified date is today's date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the specified date is today's date; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method compares the specified date to today's date using the <see cref="DateOnly.Equals(DateOnly)"/> method. Today's date is obtained by calling the <see cref="DateTime.Today"/> property and converting it to a <see cref="DateOnly"/> object using the <see cref="ToDateOnly"/> extension method.
    /// </remarks>
    public static bool IsToday(this DateOnly date)
    {
        return date == DateTime.Today.ToDateOnly();
    }

    /// <summary>
    /// Determines whether the specified date is a leap day.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the specified date is a leap day (February 29th); otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method uses a pattern-matching expression to check if the month and day of the specified date match February 29th.
    /// </remarks>
    public static bool IsLeapDay(this DateOnly date)
    {
        return date is { Month: 2, Day: 29 };
    }

    /// <summary>
    /// Returns the date of the next day after the specified date.
    /// </summary>
    /// <param name="date">The current date.</param>
    /// <returns>The date of the next day.</returns>
    /// <remarks>
    /// This method uses the <see cref="DateOnly.AddDays"/> method to add one day to the specified date and return the resulting date.
    /// </remarks>
    public static DateOnly NextDay(this DateOnly date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    /// Returns the date of the previous day before the specified date.
    /// </summary>
    /// <param name="date">The current date.</param>
    /// <returns>The date of the previous day.</returns>
    /// <remarks>
    /// This method uses the <see cref="DateOnly.AddDays"/> method to subtract one day from the specified date and return the resulting date.
    /// </remarks>
    public static DateOnly PreviousDay(this DateOnly date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    /// Returns a list of <see cref="DateOnly"/> objects representing the dates between the given range, inclusive.
    /// </summary>
    /// <param name="fromDate">The start of the date range. Must not be <c>null</c>.</param>
    /// <param name="toDate">The end of the date range. Must not be <c>null</c>.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> objects representing the dates between the given range, inclusive.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="fromDate"/> or <paramref name="toDate"/> is <c>null</c>.</exception>

    public static IEnumerable<DateOnly> GetDatesInRange(this DateOnly fromDate, DateOnly toDate)
    {
        if (fromDate == toDate)
        {
            return new[] { new DateOnly(toDate.Year, toDate.Month, toDate.Day) };
        }

        var dates = new List<DateOnly>();

        if (fromDate < toDate)
        {
            for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
        }
        else
        {
            for (var dt = fromDate; dt >= toDate; dt = dt.AddDays(-1))
            {
                dates.Add(dt);
            }
        }

        return dates;
    }

    /// <summary>
    /// Determines whether the given <see cref="DateOnly"/> object falls within a given date range.
    /// </summary>
    /// <param name="dt">The <see cref="DateOnly"/> object to check. Must not be <c>null</c>.</param>
    /// <param name="rangeBeg">The start of the date range. Must not be <c>null</c>.</param>
    /// <param name="rangeEnd">The end of the date range. Must not be <c>null</c>.</param>
    /// <param name="isInclusive">Indicates whether the range includes the start and end dates. Default is <c>true</c>.</param>
    /// <returns><c>true</c> if the given <see cref="DateOnly"/> object falls within the given range; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="dt"/>, <paramref name="rangeBeg"/>, or <paramref name="rangeEnd"/> is <c>null</c>.</exception>
    public static bool IsBetween(this DateOnly dt, DateOnly rangeBeg, DateOnly rangeEnd, bool isInclusive = true)
    {
        // ReSharper disable once InvertIf
        if (isInclusive)
        {
            if (rangeBeg == rangeEnd)
            {
                return dt == rangeBeg;
            }

            return dt >= rangeBeg && dt <= rangeEnd;
        }

        return dt > rangeBeg && dt < rangeEnd;
    }
}