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
public readonly record struct DateRange : IComparable<DateRange>
{
    private const char Separator = '-';
    private static readonly DateOnly MaxDate = DateOnly.MaxValue;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateRange" /> struct with default values. Start date is defaulted to
    ///     today and end date is set to the maximum date.
    /// </summary>
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
    public DateRange(DateOnly startDate, DateOnly? endDate)
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
    ///     Compares the current instance to another <see cref="DateRange" /> instance and returns an integer that indicates
    ///     whether the current instance precedes, follows, or occurs in the same position in the sort order as the other
    ///     instance.
    /// </summary>
    /// <param name="other">The <see cref="DateRange" /> instance to compare with the current instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 Less than zero: The current instance precedes <paramref name="other" /> in the sort order.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Zero: The current instance occurs in the same position in the sort order as <paramref name="other" />.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Greater than zero: The current instance follows <paramref name="other" /> in the sort order.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </returns>
    public int CompareTo(DateRange other)
    {
        var startDateComparison = StartDate.CompareTo(other.StartDate);
        return startDateComparison != 0 ? startDateComparison : EndDate.CompareTo(other.EndDate);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
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
    ///     Tries to parse a string representation of a date range into a <see cref="DateRange" /> instance.
    /// </summary>
    /// <param name="input">The string representation of the date range, expected in "yyyy-MM-dd-yyyy-MM-dd" format.</param>
    /// <param name="range">The parsed date range, if parsing is successful.</param>
    /// <returns>True if parsing is successful; otherwise, false.</returns>
    public static bool TryParse(string input, out DateRange range)
    {
        range = new DateRange();
        var parts = input.Trim().Split(Separator);
        if (parts.Length != 2)
        {
            return false;
        }

        if (!DateOnly.TryParse(parts[0], out var startDate) || !DateOnly.TryParse(parts[1], out var endDate))
        {
            return false;
        }

        range = new DateRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Returns a string representation of the date range.
    /// </summary>
    /// <returns>A string that represents the date range.</returns>
    public override string ToString()
    {
        return EndDate == MaxDate ? $"{StartDate:yyyy-MM-dd}-∞" : $"{StartDate:yyyy-MM-dd}-{EndDate:yyyy-MM-dd}";
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

    private static void ValidateDates(DateOnly startDate, DateOnly? endDate)
    {
        if (startDate == default)
        {
            throw new ArgumentException("Start date cannot be the default value.", nameof(startDate));
        }

        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));
        }
    }
}