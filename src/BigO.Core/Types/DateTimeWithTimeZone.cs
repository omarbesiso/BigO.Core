using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a date time value within a specific timezone.
/// </summary>
[PublicAPI]
public struct DateTimeWithTimeZone : IComparable<DateTimeWithTimeZone>, IEquatable<DateTimeWithTimeZone>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeWithTimeZone" /> class.
    /// </summary>
    /// <param name="value">The date time.</param>
    /// <param name="timeZone">The time zone.</param>
    public DateTimeWithTimeZone(DateTime value, TimeZoneInfo timeZone)
    {
        Value = value;
        TimeZone = timeZone;
    }

    /// <summary>
    ///     Gets the date time.
    /// </summary>
    public DateTime Value { get; set; }

    /// <summary>
    ///     Gets the time zone.
    /// </summary>
    public TimeZoneInfo TimeZone { get; set; }

    /// <summary>
    ///     Gets the <see cref="DateTimeOffset" /> representing this instance.
    /// </summary>
    public DateTimeOffset DateTimeOffset => new(Value, TimeZone.GetUtcOffset(Value));

    /// <summary>
    ///     Converts the current instance to a <see cref="Value" /> to a UTC equivalent datetime instance.
    /// </summary>
    /// <returns>A datetime instance in UTC.</returns>
    public DateTime ToUtcDateTime()
    {
        return TimeZoneInfo.ConvertTimeToUtc(Value, TimeZone);
    }

    /// <summary>
    ///     Compares the current instance with another object of the same type and returns an integer that indicates whether
    ///     the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     <list type="table">
    ///         <listheader>
    ///             <term> Value</term><description> Meaning</description>
    ///         </listheader>
    ///         <item>
    ///             <term> Less than zero</term>
    ///             <description> This instance precedes <paramref name="other" /> in the sort order.</description>
    ///         </item>
    ///         <item>
    ///             <term> Zero</term>
    ///             <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
    ///         </item>
    ///         <item>
    ///             <term> Greater than zero</term>
    ///             <description> This instance follows <paramref name="other" /> in the sort order.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    public int CompareTo(DateTimeWithTimeZone other)
    {
        // Convert the date and time value to UTC
        var dateTimeUtc = ToUtcDateTime();
        var otherDateTimeUtc = other.ToUtcDateTime();

        // Compare the date and time values in UTC
        return dateTimeUtc.CompareTo(otherDateTimeUtc);
    }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(DateTimeWithTimeZone other)
    {
        return Value.Equals(other.Value) && TimeZone.Equals(other.TimeZone);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeWithTimeZone other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, TimeZone);
    }

    /// <summary>
    ///     Implements the == operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Implements the != operator.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return !left.Equals(right);
    }

    public DateTimeWithTimeZone AddMinutes(int minutes)
    {
        return new DateTimeWithTimeZone(Value.AddMinutes(minutes), TimeZone);
    }

    public DateTimeWithTimeZone AddHours(int hours)
    {
        return new DateTimeWithTimeZone(Value.AddHours(hours), TimeZone);
    }
}