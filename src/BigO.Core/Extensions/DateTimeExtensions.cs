using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DateTime" /> objects.
/// </summary>
[PublicAPI]
public static class DateTimeExtensions
{
    /// <summary>
    ///     Calculates the age in years of a person based on their date of birth and a given maturity date.
    ///     If no maturity date is provided, the current date will be used.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the person.</param>
    /// <param name="maturityDate">
    ///     The date to use as the end of the age calculation. If <c>null</c>, the current date will be
    ///     used.
    /// </param>
    /// <returns>The age of the person in years.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="maturityDate" /> occurs before the
    ///     <paramref name="dateOfBirth" />.
    /// </exception>
    public static int Age(this DateTime dateOfBirth, DateTime? maturityDate = null)
    {
        var maturityDateTime = maturityDate ?? DateTime.Now.Date;
        var birthDate = dateOfBirth.Date;

        if (maturityDate < birthDate)
        {
            throw new ArgumentException(
                $"The maturity date '{maturityDate}' cannot occur before the birth date '{dateOfBirth}'.",
                nameof(maturityDate));
        }

        var years = 0;
        var currentYear = dateOfBirth.Year;

        var isBornOnALeapDay = dateOfBirth.Day == 29;

        var birthMonth = dateOfBirth.Month;
        var birthDay = dateOfBirth.Day;

        var currentDate = birthDate;

        while (currentDate < maturityDateTime)
        {
            currentYear++;
            if (isBornOnALeapDay && DateTime.IsLeapYear(currentYear))
            {
                currentDate = new DateTime(currentYear, birthMonth, 29);
            }
            else
            {
                currentDate = new DateTime(currentYear, birthMonth, birthDay);
            }

            if (currentDate <= maturityDateTime)
            {
                years++;
            }
        }

        return years;
    }

    /// <summary>
    ///     Adds a specified number of weeks to a given date.
    /// </summary>
    /// <param name="date">The base date to add the weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add to the date. This value can be fractional.</param>
    /// <returns>
    ///     A new <see cref="DateTime" /> object representing the resulting date after adding the specified number of
    ///     weeks.
    /// </returns>
    /// <remarks>
    ///     The number of weeks to add is first converted to a number of days by multiplying it by 7.
    ///     This resulting number of days is then passed to the <see cref="DateTime.AddDays(double)" /> method to calculate the
    ///     final resulting date.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AddWeeks(this DateTime date, double numberOfWeeks)
    {
        return date.AddDays((int)Math.Ceiling(numberOfWeeks * 7));
    }

    /// <summary>
    ///     Determines the number of days in the month of a given date.
    /// </summary>
    /// <param name="date">The date whose month should be used to determine the number of days.</param>
    /// <returns>The number of days in the month of the given date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the year or month values of the given <paramref name="date" />
    ///     are outside the valid range of dates.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="DateTime.DaysInMonth(int, int)" /> method to calculate the number of days in the
    ///     month of the given date.
    ///     The year and month values for the given date are passed as arguments to this method.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DaysInMonth(this DateTime date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    ///     Returns the first date of the month of a given date, optionally specifying a specific day of the week.
    /// </summary>
    /// <param name="date">The date whose month should be used to determine the first date.</param>
    /// <param name="dayOfWeek">
    ///     The day of the week to return the first date for. If <c>null</c>, the first date of the month
    ///     will be returned regardless of the day of the week.
    /// </param>
    /// <returns>The first date of the month of the given date, optionally on the specified day of the week.</returns>
    /// <remarks>
    ///     If the <paramref name="dayOfWeek" /> parameter is not provided, the first date of the month will be returned
    ///     directly.
    ///     If the <paramref name="dayOfWeek" /> parameter is provided, the method will iterate through the days of the month
    ///     until it finds the first date that matches the specified day of the week.
    /// </remarks>
    public static DateTime GetFirstDateOfMonth(this DateTime date, DayOfWeek? dayOfWeek = null)
    {
        var firstDateOfMonth = new DateTime(date.Year, date.Month, 1);

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
    ///     Gets the first date of the week of a given date, using the culture-specific first day of the week as defined by the
    ///     provided (or current) <see cref="CultureInfo" /> object.
    /// </summary>
    /// <param name="date">The date to get the first date of the week for.</param>
    /// <param name="cultureInfo">
    ///     The <see cref="CultureInfo" /> object to use to determine the first day of the week. If not
    ///     provided, the current culture is used. <c>null</c> values are allowed.
    /// </param>
    /// <returns>The first date of the week of the given date, using the culture-specific first day of the week.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if a valid <see cref="CultureInfo" /> object is not provided and the
    ///     current culture is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     The first date of the week is calculated by repeatedly subtracting one day from the given date until it falls on
    ///     the culture-specific first day of the week.
    ///     The culture-specific first day of the week is determined by the <see cref="DateTimeFormatInfo.FirstDayOfWeek" />
    ///     property of the provided (or current) <see cref="CultureInfo" /> object.
    /// </remarks>
    public static DateTime GetFirstDateOfWeek(this DateTime date, CultureInfo? cultureInfo = null)
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
    ///     Returns the last date of the month of a given date, optionally specifying a specific day of the week.
    /// </summary>
    /// <param name="date">The date whose month should be used to determine the last date.</param>
    /// <param name="dayOfWeek">
    ///     The day of the week to return the last date for. If <c>null</c>, the last date of the month
    ///     will be returned regardless of the day of the week.
    /// </param>
    /// <returns>The last date of the month of the given date, optionally on the specified day of the week.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the year or month values of the given <paramref name="date" />
    ///     are outside the valid range of dates.
    /// </exception>
    /// <remarks>
    ///     If the <paramref name="dayOfWeek" /> parameter is not provided, the last date of the month will be returned
    ///     directly.
    ///     If the <paramref name="dayOfWeek" /> parameter is provided, the method will iterate through the days of the month
    ///     starting from the last date until it finds the last date that matches the specified day of the week.
    /// </remarks>
    public static DateTime GetLastDateOfMonth(this DateTime date, DayOfWeek? dayOfWeek = null)
    {
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var lastDateOfMonth = new DateTime(date.Year, date.Month, daysInMonth);

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
    ///     Gets the last date of the week of a given date, using the culture-specific first day of the week as defined by the
    ///     provided (or current) <see cref="CultureInfo" /> object.
    /// </summary>
    /// <param name="date">The date to get the last date of the week for.</param>
    /// <param name="cultureInfo">
    ///     The <see cref="CultureInfo" /> object to use to determine the first day of the week. If not
    ///     provided, the current culture is used. <c>null</c> values are allowed.
    /// </param>
    /// <returns>The last date of the week of the given date, using the culture-specific first day of the week.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if a valid <see cref="CultureInfo" /> object is not provided and the
    ///     current culture is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     The last date of the week is calculated by getting the first date of the week using the
    ///     <see cref="GetFirstDateOfWeek(DateTime, CultureInfo)" /> extension method, and then adding six days to it.
    ///     The culture-specific first day of the week is determined by the <see cref="DateTimeFormatInfo.FirstDayOfWeek" />
    ///     property of the provided (or current) <see cref="CultureInfo" /> object.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime GetLastDateOfWeek(this DateTime date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    ///     Calculates the number of days between two given dates.
    /// </summary>
    /// <param name="fromDate">The starting date for the calculation.</param>
    /// <param name="toDate">The ending date for the calculation.</param>
    /// <returns>The number of days between the two given dates.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="toDate" /> occurs before the
    ///     <paramref name="fromDate" />.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="DateTime.Subtract(DateTime)" /> method to calculate the time span between the two
    ///     given dates, and then returns the total number of days in this time span.
    ///     The date values are first normalized to midnight by calling the <see cref="DateTime.Date" /> property to remove any
    ///     time component.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumberOfDays(this DateTime fromDate, DateTime toDate)
    {
        return (int)toDate.Date.Subtract(fromDate.Date).TotalDays;
    }

    /// <summary>
    ///     Determines whether the <see cref="DateTime" /> object represented by the <paramref name="source" /> parameter is
    ///     after the <see cref="DateTime" /> object represented by the <paramref name="other" /> parameter.
    /// </summary>
    /// <param name="source">The <see cref="DateTime" /> object to compare.</param>
    /// <param name="other">The <see cref="DateTime" /> object to compare with the <paramref name="source" /> parameter.</param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="source" /> parameter is later than the <paramref name="other" /> parameter;
    ///     otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     The <paramref name="source" /> or <paramref name="other" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAfter(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Determines whether the <see cref="DateTime" /> object represented by the <paramref name="source" /> parameter is
    ///     before the <see cref="DateTime" /> object represented by the <paramref name="other" /> parameter.
    /// </summary>
    /// <param name="source">The <see cref="DateTime" /> object to compare.</param>
    /// <param name="other">The <see cref="DateTime" /> object to compare with the <paramref name="source" /> parameter.</param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="source" /> parameter is earlier than the <paramref name="other" /> parameter;
    ///     otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     The <paramref name="source" /> or <paramref name="other" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBefore(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Determines whether the <see cref="DateTime" /> object represented by the <paramref name="source" /> parameter falls
    ///     within a given range, defined by the <paramref name="rangeBeg" /> and <paramref name="rangeEnd" /> parameters.
    /// </summary>
    /// <param name="source">The <see cref="DateTime" /> object to compare.</param>
    /// <param name="rangeBeg">The start of the range.</param>
    /// <param name="rangeEnd">The end of the range.</param>
    /// <param name="isInclusive">
    ///     A boolean value indicating whether the range should include the start and end values. If <c>true</c>, the range
    ///     includes the start and end values; if <c>false</c>, it does not.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="source" /> parameter falls within the specified range; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     The <paramref name="source" />, <paramref name="rangeBeg" />, or <paramref name="rangeEnd" /> parameter is
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     The comparison of the <paramref name="source" /> parameter to the range is done using the
    ///     <see cref="DateTime.Ticks" />
    ///     property, which represents the number of ticks (100-nanosecond intervals) that have elapsed since 12:00:00
    ///     midnight, January 1, 0001.
    /// </remarks>
    public static bool IsBetween(this DateTime source, DateTime rangeBeg, DateTime rangeEnd, bool isInclusive = true)
    {
        if (isInclusive)
        {
            return source.Ticks >= rangeBeg.Ticks && source.Ticks <= rangeEnd.Ticks;
        }

        return source.Ticks > rangeBeg.Ticks && source.Ticks < rangeEnd.Ticks;
    }

    /// <summary>
    ///     Determines whether two given dates represent the same day, ignoring the time component.
    /// </summary>
    /// <param name="date">The first date to compare.</param>
    /// <param name="dateToCompare">The second date to compare.</param>
    /// <returns><c>true</c> if the two dates represent the same day, ignoring the time component; <c>false</c> otherwise.</returns>
    /// <remarks>
    ///     The date values are first normalized to midnight by calling the <see cref="DateTime.Date" /> property to remove any
    ///     time component.
    ///     The day, month, and year values are then compared to determine if the two dates represent the same day.
    /// </remarks>
    public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
    {
        var dt = date.Date;
        var dtCompare = dateToCompare.Date;
        return dt.Day == dtCompare.Day && dt.Month == dtCompare.Month && dt.Year == dtCompare.Year;
    }

    /// <summary>
    ///     Determines whether two given dates represent the same time of day, ignoring the date component.
    /// </summary>
    /// <param name="time">The first time to compare.</param>
    /// <param name="timeToCompare">The second time to compare.</param>
    /// <returns>
    ///     <c>true</c> if the two times represent the same time of day, ignoring the date component; <c>false</c>
    ///     otherwise.
    /// </returns>
    /// <remarks>
    ///     This method compares the time of day values of the two given dates by calling the <see cref="DateTime.TimeOfDay" />
    ///     property.
    ///     The date component of the dates is ignored in this comparison.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return time.TimeOfDay == timeToCompare.TimeOfDay;
    }

    /// <summary>
    ///     Determines whether a given date represents the current day.
    /// </summary>
    /// <param name="date">The date to compare with the current day.</param>
    /// <returns><c>true</c> if the given date represents the current day; <c>false</c> otherwise.</returns>
    /// <remarks>
    ///     This method compares the date value of the given date to the date value of the current day, which is obtained by
    ///     calling the <see cref="DateTime.Today" /> property.
    ///     The time component of the given date is ignored in this comparison.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    /// <summary>
    ///     Calculates the number of days in a given year.
    /// </summary>
    /// <param name="year">The year to calculate the number of days for.</param>
    /// <param name="cultureInfo">
    ///     The culture to use for determining the first and last dates of the year. If <c>null</c>, the
    ///     current culture will be used.
    /// </param>
    /// <returns>The number of days in the given year.</returns>
    /// <remarks>
    ///     This method calculates the number of days in a year by creating two <see cref="DateTime" /> objects representing
    ///     the first and last dates of the year, and then using the <see cref="DateTime.Subtract(DateTime)" /> method to
    ///     calculate the time span between these two dates.
    ///     The calendar to use for determining the first and last dates of the year is determined by the
    ///     <paramref name="cultureInfo" /> parameter.
    ///     If the <paramref name="cultureInfo" /> parameter is not provided, the current culture is used.
    /// </remarks>
    public static int GetNumberOfDaysInYear(int year, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var first = new DateTime(year, 1, 1, cultureInfo.Calendar);
        var last = new DateTime(year + 1, 1, 1, cultureInfo.Calendar);
        return (int)last.Subtract(first).TotalDays;
    }

    /// <summary>
    ///     Determines whether a given date represents a leap day.
    /// </summary>
    /// <param name="date">The date to check for being a leap day.</param>
    /// <returns><c>true</c> if the given date represents a leap day; <c>false</c> otherwise.</returns>
    /// <remarks>
    ///     This method uses the C# 9.0 pattern matching syntax to check if the month and day values of the given date are 2
    ///     and 29, respectively.
    ///     If both of these conditions are met, the method returns <c>true</c>, indicating that the given date represents a
    ///     leap day.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapDay(this DateTime date)
    {
        return DateTime.IsLeapYear(date.Year);
    }

    /// <summary>
    ///     Calculates the time span between a given start date and the current date and time.
    /// </summary>
    /// <param name="startDate">The starting date and time for the calculation.</param>
    /// <returns>The time span between the given start date and the current date and time.</returns>
    /// <remarks>
    ///     This method uses the <see cref="DateTime.Now" /> property to obtain the current date and time, and then calls the
    ///     <see cref="DateTime.Subtract(DateTime)" /> method to calculate the time span between the given start date and the
    ///     current date and time.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Elapsed(this DateTime startDate)
    {
        return DateTime.Now.Subtract(startDate);
    }

    /// <summary>
    ///     Sets the time component of a given date to a specified time.
    /// </summary>
    /// <param name="date">The date to modify the time component of.</param>
    /// <param name="time">The time to set the time component of the date to.</param>
    /// <returns>
    ///     A new <see cref="DateTime" /> object with the same date as the given <paramref name="date" /> and the
    ///     specified <paramref name="time" />.
    /// </returns>
    /// <remarks>
    ///     This method first removes the time component of the given date by calling the <see cref="DateTime.Date" />
    ///     property, and then adds the specified time to the resulting date using the <see cref="DateTime.Add(TimeSpan)" />
    ///     method.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SetTime(this DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    /// <summary>
    ///     Sets the time component of a given date to a specified time.
    /// </summary>
    /// <param name="date">The date to modify the time component of.</param>
    /// <param name="hours">The number of hours to set the time component of the date to. The default value is 0.</param>
    /// <param name="minutes">The number of minutes to set the time component of the date to. The default value is 0.</param>
    /// <param name="seconds">The number of seconds to set the time component of the date to. The default value is 0.</param>
    /// <param name="milliseconds">The number of milliseconds to set the time component of the date to. The default value is 0.</param>
    /// <returns>
    ///     A new <see cref="DateTime" /> object with the same date as the given <paramref name="date" /> and the
    ///     specified time.
    /// </returns>
    /// <remarks>
    ///     This method adds a new time span to the given date using the <see cref="TimeSpan(int, int, int, int, int)" />
    ///     constructor and the provided time components, and then calls the <see cref="DateTime.Add(TimeSpan)" /> method to
    ///     set the time component of the date to the specified time.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SetTime(this DateTime date, int hours = 0, int minutes = 0, int seconds = 0,
        int milliseconds = 0)
    {
        return date.Date.Add(new TimeSpan(0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    ///     Sets the time component of a given date to a specified time.
    /// </summary>
    /// <param name="date">The date to modify the time component of.</param>
    /// <param name="timeOnly">A <see cref="TimeOnly" /> object representing the time to set the time component of the date to.</param>
    /// <returns>
    ///     A new <see cref="DateTime" /> object with the same date as the given <paramref name="date" /> and the time
    ///     represented by the given <paramref name="timeOnly" /> object.
    /// </returns>
    /// <remarks>
    ///     This method removes the time component of the given date by calling the <see cref="DateTime.Date" /> property, and
    ///     then adds the time represented by the <paramref name="timeOnly" /> object to the resulting date using the
    ///     <see cref="TimeOnly.ToTimeSpan" /> method and the <see cref="DateTime.Add(TimeSpan)" /> method.
    /// </remarks>
    public static DateTime SetTime(this DateTime date, TimeOnly timeOnly)
    {
        return date.Date.Add(timeOnly.ToTimeSpan());
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> object representing the next day from a given date.
    /// </summary>
    /// <param name="date">The date to get the next day from.</param>
    /// <returns>A new <see cref="DateTime" /> object representing the next day from the given <paramref name="date" />.</returns>
    /// <remarks>
    ///     This method returns a new <see cref="DateTime" /> object that is the result of adding 1 day to the given
    ///     <paramref name="date" /> using the <see cref="DateTime.AddDays(double)" /> method.
    /// </remarks>
    public static DateTime NextDay(this DateTime date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> object representing the previous day from a given date.
    /// </summary>
    /// <param name="date">The date to get the previous day from.</param>
    /// <returns>A new <see cref="DateTime" /> object representing the previous day from the given <paramref name="date" />.</returns>
    /// <remarks>
    ///     This method returns a new <see cref="DateTime" /> object that is the result of subtracting 1 day from the given
    ///     <paramref name="date" /> using the <see cref="DateTime.AddDays(double)" /> method.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime PreviousDay(this DateTime date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Generates a timestamp string from the given <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> object to generate the timestamp from.</param>
    /// <returns>A string representing the timestamp in the format "yyyyMMddHHmmssffff".</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dateTime" /> parameter is <c>null</c>.</exception>
    /// <remarks>
    ///     This method does not modify the original <see cref="DateTime" /> object. It returns a new string representing the
    ///     timestamp.
    ///     The format of the timestamp string is as follows:
    ///     - "yyyy" represents the year with 4 digits.
    ///     - "MM" represents the month with 2 digits.
    ///     - "dd" represents the day of the month with 2 digits.
    ///     - "HH" represents the hour in 24-hour format with 2 digits.
    ///     - "mm" represents the minute with 2 digits.
    ///     - "ss" represents the second with 2 digits.
    ///     - "ffff" represents the fraction of a second with 4 digits.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GenerateTimestamp(this DateTime dateTime)
    {
        return dateTime.ToString(@"yyyyMMddHHmmssffff");
    }

    /// <summary>
    ///     Returns a sequence of <see cref="DateTime" /> objects representing all the dates in a range.
    /// </summary>
    /// <param name="fromDate">The starting date of the range.</param>
    /// <param name="toDate">The ending date of the range.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> of <see cref="DateTime" /> objects representing all the dates in the range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="fromDate" /> is greater than the <paramref name="toDate" />.
    /// </exception>
    /// <remarks>
    ///     This method returns an <see cref="IEnumerable{T}" /> of <see cref="DateTime" /> objects representing all the dates
    ///     in the range between the given <paramref name="fromDate" /> and <paramref name="toDate" />. If the
    ///     <paramref name="fromDate" /> and <paramref name="toDate" /> are equal, a single-item sequence containing the
    ///     <paramref name="toDate" /> is returned. If the <paramref name="fromDate" /> is less than the
    ///     <paramref name="toDate" />, the dates are returned in ascending order. If the <paramref name="fromDate" /> is
    ///     greater than the <paramref name="toDate" />, the dates are returned in descending order.
    /// </remarks>
    public static IEnumerable<DateTime> GetDatesInRange(this DateTime fromDate, DateTime toDate)
    {
        if (fromDate == toDate)
        {
            return new[] { new DateTime(toDate.Year, toDate.Month, toDate.Day) };
        }

        var dates = new List<DateTime>();

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
    ///     Converts a <see cref="DateTime" /> object to a <see cref="DateOnly" /> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> object to convert.</param>
    /// <returns>A new <see cref="DateOnly" /> object with the same date as the input <paramref name="dateTime" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dateTime" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    ///     Converts a <see cref="DateTime" /> object to a <see cref="TimeOnly" /> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> object to convert.</param>
    /// <returns>A new <see cref="TimeOnly" /> object with the same time as the input <paramref name="dateTime" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dateTime" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond,
            dateTime.Microsecond);
    }
}