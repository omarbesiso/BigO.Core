using System.Globalization;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
{
    /// <summary>
    ///     Converts a <see cref="DateOnly" /> object to a <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="dateOnly">The <see cref="DateOnly" /> object to convert.</param>
    /// <returns>A <see cref="DateTime" /> object representing the date with the time set to midnight.</returns>
    /// <remarks>
    ///     This method creates a <see cref="DateTime" /> object from a <see cref="DateOnly" /> object.
    ///     The resulting <see cref="DateTime" /> has the same year, month, and day as the <see cref="DateOnly" /> object,
    ///     and the time component is set to 00:00:00 (midnight). It is useful for scenarios where a <see cref="DateOnly" />
    ///     needs to be used in contexts that require a full <see cref="DateTime" /> object.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly dateOnly = new DateOnly(2023, 1, 15);
    ///     DateTime dateTime = dateOnly.ToDateTime();
    ///     // dateTime is January 15, 2023, at 00:00:00
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    }

    /// <summary>
    ///     Calculates the age of a person based on their date of birth, an optional maturity date, and an optional time zone.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the person.</param>
    /// <param name="maturityDate">
    ///     An optional maturity date to calculate the age. If not provided, the current date will be used.
    /// </param>
    /// <param name="timeZoneInfo">
    ///     An optional time zone to determine the current date if <paramref name="maturityDate" /> is
    ///     <c>null</c>. If not provided, local system time zone will be used.
    /// </param>
    /// <returns>The calculated age in years.</returns>
    /// <remarks>
    ///     This method calculates the age in years based on the number of full years, rounding down if necessary.
    ///     If the <paramref name="maturityDate" /> is null, the current date is used. If the
    ///     <paramref name="timeZoneInfo" /> is null, the local system time zone is used.
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
        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Now))
        {
            ThrowHelper.ThrowArgumentException(nameof(dateOfBirth), "Date of birth cannot be in the future.");
        }

        timeZoneInfo ??= TimeZoneInfo.Local;
        var today = maturityDate ?? TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo).ToDateOnly();

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    ///     Adds a specified number of weeks to a <see cref="DateOnly" /> object.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object to which weeks will be added.</param>
    /// <param name="numberOfWeeks">The number of weeks to add, which can be fractional.</param>
    /// <returns>
    ///     A <see cref="DateOnly" /> object that is the result of adding the specified number of weeks to the original
    ///     date.
    /// </returns>
    /// <remarks>
    ///     This method allows for the addition of fractional weeks to a date by converting the weeks to days and using
    ///     <see cref="DateOnly.AddDays" />.
    ///     It employs <see cref="Math.Ceiling(decimal)" /> to round up to the nearest day, ensuring that partial weeks are
    ///     counted as full days.
    ///     The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option, allowing the JIT compiler to inline the method's body
    ///     at the call site for improved performance.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 1);
    ///     DateOnly newDate = date.AddWeeks(2.5);
    ///     // newDate is January 18, 2023, as 2.5 weeks (17.5 days) rounds up to 18 days
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly AddWeeks(this DateOnly date, double numberOfWeeks)
    {
        var numberOfDaysToAdd = (int)Math.Ceiling(numberOfWeeks * 7);
        return date.AddDays(numberOfDaysToAdd);
    }

    /// <summary>
    ///     Gets the number of days in the month of the specified <see cref="DateOnly" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to extract the month and year.</param>
    /// <returns>The number of days in the month of the given date.</returns>
    /// <remarks>
    ///     This extension method simplifies obtaining the number of days in a specific month and year by using the
    ///     <see cref="DateTime.DaysInMonth" /> method.
    ///     It is particularly useful for date-related calculations, such as generating calendars, planning monthly schedules,
    ///     or validating date input.
    ///     The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option, allowing the JIT compiler to inline the method's body
    ///     at the call site for improved performance.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>The month component of <paramref name="date" /> is less than 1 or greater than 12.</description>
    ///         </item>
    ///         <item>
    ///             <description>The year component of <paramref name="date" /> is less than 1 or greater than 9999.</description>
    ///         </item>
    ///     </list>
    /// </exception>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 2, 15);
    ///     int days = date.DaysInMonth();
    ///     // days is 28, as February 2023 has 28 days
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    ///     Gets the first date of the month for the specified <see cref="DateOnly" />. Optionally, finds the first occurrence
    ///     of a specific <see cref="DayOfWeek" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to extract the month and year.</param>
    /// <param name="dayOfWeek">
    ///     Optional. The day of the week to find within the month. If null, the first day of the month is
    ///     returned.
    /// </param>
    /// <returns>
    ///     The first date of the month, or the first occurrence of the specified <see cref="DayOfWeek" /> within that month.
    /// </returns>
    /// <remarks>
    ///     This method calculates the first date of the specified month from the given date. If a <see cref="DayOfWeek" /> is
    ///     provided,
    ///     it iterates through the dates of the month until it finds the first date that matches the specified day of the
    ///     week.
    ///     This is useful for scheduling and calendar-related functionality where the first occurrence of a particular day is
    ///     needed.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 15);
    ///     DateOnly firstDate = date.GetFirstDateOfMonth();
    ///     // firstDate is January 1, 2023
    /// 
    ///     DateOnly firstMonday = date.GetFirstDateOfMonth(DayOfWeek.Monday);
    ///     // firstMonday is January 2, 2023
    ///     ]]></code>
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
    ///     Gets the first date of the week for the specified <see cref="DateOnly" />, based on the specified or current
    ///     culture's first day of the week.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to find the first date of the week.</param>
    /// <param name="cultureInfo">
    ///     Optional. The <see cref="CultureInfo" /> to determine the first day of the week.
    ///     If null, the current culture is used.
    /// </param>
    /// <returns>
    ///     A <see cref="DateOnly" /> object representing the first date of the week for the specified date.
    /// </returns>
    /// <remarks>
    ///     This method calculates the first date of the week based on the specified or current culture's definition of the
    ///     first day of the week.
    ///     It iterates backwards from the given date until it reaches the defined first day of the week.
    ///     This method is particularly useful in applications involving calendar and scheduling functionality where the start
    ///     of the week varies depending on cultural settings.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 15); // Assuming this is a Wednesday
    ///     DateOnly firstDateOfWeek = date.GetFirstDateOfWeek();
    ///     // firstDateOfWeek is the previous Sunday, based on the current culture's first day of the week
    /// 
    ///     // Using a specific culture
    ///     CultureInfo germanCulture = new CultureInfo("de-DE");
    ///     firstDateOfWeek = date.GetFirstDateOfWeek(germanCulture);
    ///     // firstDateOfWeek is the previous Monday, based on German culture's first day of the week
    ///     ]]></code>
    /// </example>
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
    ///     Gets the last date of the month for the specified <see cref="DateOnly" />. Optionally, finds the last occurrence of
    ///     a specific <see cref="DayOfWeek" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to find the last date of the month.</param>
    /// <param name="dayOfWeek">
    ///     Optional. The day of the week to find within the month. If null, the last day of the month is
    ///     returned.
    /// </param>
    /// <returns>
    ///     The last date of the month, or the last occurrence of the specified <see cref="DayOfWeek" /> within that month.
    /// </returns>
    /// <remarks>
    ///     This method calculates the last date of the specified month from the given date. If a <see cref="DayOfWeek" /> is
    ///     provided,
    ///     it iterates backwards from the last day of the month until it finds the last date that matches the specified day of
    ///     the week.
    ///     This method is useful for scheduling and calendar-related functionality, such as determining the end of a billing
    ///     cycle or planning monthly events.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 15);
    ///     DateOnly lastDate = date.GetLastDateOfMonth();
    ///     // lastDate is January 31, 2023
    /// 
    ///     DateOnly lastFriday = date.GetLastDateOfMonth(DayOfWeek.Friday);
    ///     // lastFriday is January 27, 2023
    ///     ]]></code>
    /// </example>
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