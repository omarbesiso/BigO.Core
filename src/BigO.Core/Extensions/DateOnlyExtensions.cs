using System.Globalization;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
{
    /// <summary>
    ///     Calculates the age based on the date of birth, an optional maturity date, and an optional time zone.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="maturityDate">
    ///     An optional maturity date to calculate the age. If not provided, the current date in the specified time zone is
    ///     used.
    /// </param>
    /// <param name="timeZoneInfo">
    ///     An optional time zone to determine the current date if <paramref name="maturityDate" /> is <c>null</c>.
    ///     If not provided, the local system time zone is used.
    /// </param>
    /// <returns>The calculated age in years.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="dateOfBirth" /> is in the future relative to the maturity date or current date.
    /// </exception>
    /// <remarks>
    ///     This method calculates the age in years based on full years elapsed, accounting for birthdays.
    ///     If the <paramref name="maturityDate" /> is <c>null</c>, the current date in the specified time zone is used.
    ///     If the <paramref name="timeZoneInfo" /> is <c>null</c>, the local system time zone is used.
    ///     **Thread Safety:** This method is not thread-safe if used with mutable shared state.
    /// </remarks>
    /// <example>
    ///     The following code example calculates the age based on the date of birth:
    ///     <code><![CDATA[
    /// var dateOfBirth = new DateOnly(1980, 2, 29);
    /// int age = dateOfBirth.Age();
    /// Console.WriteLine(age); // The current age based on the date of birth.
    /// 
    /// age = dateOfBirth.Age(new DateOnly(2023, 2, 28));
    /// Console.WriteLine(age); // The age as of February 28, 2023.
    /// 
    /// var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
    /// age = dateOfBirth.Age(timeZoneInfo: timeZone);
    /// Console.WriteLine(age); // The age based on the date of birth and a specific time zone.
    /// ]]></code>
    /// </example>
    public static int Age(this DateOnly dateOfBirth, DateOnly? maturityDate = null, TimeZoneInfo? timeZoneInfo = null)
    {
        timeZoneInfo ??= TimeZoneInfo.Local;

        var today = maturityDate ?? DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo));

        if (dateOfBirth > today)
        {
            throw new ArgumentException(
                "Date of birth cannot be in the future relative to the maturity date or current date.",
                nameof(dateOfBirth));
        }

        var age = today.Year - dateOfBirth.Year;

        if (today.Month < dateOfBirth.Month ||
            (today.Month == dateOfBirth.Month && today.Day < dateOfBirth.Day))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    ///     Adds a specified number of weeks to a <see cref="DateOnly" /> object.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object to which weeks will be added.</param>
    /// <param name="numberOfWeeks">
    ///     The number of weeks to add, which can be fractional. Positive values add weeks; negative values subtract weeks.
    /// </param>
    /// <returns>
    ///     A <see cref="DateOnly" /> object that is the result of adding the specified number of weeks to the original date.
    /// </returns>
    /// <remarks>
    ///     This method converts the number of weeks to days by multiplying by 7 and then rounds the result to the nearest
    ///     whole number using
    ///     <see cref="Math.Round(double, MidpointRounding)" /> with <see cref="MidpointRounding.AwayFromZero" />.
    ///     The calculated number of days is then added to the original date using <see cref="DateOnly.AddDays(int)" />.
    ///     **Fractional Weeks Handling:**
    ///     - Fractions of a week are converted to days and rounded to the nearest whole day.
    ///     - For example, adding 2.5 weeks results in adding 18 days (2.5 * 7 = 17.5, rounded to 18).
    ///     - Negative fractional weeks are handled similarly, subtracting the rounded number of days.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 1);
    ///     DateOnly newDate = date.AddWeeks(2.5);
    ///     // newDate is January 19, 2023, as 2.5 weeks (17.5 days) rounds to 18 days
    ///     
    ///     newDate = date.AddWeeks(-1.5);
    ///     // newDate is December 21, 2022, as -1.5 weeks (-10.5 days) rounds to -11 days
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly AddWeeks(this DateOnly date, double numberOfWeeks)
    {
        var numberOfDaysToAdd = (int)Math.Round(numberOfWeeks * 7, MidpointRounding.AwayFromZero);
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
    ///     Optional. The day of the week to find within the month. If <c>null</c>, the first day of the month is
    ///     returned.
    /// </param>
    /// <returns>
    ///     The first date of the month, or the first occurrence of the specified <see cref="DayOfWeek" /> within that month.
    /// </returns>
    /// <remarks>
    ///     This method calculates the first date of the specified month from the given date. If a <see cref="DayOfWeek" /> is
    ///     provided, it calculates the offset to the first occurrence of that day within the month without iteration.
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetFirstDateOfMonth(this DateOnly date, DayOfWeek? dayOfWeek = null)
    {
        var firstDateOfMonth = new DateOnly(date.Year, date.Month, 1);

        if (dayOfWeek == null)
        {
            return firstDateOfMonth;
        }

        var daysToAdd = ((int)dayOfWeek.Value - (int)firstDateOfMonth.DayOfWeek + 7) % 7;
        return firstDateOfMonth.AddDays(daysToAdd);
    }

    /// <summary>
    ///     Gets the first date of the week for the specified <see cref="DateOnly" />, based on the specified or current
    ///     culture's first day of the week.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to find the first date of the week.</param>
    /// <param name="cultureInfo">
    ///     Optional. The <see cref="CultureInfo" /> to determine the first day of the week.
    ///     If <c>null</c>, the current culture is used.
    /// </param>
    /// <returns>
    ///     A <see cref="DateOnly" /> object representing the first date of the week for the specified date.
    /// </returns>
    /// <remarks>
    ///     This method calculates the first date of the week based on the specified or current culture's definition of the
    ///     first day of the week.
    ///     It computes the offset from the given date to the first day of the week without iteration.
    ///     This method is particularly useful in applications involving calendar and scheduling functionality where the start
    ///     of the week varies depending on cultural settings.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DateOnly date = new DateOnly(2023, 1, 18); // This is a Wednesday
    ///     DateOnly firstDateOfWeek = date.GetFirstDateOfWeek();
    ///     // firstDateOfWeek is Sunday, January 15, 2023, based on the current culture's first day of the week
    /// 
    ///     // Using a specific culture
    ///     CultureInfo germanCulture = new CultureInfo("de-DE");
    ///     firstDateOfWeek = date.GetFirstDateOfWeek(germanCulture);
    ///     // firstDateOfWeek is Monday, January 16, 2023, based on German culture's first day of the week
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetFirstDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
    {
        var ci = cultureInfo ?? CultureInfo.CurrentCulture;
        var firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;

        var offset = ((int)date.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        return date.AddDays(-offset);
    }

    /// <summary>
    ///     Gets the last date of the month for the specified <see cref="DateOnly" />. Optionally, finds the last occurrence of
    ///     a specific <see cref="DayOfWeek" />.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> object from which to find the last date of the month.</param>
    /// <param name="dayOfWeek">
    ///     Optional. The day of the week to find within the month. If <c>null</c>, the last day of the month is
    ///     returned.
    /// </param>
    /// <returns>
    ///     The last date of the month, or the last occurrence of the specified <see cref="DayOfWeek" /> within that month.
    /// </returns>
    /// <remarks>
    ///     This method calculates the last date of the specified month from the given date. If a <see cref="DayOfWeek" /> is
    ///     provided, it calculates the offset to the last occurrence of that day within the month without iteration.
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetLastDateOfMonth(this DateOnly date, DayOfWeek? dayOfWeek = null)
    {
        var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
        var lastDateOfMonth = new DateOnly(date.Year, date.Month, lastDay);

        if (!dayOfWeek.HasValue)
        {
            return lastDateOfMonth;
        }

        var daysToSubtract = ((int)lastDateOfMonth.DayOfWeek - (int)dayOfWeek.Value + 7) % 7;
        return lastDateOfMonth.AddDays(-daysToSubtract);
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
    /// <returns>
    ///     The number of days between the two dates. The result can be positive or negative depending on the order of the
    ///     dates.
    /// </returns>
    /// <remarks>
    ///     This extension method calculates the number of days between two <see cref="DateOnly" /> instances by subtracting
    ///     their
    ///     <see cref="DateOnly.DayNumber" /> properties.
    ///     **Note:** If <paramref name="toDate" /> is earlier than <paramref name="fromDate" />, the result will be negative.
    ///     Use <c>Math.Abs</c> if you require the absolute number of days.
    /// </remarks>
    /// <example>
    ///     Calculating positive day difference:
    ///     <code><![CDATA[
    /// DateOnly fromDate = new(2023, 3, 1);
    /// DateOnly toDate = new(2023, 3, 28);
    /// int numberOfDays = fromDate.GetNumberOfDays(toDate); // numberOfDays will be 27
    /// ]]></code>
    ///     Calculating negative day difference:
    ///     <code><![CDATA[
    /// DateOnly fromDate = new(2023, 3, 28);
    /// DateOnly toDate = new(2023, 3, 1);
    /// int numberOfDays = fromDate.GetNumberOfDays(toDate); // numberOfDays will be -27
    /// ]]></code>
    ///     Using absolute value:
    ///     <code><![CDATA[
    /// DateOnly fromDate = new(2023, 3, 28);
    /// DateOnly toDate = new(2023, 3, 1);
    /// int numberOfDays = Math.Abs(fromDate.GetNumberOfDays(toDate)); // numberOfDays will be 27
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumberOfDays(this DateOnly fromDate, DateOnly toDate)
    {
        return toDate.DayNumber - fromDate.DayNumber;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance is after another <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="source">The source date to compare.</param>
    /// <param name="other">The other date to compare against.</param>
    /// <returns>
    ///     <see langword="true" /> if the source date is after the other date; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This extension method compares two <see cref="DateOnly" /> instances to determine whether the source date is after
    ///     the other date. It uses the <see cref="DateOnly.CompareTo(DateOnly)" /> method to perform the comparison.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly sourceDate = new(2023, 3, 28);
    /// DateOnly otherDate = new(2023, 3, 1);
    /// bool isAfter = sourceDate.IsAfter(otherDate); // isAfter will be true
    /// ]]></code>
    /// </example>
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
    /// <returns>
    ///     <see langword="true" /> if the source date is before the other date; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This extension method compares two <see cref="DateOnly" /> instances to determine whether the source date is before
    ///     the other date. It uses the <see cref="DateOnly.CompareTo(DateOnly)" /> method to perform the comparison.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly sourceDate = new(2023, 3, 1);
    /// DateOnly otherDate = new(2023, 3, 28);
    /// bool isBefore = sourceDate.IsBefore(otherDate); // isBefore will be true
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBefore(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance represents today's date.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>
    ///     <see langword="true" /> if the date is today; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This method compares the specified <see cref="DateOnly" /> instance to today's date to
    ///     determine whether the instance represents today's date. It uses <see cref="DateOnly.FromDateTime(DateTime)" />
    ///     with <see cref="DateTime.Today" /> to obtain today's date.
    ///     **Note:** This method uses the system's local time zone. If you need to consider different time zones,
    ///     consider using an overload that accepts a <see cref="TimeZoneInfo" /> parameter.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly date = new(2023, 3, 28);
    /// bool isToday = date.IsToday(); // Depends on the current date
    /// 
    /// DateOnly todayDate = DateOnly.FromDateTime(DateTime.Today);
    /// bool isToday = todayDate.IsToday(); // isToday will be true
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsToday(this DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return date == today;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateOnly" /> instance represents a leap day.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>
    ///     <see langword="true" /> if the date is a leap day; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     A leap day is defined as February 29th. This method checks if the date is February 29th.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly leapDay = new(2020, 2, 29);
    /// bool isLeapDay = leapDay.IsLeapDay(); // isLeapDay will be true
    /// 
    /// DateOnly nonLeapDay = new(2021, 2, 28);
    /// bool isLeapDay = nonLeapDay.IsLeapDay(); // isLeapDay will be false
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapDay(this DateOnly date)
    {
        return date.Month == 2 && date.Day == 29;
    }

    /// <summary>
    ///     Determines whether the year of the specified <see cref="DateOnly" /> instance is a leap year.
    /// </summary>
    /// <param name="date">The date whose year to check.</param>
    /// <returns>
    ///     <see langword="true" /> if the year is a leap year; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This method uses <see cref="DateTime.IsLeapYear(int)" /> to determine if the year is a leap year.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// DateOnly dateInLeapYear = new(2020, 3, 1);
    /// bool isLeapYear = dateInLeapYear.IsLeapYear(); // isLeapYear will be true
    /// 
    /// DateOnly dateInNonLeapYear = new(2021, 3, 1);
    /// bool isLeapYear = dateInNonLeapYear.IsLeapYear(); // isLeapYear will be false
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(this DateOnly date)
    {
        return DateTime.IsLeapYear(date.Year);
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> instance representing the day after the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the next day.</param>
    /// <returns>
    ///     A new <see cref="DateOnly" /> instance representing the day after the specified date.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the date is <see cref="DateOnly.MaxValue" /> and cannot have a next day.
    /// </exception>
    /// <remarks>
    ///     Adds one day to the specified <see cref="DateOnly" /> instance to calculate the next day.
    ///     If the date is <see cref="DateOnly.MaxValue" />, an <see cref="InvalidOperationException" /> is thrown.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// var today = DateOnly.Today;
    /// var tomorrow = today.NextDay(); // Tomorrow's date
    /// 
    /// // Edge case:
    /// var maxDate = DateOnly.MaxValue;
    /// // Throws InvalidOperationException
    /// var nextDay = maxDate.NextDay();
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly NextDay(this DateOnly date)
    {
        if (date == DateOnly.MaxValue)
        {
            throw new InvalidOperationException("Cannot get the next day of DateOnly.MaxValue.");
        }

        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> instance representing the day before the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the previous day.</param>
    /// <returns>
    ///     A new <see cref="DateOnly" /> instance representing the day before the specified date.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the date is <see cref="DateOnly.MinValue" /> and cannot have a previous day.
    /// </exception>
    /// <remarks>
    ///     Subtracts one day from the specified <see cref="DateOnly" /> instance to calculate the previous day.
    ///     If the date is <see cref="DateOnly.MinValue" />, an <see cref="InvalidOperationException" /> is thrown.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    /// var today = DateOnly.Today;
    /// var yesterday = today.PreviousDay(); // Yesterday's date
    /// 
    /// // Edge case:
    /// var minDate = DateOnly.MinValue;
    /// // Throws InvalidOperationException
    /// var previousDay = minDate.PreviousDay();
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly PreviousDay(this DateOnly date)
    {
        if (date == DateOnly.MinValue)
        {
            throw new InvalidOperationException("Cannot get the previous day of DateOnly.MinValue.");
        }

        return date.AddDays(-1);
    }

    /// <summary>
    ///     Returns an enumerable of <see cref="DateOnly" /> instances representing the dates in the range between the
    ///     specified <paramref name="fromDate" /> and <paramref name="toDate" />, inclusive.
    /// </summary>
    /// <param name="fromDate">The start date of the range.</param>
    /// <param name="toDate">The end date of the range.</param>
    /// <returns>
    ///     An enumerable of <see cref="DateOnly" /> instances representing the dates in the range between the specified
    ///     <paramref name="fromDate" /> and <paramref name="toDate" />, inclusive.
    /// </returns>
    /// <remarks>
    ///     This method generates an enumerable of dates from <paramref name="fromDate" /> to <paramref name="toDate" />,
    ///     inclusive, in either ascending or descending order depending on the date values.
    /// </remarks>
    /// <example>
    ///     Ascending date range:
    ///     <code><![CDATA[
    /// DateOnly startDate = new(2023, 1, 1);
    /// DateOnly endDate = new(2023, 1, 5);
    /// IEnumerable<DateOnly> dateRange = startDate.GetDatesInRange(endDate);
    /// // dateRange will contain dates from January 1st to January 5th, 2023.
    /// ]]></code>
    ///     Descending date range:
    ///     <code><![CDATA[
    /// DateOnly startDate = new(2023, 1, 5);
    /// DateOnly endDate = new(2023, 1, 1);
    /// IEnumerable<DateOnly> dateRange = startDate.GetDatesInRange(endDate);
    /// // dateRange will contain dates from January 5th to January 1st, 2023.
    /// ]]></code>
    ///     Single date:
    ///     <code><![CDATA[
    /// DateOnly date = new(2023, 1, 1);
    /// IEnumerable<DateOnly> dateRange = date.GetDatesInRange(date);
    /// // dateRange will contain only January 1st, 2023.
    /// ]]></code>
    /// </example>
    public static IEnumerable<DateOnly> GetDatesInRange(this DateOnly fromDate, DateOnly toDate)
    {
        var step = fromDate <= toDate ? 1 : -1;
        var dt = fromDate;

        while (true)
        {
            yield return dt;
            if (dt == toDate)
            {
                break;
            }

            dt = dt.AddDays(step);
        }
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="date" /> is between the specified <paramref name="rangeStart" />
    ///     and
    ///     <paramref name="rangeEnd" /> dates.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <param name="rangeStart">The start date of the range.</param>
    /// <param name="rangeEnd">The end date of the range.</param>
    /// <param name="isInclusive">
    ///     A boolean value indicating whether the range is inclusive or exclusive. Default is
    ///     <see langword="true" /> (inclusive).
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if <paramref name="date" /> is within the specified range; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     This extension method checks if the specified <paramref name="date" /> is between the
    ///     <paramref name="rangeStart" />
    ///     and <paramref name="rangeEnd" /> dates. The order of the range dates does not matter; the method will automatically
    ///     handle cases where <paramref name="rangeStart" /> is after <paramref name="rangeEnd" />.
    ///     If <paramref name="isInclusive" /> is <see langword="true" />, the range is considered
    ///     inclusive, and the method will return <see langword="true" /> if <paramref name="date" /> is equal to either
    ///     <paramref name="rangeStart" /> or <paramref name="rangeEnd" />. If <paramref name="isInclusive" /> is
    ///     <see langword="false" />,
    ///     the range is considered exclusive, and the method will return <see langword="false" /> if <paramref name="date" />
    ///     is equal to
    ///     either <paramref name="rangeStart" /> or <paramref name="rangeEnd" />.
    /// </remarks>
    /// <example>
    ///     Ascending range example:
    ///     <code><![CDATA[
    /// DateOnly date = new DateOnly(2023, 1, 15);
    /// DateOnly rangeStart = new DateOnly(2023, 1, 1);
    /// DateOnly rangeEnd = new DateOnly(2023, 1, 31);
    /// bool result = date.IsBetween(rangeStart, rangeEnd);
    /// // result will be true
    ///     ]]></code>
    ///     Descending range example:
    ///     <code><![CDATA[
    /// DateOnly date = new DateOnly(2023, 1, 15);
    /// DateOnly rangeStart = new DateOnly(2023, 1, 31);
    /// DateOnly rangeEnd = new DateOnly(2023, 1, 1);
    /// bool result = date.IsBetween(rangeStart, rangeEnd);
    /// // result will be true
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this DateOnly date, DateOnly rangeStart, DateOnly rangeEnd, bool isInclusive = true)
    {
        if (rangeStart > rangeEnd)
        {
            // Swap the range to ensure rangeStart <= rangeEnd
            (rangeStart, rangeEnd) = (rangeEnd, rangeStart);
        }

        return isInclusive
            ? date >= rangeStart && date <= rangeEnd
            : date > rangeStart && date < rangeEnd;
    }
}