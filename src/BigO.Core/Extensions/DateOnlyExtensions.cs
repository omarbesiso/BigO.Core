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
    ///     Calculates the age of a person based on their date of birth, an optional maturity date, and an optional time zone.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the person.</param>
    /// <param name="maturityDate">
    ///     An optional maturity date to calculate the age. If not provided, the current date will be
    ///     used.
    /// </param>
    /// <param name="timeZoneInfo">
    ///     An optional time zone to determine the current date if <paramref name="maturityDate" /> is
    ///     <c>null</c>. If not provided, UTC will be used.
    /// </param>
    /// <returns>The calculated age in years.</returns>
    /// <remarks>
    ///     This method will calculate the age in years based on the <paramref name="dateOfBirth" /> and optional
    ///     <paramref name="maturityDate" />. If the <paramref name="maturityDate" /> is null, the current date is used. If the
    ///     <paramref name="timeZoneInfo" /> is null, UTC is used. The age is calculated based on the number of full
    ///     years, rounding down if necessary.
    /// </remarks>
    /// <example>
    ///     The following code example calculates the age based on the date of birth:
    ///     <code><![CDATA[
    /// var dateOfBirth = new DateOnly(1980, 1, 1);
    /// int age = dateOfBirth.Age();
    /// Console.WriteLine(age); // The current age based on the date of birth.
    /// age = dateOfBirth.Age(new DateOnly(2022, 1, 1));
    /// Console.WriteLine(age); // The age based on the date of birth and a maturity date.
    /// age = dateOfBirth.Age(timeZoneInfo: TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    /// Console.WriteLine(age); // The age based on the date of birth and a time zone.
    /// ]]></code>
    /// </example>
    public static int Age(this DateOnly dateOfBirth, DateOnly? maturityDate = null, TimeZoneInfo? timeZoneInfo = null)
    {
        var today = maturityDate ?? (timeZoneInfo == null
            ? DateTime.UtcNow.ToDateOnly()
            : TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo).ToDateOnly());

        var age = today.Year - dateOfBirth.Year;

        return dateOfBirth > today.AddYears(-age) ? age - 1 : age;
    }

    /// <summary>
    ///     Adds a specified number of weeks to the specified date.
    /// </summary>
    /// <param name="date">The date to add weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add to the <paramref name="date" />.</param>
    /// <returns>
    ///     The <see cref="DateOnly" /> value that is <paramref name="numberOfWeeks" /> weeks ahead of the
    ///     <paramref name="date" />.
    /// </returns>
    /// <remarks>
    ///     This method adds a specified number of weeks to the specified <paramref name="date" />.
    ///     The number of days to add is calculated as <paramref name="numberOfWeeks" /> times 7.
    /// </remarks>
    /// <example>
    ///     The following code example adds 2 weeks to a date:
    ///     <code><![CDATA[
    /// var date = new DateOnly(2022, 3, 27);
    /// var newDate = date.AddWeeks(2);
    /// Console.WriteLine(newDate); // 2022-04-10
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly AddWeeks(this DateOnly date, double numberOfWeeks)
    {
        return date.AddDays((int)Math.Ceiling(numberOfWeeks * 7));
    }

    /// <summary>
    ///     Gets the number of days in the month of the specified date.
    /// </summary>
    /// <param name="date">The date to get the number of days in the month for.</param>
    /// <returns>The number of days in the month of the specified <paramref name="date" />.</returns>
    /// <remarks>
    ///     This method will get the number of days in the month of the specified <paramref name="date" />.
    /// </remarks>
    /// <example>
    ///     The following code example gets the number of days in the current month:
    ///     <code><![CDATA[
    /// var date = new DateOnly(2022, 3, 27);
    /// int daysInMonth = date.DaysInMonth();
    /// Console.WriteLine(daysInMonth); // 31
    /// ]]></code>
    /// </example>
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    ///     Gets the first date of the month for the specified date, optionally for the specified day of the week.
    /// </summary>
    /// <param name="date">The date to get the first date of the month for.</param>
    /// <param name="dayOfWeek">The optional day of the week to get the first date of the month for.</param>
    /// <returns>The first date of the month for the specified <paramref name="date" />.</returns>
    /// <remarks>
    ///     This method will get the first date of the month for the specified <paramref name="date" />.
    ///     If the optional <paramref name="dayOfWeek" /> is specified, this method will return the first date of the month
    ///     that matches the specified <paramref name="dayOfWeek" />.
    /// </remarks>
    /// <example>
    ///     The following code example gets the first date of the current month:
    ///     <code><![CDATA[
    /// var date = new DateOnly(2022, 3, 27);
    /// var firstDateOfMonth = date.GetFirstDateOfMonth();
    /// Console.WriteLine(firstDateOfMonth); // 2022-03-01
    /// ]]></code>
    ///     The following code example gets the first Friday of the current month:
    ///     <code><![CDATA[
    /// var date = new DateOnly(2022, 3, 27);
    /// var firstFridayOfMonth = date.GetFirstDateOfMonth(DayOfWeek.Friday);
    /// Console.WriteLine(firstFridayOfMonth); // 2022-03-04
    /// ]]></code>
    /// </example>
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
    ///     Gets the first date of the week for a given date, based on the week rules of an optional <see cref="CultureInfo" />
    ///     .
    /// </summary>
    /// <param name="date">The date for which to find the first date of the week.</param>
    /// <param name="cultureInfo">
    ///     An optional <see cref="CultureInfo" /> to determine the first day of the week. If not
    ///     provided, the current culture will be used.
    /// </param>
    /// <returns>The first date of the week for the given date.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly date = new(2023, 3, 28);
    /// DateOnly firstDateOfWeek = date.GetFirstDateOfWeek(); // firstDateOfWeek will be the first date of the week based on the current culture
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the first date of the week for a given date based on the week rules of an optional
    ///     <see cref="CultureInfo" />. If no <paramref name="cultureInfo" /> is provided, the current culture will be used.
    ///     The method iterates through the days of the week, starting from the given date, moving backwards until it finds the
    ///     first day of the week according to the specified culture.
    /// </remarks>
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
    ///     Gets the last date of the month for a given date, optionally filtered by a specific day of the week.
    /// </summary>
    /// <param name="date">The date for which to find the last date of the month.</param>
    /// <param name="dayOfWeek">
    ///     An optional <see cref="DayOfWeek" /> to filter the last date of the month. If not provided, the
    ///     last day of the month will be returned.
    /// </param>
    /// <returns>The last date of the month, optionally filtered by the specified day of the week.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly date = new(2023, 3, 28);
    /// DateOnly lastDateOfMonth = date.GetLastDateOfMonth(); // lastDateOfMonth will be the last date of the month
    /// DateOnly lastFridayOfMonth = date.GetLastDateOfMonth(DayOfWeek.Friday); // lastFridayOfMonth will be the last Friday of the month
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the last date of the month for a given date, optionally filtered by a specific day
    ///     of the week.
    ///     If no <paramref name="dayOfWeek" /> is provided, the last date of the month will be returned. Otherwise, the method
    ///     iterates through the days of the month, starting from the last date, moving backwards until it finds the specified
    ///     day of the week.
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
    ///     Gets the last date of the week for the specified <see cref="DateOnly" /> value.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> value for which to find the last date of the week.</param>
    /// <param name="cultureInfo">
    ///     The <see cref="CultureInfo" /> to use for determining the first day of the week; if
    ///     <c>null</c>, the current culture is used.
    /// </param>
    /// <returns>The last date of the week for the specified <see cref="DateOnly" /> value.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly currentDate = DateOnly.Today;
    /// CultureInfo cultureInfo = new CultureInfo("en-US");
    /// DateOnly lastDateOfWeek = currentDate.GetLastDateOfWeek(cultureInfo);
    /// Console.WriteLine($"Last date of the week: {lastDateOfWeek}");
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method uses the <see cref="GetFirstDateOfWeek" /> method to find the first date of the week and then adds 6
    ///     days to get the last date of the week.
    ///     If <paramref name="cultureInfo" /> is <c>null</c>, the <see cref="CultureInfo.CurrentCulture" /> is used to
    ///     determine the first day of the week.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetLastDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    ///     Calculates the number of days between two <see cref="DateOnly" /> instances.
    /// </summary>
    /// <param name="fromDate">The starting date for the calculation.</param>
    /// <param name="toDate">The ending date for the calculation.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly fromDate = new(2023, 3, 1);
    /// DateOnly toDate = new(2023, 3, 28);
    /// int numberOfDays = fromDate.GetNumberOfDays(toDate); // numberOfDays will be 27
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the number of days between two <see cref="DateOnly" /> instances by converting
    ///     them to <see cref="DateTime" /> instances with <see cref="TimeOnly.MinValue" /> as the time component, and then
    ///     subtracting the starting date from the ending date. The resulting <see cref="TimeSpan" /> is used to calculate the
    ///     total number of days between the dates.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumberOfDays(this DateOnly fromDate, DateOnly toDate)
    {
        var timeSpan = toDate.ToDateTime(TimeOnly.MinValue).Subtract(fromDate.ToDateTime(TimeOnly.MinValue));
        return Convert.ToInt32(timeSpan.TotalDays);
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance is after another <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="source">The source date to compare.</param>
    /// <param name="other">The other date to compare against.</param>
    /// <returns><c>true</c> if the source date is after the other date; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly sourceDate = new(2023, 3, 28);
    /// DateOnly otherDate = new(2023, 3, 1);
    /// bool isAfter = sourceDate.IsAfter(otherDate); // isAfter will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares two <see cref="DateOnly" /> instances to determine whether the source date is after
    ///     the other date. It uses the <see cref="DateOnly.CompareTo(DateOnly)" /> method to perform the comparison and
    ///     returns <c>true</c> if the source date is greater than the other date, indicating that it is after the other date;
    ///     otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAfter(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance is before another <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="source">The source date to compare.</param>
    /// <param name="other">The other date to compare against.</param>
    /// <returns><c>true</c> if the source date is before the other date; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly sourceDate = new(2023, 3, 1);
    /// DateOnly otherDate = new(2023, 3, 28);
    /// bool isBefore = sourceDate.IsBefore(otherDate); // isBefore will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares two <see cref="DateOnly" /> instances to determine whether the source date is before
    ///     the other date. It uses the <see cref="DateOnly.CompareTo(DateOnly)" /> method to perform the comparison and
    ///     returns <c>true</c> if the source date is less than the other date, indicating that it is before the other date;
    ///     otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBefore(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance represents today's date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is today; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly todayDate = DateOnly.Today;
    /// bool isToday = todayDate.IsToday(); // isToday will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares the specified <see cref="DateOnly" /> instance to the current system date to
    ///     determine whether the instance represents today's date. It does so by comparing the year, month, and day components
    ///     of the date to the current system date. If the year, month, and day components match, the method returns
    ///     <c>true</c>, indicating that the date is today; otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsToday(this DateOnly date)
    {
        var today = DateTime.Today;
        return today.Year == date.Year && today.Month == date.Month && today.Day == date.Day;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance represents a leap day.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is a leap day; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly leapDay = new(2020, 2, 29);
    /// bool isLeapDay = leapDay.IsLeapDay(); // isLeapDay will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method checks if the specified <see cref="DateOnly" /> instance represents a leap day. A leap day is
    ///     defined as the 29th day of February in a leap year. If the date's month component is 2 (February) and the day
    ///     component is 29, the method returns <c>true</c>, indicating that the date is a leap day; otherwise, it returns
    ///     <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapDay(this DateOnly date)
    {
        return date is { Month: 2, Day: 29 };
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> instance representing the day after the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the next day.</param>
    /// <returns>A new <see cref="DateOnly" /> instance representing the day after the specified date.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly today = DateOnly.Today;
    /// DateOnly tomorrow = today.NextDay(); // tomorrow will be the day after today
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method adds one day to the specified <see cref="DateOnly" /> instance to calculate the next day. It
    ///     returns a new <see cref="DateOnly" /> instance representing the day after the specified date, without modifying the
    ///     original date. If the specified date is the last day of the year, the returned date will be the first day of the
    ///     next year.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly NextDay(this DateOnly date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> instance representing the day before the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the previous day.</param>
    /// <returns>A new <see cref="DateOnly" /> instance representing the day before the specified date.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly today = DateOnly.Today;
    /// DateOnly yesterday = today.PreviousDay(); // yesterday will be the day before today
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method subtracts one day from the specified <see cref="DateOnly" /> instance to calculate the
    ///     previous day. It returns a new <see cref="DateOnly" /> instance representing the day before the specified date,
    ///     without modifying the original date. If the specified date is the first day of the year, the returned date will be
    ///     the last day of the previous year.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly PreviousDay(this DateOnly date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Returns an enumerable of <see cref="DateOnly" /> instances representing the dates in the range between the
    ///     specified <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive).
    /// </summary>
    /// <param name="fromDate">The start date of the range.</param>
    /// <param name="toDate">The end date of the range.</param>
    /// <returns>
    ///     An enumerable of <see cref="DateOnly" /> instances representing the dates in the range between the specified
    ///     <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive).
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly startDate = new DateOnly(2023, 1, 1);
    /// DateOnly endDate = new DateOnly(2023, 1, 5);
    /// IEnumerable<DateOnly> dateRange = startDate.GetDatesInRange(endDate);
    /// // dateRange will contain DateOnly instances for the dates 1st, 2nd, 3rd, 4th, and 5th of January 2023
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method generates an enumerable of <see cref="DateOnly" /> instances representing the dates in the
    ///     range between the specified <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive). If
    ///     <paramref name="fromDate" /> is equal to <paramref name="toDate" />, the enumerable will contain a single element
    ///     representing the specified date. If <paramref name="fromDate" /> is less than <paramref name="toDate" />, the
    ///     enumerable will be generated in ascending order. If <paramref name="fromDate" /> is greater than
    ///     <paramref name="toDate" />, the enumerable will be generated in descending order.
    /// </remarks>
    public static IEnumerable<DateOnly> GetDatesInRange(this DateOnly fromDate, DateOnly toDate)
    {
        if (fromDate == toDate)
        {
            yield return fromDate;
        }
        else if (fromDate < toDate)
        {
            for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
            {
                yield return dt;
            }
        }
        else
        {
            for (var dt = fromDate; dt >= toDate; dt = dt.AddDays(-1))
            {
                yield return dt;
            }
        }
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="dt" /> is between the specified <paramref name="rangeBeg" /> and
    ///     <paramref name="rangeEnd" /> dates.
    /// </summary>
    /// <param name="dt">The date to check.</param>
    /// <param name="rangeBeg">The start date of the range.</param>
    /// <param name="rangeEnd">The end date of the range.</param>
    /// <param name="isInclusive">
    ///     A boolean value indicating whether the range is inclusive or exclusive. Default is
    ///     <c>true</c> (inclusive).
    /// </param>
    /// <returns><c>true</c> if <paramref name="dt" /> is within the specified range; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly date = new DateOnly(2023, 1, 15);
    /// DateOnly rangeStart = new DateOnly(2023, 1, 1);
    /// DateOnly rangeEnd = new DateOnly(2023, 1, 31);
    /// bool result = date.IsBetween(rangeStart, rangeEnd);
    /// // result will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method checks if the specified <paramref name="dt" /> is between the <paramref name="rangeBeg" />
    ///     and <paramref name="rangeEnd" /> dates. If <paramref name="isInclusive" /> is <c>true</c>, the range is considered
    ///     inclusive, and the method will return <c>true</c> if <paramref name="dt" /> is equal to either
    ///     <paramref name="rangeBeg" /> or <paramref name="rangeEnd" />. If <paramref name="isInclusive" /> is <c>false</c>,
    ///     the range is considered exclusive, and the method will return <c>false</c> if <paramref name="dt" /> is equal to
    ///     either <paramref name="rangeBeg" /> or <paramref name="rangeEnd" />.
    /// </remarks>
    public static bool IsBetween(this DateOnly dt, DateOnly rangeBeg, DateOnly rangeEnd, bool isInclusive = true)
    {
        return isInclusive
            ? dt >= rangeBeg && dt <= rangeEnd
            : dt > rangeBeg && dt < rangeEnd;
    }
}