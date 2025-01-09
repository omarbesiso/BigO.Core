using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BigO.Core.Extensions;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of <see cref="DateTime" />.
/// </summary>
[PublicAPI]
[DataContract]
public readonly record struct DateTimeRange
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date/time.</param>
    /// <param name="endDate">The end date/time.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="endDate" /> is less than or equal to <paramref name="startDate" />.
    /// </exception>
    [JsonConstructor]
    public DateTimeRange(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException(
                $"The end date/time '{endDate}' must be strictly greater than the start date/time '{startDate}'.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    ///     Gets the start date/time.
    /// </summary>
    [Required]
    [JsonInclude]
    [JsonPropertyOrder(10)]
    [JsonPropertyName("startDate")]
    [DataMember(Name = "startDate", Order = 10)]
    public DateTime StartDate { get; }

    /// <summary>
    ///     Gets the end date/time.
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
    ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(DateTimeRange other)
    {
        return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms
    ///     and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="date">The date/time to be checked.</param>
    /// <returns>
    ///     <c>true</c> if the date/time value falls within (inclusive) the range represented by this instance;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(DateTime date)
    {
        // Assuming IsBetween is inclusive: start <= date <= end
        return date.IsBetween(StartDate, EndDate);
    }

    /// <summary>
    ///     Checks if another <see cref="DateTimeRange" /> instance overlaps with this instance (inclusive).
    /// </summary>
    /// <param name="dateRange">The other date range.</param>
    /// <returns>
    ///     <c>true</c> if the two ranges overlap (including boundary points); otherwise, <c>false</c>.
    /// </returns>
    public bool Overlaps(DateTimeRange dateRange)
    {
        return dateRange.StartDate <= EndDate && dateRange.EndDate >= StartDate;
    }

    /// <summary>
    ///     Gets the duration of the date range as a <see cref="TimeSpan" />.
    /// </summary>
    /// <returns>The duration (EndDate - StartDate).</returns>
    public TimeSpan Duration()
    {
        return EndDate - StartDate;
    }

    /// <summary>
    ///     Returns a string representation of the date range, using a fixed format.
    /// </summary>
    /// <returns>
    ///     A string in the format "MMM dd, yyyy hh:mm tt - MMM dd, yyyy hh:mm tt"
    ///     representing the start and end dates of the range.
    /// </returns>
    public override string ToString()
    {
        // If you want culture invariance, specify CultureInfo.InvariantCulture
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0:MMM dd, yyyy hh:mm tt} - {1:MMM dd, yyyy hh:mm tt}",
            StartDate,
            EndDate
        );
    }

    /// <summary>
    ///     Tries to parse a string representation of a date range in the format "Start - End".
    /// </summary>
    /// <param name="input">The input string, e.g., "2023-01-01 09:00:00 - 2023-01-02 18:00:00".</param>
    /// <param name="dateRange">The parsed date range, if successful.</param>
    /// <returns><c>true</c> if the input string was successfully parsed; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string input, out DateTimeRange dateRange)
    {
        dateRange = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        // Split into exactly two parts
        var parts = input.Split('-', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return false;
        }

        // Attempt to parse start & end
        if (!DateTime.TryParse(parts[0], out var startDate) ||
            !DateTime.TryParse(parts[1], out var endDate))
        {
            return false;
        }

        // Ensure end > start
        if (endDate <= startDate)
        {
            return false;
        }

        dateRange = new DateTimeRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Checks if the date range is open-ended (i.e., if the end date is <see cref="DateTime.MaxValue" />).
    /// </summary>
    /// <returns><c>true</c> if the range ends at <see cref="DateTime.MaxValue" />; otherwise, <c>false</c>.</returns>
    public bool IsOpenEnded()
    {
        return EndDate == DateTime.MaxValue;
    }

    /// <summary>
    ///     Shifts the date range by a specified number of days (positive to shift forward, negative to shift backward).
    /// </summary>
    /// <param name="days">The number of days to shift.</param>
    /// <returns>
    ///     A new <see cref="DateTimeRange" /> instance representing the shifted range.
    /// </returns>
    public DateTimeRange ShiftDays(int days)
    {
        var newStartDate = StartDate.AddDays(days);
        var newEndDate = EndDate.AddDays(days);
        return new DateTimeRange(newStartDate, newEndDate);
    }

    /// <summary>
    ///     Shifts the date range by a specified number of hours (positive to shift forward, negative to shift backward).
    /// </summary>
    /// <param name="hours">The number of hours to shift.</param>
    /// <returns>
    ///     A new <see cref="DateTimeRange" /> instance representing the shifted range.
    /// </returns>
    public DateTimeRange ShiftHours(int hours)
    {
        var newStartDate = StartDate.AddHours(hours);
        var newEndDate = EndDate.AddHours(hours);
        return new DateTimeRange(newStartDate, newEndDate);
    }
}