namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="TimeOnly" /> objects.
/// </summary>
[PublicAPI]
public static class TimeOnlyExtensions
{
    /// <summary>
    ///     Adds a specified number of minutes to a <see cref="TimeOnly" /> object.
    /// </summary>
    /// <param name="time">The <see cref="TimeOnly" /> object to which minutes will be added.</param>
    /// <param name="minutesToAdd">The number of minutes to add.</param>
    /// <returns>
    ///     A <see cref="TimeOnly" /> object that is the result of adding the specified number of minutes to the original
    ///     time.
    /// </returns>
    public static TimeOnly AddMinutes(this TimeOnly time, int minutesToAdd)
    {
        return time.AddMinutes(minutesToAdd);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="TimeOnly" /> instance represents the current system time.
    /// </summary>
    /// <param name="time">The time to check.</param>
    /// <returns><c>true</c> if the time is within the current minute; otherwise, <c>false</c>.</returns>
    public static bool IsCurrentTime(this TimeOnly time)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        return time == now;
    }

    /// <summary>
    ///     Calculates the difference in minutes between two <see cref="TimeOnly" /> instances.
    /// </summary>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>The difference in minutes between the two times.</returns>
    public static int DifferenceInMinutes(this TimeOnly startTime, TimeOnly endTime)
    {
        return (int)(endTime - startTime).TotalMinutes;
    }

    /// <summary>
    ///     Checks if a <see cref="TimeOnly" /> is between two other <see cref="TimeOnly" /> values, inclusive by default.
    /// </summary>
    /// <param name="current">The current time to check.</param>
    /// <param name="start">The start time.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusive">Whether the comparison is inclusive or exclusive of the end points.</param>
    /// <returns><c>true</c> if the current time is between the start and end times; otherwise, <c>false</c>.</returns>
    public static bool IsBetween(this TimeOnly current, TimeOnly start, TimeOnly end, bool inclusive = true)
    {
        return inclusive ? current >= start && current <= end : current > start && current < end;
    }

    /// <summary>
    ///     Formats a <see cref="TimeOnly" /> object according to a specified format.
    /// </summary>
    /// <param name="time">The <see cref="TimeOnly" /> to format.</param>
    /// <param name="format">The format string.</param>
    /// <returns>A string representing the formatted time.</returns>
    public static string ToStringFormatted(this TimeOnly time, string format)
    {
        return time.ToString(format);
    }
}