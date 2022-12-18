using System.Globalization;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
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
    ///     Returns a new <see cref="DateOnly" /> value that adds the specified number of weeks to a <see cref="DateOnly" />
    ///     instance.
    /// </summary>
    /// <param name="date">The date to which weeks are added.</param>
    /// <param name="numberOfWeeks">
    ///     A number of whole and fractional weeks. The numberOfWeeks
    ///     parameter can be negative or positive.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.DateOnly" /> object with a value is the sum of the date and
    ///     time represented by the instance and the number of weeks represented by
    ///     <paramref name="numberOfWeeks" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="DateOnly" /> is less than <see cref="System.DateOnly.MinValue" /> or greater than
    ///     <see cref="System.DateOnly.MaxValue" />".
    /// </exception>
    public static DateOnly AddWeeks(this DateOnly date, int numberOfWeeks)
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
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
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
    public static DateOnly GetLastDateOfWeek(this DateOnly date, CultureInfo? cultureInfo = null)
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
    ///     Output value is greater than <see cref="Int32.MaxValue" /> or less than
    ///     <see cref="Int32.MinValue" />.
    /// </exception>
    public static int GetNumberOfDays(this DateOnly fromDate, DateOnly toDate)
    {
        var timeSpan = toDate.ToDateTime(TimeOnly.MinValue).Subtract(fromDate.ToDateTime(TimeOnly.MinValue));
        return Convert.ToInt32(timeSpan.TotalDays);
    }

    /// <summary>
    ///     Returns <c>true</c> if the <see cref="DateOnly" /> occurs after the <paramref name="other" />.
    /// </summary>
    /// <param name="source">The <see cref="DateOnly" /> instance.</param>
    /// <param name="other">The <see cref="DateOnly" /> object to be compared to.</param>
    /// <returns><c>true</c> if the date instance occurs after the passed in <see cref="DateOnly" />, otherwise <c>false</c>.</returns>
    public static bool IsAfter(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) > 0;
    }

    /// <summary>
    ///     Returns <c>true</c> if the <see cref="DateOnly" /> occurs before the <paramref name="other" />.
    /// </summary>
    /// <param name="source">The <see cref="DateOnly" /> object instance.</param>
    /// <param name="other">The <see cref="DateOnly" /> object to be compared to.</param>
    /// <returns><c>true</c> if the date instance occurs before the passed in <see cref="DateOnly" />.</returns>
    public static bool IsBefore(this DateOnly source, DateOnly other)
    {
        return source.CompareTo(other) < 0;
    }

    /// <summary>
    ///     Returns <c>true</c> if the passed in date is corresponds to today's date, otherwise returns <c>false</c>.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is today, otherwise <c>false</c>.</returns>
    public static bool IsToday(this DateOnly date)
    {
        return date == DateTime.Today.ToDateOnly();
    }

    /// <summary>
    ///     Determines whether the specified date is a leap day i.e. the 29th of February.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <returns><c>true</c> if the specified date is a leap day; otherwise, <c>false</c>.</returns>
    public static bool IsLeapDay(this DateOnly date)
    {
        return date is { Month: 2, Day: 29 };
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> with one day added to the <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> instance.</param>
    /// <returns>A new <see cref="DateOnly" /> object with 1 day added.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="System.DateOnly" /> is less than <see cref="System.DateOnly.MinValue" /> or greater than
    ///     <see cref="System.DateOnly.MaxValue" />.
    /// </exception>
    public static DateOnly NextDay(this DateOnly date)
    {
        return date.AddDays(1);
    }

    /// <summary>
    ///     Returns a new <see cref="DateOnly" /> with one day subtracted to the <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly" /> instance.</param>
    /// <returns>A new <see cref="DateOnly" /> object with 1 day subtracted.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The resulting <see cref="System.DateOnly" /> is less than <see cref="System.DateOnly.MinValue" /> or greater than
    ///     <see cref="System.DateOnly.MaxValue" />.
    /// </exception>
    public static DateOnly PreviousDay(this DateOnly date)
    {
        return date.AddDays(-1);
    }

    /// <summary>
    ///     Gets the dates between two dates as as an <see cref="IEnumerable{DateOnly}" /> instance.
    /// </summary>
    /// <param name="fromDate">The start of the date range.</param>
    /// <param name="toDate">The end of the date range.</param>
    /// <returns>The list of dates between the two days.</returns>
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
    ///     Indicates if the specified date is within the specified range.
    /// </summary>
    /// <param name="dt">The date  object to be checked.</param>
    /// <param name="rangeBeg">The range's starting point.</param>
    /// <param name="rangeEnd">The range's ending point.</param>
    /// <param name="isInclusive">Indicates if the date range end and start are inclusive or not.</param>
    /// <returns><c>true</c> if the date is within the range, <c>false</c> otherwise.</returns>
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