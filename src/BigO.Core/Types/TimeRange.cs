using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Value object defining a range of <see cref="TimeOnly" />.
/// </summary>
[PublicAPI]
public struct TimeRange : IEquatable<TimeRange>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="T:BigO.Core.Types.TimeRange" /> struct with a range between
    ///     <paramref name="startTime" /> and <paramref name="endTime" />.
    /// </summary>
    /// <param name="startTime">The <see cref="TimeOnly" /> object representing the start time of the range.</param>
    /// <param name="endTime">The <see cref="TimeOnly" /> object representing the end time of the range.</param>
    /// <exception cref="System.ArgumentException">
    ///     Thrown when the <paramref name="endTime" /> is less than or equal the
    ///     <paramref name="startTime" />.
    /// </exception>
    public TimeRange(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentException(
                $"The end time '{endTime}' is less than or equal the start time '{startTime}'.");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    /// <summary>
    ///     Gets the start time of the range.
    /// </summary>
    public TimeOnly StartTime { get; }

    /// <summary>
    ///     Gets the end time of the range.
    /// </summary>
    public TimeOnly EndTime { get; }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(TimeRange other)
    {
        return StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is TimeRange other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }

    public static bool operator ==(TimeRange left, TimeRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TimeRange left, TimeRange right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="TimeOnly" /> instance.
    /// </summary>
    /// <param name="time">The time to be checked.</param>
    /// <returns><c>true</c> if the time value falls within the range represented by this instance; otherwise, <c>false</c>.</returns>
    public bool Contains(TimeOnly time)
    {
        return time.IsBetween(StartTime, EndTime);
    }

    /// <summary>
    ///     Checks if another <see cref="TimeRange" /> instance overlaps with this instance.
    /// </summary>
    /// <param name="timeRange">The time range.</param>
    /// <returns><c>true</c> if the provided time range overlaps this instance, <c>false</c> otherwise.</returns>
    public bool Overlaps(TimeRange timeRange)
    {
        return StartTime <= timeRange.EndTime && timeRange.StartTime <= EndTime;
    }
}