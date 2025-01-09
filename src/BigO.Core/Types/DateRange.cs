using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BigO.Core.Extensions;
using BigO.Core.Serialization;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of dates.
/// </summary>
[PublicAPI]
[DataContract]
[JsonConverter(typeof(DateRangeConverter))]
public readonly record struct DateRange
{
    private const char Separator = '-';
    private static readonly DateOnly MaxDate = DateOnly.MaxValue;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateRange" /> struct with default values.
    ///     The start date defaults to today and the end date defaults to the maximum allowable date.
    /// </summary>
    /// <remarks>
    ///     If you do not want this behavior, consider removing this constructor
    ///     or replacing it with a static factory method.
    /// </remarks>
    public DateRange() : this(DateTime.Today.ToDateOnly(), MaxDate)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range, or null to indicate an open-ended range.</param>
    /// <exception cref="ArgumentException">Thrown if the end date is before the start date.</exception>
    [JsonConstructor]
    public DateRange(DateOnly startDate, DateOnly? endDate = null)
    {
        ValidateDates(startDate, endDate);
        StartDate = startDate;
        EndDate = endDate.GetValueOrDefault(MaxDate);
    }

    /// <summary>
    ///     Gets the start date of the date range.
    /// </summary>
    [JsonInclude]
    [JsonPropertyOrder(10)]
    [JsonPropertyName("startDate")]
    [DataMember(Name = "startDate", Order = 10)]
    public DateOnly StartDate { get; private init; }

    /// <summary>
    ///     Gets the end date of the date range.
    /// </summary>
    [JsonInclude]
    [JsonPropertyOrder(20)]
    [JsonPropertyName("endDate")]
    [DataMember(Name = "endDate", Order = 20)]
    public DateOnly EndDate { get; private init; }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    /// <summary>
    ///     Determines whether the current date range overlaps with another date range.
    /// </summary>
    /// <param name="other">The other date range to check for overlap with.</param>
    /// <returns>True if the date ranges overlap; otherwise, false.</returns>
    public bool Overlaps(DateRange other)
    {
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    /// <summary>
    ///     Checks if the given date is within the date range.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the date is within the date range; otherwise, false.</returns>
    public bool Contains(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }

    /// <summary>
    ///     Calculates the duration of the date range in days.
    /// </summary>
    /// <returns>The total number of days in the date range, inclusive of both start and end dates.</returns>
    public int Duration()
    {
        return EndDate.DayNumber - StartDate.DayNumber;
    }

    /// <summary>
    ///     Finds the intersection of this date range with another date range.
    /// </summary>
    /// <param name="other">The other date range to intersect with.</param>
    /// <returns>The intersected date range, or null if there is no intersection.</returns>
    public DateRange? Intersection(DateRange other)
    {
        if (!Overlaps(other))
        {
            return null;
        }

        var startDate = StartDate > other.StartDate ? StartDate : other.StartDate;
        var endDate = EndDate < other.EndDate ? EndDate : other.EndDate;
        return new DateRange(startDate, endDate);
    }

    /// <summary>
    ///     Creates a new date range that is the union of this date range and another date range.
    /// </summary>
    /// <param name="other">The other date range to union with.</param>
    /// <returns>The union of the two date ranges.</returns>
    public DateRange Union(DateRange other)
    {
        var startDate = StartDate < other.StartDate ? StartDate : other.StartDate;
        var endDate = EndDate > other.EndDate ? EndDate : other.EndDate;
        return new DateRange(startDate, endDate);
    }

    /// <summary>
    ///     Tries to parse a string representation of a date range into a <see cref="DateRange" />.
    ///     Supports the format "yyyy-MM-dd-yyyy-MM-dd" or "yyyy-MM-dd-∞" (for open-ended).
    /// </summary>
    /// <param name="input">The string representation of the date range.</param>
    /// <param name="range">The parsed date range, if parsing is successful.</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public static bool TryParse(string input, out DateRange range)
    {
        range = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var parts = input.Trim().Split(Separator);
        if (parts.Length != 2)
        {
            return false;
        }

        // Attempt to parse start date
        if (!DateOnly.TryParse(parts[0], out var startDate))
        {
            return false;
        }

        // Check if the end date is "∞"
        if (parts[1].Trim() == "∞")
        {
            // Acceptable if startDate != default
            if (startDate == default)
            {
                return false;
            }

            range = new DateRange(startDate);
            return true;
        }

        // Otherwise parse the end date
        if (!DateOnly.TryParse(parts[1], out var endDate))
        {
            return false;
        }

        // Validate order
        if (startDate == default || endDate < startDate)
        {
            return false;
        }

        range = new DateRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Returns a string representation of the date range.
    ///     For open-ended ranges (i.e., <see cref="EndDate" /> == <see cref="DateOnly.MaxValue" />), prints "-∞".
    /// </summary>
    /// <returns>A string that represents the date range.</returns>
    public override string ToString()
    {
        return EndDate == MaxDate
            ? $"{StartDate:yyyy-MM-dd}-∞"
            : $"{StartDate:yyyy-MM-dd}-{EndDate:yyyy-MM-dd}";
    }

    /// <summary>
    ///     Enumerates each day within the date range.
    /// </summary>
    /// <returns>A collection of <see cref="DateOnly" /> representing each day in the range.</returns>
    public IEnumerable<DateOnly> DaysInRange()
    {
        for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
        {
            yield return date;
        }
    }

    /// <summary>
    ///     Splits the date range into multiple date ranges, each representing a week (7 days).
    /// </summary>
    /// <returns>A collection of <see cref="DateRange" /> objects, each representing a week within the original range.</returns>
    public IEnumerable<DateRange> GetWeeksInRange()
    {
        var currentStart = StartDate;
        var currentEnd = StartDate.AddDays(6);

        while (currentStart <= EndDate)
        {
            if (currentEnd > EndDate)
            {
                currentEnd = EndDate;
            }

            yield return new DateRange(currentStart, currentEnd);

            currentStart = currentEnd.AddDays(1);
            currentEnd = currentStart.AddDays(6);
        }
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="DateRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range, or null to indicate an open-ended range.</param>
    /// <returns>A new <see cref="DateRange" /> instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the end date is before the start date.</exception>
    public static DateRange Create(DateOnly startDate, DateOnly? endDate = null)
    {
        return new DateRange(startDate, endDate);
    }

    /// <summary>
    ///     Shifts the date range by a specified number of days.
    /// </summary>
    /// <param name="days">
    ///     The number of days to shift the date range. Positive values shift forward, negative values shift
    ///     backward.
    /// </param>
    /// <returns>A new <see cref="DateRange" /> instance representing the shifted date range.</returns>
    public DateRange Shift(int days)
    {
        var newStartDate = StartDate.AddDays(days);
        var newEndDate = EndDate.AddDays(days);
        return new DateRange(newStartDate, newEndDate);
    }

    /// <summary>
    ///     Checks if the date range is open-ended (i.e., if the end date is the maximum allowable date).
    /// </summary>
    /// <returns>True if the date range is open-ended; otherwise, false.</returns>
    public bool IsOpenEnded()
    {
        return EndDate == MaxDate;
    }

    /// <summary>
    ///     Validates the start and end dates.
    /// </summary>
    /// <param name="startDate">The start date to validate.</param>
    /// <param name="endDate">The end date to validate, or null for an open-ended range.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if the end date is before the start date
    ///     or if the start date is the default (0001-01-01).
    /// </exception>
    private static void ValidateDates(DateOnly startDate, DateOnly? endDate)
    {
        if (startDate == default)
        {
            throw new ArgumentException("Start date cannot be the default (0001-01-01).", nameof(startDate));
        }

        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));
        }
    }
}