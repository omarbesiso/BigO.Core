using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range between two <see cref="DateTimeWithTimeZone" /> instances.
/// </summary>
[PublicAPI]
public record struct DateTimeWithTimeZoneRange
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeWithTimeZoneRange" /> class.
    /// </summary>
    /// <param name="start">The start of the range.</param>
    /// <param name="end">The end of the range.</param>
    public DateTimeWithTimeZoneRange(DateTimeWithTimeZone start, DateTimeWithTimeZone end)
    {
        if (start > end)
        {
            throw new ArgumentException("The start of the range must be earlier than the end of the range.");
        }

        Start = start;
        End = end;
    }

    /// <summary>
    ///     Gets the start of the range.
    /// </summary>
    public DateTimeWithTimeZone Start { get; }

    /// <summary>
    ///     Gets the end of the range.
    /// </summary>
    public DateTimeWithTimeZone End { get; }
    
    /// <summary>
    ///     Determines whether this instance contains the object.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if the value falls within the range; otherwise, <c>false</c>.</returns>
    public bool Contains(DateTimeWithTimeZone value)
    {
        return Start <= value && value <= End;
    }

    /// <summary>
    ///     Determines whether this range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range to check.</param>
    /// <returns>True if the ranges overlap, false otherwise.</returns>
    public bool Overlaps(DateTimeWithTimeZoneRange other)
    {
        return Contains(other.Start) || Contains(other.End) || other.Contains(Start) || other.Contains(End);
    }

    /// <summary>
    ///     Gets the union of this range and another range.
    /// </summary>
    /// <param name="other">The other range to include in the union.</param>
    /// <returns>A new range that includes both this range and the other range.</returns>
    public DateTimeWithTimeZoneRange Union(DateTimeWithTimeZoneRange other)
    {
        return new DateTimeWithTimeZoneRange(Start < other.Start ? Start : other.Start,
            End > other.End ? End : other.End);
    }

    /// <summary>
    ///     Gets the intersection of this range and another range.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>A new range that includes only the dates and times that are present in both this range and the other range.</returns>
    public DateTimeWithTimeZoneRange Intersection(DateTimeWithTimeZoneRange other)
    {
        return new DateTimeWithTimeZoneRange(Start > other.Start ? Start : other.Start,
            End < other.End ? End : other.End);
    }

    /// <summary>
    ///     Returns the hash code for this range.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }
}