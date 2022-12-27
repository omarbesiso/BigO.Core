using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of times.
/// </summary>
[PublicAPI]
public readonly struct TimeRange : IEquatable<TimeRange>
{
    /// <summary>
    ///     Gets the start time of the time range.
    /// </summary>
    public TimeOnly StartTime { get; }

    /// <summary>
    ///     Gets the end time of the time range.
    /// </summary>
    public TimeOnly EndTime { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRange" /> struct.
    /// </summary>
    /// <param name="startTime">The start time of the time range.</param>
    /// <param name="endTime">The end time of the time range.</param>
    /// <exception cref="ArgumentException">Thrown if the end time is before the start time.</exception>
    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime < startTime)
        {
            throw new ArgumentException("End time cannot be before start time.");
        }

        StartTime = startTime;
        EndTime = endTime;
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
    ///     Determines whether the specified object is a <see cref="TimeRange" /> and whether it has the same value as the
    ///     current instance.
    /// </summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="obj" /> is a <see cref="TimeRange" /> and its value is the same as the value of
    ///     this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is TimeRange range)
        {
            return Equals(range);
        }

        return false;
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
    ///     Determines whether two specified <see cref="TimeRange" /> objects have the same value.
    /// </summary>
    /// <param name="left">The first <see cref="TimeRange" /> to compare, or <c>null</c>.</param>
    /// <param name="right">The second <see cref="TimeRange" /> to compare, or <c>null</c>.</param>
    /// <returns>
    ///     <c>true</c> if the value of <paramref name="left" /> is the same as the value of <paramref name="right" />;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(TimeRange left, TimeRange right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines whether two specified <see cref="TimeRange" /> objects have different values.
    /// </summary>
    /// <param name="left">The first <see cref="TimeRange" /> to compare, or <c>null</c>.</param>
    /// <param name="right">The second <see cref="TimeRange" /> to compare, or <c>null</c>.</param>
    /// <returns>
    ///     <c>true</c> if the value of <paramref name="left" /> is different from the value of <paramref name="right" />;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(TimeRange left, TimeRange right)
    {
        return !left.Equals(right);
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
        return time.IsBetween(StartTime, EndTime);
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
    ///     Tries to parse the specified string representation of a time range.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="timeRange">The resulting <see cref="TimeRange" /> object.</param>
    /// <returns>
    ///     <c>true</c> if the string was successfully parsed; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParse(string s, out TimeRange timeRange)
    {
        var parts = s.Split(" - ");
        if (parts.Length == 2 && TimeOnly.TryParse(parts[0], out var start) && TimeOnly.TryParse(parts[1], out var end))
        {
            timeRange = new TimeRange(start, end);
            return true;
        }

        timeRange = default;
        return false;
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
    ///     Returns a new <see cref="TimeRange" /> that is the overlap between the current range and the given range.
    /// </summary>
    /// <param name="other">The other time range to intersect with.</param>
    /// <returns>A new <see cref="TimeRange" /> that is the overlap between the current range and the given range.</returns>
    public TimeRange Intersect(TimeRange other)
    {
        var startTime = StartTime > other.StartTime ? StartTime : other.StartTime;
        var endTime = EndTime < other.EndTime ? EndTime : other.EndTime;
        return new TimeRange(startTime, endTime);
    }
}