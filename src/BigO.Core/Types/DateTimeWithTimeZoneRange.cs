using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a timezone specified date time range.
/// </summary>
[PublicAPI]
public struct DateTimeWithTimeZoneRange : IEquatable<DateTimeWithTimeZoneRange>
{
    /// <summary>
    ///     Gets or sets the start of the range.
    /// </summary>
    public DateTimeWithTimeZone Start { get; set; }

    /// <summary>
    ///     Gets or sets the end of the range.
    /// </summary>
    public DateTimeWithTimeZone End { get; set; }

    public DateTimeWithTimeZoneRange(DateTimeWithTimeZone start, DateTimeWithTimeZone end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    ///     Determines whether this instance contains the object.
    /// </summary>
    /// <param name="dateTimeWithTimeZone">The date time with time zone.</param>
    /// <returns><c>true</c> if the range contains the <paramref name="dateTimeWithTimeZone" />; otherwise, <c>false</c>.</returns>
    public bool Contains(DateTimeWithTimeZone dateTimeWithTimeZone)
    {
        var utcDateTime = dateTimeWithTimeZone.ToUtcDateTime();
        return utcDateTime >= Start.ToUtcDateTime() && utcDateTime <= End.ToUtcDateTime();
    }

    /// <summary>
    ///     Checks if another <see cref="DateTimeWithTimeZoneRange" /> instance overlaps with this instance.
    /// </summary>
    /// <param name="dateTimeRange">The date time with timezone range.</param>
    /// <returns><c>true</c> if the provided date time range overlaps this instance, <c>false</c> otherwise.</returns>
    public bool Overlaps(DateTimeWithTimeZoneRange dateTimeRange)
    {
        return Start.ToUtcDateTime() <= dateTimeRange.End.ToUtcDateTime() &&
               dateTimeRange.Start.ToUtcDateTime() <= End.ToUtcDateTime();
    }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(DateTimeWithTimeZoneRange other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeWithTimeZoneRange other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    /// <summary>
    ///     Implements the == operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(DateTimeWithTimeZoneRange left, DateTimeWithTimeZoneRange right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Implements the != operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(DateTimeWithTimeZoneRange left, DateTimeWithTimeZoneRange right)
    {
        return !left.Equals(right);
    }
}