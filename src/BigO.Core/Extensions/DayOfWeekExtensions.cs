using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="DayOfWeek" /> objects.
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    ///     Increments a given <see cref="DayOfWeek" /> value by a specified number of days.
    /// </summary>
    /// <param name="dayOfWeek">The <see cref="DayOfWeek" /> value to increment.</param>
    /// <param name="numberOfDays">The number of days to increment the <see cref="DayOfWeek" /> value by. Can be negative.</param>
    /// <returns>The incremented <see cref="DayOfWeek" /> value.</returns>
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        return output;
    }
}