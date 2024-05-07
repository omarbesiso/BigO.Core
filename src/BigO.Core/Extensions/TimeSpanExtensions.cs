namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of utility extension methods for the <see cref="TimeSpan" /> structure.
/// </summary>
[PublicAPI]
public static class TimeSpanExtensions
{
    /// <summary>
    ///     Rounds the TimeSpan to the nearest specified interval.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to round.</param>
    /// <param name="roundTo">The interval to round to.</param>
    /// <returns>A new TimeSpan rounded to the nearest interval.</returns>
    public static TimeSpan RoundToNearestInterval(this TimeSpan timeSpan, TimeSpan roundTo)
    {
        var ticks = (long)(Math.Round(timeSpan.Ticks / (double)roundTo.Ticks) * roundTo.Ticks);
        return new TimeSpan(ticks);
    }

    /// <summary>
    ///     Adds a specified number of business days to a TimeSpan, assuming a 5-day business week.
    /// </summary>
    /// <param name="timeSpan">The original TimeSpan.</param>
    /// <param name="businessDays">The number of business days to add.</param>
    /// <returns>A new TimeSpan increased by the specified number of business days.</returns>
    public static TimeSpan AddBusinessDays(this TimeSpan timeSpan, int businessDays)
    {
        const int daysPerWeek = 5;
        var additionalWeeks = businessDays / daysPerWeek;
        var extraDays = businessDays % daysPerWeek;

        var totalDaysToAdd = TimeSpan.FromDays(additionalWeeks * 7 + extraDays);
        return timeSpan.Add(totalDaysToAdd);
    }

    /// <summary>
    ///     Determines whether the TimeSpan is zero or negative.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to check.</param>
    /// <returns><c>true</c> if the TimeSpan is zero or negative; otherwise, <c>false</c>.</returns>
    public static bool IsZeroOrNegative(this TimeSpan timeSpan)
    {
        return timeSpan <= TimeSpan.Zero;
    }

    /// <summary>
    ///     Converts the TimeSpan to a readable string format, emphasizing the largest time unit.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to format.</param>
    /// <returns>A string representing the formatted TimeSpan.</returns>
    public static string ToReadableString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
        {
            return $"{timeSpan.Days} day{(timeSpan.Days == 1 ? "" : "s")}";
        }

        if (timeSpan.TotalHours >= 1)
        {
            return $"{timeSpan.Hours} hour{(timeSpan.Hours == 1 ? "" : "s")}";
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (timeSpan.TotalMinutes >= 1)
        {
            return $"{timeSpan.Minutes} minute{(timeSpan.Minutes == 1 ? "" : "s")}";
        }

        return $"{timeSpan.Seconds} second{(timeSpan.Seconds == 1 ? "" : "s")}";
    }

    /// <summary>
    ///     Negates the TimeSpan, returning a new TimeSpan that has the opposite duration.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to negate.</param>
    /// <returns>A new TimeSpan that is the negation of the original.</returns>
    public static TimeSpan Negate(this TimeSpan timeSpan)
    {
        return TimeSpan.FromTicks(-timeSpan.Ticks);
    }

    /// <summary>
    ///     Converts a TimeSpan to a TimeOnly instance, wrapping around if the duration exceeds 24 hours.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to convert.</param>
    /// <returns>A TimeOnly instance representing the time of day equivalent to the TimeSpan modulo 24 hours.</returns>
    public static TimeOnly ToTimeOnly(this TimeSpan timeSpan)
    {
        // Normalize the TimeSpan to ensure it is within a day's duration
        const long ticksInADay = TimeSpan.TicksPerDay;
        var normalizedTicks = timeSpan.Ticks % ticksInADay;
        if (normalizedTicks < 0)
        {
            // Ensure positive ticks by adding one day's worth of ticks if the result is negative
            normalizedTicks += ticksInADay;
        }

        return TimeOnly.FromTimeSpan(new TimeSpan(normalizedTicks));
    }

    /// <summary>
    ///     Converts a nullable TimeSpan to a nullable TimeOnly instance, wrapping around if the duration exceeds 24 hours.
    /// </summary>
    /// <param name="timeSpan">The nullable TimeSpan to convert.</param>
    /// <returns>
    ///     A nullable TimeOnly instance representing the time of day equivalent to the TimeSpan modulo 24 hours,
    ///     or null if the TimeSpan is null.
    /// </returns>
    public static TimeOnly? ToTimeOnly(this TimeSpan? timeSpan)
    {
        return timeSpan?.ToTimeOnly();
    }
}