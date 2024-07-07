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
    ///     A <see cref="TimeOnly" /> object that is the result of adding the specified number of minutes to the original time.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the resulting <see cref="TimeOnly" /> is out of range.</exception>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly AddMinutes(this TimeOnly time, int minutesToAdd)
    {
        try
        {
            return time.AddMinutes(minutesToAdd);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentOutOfRangeException("The resulting TimeOnly is out of range.", ex);
        }
    }

    /// <summary>
    ///     Determines whether the specified <see cref="TimeOnly" /> instance represents the current system time to the minute.
    /// </summary>
    /// <param name="time">The time to check.</param>
    /// <returns><c>true</c> if the <paramref name="time" /> is within the current minute; otherwise, <c>false</c>.</returns>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCurrentTime(this TimeOnly time)
    {
        // Get the current time as TimeOnly
        var now = TimeOnly.FromDateTime(DateTime.Now);

        // Check if the specified time matches the current time to the minute
        return time.Hour == now.Hour && time.Minute == now.Minute;
    }

    /// <summary>
    ///     Calculates the difference in minutes between two <see cref="TimeOnly" /> instances.
    /// </summary>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <returns>The difference in minutes between the <paramref name="startTime" /> and <paramref name="endTime" />.</returns>
    /// <remarks>
    ///     This method calculates the absolute difference in minutes, considering only the time of day.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DifferenceInMinutes(this TimeOnly startTime, TimeOnly endTime)
    {
        // Calculate the difference in minutes between the two times
        return Math.Abs((int)(endTime - startTime).TotalMinutes);
    }

    /// <summary>
    ///     Checks if a <see cref="TimeOnly" /> is between two other <see cref="TimeOnly" /> values.
    /// </summary>
    /// <param name="current">The current time to check.</param>
    /// <param name="start">The start time.</param>
    /// <param name="end">The end time.</param>
    /// <param name="inclusive">If <c>true</c>, the comparison includes the start and end times; otherwise, it is exclusive.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="current" /> is between <paramref name="start" /> and <paramref name="end" />;
    ///     otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method compares only the time of day, not dates. It assumes the <paramref name="start" /> time is earlier than
    ///     or equal to the <paramref name="end" /> time within the same day.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this TimeOnly current, TimeOnly start, TimeOnly end, bool inclusive = true)
    {
        // Perform the comparison based on the inclusive flag
        return inclusive ? current >= start && current <= end : current > start && current < end;
    }

    /// <summary>
    ///     Formats a <see cref="TimeOnly" /> object according to a specified format.
    /// </summary>
    /// <param name="time">The <see cref="TimeOnly" /> to format.</param>
    /// <param name="format">The format string.</param>
    /// <returns>A string representing the formatted time.</returns>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    /// <remarks>
    ///     This method uses the standard .NET format strings for <see cref="TimeOnly" /> objects.
    ///     Refer to the official documentation for valid format specifiers.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStringFormatted(this TimeOnly time, string format)
    {
        try
        {
            return time.ToString(format);
        }
        catch (FormatException ex)
        {
            // Handle potential format exceptions and rethrow to indicate the error
            throw new FormatException($"The format string '{format}' is invalid.", ex);
        }
    }
}