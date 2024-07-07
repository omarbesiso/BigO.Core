using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DayOfWeek" /> enums.
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    ///     Adds a specified number of days to the current DayOfWeek value.
    /// </summary>
    /// <param name="dayOfWeek">The DayOfWeek value to add days to.</param>
    /// <param name="numberOfDays">The number of days to add. Can be negative to subtract days.</param>
    /// <returns>
    ///     A DayOfWeek value that is <paramref name="numberOfDays" /> days from the <paramref name="dayOfWeek" /> parameter.
    /// </returns>
    /// <remarks>
    ///     This method allows the addition of a positive or negative number of days to a DayOfWeek value.
    ///     Be cautious with extreme values for <paramref name="numberOfDays" />, as they may lead to integer overflow.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the AddDays method:
    ///     <code><![CDATA[
    /// DayOfWeek monday = DayOfWeek.Monday;
    /// DayOfWeek wednesday = monday.AddDays(2); // Adding 2 days to Monday
    /// DayOfWeek sunday = monday.AddDays(-1); // Subtracting 1 day from Monday
    /// Console.WriteLine(wednesday); // Output: Wednesday
    /// Console.WriteLine(sunday); // Output: Sunday
    /// ]]></code>
    /// </example>
    public static DayOfWeek AddDays(this DayOfWeek dayOfWeek, int numberOfDays = 1)
    {
        var offset = (int)dayOfWeek + numberOfDays;
        return (DayOfWeek)(offset % 7);
    }

    /// <summary>
    ///     Gets the next specified number of days of the week starting from the specified day.
    /// </summary>
    /// <param name="startDay">The day of the week to start from.</param>
    /// <param name="count">The number of days to generate. Defaults to 7.</param>
    /// <returns>An <see cref="IEnumerable{DayOfWeek}" /> representing the next specified number of days of the week.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if count is less than or equal to 0.</exception>
    public static IEnumerable<DayOfWeek> GetNextDays(this DayOfWeek startDay, int count = 7)
    {
        Guard.Minimum(count, 1);
        return Enumerable.Range(0, count).Select(i => (DayOfWeek)(((int)startDay + i) % 7));
    }
}