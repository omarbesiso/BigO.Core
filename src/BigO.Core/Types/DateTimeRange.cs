using BigO.Core.Extensions;
using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Value object defining a range of <see cref="DateTime" />.
/// </summary>
[PublicAPI]
public struct DateTimeRange : IEquatable<DateTimeRange>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <exception cref="ArgumentException">
    ///     The end time <paramref name="startDate" /> is less than or equal the start time
    ///     <paramref name="endDate" />.
    /// </exception>
    public DateTimeRange(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException(
                $"The end date '{endDate}' is less than or equal the start date '{startDate}'.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    ///     Gets the start date.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    ///     Gets the end date.
    /// </summary>
    public DateTime EndDate { get; }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <c>false</c>.
    /// </returns>
    public bool Equals(DateTimeRange other)
    {
        return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeRange other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <returns><c>true</c> if the date value falls within the range represented by this instance; otherwise, <c>false</c>.</returns>
    public bool Contains(DateTime date)
    {
        return date.IsBetween(StartDate, EndDate);
    }

    /// <summary>
    ///     Checks if another <see cref="DateTimeRange" /> instance overlaps with this instance.
    /// </summary>
    /// <param name="dateRange">The date range.</param>
    /// <returns><c>true</c> if the date range overlaps with this instance; otherwise, <c>false</c>.</returns>
    public bool Overlaps(DateTimeRange dateRange)
    {
        return dateRange.StartDate <= EndDate && dateRange.EndDate >= StartDate;
    }

    /// <summary>
    ///     Gets the duration of the date range in days.
    /// </summary>
    /// <returns>The number of days between the start and end dates of the date range.</returns>
    public int Duration()
    {
        return (EndDate - StartDate).Days;
    }

    /// <summary>
    ///     Returns a string representation of the date range.
    /// </summary>
    /// <returns>
    ///     A string in the format "MMM dd, yyyy hh:mm tt - MMM dd, yyyy hh:mm tt" representing the start and end dates of
    ///     the date range.
    /// </returns>
    public override string ToString()
    {
        return $"{StartDate:MMM dd, yyyy hh:mm tt} - {EndDate:MMM dd, yyyy hh:mm tt}";
    }

    /// <summary>
    ///     Tries to parse a string representation of a date range.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="dateRange">The parsed date range.</param>
    /// <returns><c>true</c> if the input string was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string input, out DateTimeRange dateRange)
    {
        dateRange = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var parts = input.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        if (!DateTime.TryParse(parts[0], out var startDate) || !DateTime.TryParse(parts[1], out var endDate))
        {
            return false;
        }

        if (endDate <= startDate)
        {
            throw new ArgumentException(
                $"The end date '{endDate}' is less than or equal the start date '{startDate}'.");
        }

        dateRange = new DateTimeRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Determines whether the two specified <see cref="DateTimeRange" /> instances are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same date range; otherwise,
    ///     <c>false</c>.
    /// </returns>
    public static bool operator ==(DateTimeRange left, DateTimeRange right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines whether the two specified <see cref="DateTimeRange" /> instances are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same date range;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(DateTimeRange left, DateTimeRange right)
    {
        return !left.Equals(right);
    }
}