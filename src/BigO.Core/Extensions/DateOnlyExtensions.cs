using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
{
    /// <summary>
    ///     Calculates the age in years between the given <paramref name="dateOfBirth" /> and the optional
    ///     <paramref name="maturityDate" />. If the <paramref name="maturityDate" /> is not provided, the current date is
    ///     used.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth to calculate the age from.</param>
    /// <param name="maturityDate">Optional date to calculate the age up to. If not provided, the current date is used.</param>
    /// <returns>
    ///     The age in years between the given <paramref name="dateOfBirth" /> and the optional
    ///     <paramref name="maturityDate" />. If the <paramref name="maturityDate" /> is not provided, the current date is
    ///     used.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="maturityDate" /> occurs before the <paramref name="dateOfBirth" />.
    /// </exception>
    public static int Age(this DateOnly dateOfBirth, DateOnly? maturityDate = null)
    {
        var maturity = maturityDate ?? DateTime.Now.ToDateOnly();

        if (maturityDate < dateOfBirth)
        {
            throw new ArgumentException(
                $"The maturity date '{maturityDate}' cannot occur before the birth date '{dateOfBirth}'",
                nameof(maturityDate));
        }

        var years = 0;
        var currentYear = dateOfBirth.Year;

        var isBornOnALeapDay = dateOfBirth.Day == 29;

        var birthMonth = dateOfBirth.Month;
        var birthDay = dateOfBirth.Day;

        var currentDate = dateOfBirth;

        while (currentDate < maturity)
        {
            currentYear++;
            if (isBornOnALeapDay && DateTime.IsLeapYear(currentYear))
            {
                currentDate = new DateOnly(currentYear, birthMonth, 29);
            }
            else
            {
                currentDate = new DateOnly(currentYear, birthMonth, birthDay);
            }

            if (currentDate <= maturity)
            {
                years++;
            }
        }

        return years;
    }

    /// <summary>
    ///     Adds the specified number of weeks to the given <paramref name="date" />.
    /// </summary>
    /// <param name="date">The date to add weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add to the date. This value can be negative.</param>
    /// <returns>A new <see cref="DateOnly" /> instance representing the resulting date.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly AddWeeks(this DateOnly date, int numberOfWeeks)
    {
        return date.AddDays(numberOfWeeks * 7);
    }

    /// <summary>
    ///     Gets the number of days in the month of the specified <see cref="DateOnly" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> to get the number of days in the month for.</param>
    /// <returns>The number of days in the month of the specified <see cref="DateOnly" />.</returns>
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    ///     Returns the first date of the month for the given <paramref name="date" />, with the option to specify a specific
    ///     <paramref name="dayOfWeek" />.
    ///     If no <paramref name="dayOfWeek" /> is specified, the first date of the month is returned.
    /// </summary>
    /// <param name="date">The date to get the first date of the month for.</param>
    /// <param name="dayOfWeek">
    ///     Optional parameter to specify a specific day of the week for the first date of the month. If
    ///     not specified, the first date of the month is returned.
    /// </param>
    /// <returns>
    ///     The first date of the month for the given <paramref name="date" />, with the option to specify a specific
    ///     <paramref name="dayOfWeek" />.
    /// </returns>
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
    ///     Returns the first date of the week in which the specified <paramref name="date" /> falls, according to the culture
    ///     specified by the optional <paramref name="cultureInfo" /> parameter. If no culture is provided, the current culture
    ///     is used.
    /// </summary>
    /// <param name="date">The date for which to get the first date of the week.</param>
    /// <param name="cultureInfo">
    ///     The culture information that determines the first day of the week. If this parameter is <c>null</c>, the current
    ///     culture is used.
    /// </param>
    /// <returns>The first date of the week in which <paramref name="date" /> falls.</returns>
    public static DateOnly GetFirstDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        var ci = cultureInfo ?? CultureInfo.CurrentCulture;

        var firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
        while (date.DayOfWeek != firstDayOfWeek)
        {
            date = date.AddDays(-1);
        }

        return date;
    }

    /// <summary>
    ///     Gets the last date of the month for the given <paramref name="date" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> value to get the last date of the month for.</param>
    /// <param name="dayOfWeek">
    ///     The <see cref="DayOfWeek" /> value to get the last date of the month with.
    ///     If not specified, the last date of the month will be returned.
    /// </param>
    /// <returns>
    ///     A <see cref="DateOnly" /> value representing the last date of the month for the given <paramref name="date" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     The <paramref name="date" /> cannot be null.
    /// </exception>
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
    ///     Returns the last day of the week for the specified <paramref name="date" />.
    /// </summary>
    /// <param name="date">The date to get the last day of the week for.</param>
    /// <param name="cultureInfo">
    ///     The culture information to use to determine the first day of the week. If <c>null</c>, the current culture is used.
    /// </param>
    /// <returns>The last day of the week for the specified <paramref name="date" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="date" /> is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetLastDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    ///     Returns the number of days between two dates.
    /// </summary>
    /// <param name="fromDate">The starting date.</param>
    /// <param name="toDate">The ending date.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="toDate" /> occurs before <paramref name="fromDate" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumberOfDays(this DateOnly fromDate, DateOnly toDate)
    {
        var timeSpan = toDate.ToDateTime(TimeOnly.MinValue).Subtract(fromDate.ToDateTime(TimeOnly.MinValue));
        return Convert.ToInt32(timeSpan.TotalDays);
    }

    /// <summary>
    ///     Determines whether the <see cref="DateOnly" /> instance is after the specified <paramref name="other" /> date.
    /// </summary>
    /// <param name="source">The <see cref="DateOnly" /> instance to compare.</param>
    /// <param name="other">The other <see cref="DateOnly" /> to compare with the <paramref name="source" />.</param>
    /// <returns>True if the <paramref name="source" /> is after the <paramref name="other" /> date; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAfter(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Determines whether the source <see cref="DateOnly" /> is before the specified <see cref="DateOnly" />.
    /// </summary>
    /// <param name="source">The source <see cref="DateOnly" /> to compare.</param>
    /// <param name="other">The <see cref="DateOnly" /> to compare with the source.</param>
    /// <returns>
    ///     <c>true</c> if the source <see cref="DateOnly" /> is before the specified <see cref="DateOnly" />; otherwise,
    ///     <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBefore(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Determines whether the given <see cref="DateOnly" /> represents the current date.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> to compare with the current date.</param>
    /// <returns>
    ///     <c>true</c> if the given <see cref="DateOnly" /> represents the current date; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="date" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsToday(this DateOnly date)
    {
        var today = DateTime.Today;
        return today.Year == date.Year && today.Month == date.Month && today.Day == date.Day;
    }

    /// <summary>
    ///     Determines whether the given <see cref="DateOnly" /> represents a leap day (February 29).
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> to check.</param>
    /// <returns>
    ///     <c>true</c> if the given <see cref="DateOnly" /> represents a leap day; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="date" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapDay(this DateOnly date)
    {
        return date is { Month: 2, Day: 29 };
    }

    /// <summary>
    ///     Returns the next day after the given <see cref="DateOnly" />.
    /// </summary>
    /// <param name="date">The starting <see cref="DateOnly" />.</param>
    /// <returns>A new <see cref="DateOnly" /> object representing the next day after the given <see cref="DateOnly" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="date" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly NextDay(this DateOnly date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns the previous day before the given <see cref="DateOnly" />.
    /// </summary>
    /// <param name="date">The starting <see cref="DateOnly" />.</param>
    /// <returns>A new <see cref="DateOnly" /> object representing the previous day before the given <see cref="DateOnly" />.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="date" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly PreviousDay(this DateOnly date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Returns a sequence of <see cref="DateOnly" /> objects representing the dates in the given range.
    /// </summary>
    /// <param name="fromDate">The start of the date range.</param>
    /// <param name="toDate">The end of the date range.</param>
    /// <returns>A sequence of <see cref="DateOnly" /> objects representing the dates in the given range.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="fromDate" /> or <paramref name="toDate" /> is <c>null</c>.</exception>
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
    ///     Determines whether the given <see cref="DateOnly" /> is between the given start and end dates.
    /// </summary>
    /// <param name="dt">The <see cref="DateOnly" /> to check.</param>
    /// <param name="rangeBeg">The start of the date range.</param>
    /// <param name="rangeEnd">The end of the date range.</param>
    /// <param name="isInclusive">
    ///     A value indicating whether the range includes the start and end dates.
    ///     If <c>true</c>, the range is inclusive; if <c>false</c>, the range is exclusive.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the given <see cref="DateOnly" /> is between the given start and end dates; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="dt" />, <paramref name="rangeBeg" />, or <paramref name="rangeEnd" /> is <c>null</c>.
    /// </exception>
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