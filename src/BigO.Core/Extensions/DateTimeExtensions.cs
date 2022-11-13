using System.Globalization;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="DateTime" /> objects.
/// </summary>
[PublicAPI]
public static class DateTimeExtensions
{
    /// <summary>
    ///     Calculates the age provided with a birth date and an optional maturity date.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="maturityDate">
    ///     The date to which age is calculated. If <c>null</c> then the age will be calculated using the
    ///     today's date.
    /// </param>
    /// <returns>The calculated age.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="maturityDate" /> passed occurs before the
    ///     <paramref name="dateOfBirth" /> or if the <paramref name="maturityDate" /> is <c>null</c> and the
    ///     <paramref name="dateOfBirth" /> occurs in the future.
    /// </exception>
    public static int Age(this DateTime dateOfBirth, DateTime? maturityDate = null)
    {
        maturityDate ??= DateTime.Today;

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
    ///     Returns a new <see cref="DateTime" /> value that adds the specified number of weeks to a <see cref="DateTime" />
    ///     instance.
    /// </summary>
    /// <param name="date">The date to which weeks are added.</param>
    /// <param name="numberOfWeeks">
    ///     A number of whole and fractional weeks. The numberOfWeeks
    ///     parameter can be negative or positive.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.DateTime" /> object with a value is the sum of the date and
    ///     time represented by the instance and the number of weeks represented by
    ///     <paramref name="numberOfWeeks" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="DateTime" /> is less than <see cref="System.DateTime.MinValue" /> or greater than
    ///     <see cref="System.DateTime.MaxValue" />".
    /// </exception>
    public static DateTime AddWeeks(this DateTime date, double numberOfWeeks)
    {
        return date.AddDays(numberOfWeeks * 7);
    }

    /// <summary>
    ///     Returns the number of days in the month of a specified <see cref="DateTime" />
    ///     instance.
    /// </summary>
    /// <param name="date">The date from which the number of days is calculated.</param>
    /// <returns>
    ///     The number of days in the month.
    /// </returns>
    public static int GetCountOfDaysInMonth(this DateTime date)
    {
        var nextMonth = date.AddMonths(1);
        return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
    }

    /// <summary>
    ///     Returns the first date in a specified month. If the <paramref name="dayOfWeek" />
    ///     is specified it returns the first date/occurrence of that day.
    /// </summary>
    /// <param name="date">The date in the month for which to retrieve the first date.</param>
    /// <param name="dayOfWeek">
    ///     {Optional} If provided then the first date of the month is always the first, otherwise it's the
    ///     first specified calendar week day of the month.
    /// </param>
    /// <returns>
    ///     The first date or the first day of week in the month.
    /// </returns>
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
    ///     Returns the first date of a week in a <see cref="DateTime" /> date.
    /// </summary>
    /// <param name="date">The date for which to retrieve the first day of the week.</param>
    /// <param name="cultureInfo">
    ///     {Optional} The culture specification to be used in
    ///     checking for the first day of the week. Default is <see cref="CultureInfo.CurrentCulture" />.
    /// </param>
    /// <returns>
    ///     The first date of the week.
    /// </returns>
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
    ///     Returns the last date in a specified month. If the <paramref name="dayOfWeek" />
    ///     is specified it returns the last date/occurrence of that day.
    /// </summary>
    /// <param name="date">The date in the month for which to retrieve the last date.</param>
    /// <param name="dayOfWeek">
    ///     {Optional} If provided then the last date of the month is always the last, otherwise it's the
    ///     last specified calendar week day of the month.
    /// </param>
    /// <returns>
    ///     The last date or the first day of week in the month.
    /// </returns>
    public static DateTime GetLastDateOfMonth(this DateTime date, DayOfWeek? dayOfWeek = null)
    {
        var lastDateOfMonth = new DateTime(date.Year, date.Month, GetCountOfDaysInMonth(date));

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
    ///     Returns the last date of a week in a <see cref="DateTime" /> date.
    /// </summary>
    /// <param name="date">The date for which to retrieve the last day of the week.</param>
    /// <param name="cultureInfo">
    ///     {Optional} The culture specification to be used in
    ///     checking for the last day of the week. Default is <see cref="CultureInfo.CurrentCulture" />.
    /// </param>
    /// <returns>
    ///     The last date of the week.
    /// </returns>
    public static DateTime GetLastDateOfWeek(this DateTime date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    ///     Returns the number of days/difference between two specified <see cref="DateTime" /> objects
    /// </summary>
    /// <param name="fromDate">The first date boundary.</param>
    /// <param name="toDate">The second date boundary</param>
    /// <returns>
    ///     The number of days in a date range.
    /// </returns>
    /// <exception cref="OverflowException">
    ///     Output numberOfWeeks is greater than <see cref="Int32.MaxValue" /> or less than
    ///     <see cref="Int32.MinValue" />.
    /// </exception>
    public static int GetNumberOfDays(this DateTime fromDate, DateTime toDate)
    {
        return Convert.ToInt32(toDate.Subtract(fromDate).TotalDays);
    }

    /// <summary>
    ///     Returns <c>true</c> if the <see cref="DateTime" /> occurs after the <paramref name="other" />.
    /// </summary>
    /// <param name="source">The <see cref="DateTime" /> instance.</param>
    /// <param name="other">The <see cref="DateTime" /> object to be compared to.</param>
    /// <returns><c>true</c> if the date instance occurs after the passed in <see cref="DateTime" />, otherwise <c>false</c>.</returns>
    public static bool IsAfter(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Returns <c>true</c> if the <see cref="DateTime" /> occurs before the <paramref name="other" />.
    /// </summary>
    /// <param name="source">The <see cref="DateTime" /> object instance.</param>
    /// <param name="other">The <see cref="DateTime" /> object to be compared to.</param>
    /// <returns><c>true</c> if the date instance occurs before the passed in <see cref="DateTime" />.</returns>
    public static bool IsBefore(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Indicates if the specified date time is within the specified range.
    /// </summary>
    /// <param name="dt">The date time object to be checked.</param>
    /// <param name="rangeBeg">The range's starting point.</param>
    /// <param name="rangeEnd">The range's ending point.</param>
    /// <param name="isInclusive">Indicates if the date range end and start are inclusive or not.</param>
    /// <returns><c>true</c> if the datetime is within the range, <c>false</c> otherwise.</returns>
    public static bool IsBetween(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd, bool isInclusive = true)
    {
        if (isInclusive)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        return dt.Ticks > rangeBeg.Ticks && dt.Ticks < rangeEnd.Ticks;
    }

    /// <summary>
    ///     Checks if the date portion of two <see cref="DateTime" /> objects are the same.
    /// </summary>
    /// <param name="date">The first <see cref="DateTime" /> object to be checked.</param>
    /// <param name="dateToCompare">The second <see cref="DateTime" /> object to be checked.</param>
    /// <returns>
    ///     <c>true</c> if the date portion of the <see cref="DateTime" /> objects are the same, otherwise <c>false</c>.
    /// </returns>
    public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
    {
        return date.Date == dateToCompare.Date;
    }

    /// <summary>
    ///     Checks if the time portion of two <see cref="DateTime" /> objects are the same.
    /// </summary>
    /// <param name="time">The first <see cref="DateTime" /> object to be checked.</param>
    /// <param name="timeToCompare">The second <see cref="DateTime" /> object to be checked.</param>
    /// <returns>
    ///     <c>true</c> if the time portion of the <see cref="DateTime" /> objects are the same, otherwise <c>false</c>.
    /// </returns>
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return time.TimeOfDay == timeToCompare.TimeOfDay;
    }

    /// <summary>
    ///     Returns <c>true</c> if the passed in date is corresponds to today's date, otherwise returns <c>false</c>.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is today, otherwise <c>false</c>.</returns>
    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    /// <summary>
    ///     Gets the number of days/difference in a specified year.
    /// </summary>
    /// <param name="year">The year to be checked.</param>
    /// <param name="cultureInfo">An optional <see cref="CultureInfo" /> object to be used in processing.</param>
    /// <returns>The number of days in a specified year.</returns>
    /// <exception cref="OverflowException">
    ///     Output numberOfWeeks is greater than <see cref="Int32.MaxValue" /> or less than
    ///     <see cref="Int32.MinValue" />.
    /// </exception>
    public static int GetNumberOfDaysInYear(int year, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var first = new DateTime(year, 1, 1, cultureInfo.Calendar);
        var last = new DateTime(year + 1, 1, 1, cultureInfo.Calendar);
        return GetNumberOfDays(first, last);
    }

    /// <summary>
    ///     Determines whether the specified date is a leap day i.e. the 29th of February.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <returns><c>true</c> if the specified date is a leap day; otherwise, <c>false</c>.</returns>
    public static bool IsLeapDay(this DateTime date)
    {
        return date.Month == 2 && date.Day == 29;
    }

    /// <summary>
    ///     Get the elapsed time since the specified <paramref name="startDate" />.
    /// </summary>
    /// <param name="startDate">The <see cref="DateTime" /> instance to get the elapsed time from.</param>
    /// <returns>Returns a <see cref="TimeSpan" /> numberOfWeeks with the elapsed time since the <paramref name="startDate" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The result is less than <see cref="DateTime.MinValue" /> or greater than
    ///     <see cref="DateTime.MaxValue" />.
    /// </exception>
    public static TimeSpan Elapsed(this DateTime startDate)
    {
        return DateTime.Now.Subtract(startDate);
    }

    /// <summary>
    ///     Sets the time on the <see cref="DateTime" /> instance to 12:00:00AM.
    /// </summary>
    /// <param name="time">The <see cref="DateTime" /> instance.</param>
    /// <returns>
    ///     A <see cref="DateTime" /> object with the time set to 12:00:00AM and the date equal to the passed in
    ///     <see cref="DateTime" /> instance.
    /// </returns>
    public static DateTime Midnight(this DateTime time)
    {
        return time.SetTime();
    }

    /// <summary>
    ///     Sets the time on the <see cref="DateTime" /> instance to 12:00:00PM.
    /// </summary>
    /// <param name="time">The <see cref="DateTime" /> instance.</param>
    /// <returns>
    ///     A <see cref="DateTime" /> object with the time set to 12:00:00PM and the date equal to the passed in
    ///     <see cref="DateTime" /> instance.
    /// </returns>
    public static DateTime Noon(this DateTime time)
    {
        return time.SetTime(12);
    }

    /// <summary>
    ///     Sets the time on the passed on date to the specified time variables.
    /// </summary>
    /// <param name="date">The date to set time for.</param>
    /// <param name="hours">The hours to be set.</param>
    /// <param name="minutes">The minutes to be set.</param>
    /// <param name="seconds">The seconds to be set.</param>
    /// <param name="milliseconds">The milliseconds to be set.</param>
    /// <returns>The date with the specified time set.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The parameters specify a System.TimeSpan numberOfWeeks less than
    ///     System.TimeSpan.MinValue or greater than System.TimeSpan.MaxValue.
    /// </exception>
    public static DateTime SetTime(this DateTime date, int hours = 0, int minutes = 0, int seconds = 0,
        int milliseconds = 0)
    {
        return date.SetTime(new TimeSpan(0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    ///     Sets the time on the passed on date to the specified TimeSpan.
    /// </summary>
    /// <param name="date">The date to set time for.</param>
    /// <param name="time">The timespan to apply to the date.</param>
    /// <returns>The date with the specified time set.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The parameters specify a System.TimeSpan numberOfWeeks less than
    ///     System.TimeSpan.MinValue or greater than System.TimeSpan.MaxValue.
    /// </exception>
    public static DateTime SetTime(this DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    /// <summary>
    ///     Sets the time on the passed on date to the specified <see cref="TimeOnly" /> instance.
    /// </summary>
    /// <param name="date">The date to set time for.</param>
    /// <param name="timeOnly">The <see cref="TimeOnly" /> numberOfWeeks to apply to the date.</param>
    /// <returns>The date with the specified <see cref="TimeOnly" /> numberOfWeeks set.</returns>
    public static DateTime SetTime(this DateTime date, TimeOnly timeOnly)
    {
        return date.Date.Add(timeOnly.ToTimeSpan());
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> with one day added to the <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="date">The <see cref="DateTime" /> instance.</param>
    /// <returns>A new <see cref="DateTime" /> object with 1 day added.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="System.DateTime" /> is less than <see cref="System.DateTime.MinValue" /> or greater than
    ///     <see cref="System.DateTime.MaxValue" />.
    /// </exception>
    public static DateTime NextDay(this DateTime date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> with one day subtracted to the <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="date">The <see cref="DateTime" /> instance.</param>
    /// <returns>A new <see cref="DateTime" /> object with 1 day subtracted.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="System.DateTime" /> is less than <see cref="System.DateTime.MinValue" /> or greater than
    ///     <see cref="System.DateTime.MaxValue" />.
    /// </exception>
    public static DateTime PreviousDay(this DateTime date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Generates a timestamp representation for the passed in DateTime numberOfWeeks.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>Generated timestamp.</returns>
    public static string GenerateTimestamp(this DateTime dateTime)
    {
        return dateTime.ToString(@"yyyyMMddHHmmssffff");
    }

    /// <summary>
    ///     Gets the dates between two dates as as an <see cref="IEnumerable{DateTime}" /> instance.
    /// </summary>
    /// <param name="fromDate">The start of the date range.</param>
    /// <param name="toDate">The end of the date range.</param>
    /// <returns>The list of dates between the two days.</returns>
    public static IEnumerable<DateTime> GetDatesInRange(this DateTime fromDate, DateTime toDate)
    {
        var days = (toDate - fromDate).Days;
        var dates = new DateTime[days];

        for (var i = 0; i < days; i++)
        {
            dates[i] = fromDate.AddDays(i).Date;
        }

        return dates;
    }

    /// <summary>
    ///     Determines whether the specified date is a working day.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <param name="cultureInfo">The culture information. Default is "en-US".</param>
    /// <returns><c>true</c> if the date is a working day; otherwise, <c>false</c>.</returns>
    public static bool IsWorkingDay(this DateTime date, CultureInfo? cultureInfo = null)
    {
        return !date.IsWeekendDay(cultureInfo);
    }

    /// <summary>
    ///     Determines whether the specified date is a weekend day.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <param name="cultureInfo">The culture information. Default is the culture of the current thread".</param>
    /// <returns><c>true</c> if the date is a weekend day; otherwise, <c>false</c>.</returns>
    public static bool IsWeekendDay(this DateTime date, CultureInfo? cultureInfo = null)
    {
        var internalCultureInfo = cultureInfo ?? Thread.CurrentThread.CurrentCulture;

        var firstDay = internalCultureInfo.DateTimeFormat.FirstDayOfWeek;

        var currentDayInProvidedDatetime = date.DayOfWeek;
        var lastDayOfWeek = firstDay + 4;

        var isInWeekend = currentDayInProvidedDatetime == lastDayOfWeek + 1 ||
                          currentDayInProvidedDatetime == lastDayOfWeek + 2;

        return isInWeekend;
    }

    /// <summary>
    ///     Get the next working day after the date provided.
    /// </summary>
    /// <param name="date">The date to be to be used.</param>
    /// <param name="cultureInfo">The culture information. Default is "en-US".</param>
    /// <returns>A <see cref="DateTime" /> object indicating the next working day.</returns>
    public static DateTime NextWorkday(this DateTime date, CultureInfo? cultureInfo = null)
    {
        var nextDay = date.AddDays(1);
        while (!nextDay.IsWorkingDay(cultureInfo))
        {
            nextDay = nextDay.AddDays(1);
        }

        return nextDay;
    }

    /// <summary>
    ///     Converts the <see cref="DateTime" /> instance to the equivalent <see cref="DateOnly" /> numberOfWeeks.
    /// </summary>
    /// <param name="dateTime">The date to be to be converted.</param>
    /// <returns>The <see cref="DateOnly" /> numberOfWeeks equivalent to the <see cref="DateTime" /> instance.</returns>
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    ///     Converts the <see cref="DateTime" /> instance to the equivalent <see cref="TimeOnly" /> numberOfWeeks.
    /// </summary>
    /// <param name="dateTime">The date to be to be converted.</param>
    /// <returns>The <see cref="TimeOnly" /> numberOfWeeks equivalent to the <see cref="DateTime" /> instance.</returns>
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond,
            dateTime.Microsecond);
    }
}