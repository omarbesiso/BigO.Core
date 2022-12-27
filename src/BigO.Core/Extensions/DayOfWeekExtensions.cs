using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DayOfWeek" /> enums.
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    ///     Increments a given <see cref="DayOfWeek" /> by a specified number of days.
    /// </summary>
    /// <param name="dayOfWeek">The <see cref="DayOfWeek" /> to increment.</param>
    /// <param name="numberOfDays">The number of days to increment the <see cref="DayOfWeek" /> by. Default is 1.</param>
    /// <returns>The incremented <see cref="DayOfWeek" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     The <paramref name="dayOfWeek" /> parameter is not a valid
    ///     <see cref="DayOfWeek" /> value.
    /// </exception>
    /// <remarks>
    ///     This method increments the given <see cref="DayOfWeek" /> by the specified number of days. If the resulting
    ///     <see cref="DayOfWeek" /> is greater than Saturday, it wraps around to Sunday.
    ///     If the <paramref name="numberOfDays" /> is negative, the <see cref="DayOfWeek" /> is decremented instead.
    /// </remarks>
    public static DayOfWeek Increment(this DayOfWeek dayOfWeek, int numberOfDays = 1)
    {
        var output = dayOfWeek;

        for (var i = 0; i < numberOfDays; i++)
        {
            switch (output)
            {
                case DayOfWeek.Sunday:
                    output = DayOfWeek.Monday;
                    break;
                case DayOfWeek.Monday:
                    output = DayOfWeek.Tuesday;
                    break;
                case DayOfWeek.Tuesday:
                    output = DayOfWeek.Wednesday;
                    break;
                case DayOfWeek.Wednesday:
                    output = DayOfWeek.Thursday;
                    break;
                case DayOfWeek.Thursday:
                    output = DayOfWeek.Friday;
                    break;
                case DayOfWeek.Friday:
                    output = DayOfWeek.Saturday;
                    break;
                case DayOfWeek.Saturday:
                    output = DayOfWeek.Sunday;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }
        }

        return output;
    }
}