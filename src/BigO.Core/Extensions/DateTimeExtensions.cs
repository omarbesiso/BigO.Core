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
    /// Calculates the age of a person based on their date of birth and (optional) maturity date.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth of the person.</param>
    /// <param name="maturityDate">The date at which the person is considered to have reached maturity. If not provided, the current date is used as the maturity date. <c>null</c> values are allowed.</param>
    /// <returns>The age of the person in years.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the date of birth is after the maturity date.</exception>
    /// <remarks>
    /// The age is calculated by subtracting the date of birth from the maturity date, and then dividing the result by the number of days in a year.
    /// The result is rounded down to the nearest whole number, so a person with a birthday within the current year will not yet be considered one year old.
    /// If a maturity date is not provided, the current date is used as the maturity date.
    /// </remarks>
    public static int Age(this DateTime dateOfBirth, DateTime? maturityDate = null)
    {
        return dateOfBirth.ToDateOnly().Age(maturityDate?.ToDateOnly());
    }

    /// <summary>
    /// Adds a specified number of weeks to a given date.
    /// </summary>
    /// <param name="date">The date to add weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add. This can be a fractional value, in which case the result will be rounded up to the nearest whole day.</param>
    /// <returns>A new <see cref="DateTime"/> object that represents the resulting date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the resulting date is earlier than <see cref="DateTime.MinValue"/> or later than <see cref="DateTime.MaxValue"/>.</exception>
    /// <remarks>
    /// The number of weeks is converted to the equivalent number of days using the formula `numberOfWeeks * 7`, and then passed to the <see cref="DateTime.AddDays"/> method.
    /// If the number of weeks is a fractional value, it is rounded up to the nearest whole day using the <see cref="Math.Ceiling"/> method.
    /// </remarks>
    public static DateTime AddWeeks(this DateTime date, double numberOfWeeks)
    {
        return date.AddDays(Convert.ToInt32(Math.Ceiling(numberOfWeeks * 7)));
    }

    /// <summary>
    /// Calculates the number of days in the month of a given date.
    /// </summary>
    /// <param name="date">The date to calculate the number of days in the month for.</param>
    /// <returns>The number of days in the month of the given date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the month of the given date is not a valid month (1-12).</exception>
    /// <remarks>
    /// The number of days in the month is calculated using the <see cref="DateTime.DaysInMonth"/> method, with the year and month of the given date as arguments.
    /// This method is useful for calculating the number of days in a month, regardless of the day of the month.
    /// </remarks>

    public static int DaysInMonth(this DateTime date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    /// Gets the first date of the month of a given date, optionally filtered by day of the week.
    /// </summary>
    /// <param name="date">The date to get the first date of the month for.</param>
    /// <param name="dayOfWeek">The day of the week to filter the result by. If provided, the result will be the first date of the month that falls on this day of the week. <c>null</c> values are allowed.</param>
    /// <returns>The first date of the month of the given date, optionally filtered by day of the week.</returns>
    /// <remarks>
    /// The first date of the month is calculated by creating a new <see cref="DateTime"/> object with the year and month of the given date, and a day of 1.
    /// If a day of the week is not provided, this date is returned as the result.
    /// If a day of the week is provided, the result is the first date of the month that falls on this day of the week. This is achieved by repeatedly adding one day to the first date of the month until it falls on the desired day of the week.
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
    /// Gets the first date of the week of a given date, using the culture-specific first day of the week as defined by the provided (or current) <see cref="CultureInfo"/> object.
    /// </summary>
    /// <param name="date">The date to get the first date of the week for.</param>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> object to use to determine the first day of the week. If not provided, the current culture is used. <c>null</c> values are allowed.</param>
    /// <returns>The first date of the week of the given date, using the culture-specific first day of the week.</returns>
    /// <exception cref="ArgumentNullException">Thrown if a valid <see cref="CultureInfo"/> object is not provided and the current culture is <c>null</c>.</exception>
    /// <remarks>
    /// The first date of the week is calculated by repeatedly subtracting one day from the given date until it falls on the culture-specific first day of the week.
    /// The culture-specific first day of the week is determined by the <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> property of the provided (or current) <see cref="CultureInfo"/> object.
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
    /// Gets the last date of the month of a given date, optionally filtered by day of the week.
    /// </summary>
    /// <param name="date">The date to get the last date of the month for.</param>
    /// <param name="dayOfWeek">The day of the week to filter the result by. If provided, the result will be the last date of the month that falls on this day of the week. <c>null</c> values are allowed.</param>
    /// <returns>The last date of the month of the given date, optionally filtered by day of the week.</returns>
    /// <remarks>
    /// The last date of the month is calculated by creating a new <see cref="DateTime"/> object with the year and month of the given date, and the maximum number of days in the month as the day.
    /// If a day of the week is not provided, this date is returned as the result.
    /// If a day of the week is provided, the result is the last date of the month that falls on this day of the week. This is achieved by repeatedly subtracting one day from the last date of the month until it falls on the desired day of the week.
    /// </remarks>
    public static DateTime GetLastDateOfMonth(this DateTime date, DayOfWeek? dayOfWeek = null)
    {
        var lastDateOfMonth = new DateTime(date.Year, date.Month, DaysInMonth(date));

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
    /// Gets the last date of the week of a given date, using the culture-specific first day of the week as defined by the provided (or current) <see cref="CultureInfo"/> object.
    /// </summary>
    /// <param name="date">The date to get the last date of the week for.</param>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> object to use to determine the first day of the week. If not provided, the current culture is used. <c>null</c> values are allowed.</param>
    /// <returns>The last date of the week of the given date, using the culture-specific first day of the week.</returns>
    /// <exception cref="ArgumentNullException">Thrown if a valid <see cref="CultureInfo"/> object is not provided and the current culture is <c>null</c>.</exception>
    /// <remarks>
    /// The last date of the week is calculated by getting the first date of the week using the <see cref="GetFirstDateOfWeek(DateTime, CultureInfo)"/> extension method, and then adding six days to it.
    /// The culture-specific first day of the week is determined by the <see cref="DateTimeFormatInfo.FirstDayOfWeek"/> property of the provided (or current) <see cref="CultureInfo"/> object.
    /// </remarks>
    public static DateTime GetLastDateOfWeek(this DateTime date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    /// Calculates the number of days between the given `fromDate` and `toDate`.
    /// </summary>
    /// <param name="fromDate">The start date for the calculation.</param>
    /// <param name="toDate">The end date for the calculation.</param>
    /// <returns>The number of days between `fromDate` and `toDate`.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the `fromDate` is later than the `toDate`.
    /// </exception>
    /// <remarks>
    /// This method converts the given `fromDate` and `toDate` to <c>DateTime</c> objects with their
    /// time components set to zero. It then calculates the number of days between the two dates by
    /// subtracting `fromDate` from `toDate` and returning the result as an integer.
    /// </remarks>
    public static int GetNumberOfDays(this DateTime fromDate, DateTime toDate)
    {
        return Convert.ToInt32(toDate.Date.Subtract(fromDate.Date).TotalDays);
    }

    /// <summary>
    /// Determines whether the <see cref="DateTime"/> object represented by the <paramref name="source"/> parameter is after the <see cref="DateTime"/> object represented by the <paramref name="other"/> parameter.
    /// </summary>
    /// <param name="source">The <see cref="DateTime"/> object to compare.</param>
    /// <param name="other">The <see cref="DateTime"/> object to compare with the <paramref name="source"/> parameter.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="source"/> parameter is later than the <paramref name="other"/> parameter; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="source"/> or <paramref name="other"/> parameter is <c>null</c>.
    /// </exception>
    public static bool IsAfter(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    /// Determines whether the <see cref="DateTime"/> object represented by the <paramref name="source"/> parameter is before the <see cref="DateTime"/> object represented by the <paramref name="other"/> parameter.
    /// </summary>
    /// <param name="source">The <see cref="DateTime"/> object to compare.</param>
    /// <param name="other">The <see cref="DateTime"/> object to compare with the <paramref name="source"/> parameter.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="source"/> parameter is earlier than the <paramref name="other"/> parameter; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="source"/> or <paramref name="other"/> parameter is <c>null</c>.
    /// </exception>
    public static bool IsBefore(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    /// Determines whether the <see cref="DateTime"/> object represented by the <paramref name="dt"/> parameter falls within a given range, defined by the <paramref name="rangeBeg"/> and <paramref name="rangeEnd"/> parameters.
    /// </summary>
    /// <param name="dt">The <see cref="DateTime"/> object to compare.</param>
    /// <param name="rangeBeg">The start of the range.</param>
    /// <param name="rangeEnd">The end of the range.</param>
    /// <param name="isInclusive">
    /// A boolean value indicating whether the range should include the start and end values. If <c>true</c>, the range includes the start and end values; if <c>false</c>, it does not.
    /// </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="dt"/> parameter falls within the specified range; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="dt"/>, <paramref name="rangeBeg"/>, or <paramref name="rangeEnd"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The comparison of the <paramref name="dt"/> parameter to the range is done using the <see cref="DateTime.Ticks"/> property, which represents the number of ticks (100-nanosecond intervals) that have elapsed since 12:00:00 midnight, January 1, 0001.
    /// </remarks>
    public static bool IsBetween(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd, bool isInclusive = true)
    {
        if (isInclusive)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        return dt.Ticks > rangeBeg.Ticks && dt.Ticks < rangeEnd.Ticks;
    }

    /// <summary>
    /// Determines whether the date represented by the <paramref name="date"/> parameter is equal to the date represented by the <paramref name="dateToCompare"/> parameter.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> object to compare.</param>
    /// <param name="dateToCompare">The <see cref="DateTime"/> object to compare with the <paramref name="date"/> parameter.</param>
    /// <returns>
    /// <c>true</c> if the dates represented by the <paramref name="date"/> and <paramref name="dateToCompare"/> parameters are equal; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="date"/> or <paramref name="dateToCompare"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The comparison is done by comparing the <see cref="DateTime.Date"/> property of each <see cref="DateTime"/> object. This property returns the date component of the <see cref="DateTime"/> object, with the time component set to 12:00:00 midnight.
    /// </remarks>
    public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
    {
        return date.Date == dateToCompare.Date;
    }

    /// <summary>
    /// Determines whether the time represented by the <paramref name="time"/> parameter is equal to the time represented by the <paramref name="timeToCompare"/> parameter.
    /// </summary>
    /// <param name="time">The <see cref="DateTime"/> object to compare.</param>
    /// <param name="timeToCompare">The <see cref="DateTime"/> object to compare with the <paramref name="time"/> parameter.</param>
    /// <returns>
    /// <c>true</c> if the times represented by the <paramref name="time"/> and <paramref name="timeToCompare"/> parameters are equal; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="time"/> or <paramref name="timeToCompare"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The comparison is done by comparing the result of the <see cref="ToTimeOnly"/> method applied to each <see cref="DateTime"/> object. This method returns the time component of the <see cref="DateTime"/> object, with the date component set to January 1, 0001.
    /// </remarks>
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return time.ToTimeOnly() == timeToCompare.ToTimeOnly();
    }

    /// <summary>
    /// Determines whether the date represented by the <paramref name="date"/> parameter is today's date.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> object to compare.</param>
    /// <returns>
    /// <c>true</c> if the date represented by the <paramref name="date"/> parameter is today's date; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="date"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The comparison is done by comparing the <see cref="DateTime.Date"/> property of the <paramref name="date"/> parameter to the <see cref="DateTime.Today"/> property, which returns today's date with the time component set to 12:00:00 midnight.
    /// </remarks>
    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    /// <summary>
    /// Returns the number of days in the year specified by the <paramref name="year"/> parameter.
    /// </summary>
    /// <param name="year">The year for which the number of days is to be calculated.</param>
    /// <param name="cultureInfo">
    /// The <see cref="CultureInfo"/> object that represents the culture for which the number of days is to be calculated. If this parameter is <c>null</c>, the <see cref="CultureInfo.CurrentCulture"/> property is used.
    /// </param>
    /// <returns>
    /// The number of days in the year specified by the <paramref name="year"/> parameter.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The <paramref name="year"/> parameter is less than 1 or greater than 9999.
    /// </exception>
    /// <remarks>
    /// The number of days in a year is calculated by taking the difference between the number of days from the first day of the year to the first day of the following year. The first day of the year is represented by a <see cref="DateTime"/> object with the month and day set to 1 and the year set to the <paramref name="year"/> parameter. The first day of the following year is represented by a <see cref="DateTime"/> object with the month and day set to 1 and the year set to the <paramref name="year"/> parameter + 1.
    /// </remarks>
    public static int GetNumberOfDaysInYear(int year, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var first = new DateTime(year, 1, 1, cultureInfo.Calendar);
        var last = new DateTime(year + 1, 1, 1, cultureInfo.Calendar);
        return GetNumberOfDays(first, last);
    }

    /// <summary>
    /// Determines whether the date represented by the <paramref name="date"/> parameter is a leap day.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> object to check.</param>
    /// <returns>
    /// <c>true</c> if the date represented by the <paramref name="date"/> parameter is a leap day; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="date"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// A leap day is a day that is added to the calendar in a leap year to synchronize it with the Earth's orbit around the sun. Leap days typically occur on February 29th.
    /// </remarks>
    public static bool IsLeapDay(this DateTime date)
    {
        return date is { Month: 2, Day: 29 };
    }

    /// <summary>
    /// Calculates the elapsed time between the <paramref name="startDate"/> parameter and the current date and time.
    /// </summary>
    /// <param name="startDate">The start date and time.</param>
    /// <returns>
    /// A <see cref="TimeSpan"/> object that represents the elapsed time between the <paramref name="startDate"/> parameter and the current date and time.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="startDate"/> parameter is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// The elapsed time is calculated by subtracting the <paramref name="startDate"/> parameter from the current date and time, as represented by the <see cref="DateTime.Now"/> property.
    /// </remarks>
    public static TimeSpan Elapsed(this DateTime startDate)
    {
        return DateTime.Now.Subtract(startDate);
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> object with the time set to midnight (00:00:00).
    /// </summary>
    /// <param name="time">The <see cref="DateTime"/> object to modify.</param>
    /// <returns>A new <see cref="DateTime"/> object with the time set to midnight (00:00:00).</returns>
    /// <remarks>
    /// This method is useful for creating a new <see cref="DateTime"/> object with the same date as the original
    /// but with the time set to midnight.
    /// </remarks>
    public static DateTime Midnight(this DateTime time)
    {
        return time.SetTime();
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance with the same date and time set to noon (12:00:00).
    /// </summary>
    /// <param name="time">The original <see cref="DateTime"/> instance.</param>
    /// <returns>A new <see cref="DateTime"/> instance with the same date and time set to noon (12:00:00).</returns>
    /// <remarks>
    /// This method returns a new <see cref="DateTime"/> instance with the same date as the original 
    /// instance, but the time set to noon (12:00:00). It does not modify the original instance.
    /// </remarks>
    public static DateTime Noon(this DateTime time)
    {
        return time.SetTime(12);
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance with the same date and a specified time.
    /// </summary>
    /// <param name="date">The original <see cref="DateTime"/> instance.</param>
    /// <param name="hours">The number of hours. Default is 0.</param>
    /// <param name="minutes">The number of minutes. Default is 0.</param>
    /// <param name="seconds">The number of seconds. Default is 0.</param>
    /// <param name="milliseconds">The number of milliseconds. Default is 0.</param>
    /// <returns>A new <see cref="DateTime"/> instance with the same date and a specified time.</returns>
    /// <remarks>
    /// This method returns a new <see cref="DateTime"/> instance with the same date as the original 
    /// instance, but with a specified time. It does not modify the original instance.
    /// </remarks>
    public static DateTime SetTime(this DateTime date, int hours = 0, int minutes = 0, int seconds = 0,
        int milliseconds = 0)
    {
        return date.SetTime(new TimeSpan(0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance with the same date and a specified time.
    /// </summary>
    /// <param name="date">The original <see cref="DateTime"/> instance.</param>
    /// <param name="time">The <see cref="TimeSpan"/> representing the time to set.</param>
    /// <returns>A new <see cref="DateTime"/> instance with the same date and a specified time.</returns>
    /// <remarks>
    /// This method returns a new <see cref="DateTime"/> instance with the same date as the original 
    /// instance, but with a specified time. It does not modify the original instance.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The <paramref name="time"/> parameter specifies a value less than <c>TimeSpan.MinValue</c> or 
    /// greater than <c>TimeSpan.MaxValue</c>.
    /// </exception>
    public static DateTime SetTime(this DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance with the same date and a specified time.
    /// </summary>
    /// <param name="date">The original <see cref="DateTime"/> instance.</param>
    /// <param name="timeOnly">The <see cref="TimeOnly"/> instance representing the time to set.</param>
    /// <returns>A new <see cref="DateTime"/> instance with the same date and a specified time.</returns>
    /// <remarks>
    /// This method returns a new <see cref="DateTime"/> instance with the same date as the original 
    /// instance, but with a specified time. It does not modify the original instance.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="timeOnly"/> parameter is <c>null</c>.
    /// </exception>
    public static DateTime SetTime(this DateTime date, TimeOnly timeOnly)
    {
        return date.Date.Add(timeOnly.ToTimeSpan());
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance representing the next day.
    /// </summary>
    /// <param name="date">The original <see cref="DateTime"/> instance.</param>
    /// <returns>A new <see cref="DateTime"/> instance representing the next day.</returns>
    /// <remarks>
    /// This method returns a new <see cref="DateTime"/> instance with the same time as the original 
    /// instance, but with the date set to the next day. It does not modify the original instance.
    /// </remarks>
    public static DateTime NextDay(this DateTime date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    /// Returns a new <see cref="DateTime"/> object that represents the previous day of the given <see cref="DateTime"/>.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> object to get the previous day for.</param>
    /// <returns>A new <see cref="DateTime"/> object that represents the previous day of the given <see cref="DateTime"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The resulting <see cref="DateTime"/> is less than <see cref="DateTime.MinValue"/> or greater than <see cref="DateTime.MaxValue"/>.</exception>
    /// <remarks>
    /// This method does not modify the original <see cref="DateTime"/> object. It returns a new object with the previous day.
    /// </remarks>
    public static DateTime PreviousDay(this DateTime date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    /// Generates a timestamp string from the given <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to generate the timestamp from.</param>
    /// <returns>A string representing the timestamp in the format "yyyyMMddHHmmssffff".</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dateTime"/> parameter is <c>null</c>.</exception>
    /// <remarks>
    /// This method does not modify the original <see cref="DateTime"/> object. It returns a new string representing the timestamp.
    /// The format of the timestamp string is as follows:
    /// - "yyyy" represents the year with 4 digits.
    /// - "MM" represents the month with 2 digits.
    /// - "dd" represents the day of the month with 2 digits.
    /// - "HH" represents the hour in 24-hour format with 2 digits.
    /// - "mm" represents the minute with 2 digits.
    /// - "ss" represents the second with 2 digits.
    /// - "ffff" represents the fraction of a second with 4 digits.
    /// </remarks>
    public static string GenerateTimestamp(this DateTime dateTime)
    {
        return dateTime.ToString(@"yyyyMMddHHmmssffff");
    }

    /// <summary>
    /// Returns a sequence of <see cref="DateTime"/> objects representing all the dates within a given range.
    /// </summary>
    /// <param name="fromDate">The start date of the range.</param>
    /// <param name="toDate">The end date of the range.</param>
    /// <returns>A sequence of <see cref="DateTime"/> objects representing all the dates within the given range.</returns>
    /// <exception cref="ArgumentException">The <paramref name="fromDate"/> is equal to the <paramref name="toDate"/>.</exception>
    /// <remarks>
    /// This method returns a sequence of <see cref="DateTime"/> objects representing all the dates within the given range, including the start and end dates.
    /// If the <paramref name="fromDate"/> is greater than the <paramref name="toDate"/>, the method returns the dates in reverse order.
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
    /// Converts a <see cref="DateTime"/> object to a <see cref="DateOnly"/> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to convert.</param>
    /// <returns>A new <see cref="DateOnly"/> object representing the date part of the given <see cref="DateTime"/> object.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dateTime"/> parameter is <c>null</c>.</exception>
    /// <remarks>
    /// This method does not modify the original <see cref="DateTime"/> object. It returns a new <see cref="DateOnly"/> object with the same date as the given <see cref="DateTime"/> object.
    /// The time part of the <see cref="DateTime"/> object is ignored in the conversion.
    /// </remarks>
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> object to a <see cref="TimeOnly"/> object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to convert.</param>
    /// <returns>A new <see cref="TimeOnly"/> object representing the time part of the given <see cref="DateTime"/> object.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="dateTime"/> parameter is <c>null</c>.</exception>
    /// <remarks>
    /// This method does not modify the original <see cref="DateTime"/> object. It returns a new <see cref="TimeOnly"/> object with the same time as the given <see cref="DateTime"/> object.
    /// The date part of the <see cref="DateTime"/> object is ignored in the conversion.
    /// </remarks>
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond,
            dateTime.Microsecond);
    }
}