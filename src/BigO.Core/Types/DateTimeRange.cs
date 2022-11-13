using BigO.Core.Extensions;
using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Value object defining a range of <see cref="DateTime" />.
/// </summary>
[PublicAPI]
public struct DateTimeRange : IEquatable<DateTimeRange>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateTimeRange" /> struct.
    /// </summary>
    /// <param name="startDateTime">The start date time.</param>
    /// <param name="endDateTime">The end date time.</param>
    /// <exception cref="System.ArgumentException">
    ///     The end time <paramref name="endDateTime" /> is less than or equal the start time
    ///     <paramref name="startDateTime" />.
    /// </exception>
    public DateTimeRange(DateTime startDateTime, DateTime endDateTime)
    {
        if (endDateTime <= startDateTime)
        {
            throw new ArgumentException(
                $"The end time '{endDateTime}' is less than or equal the start time '{startDateTime}'.");
        }

        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }

    /// <summary>
    ///     Gets the start date time.
    /// </summary>
    public DateTime StartDateTime { get; }

    /// <summary>
    ///     Gets the end date time.
    /// </summary>
    public DateTime EndDateTime { get; }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="DateTime" /> instance.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>
    ///     <c>true</c> if the date and time value falls within the range represented by this instance; otherwise,
    ///     <c>false</c>.
    /// </returns>
    public bool Contains(DateTime dateTime)
    {
        return dateTime.IsBetween(StartDateTime, EndDateTime);
    }

    /// <summary>
    ///     Checks if another <see cref="DateTimeRange" /> instance overlaps with this instance.
    /// </summary>
    /// <param name="dateTimeRange">The date time range.</param>
    /// <returns><c>true</c> if the provided date time range overlaps this instance, <c>false</c> otherwise.</returns>
    public bool Overlaps(DateTimeRange dateTimeRange)
    {
        return StartDateTime <= dateTimeRange.EndDateTime && dateTimeRange.StartDateTime <= EndDateTime;
    }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <c>false</c>.
    /// </returns>
    public bool Equals(DateTimeRange other)
    {
        return StartDateTime.Equals(other.StartDateTime) && EndDateTime.Equals(other.EndDateTime);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is DateTimeRange other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDateTime, EndDateTime);
    }

    public static bool operator ==(DateTimeRange left, DateTimeRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DateTimeRange left, DateTimeRange right)
    {
        return !left.Equals(right);
    }
}