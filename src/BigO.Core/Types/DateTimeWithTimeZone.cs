using System.Globalization;
using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a DateTime value with a specified Timezone.
/// </summary>
[PublicAPI]
public record struct DateTimeWithTimeZone : IComparable<DateTimeWithTimeZone>, IEquatable<DateTimeWithTimeZone>
{
    private readonly DateTime _dateTime;
    private readonly TimeZoneInfo _timeZone;
    private DateTime? _localTime;

    private DateTime? _universalTime;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeWithTimeZone" /> struct.
    /// </summary>
    /// <param name="dateTime">The date and time value.</param>
    /// <param name="timeZone">The time zone of the date and time value.</param>
    public DateTimeWithTimeZone(DateTime dateTime, TimeZoneInfo timeZone)
    {
        _dateTime = dateTime;
        _timeZone = timeZone;
    }

    /// <summary>
    ///     Gets the date and time value in universal time.
    /// </summary>
    public DateTime UniversalTime
    {
        get
        {
            _universalTime ??= TimeZoneInfo.ConvertTimeToUtc(_dateTime, _timeZone);
            return _universalTime.Value;
        }
    }

    /// <summary>
    ///     Gets the date and time value in the local time zone.
    /// </summary>
    public DateTime LocalTime
    {
        get
        {
            _localTime ??= TimeZoneInfo.ConvertTime(UniversalTime, TimeZoneInfo.Local, TimeZoneInfo.Local);
            return _localTime.Value;
        }
    }

    /// <summary>
    ///     Compares this <see cref="DateTimeWithTimeZone" /> value to another value.
    /// </summary>
    /// <param name="other">The other value to compare to.</param>
    /// <returns>
    ///     A value less than 0 if this value is earlier than the other value, 0 if the values are equal, or a value
    ///     greater than 0 if this value is later than the other value.
    /// </returns>
    public int CompareTo(DateTimeWithTimeZone other)
    {
        return UniversalTime.CompareTo(other.UniversalTime);
    }

    /// <summary>
    ///     Determines whether this <see cref="DateTimeWithTimeZone" /> value is equal to another value.
    /// </summary>
    /// <param name="other">The other value to compare to.</param>
    /// <returns>True if the values are equal, false otherwise.</returns>
    public bool Equals(DateTimeWithTimeZone other)
    {
        return UniversalTime == other.UniversalTime;
    }

    /// <summary>
    ///     Converts the date and time value to the specified time zone.
    /// </summary>
    /// <param name="destinationTimeZone">The destination time zone.</param>
    /// <returns>
    ///     A new <see cref="DateTimeWithTimeZone" /> value with the same date and time as this value, but in the
    ///     specified time zone.
    /// </returns>
    public DateTimeWithTimeZone WithTimeZone(TimeZoneInfo destinationTimeZone)
    {
        return new DateTimeWithTimeZone(TimeZoneInfo.ConvertTime(UniversalTime, destinationTimeZone),
            destinationTimeZone);
    }

    /// <summary>
    ///     Formats the date and time value as a string using the specified format string.
    /// </summary>
    /// <param name="format">
    ///     The format string. See the documentation for the <see cref="DateTime" /> struct for more
    ///     information.
    /// </param>
    /// <returns>A string representation of the date and time value.</returns>
    public string ToString(string format)
    {
        return _dateTime.ToString(format) + " " + _timeZone.Id;
    }

    /// <summary>
    ///     Parses a string representation of a <see cref="DateTimeWithTimeZone" /> value.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="format">
    ///     The format of the input string. See the documentation for the <see cref="DateTime" /> struct for
    ///     more information.
    /// </param>
    /// <param name="timeZone">The time zone of the resulting <see cref="DateTimeWithTimeZone" /> value.</param>
    /// <returns>A new <see cref="DateTimeWithTimeZone" /> value.</returns>
    public static DateTimeWithTimeZone Parse(string input, string format, TimeZoneInfo timeZone)
    {
        var dateTime = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
        return new DateTimeWithTimeZone(dateTime, timeZone);
    }

    /// <summary>
    ///     Converts the date and time value to a Unix timestamp.
    /// </summary>
    /// <returns>The Unix timestamp.</returns>
    public long ToUnixTimestamp()
    {
        return (long)(UniversalTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    /// <summary>
    ///     Converts a Unix timestamp to a <see cref="DateTimeWithTimeZone" /> value.
    /// </summary>
    /// <param name="timestamp">The Unix timestamp.</param>
    /// <param name="timeZone">The time zone of the resulting <see cref="DateTimeWithTimeZone" /> value.</param>
    /// <returns>A new <see cref="DateTimeWithTimeZone" /> value.</returns>
    public static DateTimeWithTimeZone FromUnixTimestamp(long timestamp, TimeZoneInfo timeZone)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
        return new DateTimeWithTimeZone(dateTime, timeZone);
    }

    /// <summary>
    ///     Returns the hash code for this <see cref="DateTimeWithTimeZone" /> value.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return UniversalTime.GetHashCode();
    }

    /// <summary>
    ///     Returns a string representation of this <see cref="DateTimeWithTimeZone" /> value.
    /// </summary>
    /// <returns>A string representation of the date and time value.</returns>
    public override string ToString()
    {
        return ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    ///     Determines whether one <see cref="DateTimeWithTimeZone" /> value is earlier than another value.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the first value is earlier than the second value, false otherwise.</returns>
    public static bool operator <(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    ///     Determines whether one <see cref="DateTimeWithTimeZone" /> value is later than another value.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the first value is later than the second value, false otherwise.</returns>
    public static bool operator >(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    ///     Determines whether one <see cref="DateTimeWithTimeZone" /> value is earlier than or equal to another value.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the first value is earlier than or equal to the second value, false otherwise.</returns>
    public static bool operator <=(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    ///     Determines whether one <see cref="DateTimeWithTimeZone" /> value is later than or equal to another value.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the first value is later than or equal to the second value, false otherwise.</returns>
    public static bool operator >=(DateTimeWithTimeZone left, DateTimeWithTimeZone right)
    {
        return left.CompareTo(right) >= 0;
    }
}