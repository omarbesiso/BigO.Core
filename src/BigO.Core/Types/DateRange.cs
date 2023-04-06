using JetBrains.Annotations;

namespace BigO.Core.Types;

/// <summary>
///     Represents a range of dates.
/// </summary>
[PublicAPI]
public readonly record struct DateRange 
{
    /// <summary>
    ///     Gets the start date of the date range.
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    ///     Gets the end date of the date range.
    /// </summary>
    public DateOnly EndDate { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DateRange" /> struct.
    /// </summary>
    /// <param name="startDate">The start date of the date range.</param>
    /// <param name="endDate">The end date of the date range.</param>
    /// <exception cref="ArgumentException">Thrown if the end date is before the start date.</exception>
    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date cannot be before start date.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    ///     Determines whether the current instance and another specified <see cref="DateRange" /> object have the same value.
    /// </summary>
    /// <param name="other">The <see cref="DateRange" /> to compare to this instance.</param>
    /// <returns>
    ///     <c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance;
    ///     otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(DateRange other)
    {
        return StartDate == other.StartDate && EndDate == other.EndDate;
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    /// <summary>
    ///     Determines if the current date range overlaps with another specified date range.
    /// </summary>
    /// <param name="other">The other date range to compare to the current instance.</param>
    /// <returns><c>true</c> if the date ranges overlap; otherwise, <c>false</c>.</returns>
    public bool Overlaps(DateRange other)
    {
        // Check if the start date of the current range is before or equal to the end date of the other range,
        // and the end date of the current range is after or equal to the start date of the other range.
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    /// <summary>
    ///     Determines if the specified date is included in the current date range.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the date is within the range; <c>false</c> otherwise.</returns>
    public bool Contains(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }

    /// <summary>
    ///     Gets the duration of the date range in days.
    /// </summary>
    /// <returns>The number of days between the start and end dates of the date range.</returns>
    public int Duration()
    {
        return (EndDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days;
    }

    /// <summary>
    ///     Gets the intersection of the current date range and another specified date range.
    /// </summary>
    /// <param name="other">The other date range to intersect with the current instance.</param>
    /// <returns>A new <see cref="DateRange" /> instance representing the intersection of the two date ranges.</returns>
    public DateRange Intersection(DateRange other)
    {
        var startDate = StartDate > other.StartDate ? StartDate : other.StartDate;
        var endDate = EndDate < other.EndDate ? EndDate : other.EndDate;
        return new DateRange(startDate, endDate);
    }

    /// <summary>
    ///     Gets the union of the current date range and another specified date range.
    /// </summary>
    /// <param name="other">The other date range to union with the current instance.</param>
    /// <returns>A new <see cref="DateRange" /> instance representing the union of the two date ranges.</returns>
    public DateRange Union(DateRange other)
    {
        var startDate = StartDate < other.StartDate ? StartDate : other.StartDate;
        var endDate = EndDate > other.EndDate ? EndDate : other.EndDate;
        return new DateRange(startDate, endDate);
    }

    /// <summary>
    ///     Tries to parse a string representation of a date range into a <see cref="DateRange" /> instance.
    /// </summary>
    /// <param name="input">The string to parse. The string should be in the format "MMM dd, yyyy - MMM dd, yyyy".</param>
    /// <param name="range">The resulting <see cref="DateRange" /> instance if the parse is successful.</param>
    /// <returns><c>true</c> if the parse is successful; <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="input" /> is <c>null</c>.
    /// </exception>
    /// <example>
    ///     <code>
    /// string input = "Jan 1, 2020 - Dec 31, 2020";
    /// if (DateRange.TryParse(input, out DateRange range))
    /// {
    ///     Console.WriteLine($"Start date: {range.StartDate:MMM dd, yyyy}");
    ///     Console.WriteLine($"End date: {range.EndDate:MMM dd, yyyy}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Failed to parse date range.");
    /// }
    /// </code>
    /// </example>
    public static bool TryParse(string input, out DateRange range)
    {
        range = new DateRange();
        var parts = input.Split(" - ");
        if (parts.Length != 2)
        {
            return false;
        }

        if (!DateOnly.TryParse(parts[0], out var startDate))
        {
            return false;
        }

        if (!DateOnly.TryParse(parts[1], out var endDate))
        {
            return false;
        }

        range = new DateRange(startDate, endDate);
        return true;
    }

    /// <summary>
    ///     Returns a string representation of the date range.
    /// </summary>
    /// <returns>A string in the format "MMM dd, yyyy - MMM dd, yyyy" representing the start and end dates of the date range.</returns>
    public override string ToString()
    {
        return $"{StartDate:MMM dd, yyyy} - {EndDate:MMM dd, yyyy}";
    }

    /// <summary>
    ///     Gets an enumerable collection of all the days in the date range.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="DateOnly" /> instances representing all the days in the date range.</returns>
    public IEnumerable<DateOnly> DaysInRange()
    {
        for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
        {
            yield return date;
        }
    }
}