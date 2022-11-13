using BigO.Core.Extensions;
using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Value object defining a range of <see cref="DateOnly" />.
/// </summary>
[PublicAPI]
public struct DateRange : IEquatable<DateRange>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DateRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <exception cref="System.ArgumentException">
    ///     The end time <paramref name="startDate" /> is less than or equal the start time
    ///     <paramref name="endDate" />.
    /// </exception>
    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException(
                $"The end date '{endDate}' is less than or equal the start date '{startDate}'.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    ///     Gets the start date.
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    ///     Gets the end date.
    /// </summary>
    public DateOnly EndDate { get; }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <c>false</c>.
    /// </returns>
    public bool Equals(DateRange other)
    {
        return StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is DateRange other && Equals(other);
    }

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    public static bool operator ==(DateRange left, DateRange right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DateRange left, DateRange right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    ///     Determines whether this range contains the specified <see cref="DateOnly" /> instance.
    /// </summary>
    /// <param name="date">The date to be checked.</param>
    /// <returns><c>true</c> if the date value falls within the range represented by this instance; otherwise, <c>false</c>.</returns>
    public bool Contains(DateOnly date)
    {
        return date.IsBetween(StartDate, EndDate);
    }

    /// <summary>
    ///     Checks if another <see cref="DateRange" /> instance overlaps with this instance.
    /// </summary>
    /// <param name="dateRange">The date range.</param>
    /// <returns><c>true</c> if the provided date range overlaps this instance, <c>false</c> otherwise.</returns>
    public bool Overlaps(DateRange dateRange)
    {
        return StartDate <= dateRange.EndDate && dateRange.StartDate <= EndDate;
    }
}