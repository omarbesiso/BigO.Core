using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BigO.Core.Extensions;

namespace BigO.Core.Types;

/// <summary>
///     Value object defining a range of <see cref="DateTime" />.
/// </summary>
[PublicAPI]
[DataContract]
public readonly record struct DateTimeRange
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if the end time <paramref name="startDate" /> is less than or equal the
    ///     start time <paramref name="endDate" />.
    /// </exception>
    [JsonConstructor]
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
    [Required]
    [JsonInclude]
    [JsonPropertyOrder(10)]
    [JsonPropertyName("startDate")]
    [DataMember(Name = "startDate", Order = 10)]
    public DateTime StartDate { get; }

    /// <summary>
    ///     Gets the end date.
    /// </summary>
    [Required]
    [JsonInclude]
    [JsonPropertyOrder(20)]
    [JsonPropertyName("endDate")]
    [DataMember(Name = "endDate", Order = 20)]
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
    public TimeSpan Duration()
    {
        return EndDate - StartDate;
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
            return false;
        }

        dateRange = new DateTimeRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Checks if the date range is open-ended (i.e., if the end date is the maximum allowable date).
    /// </summary>
    /// <returns>True if the date range is open-ended; otherwise, false.</returns>
    public bool IsOpenEnded()
    {
        return EndDate == DateTime.MaxValue;
    }

    /// <summary>
    ///     Shifts the date range by a specified number of days.
    /// </summary>
    /// <param name="days">
    ///     The number of days to shift the date range. Positive values shift forward, negative values shift
    ///     backward.
    /// </param>
    /// <returns>A new <see cref="DateTimeRange" /> instance representing the shifted date range.</returns>
    public DateTimeRange ShiftDays(int days)
    {
        var newStartDate = StartDate.AddDays(days);
        var newEndDate = EndDate.AddDays(days);
        return new DateTimeRange(newStartDate, newEndDate);
    }

    /// <summary>
    ///     Shifts the date range by a specified number of hours.
    /// </summary>
    /// <param name="hours">
    ///     The number of hours to shift the date range. Positive values shift forward, negative values shift
    ///     backward.
    /// </param>
    /// <returns>A new <see cref="DateTimeRange" /> instance representing the shifted date range.</returns>
    public DateTimeRange ShiftHours(int hours)
    {
        var newStartDate = StartDate.AddHours(hours);
        var newEndDate = EndDate.AddHours(hours);
        return new DateTimeRange(newStartDate, newEndDate);
    }
}