namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of utility extension methods for the <see cref="TimeSpan" /> structure.
/// </summary>
[PublicAPI]
public static class TimeSpanExtensions
{
    /// <summary>
    ///     Converts a <see cref="TimeSpan" /> to a <see cref="TimeOnly" /> instance, wrapping around if the duration exceeds
    ///     24 hours.
    /// </summary>
    /// <param name="timeSpan">The <see cref="TimeSpan" /> to convert.</param>
    /// <returns>
    ///     A <see cref="TimeOnly" /> instance representing the time of day equivalent to the <paramref name="timeSpan" />
    ///     modulo 24 hours.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="timeSpan" /> is out of range.</exception>
    /// <remarks>
    ///     This method normalizes the <see cref="TimeSpan" /> to ensure it is within a day's duration.
    ///     If the <paramref name="timeSpan" /> is negative, it will wrap around to a positive time of day.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly ToTimeOnly(this TimeSpan timeSpan)
    {
        const long ticksInADay = TimeSpan.TicksPerDay;

        // Normalize the ticks within a 24-hour period
        var normalizedTicks = timeSpan.Ticks % ticksInADay;
        if (normalizedTicks < 0)
        {
            normalizedTicks += ticksInADay;
        }

        return TimeOnly.FromTimeSpan(new TimeSpan(normalizedTicks));
    }

    /// <summary>
    ///     Converts a nullable <see cref="TimeSpan" /> to a nullable <see cref="TimeOnly" /> instance,
    ///     wrapping around if the duration exceeds 24 hours.
    /// </summary>
    /// <param name="timeSpan">The nullable <see cref="TimeSpan" /> to convert.</param>
    /// <returns>
    ///     A nullable <see cref="TimeOnly" /> instance representing the time of day equivalent to the
    ///     <paramref name="timeSpan" /> modulo 24 hours, or <c>null</c> if the <paramref name="timeSpan" /> is <c>null</c>.
    /// </returns>
    public static TimeOnly? ToTimeOnly(this TimeSpan? timeSpan)
    {
        return timeSpan?.ToTimeOnly();
    }
}