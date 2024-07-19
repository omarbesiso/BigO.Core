using System.Globalization;
using System.Text.Json.Serialization;
using BigO.Core.Extensions;
using BigO.Core.Validation;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of between two defined <see cref="TimeOnly" /> values.
/// </summary>
[PublicAPI]
public readonly record struct TimeRange : IComparable<TimeRange>, IComparable
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRange" /> struct.
    /// </summary>
    /// <param name="startTime">The start time of the time range.</param>
    /// <param name="endTime">The end time of the time range.</param>
    /// <exception cref="ArgumentException">Thrown if the end time is before the start time.</exception>
    [JsonConstructor]
    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        Guard.NotNull(startTime, nameof(startTime));
        Guard.NotNull(endTime, nameof(endTime));

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
    ///     Gets the duration of the time range.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    ///     Compares the current <see cref="TimeRange" /> instance to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>A value indicating the relative order of the instances being compared.</returns>
    /// <exception cref="ArgumentException">Thrown if the object is not a <see cref="TimeRange" />.</exception>
    /// <remarks>
    ///     The comparison is performed by comparing the start times of the two time ranges. If the start times are equal,
    ///     the end times are compared.
    /// </remarks>
    public int CompareTo(object? obj)
    {
        if (obj is TimeRange other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException("Object must be of type TimeRange.");
    }

    /// <summary>
    ///     Compares the current <see cref="TimeRange" /> instance to another <see cref="TimeRange" /> instance.
    /// </summary>
    /// <param name="other">The <see cref="TimeRange" /> instance to compare with the current instance.</param>
    /// <returns>A value indicating the relative order of the instances being compared.</returns>
    /// <remarks>
    ///     The comparison is performed by comparing the start times of the two time ranges. If the start times are equal,
    ///     the end times are compared.
    /// </remarks>
    public int CompareTo(TimeRange other)
    {
        var startTimeComparison = StartTime.CompareTo(other.StartTime);
        return startTimeComparison != 0 ? startTimeComparison : EndTime.CompareTo(other.EndTime);
    }

    /// <summary>
    ///     Determines whether the current instance and another specified <see cref="TimeRange" /> object have the same value.
    /// </summary>
    /// <param name="other">The <see cref="TimeRange" /> to compare to this instance.</param>
    /// <returns>
    ///     <c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(TimeRange other)
    {
        return StartTime == other.StartTime && EndTime == other.EndTime;
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="TimeOnly" /> instance.
    /// </summary>
    /// <param name="time">The time to be checked.</param>
    /// <returns>
    ///     <c>true</c> if the time value falls within the range represented by this instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(TimeOnly time)
    {
        return time.IsBetween(StartTime, EndTime, true);
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{StartTime} - {EndTime}";
    }

    /// <summary>
    ///     Attempts to parse a string representation of a time range into a new <see cref="TimeRange" /> object.
    /// </summary>
    /// <param name="s">A string containing a time range to parse.</param>
    /// <param name="timeRange">
    ///     When this method returns, contains the <see cref="TimeRange" /> value equivalent to the time
    ///     range contained in <paramref name="s" />, if the conversion succeeded, or <c>null</c> if the conversion failed.
    /// </param>
    /// <param name="culture">An optional object that supplies culture-specific formatting information.</param>
    /// <returns><c>true</c> if <paramref name="s" /> was converted successfully; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method is designed to parse a time range string of the format "StartTime - EndTime", where StartTime and
    ///     EndTime are <see cref="TimeOnly" /> values.
    ///     If the provided culture is not specified, the current culture is used.
    /// </remarks>
    public static bool TryParse(string s, out TimeRange timeRange, CultureInfo? culture = null)
    {
        var parts = s.Split(" - ");
        culture ??= CultureInfo.CurrentCulture;

        if (parts.Length == 2)
        {
#if NET6_0
            if (TryParseTimeOnlyNet6(parts[0], culture, out var start) &&
                TryParseTimeOnlyNet6(parts[1], culture, out var end))
            {
                timeRange = new TimeRange(start, end);
                return true;
            }
#else
            if (TimeOnly.TryParse(parts[0], culture, out var start) &&
                TimeOnly.TryParse(parts[1], culture, out var end))
            {
                timeRange = new TimeRange(start, end);
                return true;
            }
#endif
        }

        timeRange = default;
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
    ///     Creates a new <see cref="TimeRange" /> object from a start time and a duration.
    /// </summary>
    /// <param name="startTime">The start time of the time range.</param>
    /// <param name="duration">The duration of the time range.</param>
    /// <returns>A new <see cref="TimeRange" /> object with the specified start time and duration.</returns>
    public static TimeRange FromDuration(TimeOnly startTime, TimeSpan duration)
    {
        return new TimeRange(startTime, startTime.Add(duration));
    }

    /// <summary>
    ///     Shifts the entire time range by a specified <see cref="TimeSpan" /> and returns a new <see cref="TimeRange" />
    ///     object.
    /// </summary>
    /// <param name="offset">The <see cref="TimeSpan" /> value by which the time range will be shifted.</param>
    /// <returns>A new <see cref="TimeRange" /> object representing the shifted time range.</returns>
    public TimeRange Shift(TimeSpan offset)
    {
        return new TimeRange(StartTime.Add(offset), EndTime.Add(offset));
    }

    /// <summary>
    ///     Returns a new <see cref="TimeRange" /> that encompasses both the current range and the given range.
    /// </summary>
    /// <param name="other">The other time range to include in the union.</param>
    /// <returns>A new <see cref="TimeRange" /> that encompasses both the current range and the given range.</returns>
    public TimeRange Union(TimeRange other)
    {
        var startTime = StartTime < other.StartTime ? StartTime : other.StartTime;
        var endTime = EndTime > other.EndTime ? EndTime : other.EndTime;
        return new TimeRange(startTime, endTime);
    }

    /// <summary>
    ///     Merges the current <see cref="TimeRange" /> instance with another <see cref="TimeRange" /> instance to create a new
    ///     <see cref="TimeRange" /> that encompasses both. It's a union with the mandatory check of overlapping.
    /// </summary>
    /// <param name="other">The other <see cref="TimeRange" /> instance to merge with.</param>
    /// <returns>
    ///     A new <see cref="TimeRange" /> instance that encompasses both the current <see cref="TimeRange" /> instance
    ///     and the other <see cref="TimeRange" /> instance.
    /// </returns>
    /// <remarks>
    ///     The resulting <see cref="TimeRange" /> instance will have a start time equal to the later of the two start times,
    ///     and an end time equal to the earlier of the two end times.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when the two time ranges do not overlap.</exception>
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
    ///     Returns a new <see cref="TimeRange" /> that is the overlap between the current range and the given range.
    /// </summary>
    /// <param name="other">The other time range to intersect with.</param>
    /// <returns>A new <see cref="TimeRange" /> that is the overlap between the current range and the given range.</returns>
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
    ///     instance.
    /// </summary>
    /// <param name="other">The other <see cref="TimeRange" /> instance to compare to.</param>
    /// <returns>
    ///     <c>true</c> if the current <see cref="TimeRange" /> instance overlaps with the other <see cref="TimeRange" />
    ///     instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Overlaps(TimeRange other)
    {
        return StartTime <= other.EndTime && EndTime >= other.StartTime;
    }

    /// <summary>
    ///     Determines whether the current <see cref="TimeRange" /> instance contains another <see cref="TimeRange" />
    ///     instance.
    /// </summary>
    /// <param name="other">The other <see cref="TimeRange" /> instance to check for containment.</param>
    /// <returns>
    ///     <c>true</c> if the current <see cref="TimeRange" /> instance fully contains the other <see cref="TimeRange" />
    ///     instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(TimeRange other)
    {
        return StartTime <= other.StartTime && EndTime >= other.EndTime;
    }
}