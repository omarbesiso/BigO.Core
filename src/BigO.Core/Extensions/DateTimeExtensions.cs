﻿using System.Globalization;
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
    /// var dateOfBirth = new DateTime(1980, 1, 1);
    /// int age = dateOfBirth.Age();
    /// Console.WriteLine(age); // The current age based on the date of birth.
    /// age = dateOfBirth.Age(new DateTime(2022, 1, 1));
    /// Console.WriteLine(age); // The age based on the date of birth and a maturity date.
    /// age = dateOfBirth.Age(timeZoneInfo: TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    /// Console.WriteLine(age); // The age based on the date of birth and a time zone.
    /// ]]></code>
    /// </example>
    public static int Age(this DateTime dateOfBirth, DateTime? maturityDate = null, TimeZoneInfo? timeZoneInfo = null)
    {
        var today = maturityDate ?? (timeZoneInfo == null
            ? DateTime.UtcNow
            : TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo));

        var age = today.Year - dateOfBirth.Year;

        return dateOfBirth > today.AddYears(-age) ? age - 1 : age;
    }

    /// <summary>
    ///     Adds a specified number of weeks to the specified date.
    /// </summary>
    /// <param name="date">The date to add weeks to.</param>
    /// <param name="numberOfWeeks">The number of weeks to add to the <paramref name="date" />.</param>
    /// <returns>
    ///     The <see cref="DateTime" /> value that is <paramref name="numberOfWeeks" /> weeks ahead of the
    ///     <paramref name="date" />.
    /// </returns>
    /// <remarks>
    ///     This method adds a specified number of weeks to the specified <paramref name="date" />.
    ///     The number of days to add is calculated as <paramref name="numberOfWeeks" /> times 7.
    /// </remarks>
    /// <example>
    ///     The following code example adds 2 weeks to a date:
    ///     <code><![CDATA[
    /// var date = new DateTime(2022, 3, 27);
    /// var newDate = date.AddWeeks(2);
    /// Console.WriteLine(newDate); // 2022-04-10
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AddWeeks(this DateTime date, double numberOfWeeks)
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
    /// var date = new DateTime(2022, 3, 27);
    /// int daysInMonth = date.DaysInMonth();
    /// Console.WriteLine(daysInMonth); // 31
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DaysInMonth(this DateTime date)
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
    /// var date = new DateTime(2022, 3, 27);
    /// var firstDateOfMonth = date.GetFirstDateOfMonth();
    /// Console.WriteLine(firstDateOfMonth); // 2022-03-01
    /// ]]></code>
    ///     The following code example gets the first Friday of the current month:
    ///     <code><![CDATA[
    /// var date = new DateTime(2022, 3, 27);
    /// var firstFridayOfMonth = date.GetFirstDateOfMonth(DayOfWeek.Friday);
    /// Console.WriteLine(firstFridayOfMonth); // 2022-03-04
    /// ]]></code>
    /// </example>
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
    /// DateTime date = new(2023, 3, 28);
    /// DateTime firstDateOfWeek = date.GetFirstDateOfWeek(); // firstDateOfWeek will be the first date of the week based on the current culture
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the first date of the week for a given date based on the week rules of an optional
    ///     <see cref="CultureInfo" />. If no <paramref name="cultureInfo" /> is provided, the current culture will be used.
    ///     The method iterates through the days of the week, starting from the given date, moving backwards until it finds the
    ///     first day of the week according to the specified culture.
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
    /// DateTime date = new(2023, 3, 28);
    /// DateTime lastDateOfMonth = date.GetLastDateOfMonth(); // lastDateOfMonth will be the last date of the month
    /// DateTime lastFridayOfMonth = date.GetLastDateOfMonth(DayOfWeek.Friday); // lastFridayOfMonth will be the last Friday of the month
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the last date of the month for a given date, optionally filtered by a specific day
    ///     of the week.
    ///     If no <paramref name="dayOfWeek" /> is provided, the last date of the month will be returned. Otherwise, the method
    ///     iterates through the days of the month, starting from the last date, moving backwards until it finds the specified
    ///     day of the week.
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
    ///     Gets the last date of the week for the specified <see cref="DateTime" /> value.
    /// </summary>
    /// <param name="date">The <see cref="DateTime" /> value for which to find the last date of the week.</param>
    /// <param name="cultureInfo">
    ///     The <see cref="CultureInfo" /> to use for determining the first day of the week; if
    ///     <c>null</c>, the current culture is used.
    /// </param>
    /// <returns>The last date of the week for the specified <see cref="DateTime" /> value.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime currentDate = DateTime.Today;
    /// CultureInfo cultureInfo = new CultureInfo("en-US");
    /// DateTime lastDateOfWeek = currentDate.GetLastDateOfWeek(cultureInfo);
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
    public static DateTime GetLastDateOfWeek(this DateTime date, CultureInfo? cultureInfo = null)
    {
        return date.GetFirstDateOfWeek(cultureInfo).AddDays(6);
    }

    /// <summary>
    ///     Calculates the number of days between two <see cref="DateTime" /> instances.
    /// </summary>
    /// <param name="fromDate">The starting date for the calculation.</param>
    /// <param name="toDate">The ending date for the calculation.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime fromDate = new(2023, 3, 1);
    /// DateTime toDate = new(2023, 3, 28);
    /// int numberOfDays = fromDate.GetNumberOfDays(toDate); // numberOfDays will be 27
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method calculates the number of days between two <see cref="DateTime" /> instances by converting
    ///     them to <see cref="DateTime" /> instances with <see cref="TimeOnly.MinValue" /> as the time component, and then
    ///     subtracting the starting date from the ending date. The resulting <see cref="TimeSpan" /> is used to calculate the
    ///     total number of days between the dates.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetNumberOfDays(this DateTime fromDate, DateTime toDate)
    {
        return (int)toDate.Date.Subtract(fromDate.Date).TotalDays;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateTime" /> instance is after another <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="source">The source date to compare.</param>
    /// <param name="other">The other date to compare against.</param>
    /// <returns><c>true</c> if the source date is after the other date; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime sourceDate = new(2023, 3, 28);
    /// DateTime otherDate = new(2023, 3, 1);
    /// bool isAfter = sourceDate.IsAfter(otherDate); // isAfter will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares two <see cref="DateTime" /> instances to determine whether the source date is after
    ///     the other date. It uses the <see cref="DateTime.CompareTo(DateTime)" /> method to perform the comparison and
    ///     returns <c>true</c> if the source date is greater than the other date, indicating that it is after the other date;
    ///     otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAfter(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateTime" /> instance is before another <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="source">The source date to compare.</param>
    /// <param name="other">The other date to compare against.</param>
    /// <returns><c>true</c> if the source date is before the other date; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime sourceDate = new(2023, 3, 1);
    /// DateTime otherDate = new(2023, 3, 28);
    /// bool isBefore = sourceDate.IsBefore(otherDate); // isBefore will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares two <see cref="DateTime" /> instances to determine whether the source date is before
    ///     the other date. It uses the <see cref="DateTime.CompareTo(DateTime)" /> method to perform the comparison and
    ///     returns <c>true</c> if the source date is less than the other date, indicating that it is before the other date;
    ///     otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBefore(this DateTime source, DateTime other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="source" /> DateTime is between the specified
    ///     <paramref name="rangeBeg" /> and <paramref name="rangeEnd" /> DateTimes.
    /// </summary>
    /// <param name="source">The DateTime to check.</param>
    /// <param name="rangeBeg">The start DateTime of the range.</param>
    /// <param name="rangeEnd">The end DateTime of the range.</param>
    /// <param name="isInclusive">
    ///     A boolean value indicating whether the range is inclusive or exclusive. Default is
    ///     <c>true</c> (inclusive).
    /// </param>
    /// <returns><c>true</c> if <paramref name="source" /> is within the specified range; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime date = new DateTime(2023, 1, 15);
    /// DateTime rangeStart = new DateTime(2023, 1, 1);
    /// DateTime rangeEnd = new DateTime(2023, 1, 31);
    /// bool result = date.IsBetween(rangeStart, rangeEnd);
    /// // result will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method checks if the specified <paramref name="source" /> DateTime is between the
    ///     <paramref name="rangeBeg" /> and <paramref name="rangeEnd" /> DateTimes. If <paramref name="isInclusive" /> is
    ///     <c>true</c>, the range is considered inclusive, and the method will return <c>true</c> if
    ///     <paramref name="source" /> is equal to either <paramref name="rangeBeg" /> or <paramref name="rangeEnd" />. If
    ///     <paramref name="isInclusive" /> is <c>false</c>, the range is considered exclusive, and the method will return
    ///     <c>false</c> if <paramref name="source" /> is equal to either <paramref name="rangeBeg" /> or
    ///     <paramref name="rangeEnd" />.
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
    ///     Determines whether the specified <paramref name="date" /> is equal to <paramref name="dateToCompare" /> in terms of
    ///     their date parts (ignoring time).
    /// </summary>
    /// <param name="date">The first DateTime to compare.</param>
    /// <param name="dateToCompare">The second DateTime to compare.</param>
    /// <returns><c>true</c> if the date parts of both DateTimes are equal; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime date1 = new DateTime(2023, 3, 28, 10, 30, 0);
    /// DateTime date2 = new DateTime(2023, 3, 28, 15, 45, 0);
    /// bool result = date1.IsDateEqual(date2);
    /// // result will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares the date parts of the specified <paramref name="date" /> and
    ///     <paramref name="dateToCompare" /> DateTimes (day, month, and year) and returns <c>true</c> if they are equal. The
    ///     time parts of the DateTimes are not considered in the comparison. If you need to compare both date and time parts,
    ///     use the standard equality operator (==) or <see cref="DateTime.Equals(DateTime)" /> method.
    /// </remarks>
    public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
    {
        var dt = date.Date;
        var dtCompare = dateToCompare.Date;
        return dt.Day == dtCompare.Day && dt.Month == dtCompare.Month && dt.Year == dtCompare.Year;
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="time" /> is equal to <paramref name="timeToCompare" /> in terms of
    ///     their time parts (ignoring date).
    /// </summary>
    /// <param name="time">The first DateTime to compare.</param>
    /// <param name="timeToCompare">The second DateTime to compare.</param>
    /// <returns><c>true</c> if the time parts of both DateTimes are equal; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime time1 = new DateTime(2023, 3, 28, 10, 30, 0);
    /// DateTime time2 = new DateTime(2023, 4, 5, 10, 30, 0);
    /// bool result = time1.IsTimeEqual(time2);
    /// // result will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares the time parts of the specified <paramref name="time" /> and
    ///     <paramref name="timeToCompare" /> DateTimes (hour, minute, second, and millisecond) and returns <c>true</c> if they
    ///     are equal. The date parts of the DateTimes are not considered in the comparison. If you need to compare both date
    ///     and time parts, use the standard equality operator (==) or <see cref="DateTime.Equals(DateTime)" /> method.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
    {
        return time.TimeOfDay == timeToCompare.TimeOfDay;
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="date" /> is the same as today's date.
    /// </summary>
    /// <param name="date">The DateTime to check.</param>
    /// <returns><c>true</c> if the specified <paramref name="date" /> is today's date; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime date1 = DateTime.Today;
    /// bool result1 = date1.IsToday();
    /// // result1 will be true
    /// 
    /// DateTime date2 = DateTime.Today.AddDays(-1);
    /// bool result2 = date2.IsToday();
    /// // result2 will be false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method compares the date part of the specified <paramref name="date" /> to the current date
    ///     (ignoring time). If the date part of the <paramref name="date" /> is the same as today's date, the method returns
    ///     <c>true</c>; otherwise, it returns <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    /// <summary>
    ///     Returns the number of days in the specified year according to the specified calendar culture or the current culture
    ///     if none is specified.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="cultureInfo">
    ///     Optional. The culture used to calculate the number of days in the year. If not specified, the
    ///     current culture is used.
    /// </param>
    /// <returns>The number of days in the specified year.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown when the <paramref name="year" /> is less than 1 or greater
    ///     than 9999.
    /// </exception>
    /// <remarks>
    ///     This method calculates the number of days in the specified year according to the specified calendar culture.
    ///     If no culture is specified, the method uses the current culture.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the GetNumberOfDaysInYear method to determine the number of days in a year.
    ///     <code><![CDATA[
    /// int year = 2023;
    /// int days = GetNumberOfDaysInYear(year);
    /// Console.WriteLine("The year {0} has {1} days.", year, days);
    /// ]]></code>
    /// </example>
    public static int GetNumberOfDaysInYear(int year, CultureInfo? cultureInfo = null)
    {
        cultureInfo ??= CultureInfo.CurrentCulture;
        var first = new DateTime(year, 1, 1, cultureInfo.Calendar);
        var last = new DateTime(year + 1, 1, 1, cultureInfo.Calendar);
        return (int)last.Subtract(first).TotalDays;
    }

    /// <summary>
    ///     Determines whether a <see cref="DateTime" /> instance represents a leap day.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is a leap day; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime leapDay = new(2020, 2, 29);
    /// bool isLeapDay = leapDay.IsLeapDay(); // isLeapDay will be true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method checks if the specified <see cref="DateTime" /> instance represents a leap day. A leap day is
    ///     defined as the 29th day of February in a leap year. If the date's month component is 2 (February) and the day
    ///     component is 29, the method returns <c>true</c>, indicating that the date is a leap day; otherwise, it returns
    ///     <c>false</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapDay(this DateTime date)
    {
        return DateTime.IsLeapYear(date.Year);
    }

    /// <summary>
    ///     Returns the time interval elapsed since the specified DateTime object was created and now.
    /// </summary>
    /// <param name="startDate">The starting date and time value.</param>
    /// <returns>
    ///     A TimeSpan object representing the time interval between the specified date and time and the current date and time.
    /// </returns>
    /// <exception cref="System.ArgumentException">Thrown when the <paramref name="startDate" /> is greater than DateTime.Now.</exception>
    /// <remarks>
    ///     This method calculates the time elapsed between the <paramref name="startDate" /> and the current date and time.
    ///     If <paramref name="startDate" /> is greater than DateTime.Now, this method throws an ArgumentException.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the Elapsed method to determine the time interval between two dates.
    ///     <code><![CDATA[
    /// DateTime startDate = DateTime.Now;
    /// System.Threading.Thread.Sleep(1000); // Sleep for 1 second
    /// TimeSpan elapsed = startDate.Elapsed();
    /// Console.WriteLine("Time elapsed: {0}", elapsed.ToString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Elapsed(this DateTime startDate)
    {
        return DateTime.Now.Subtract(startDate);
    }

    /// <summary>
    ///     Returns a new DateTime object that has the same date as the specified DateTime object and the time specified by a
    ///     TimeSpan value.
    /// </summary>
    /// <param name="date">The date value.</param>
    /// <param name="time">The time value.</param>
    /// <returns>
    ///     A new DateTime object that has the same date as the specified DateTime object and the time specified by the
    ///     <paramref name="time" /> parameter.
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown when the <paramref name="time" /> is less than zero or
    ///     greater than or equal to 24 hours.
    /// </exception>
    /// <remarks>
    ///     This method returns a new DateTime object that has the same date as the specified DateTime object and the time
    ///     specified by the <paramref name="time" /> parameter.
    ///     If the <paramref name="time" /> parameter is less than zero or greater than or equal to 24 hours, this method
    ///     throws an ArgumentOutOfRangeException.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the SetTime method to set the time of a DateTime object.
    ///     <code><![CDATA[
    /// DateTime date = DateTime.Now;
    /// TimeSpan time = new TimeSpan(16, 0, 0); // 4:00 PM
    /// DateTime newDate = date.SetTime(time);
    /// Console.WriteLine("Date: {0}, Time: {1}", newDate.ToShortDateString(), newDate.ToShortTimeString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SetTime(this DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    /// <summary>
    ///     Returns a new DateTime object that has the same date as the specified DateTime object and the time specified by the
    ///     <paramref name="hours" />, <paramref name="minutes" />, <paramref name="seconds" />, and
    ///     <paramref name="milliseconds" /> parameters.
    /// </summary>
    /// <param name="date">The date value.</param>
    /// <param name="hours">Optional. The hours value. Default is 0.</param>
    /// <param name="minutes">Optional. The minutes value. Default is 0.</param>
    /// <param name="seconds">Optional. The seconds value. Default is 0.</param>
    /// <param name="milliseconds">Optional. The milliseconds value. Default is 0.</param>
    /// <returns>
    ///     A new DateTime object that has the same date as the specified DateTime object and the time specified by the
    ///     <paramref name="hours" />, <paramref name="minutes" />, <paramref name="seconds" />, and
    ///     <paramref name="milliseconds" /> parameters.
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     Thrown when any of the <paramref name="hours" />,
    ///     <paramref name="minutes" />, <paramref name="seconds" />, or <paramref name="milliseconds" /> parameters are less
    ///     than zero or greater than or equal to 24 hours, 60 minutes, 60 seconds, or 1000 milliseconds respectively.
    /// </exception>
    /// <remarks>
    ///     This method returns a new DateTime object that has the same date as the specified DateTime object and the time
    ///     specified by the <paramref name="hours" />, <paramref name="minutes" />, <paramref name="seconds" />, and
    ///     <paramref name="milliseconds" /> parameters.
    ///     If any of the <paramref name="hours" />, <paramref name="minutes" />, <paramref name="seconds" />, or
    ///     <paramref name="milliseconds" /> parameters are less than zero or greater than or equal to 24 hours, 60 minutes, 60
    ///     seconds, or 1000 milliseconds respectively, this method throws an ArgumentOutOfRangeException.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the SetTime method to set the time of a DateTime object.
    ///     <code><![CDATA[
    /// DateTime date = DateTime.Now;
    /// DateTime newDate = date.SetTime(16, 0, 0); // 4:00 PM
    /// Console.WriteLine("Date: {0}, Time: {1}", newDate.ToShortDateString(), newDate.ToShortTimeString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SetTime(this DateTime date, int hours = 0, int minutes = 0, int seconds = 0,
        int milliseconds = 0)
    {
        return date.Date.Add(new TimeSpan(0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    ///     Returns a new DateTime object that has the same date as the specified DateTime object and the time specified by the
    ///     <paramref name="timeOnly" /> parameter.
    /// </summary>
    /// <param name="date">The date value.</param>
    /// <param name="timeOnly">The TimeOnly value.</param>
    /// <returns>
    ///     A new DateTime object that has the same date as the specified DateTime object and the time specified by the
    ///     <paramref name="timeOnly" /> parameter.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="timeOnly" /> parameter is null.</exception>
    /// <remarks>
    ///     This method returns a new DateTime object that has the same date as the specified DateTime object and the time
    ///     specified by the <paramref name="timeOnly" /> parameter.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the SetTime method to set the time of a DateTime object.
    ///     <code><![CDATA[
    /// DateTime date = DateTime.Now;
    /// TimeOnly time = new TimeOnly(16, 0, 0); // 4:00 PM
    /// DateTime newDate = date.SetTime(time);
    /// Console.WriteLine("Date: {0}, Time: {1}", newDate.ToShortDateString(), newDate.ToShortTimeString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime SetTime(this DateTime date, TimeOnly timeOnly)
    {
        return date.Date.Add(timeOnly.ToTimeSpan());
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> instance representing the day after the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the next day.</param>
    /// <returns>A new <see cref="DateTime" /> instance representing the day after the specified date.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime today = DateTime.Today;
    /// DateTime tomorrow = today.NextDay(); // tomorrow will be the day after today
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method adds one day to the specified <see cref="DateTime" /> instance to calculate the next day. It
    ///     returns a new <see cref="DateTime" /> instance representing the day after the specified date, without modifying the
    ///     original date. If the specified date is the last day of the year, the returned date will be the first day of the
    ///     next year.
    /// </remarks>
    public static DateTime NextDay(this DateTime date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateTime" /> instance representing the day before the specified date.
    /// </summary>
    /// <param name="date">The date for which to find the previous day.</param>
    /// <returns>A new <see cref="DateTime" /> instance representing the day before the specified date.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime today = DateTime.Today;
    /// DateTime yesterday = today.PreviousDay(); // yesterday will be the day before today
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method subtracts one day from the specified <see cref="DateTime" /> instance to calculate the
    ///     previous day. It returns a new <see cref="DateTime" /> instance representing the day before the specified date,
    ///     without modifying the original date. If the specified date is the first day of the year, the returned date will be
    ///     the last day of the previous year.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime PreviousDay(this DateTime date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Returns a string representation of the specified DateTime object in the ISO 8601 format.
    /// </summary>
    /// <param name="dateTime">The DateTime object to format.</param>
    /// <returns>A string representation of the specified DateTime object in the ISO 8601 format.</returns>
    /// <remarks>
    ///     This method returns a string representation of the specified DateTime object in the ISO 8601 format.
    ///     The format string is "yyyy-MM-ddTHH:mm:ss.fffffffzzz", where "T" is the date and time separator, and "zzz" is the
    ///     time zone offset.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the GetTimestamp method to get a string representation of a DateTime object.
    ///     <code><![CDATA[
    /// DateTime dateTime = DateTime.Now;
    /// string timestamp = dateTime.GetTimestamp();
    /// Console.WriteLine(timestamp);
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetTimestamp(this DateTime dateTime)
    {
        return dateTime.ToString(@"O");
    }

    /// <summary>
    ///     Returns an enumerable of <see cref="DateTime" /> instances representing the dates in the range between the
    ///     specified <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive).
    /// </summary>
    /// <param name="fromDate">The start date of the range.</param>
    /// <param name="toDate">The end date of the range.</param>
    /// <returns>
    ///     An enumerable of <see cref="DateTime" /> instances representing the dates in the range between the specified
    ///     <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive).
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// DateTime startDate = new DateTime(2023, 1, 1);
    /// DateTime endDate = new DateTime(2023, 1, 5);
    /// IEnumerable<DateTime> dateRange = startDate.GetDatesInRange(endDate);
    /// // dateRange will contain DateTime instances for the dates 1st, 2nd, 3rd, 4th, and 5th of January 2023
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method generates an enumerable of <see cref="DateTime" /> instances representing the dates in the
    ///     range between the specified <paramref name="fromDate" /> and <paramref name="toDate" /> (inclusive). If
    ///     <paramref name="fromDate" /> is equal to <paramref name="toDate" />, the enumerable will contain a single element
    ///     representing the specified date. If <paramref name="fromDate" /> is less than <paramref name="toDate" />, the
    ///     enumerable will be generated in ascending order. If <paramref name="fromDate" /> is greater than
    ///     <paramref name="toDate" />, the enumerable will be generated in descending order.
    /// </remarks>
    public static IEnumerable<DateTime> GetDatesInRange(this DateTime fromDate, DateTime toDate)
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
    ///     Returns a new <see cref="DateOnly" /> object that has the same date as the specified DateTime object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> object to convert.</param>
    /// <returns>A new <see cref="DateOnly" /> object that has the same date as the specified DateTime object.</returns>
    /// <remarks>
    ///     This method returns a new <see cref="DateOnly" /> object that has the same date as the specified DateTime object.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the ToDateOnly method to convert a DateTime object to a
    ///     <see cref="DateOnly" /> object.
    ///     <code><![CDATA[
    /// DateTime dateTime = DateTime.Now;
    /// DateOnly dateOnly = dateTime.ToDateOnly();
    /// Console.WriteLine("DateOnly: {0}", dateOnly.ToShortDateString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    ///     Returns a new <see cref="TimeOnly" /> object that has the same time as the specified <see cref="DateTime" />
    ///     object.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime" /> object to convert.</param>
    /// <returns>A new <see cref="TimeOnly" /> object that has the same time as the specified <see cref="DateTime" /> object.</returns>
    /// <remarks>
    ///     This method returns a new <see cref="TimeOnly" /> object that has the same time as the specified
    ///     <see cref="DateTime" /> object.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the <see cref="ToTimeOnly" /> method to convert a <see cref="DateTime" />
    ///     object to a <see cref="TimeOnly" /> object.
    ///     <code><![CDATA[
    /// DateTime dateTime = DateTime.Now;
    /// TimeOnly timeOnly = dateTime.ToTimeOnly();
    /// Console.WriteLine("TimeOnly: {0}", timeOnly.ToString());
    /// ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly ToTimeOnly(this DateTime dateTime)
    {
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond,
            dateTime.Microsecond);
    }
}