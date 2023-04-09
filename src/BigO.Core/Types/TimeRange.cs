using BigO.Core.Extensions;
using BigO.Core.Validation;
using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of times.
/// </summary>
[PublicAPI]
public readonly record struct TimeRange : IComparable<TimeRange>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRange" /> struct.
    /// </summary>
    /// <param name="startTime">The start time of the time range.</param>
    /// <param name="endTime">The end time of the time range.</param>
    /// <exception cref="ArgumentException">Thrown if the end time is before the start time.</exception>
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
    public TimeOnly StartTime { get; }

    /// <summary>
    ///     Gets the end time of the time range.
    /// </summary>
    public TimeOnly EndTime { get; }

    /// <summary>
    ///     Gets the duration of the time range.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

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

    /// <summary>
    ///     Removes the specified <see cref="TimeRange" /> from the current instance and returns a
    ///     <see cref="TimeRangeDiffResult" /> representing the remaining time.
    /// </summary>
    /// <param name="other">The time range to remove from the current instance.</param>
    /// <exception cref="NotSupportedException">Thrown when a non-supported scenario was encountered.</exception>
    /// <returns>
    ///     A <see cref="TimeRangeDiffResult" /> containing the remaining time ranges after removing the specified time range,
    ///     or <c>null</c> if the other time range does not overlap or completely contains the current instance.
    /// </returns>
    /// <remarks>
    ///     This method can handle various scenarios:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 If the other time range does not overlap with the current instance, the method returns
    ///                 <c>null</c>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 If the other time range completely contains the current instance, the method returns
    ///                 <c>null</c>.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 If the current instance contains the other time range, the method returns two remaining time
    ///                 ranges representing the time before and after the other time range.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 If the other time range overlaps with the start or end of the current instance, the method
    ///                 returns the remaining time range after removing the overlapping time.
    ///             </description>
    ///         </item>
    ///     </list>
    ///     If a non-supported scenario is encountered, a <see cref="NotSupportedException" /> is thrown.
    /// </remarks>
    public TimeRangeDiffResult? RemoveTimeRange(TimeRange other)
    {
        if (!Overlaps(other) || other.Contains(this))
        {
            // The other time range does not over lap or completely contains the current instance.
            return null;
        }

        if (Contains(other))
        {
            // The other time range is fully contained within this instance, so split it into two time ranges.
            var beforeRange = new TimeRange(StartTime, other.StartTime);
            var afterRange = new TimeRange(other.EndTime, EndTime);
            return new TimeRangeDiffResult(null, beforeRange, afterRange);
        }

        if (other.StartTime < StartTime)
        {
            // The other time range overlaps with the start of this instance, so return the remaining time after the end of the other range.
            var range = new TimeRange(other.EndTime, EndTime);
            return new TimeRangeDiffResult(range);
        }

        if (other.EndTime > EndTime)
        {
            // The other time range overlaps with the end of this instance, so return the remaining time before the start of the other range.
            var range = new TimeRange(StartTime, other.StartTime);
            return new TimeRangeDiffResult(range);
        }

        throw new NotSupportedException(
            $"A non-supported scenario was encountered when removing time range '{other}' from {this}.");
    }
}