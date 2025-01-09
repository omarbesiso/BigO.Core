using System.Globalization;
using System.Text.Json.Serialization;
using BigO.Core.Extensions;

// using BigO.Core.Validation; // Might not need Guard for TimeOnly unless default is disallowed

namespace BigO.Core.Types;

/// <summary>
///     Represents a range between two <see cref="TimeOnly" /> values.
/// </summary>
[PublicAPI]
public readonly record struct TimeRange : IComparable<TimeRange>, IComparable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRange" /> struct.
    /// </summary>
    /// <param name="startTime">The start time of the time range.</param>
    /// <param name="endTime">The end time of the time range.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="endTime" /> is before <paramref name="startTime" />.
    /// </exception>
    [JsonConstructor]
    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        // If your domain forbids zero-length:
        // if (endTime <= startTime)
        //     throw new ArgumentException("End time must be strictly after start time.");

        // Currently, you only forbid endTime < startTime:
        if (endTime < startTime)
        {
            throw new ArgumentException("End time cannot be before start time.");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    ///     Gets the start time of the time range.
    /// </summary>
    [JsonPropertyName("@startTime")]
    [JsonInclude]
    public TimeOnly StartTime { get; }

    /// <summary>
    ///     Gets the end time of the time range.
    /// </summary>
    [JsonPropertyName("@endTime")]
    [JsonInclude]
    public TimeOnly EndTime { get; }

    /// <summary>
    ///     Gets the duration of the time range (EndTime - StartTime).
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    ///     Compares this <see cref="TimeRange" /> instance to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>A signed integer that indicates the relative values of this instance and <paramref name="obj" />.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="obj" /> is not a <see cref="TimeRange" />.</exception>
    public int CompareTo(object? obj)
    {
        if (obj is TimeRange other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Object must be of type TimeRange.");
    }

    /// <summary>
    ///     Compares this <see cref="TimeRange" /> instance to another <see cref="TimeRange" />.
    /// </summary>
    /// <param name="other">The <see cref="TimeRange" /> to compare to this instance.</param>
    /// <returns>A signed integer that indicates the relative values of this instance and <paramref name="other" />.</returns>
    public int CompareTo(TimeRange other)
    {
        var startTimeComparison = StartTime.CompareTo(other.StartTime);
        if (startTimeComparison != 0)
        {
            return startTimeComparison;
        }

        return EndTime.CompareTo(other.EndTime);
    }

    /// <summary>
    ///     Determines whether the current instance and another specified <see cref="TimeRange" /> object have the same value.
    /// </summary>
    /// <param name="other">The <see cref="TimeRange" /> to compare to this instance.</param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="other" /> parameter has the same value as this instance; otherwise, <c>false</c>
    ///     .
    /// </returns>
    public bool Equals(TimeRange other)
    {
        return StartTime == other.StartTime && EndTime == other.EndTime;
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code suitable for hashing algorithms and data structures.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="TimeOnly" /> value (inclusive).
    /// </summary>
    /// <param name="time">The time to be checked.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="time" /> falls within [StartTime..EndTime]; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(TimeOnly time)
    {
        // IsBetween presumably does: start <= time <= end if the last arg is true.
        return time.IsBetween(StartTime, EndTime, true);
    }

    /// <summary>
    ///     Returns a string that represents the current object, e.g. "08:00 - 12:00".
    /// </summary>
    public override string ToString()
    {
        // Example: "08:00 - 12:00" in current culture format
        // If you want 24-hour HH:mm, you can specify: 
        // return $"{StartTime:HH:mm} - {EndTime:HH:mm}";
        return $"{StartTime} - {EndTime}";
    }

    /// <summary>
    ///     Attempts to parse a string representation of a time range into a new <see cref="TimeRange" />.
    ///     The expected format is "Start - End" using
    ///     <see
    ///         cref="M:System.TimeOnly.TryParse(System.ReadOnlySpan{System.Char},System.IFormatProvider,System.Globalization.DateTimeStyles,out System.TimeOnly)" />
    ///     .
    /// </summary>
    /// <param name="s">A string containing a time range to parse.</param>
    /// <param name="timeRange">
    ///     When this method returns, contains the <see cref="TimeRange" /> value if parsing succeeded,
    ///     or <c>default</c> if the conversion failed.
    /// </param>
    /// <param name="culture">An optional culture-specific formatting. Defaults to current culture.</param>
    /// <returns><c>true</c> if <paramref name="s" /> was converted successfully; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string s, out TimeRange timeRange, CultureInfo? culture = null)
    {
        timeRange = default;
        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        var parts = s.Split(" - ");
        culture ??= CultureInfo.CurrentCulture;

        if (parts.Length == 2)
        {
#if NET6_0
            if (TryParseTimeOnlyNet6(parts[0], culture, out var start) &&
                TryParseTimeOnlyNet6(parts[1], culture, out var end))
            {
                // If you want to forbid zero-length: if (end <= start) return false;
                timeRange = new TimeRange(start, end);
                return true;
            }
#else
            if (TimeOnly.TryParse(parts[0], culture, out var start) &&
                TimeOnly.TryParse(parts[1], culture, out var end))
            {
                // If you want to forbid zero-length: if (end <= start) return false;
                timeRange = new TimeRange(start, end);
                return true;
            }
#endif
        }

        return false;
    }

#if NET6_0
    private static bool TryParseTimeOnlyNet6(string s, CultureInfo culture, out TimeOnly result)
    {
        if (DateTime.TryParse(s, culture, DateTimeStyles.NoCurrentDateDefault, out var dateTime))
        {
            result = TimeOnly.FromDateTime(dateTime);
            return true;
        }

        result = default;
        return false;
    }
#endif

    /// <summary>
    ///     Creates a new <see cref="TimeRange" /> from a start time and a duration.
    /// </summary>
    public static TimeRange FromDuration(TimeOnly startTime, TimeSpan duration)
    {
        return new TimeRange(startTime, startTime.Add(duration));
    }

    /// <summary>
    ///     Shifts the entire time range by a specified <see cref="TimeSpan" />.
    /// </summary>
    public TimeRange Shift(TimeSpan offset)
    {
        return new TimeRange(StartTime.Add(offset), EndTime.Add(offset));
    }

    /// <summary>
    ///     Returns a new <see cref="TimeRange" /> that encompasses both the current range and the given range (earliest start,
    ///     latest end).
    /// </summary>
    public TimeRange Union(TimeRange other)
    {
        var startTime = StartTime < other.StartTime ? StartTime : other.StartTime;
        var endTime = EndTime > other.EndTime ? EndTime : other.EndTime;
        return new TimeRange(startTime, endTime);
    }

    /// <summary>
    ///     Merges this range with another <see cref="TimeRange" /> only if they overlap; otherwise, throws an exception.
    ///     The resulting range has the earliest of the two start times and the latest of the two end times.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the two ranges do not overlap.</exception>
    public TimeRange Merge(TimeRange other)
    {
        if (!Overlaps(other))
        {
            throw new InvalidOperationException("Cannot merge two time-ranges that do not overlap.");
        }

        var startTime = StartTime < other.StartTime ? StartTime : other.StartTime;
        var endTime = EndTime > other.EndTime ? EndTime : other.EndTime;
        return new TimeRange(startTime, endTime);
    }

    /// <summary>
    ///     Returns a new <see cref="TimeRange" /> that is the overlap (intersection) between this range and the given range.
    ///     Returns <c>null</c> if they do not overlap.
    /// </summary>
    public TimeRange? Intersect(TimeRange other)
    {
        if (!Overlaps(other))
        {
            return null;
        }

        var startTime = StartTime > other.StartTime ? StartTime : other.StartTime;
        var endTime = EndTime < other.EndTime ? EndTime : other.EndTime;
        return new TimeRange(startTime, endTime);
    }

    /// <summary>
    ///     Determines whether the current <see cref="TimeRange" /> instance overlaps with another <see cref="TimeRange" />
    ///     instance (inclusive).
    /// </summary>
    public bool Overlaps(TimeRange other)
    {
        return StartTime <= other.EndTime && EndTime >= other.StartTime;
    }

    /// <summary>
    ///     Determines whether the current <see cref="TimeRange" /> instance fully contains another <see cref="TimeRange" />
    ///     instance (inclusive).
    /// </summary>
    public bool Contains(TimeRange other)
    {
        return StartTime <= other.StartTime && EndTime >= other.EndTime;
    }
}