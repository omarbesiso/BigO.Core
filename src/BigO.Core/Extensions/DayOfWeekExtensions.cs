using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DayOfWeek" /> enums.
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    ///     Adds a specified number of days to the current <see cref="DayOfWeek" /> value, cycling through the days of the
    ///     week.
    /// </summary>
    /// <param name="dayOfWeek">The <see cref="DayOfWeek" /> value to add days to.</param>
    /// <param name="numberOfDays">
    ///     The number of days to add. Can be negative to subtract days. The result will wrap around if it exceeds
    ///     the boundaries of the <see cref="DayOfWeek" /> enum (Sunday to Saturday).
    /// </param>
    /// <returns>
    ///     A <see cref="DayOfWeek" /> value that is <paramref name="numberOfDays" /> days from the
    ///     <paramref name="dayOfWeek" />
    ///     parameter.
    /// </returns>
    /// <remarks>
    ///     This method allows the addition or subtraction of days from a <see cref="DayOfWeek" /> value.
    ///     Negative values of <paramref name="numberOfDays" /> will subtract days, and positive values will add days.
    ///     The result is always normalized within the range of the <see cref="DayOfWeek" /> enum (0 = Sunday to 6 = Saturday).
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     DayOfWeek monday = DayOfWeek.Monday;
    ///     DayOfWeek wednesday = monday.AddDays(2); // Adding 2 days to Monday
    ///     DayOfWeek sunday = monday.AddDays(-1);   // Subtracting 1 day from Monday
    ///     Console.WriteLine(wednesday);            // Output: Wednesday
    ///     Console.WriteLine(sunday);               // Output: Sunday
    ///     ]]></code>
    /// </example>
    public static DayOfWeek AddDays(this DayOfWeek dayOfWeek, int numberOfDays = 1)
    {
        var offset = ((int)dayOfWeek + numberOfDays) % 7;
        return (DayOfWeek)((offset + 7) % 7); // Handles negative values correctly
    }

    /// <summary>
    ///     Generates a sequence of the next specified number of days of the week, starting from the provided
    ///     <paramref name="startDay" />.
    /// </summary>
    /// <param name="startDay">
    ///     The <see cref="DayOfWeek" /> value to start from. The sequence begins with this day.
    /// </param>
    /// <param name="count">
    ///     The number of days to generate, starting from <paramref name="startDay" />.
    ///     Defaults to 7, representing a full week. Must be 1 or greater.
    /// </param>
    /// <returns>
    ///     An <see cref="IEnumerable{DayOfWeek}" /> representing the next specified number of days of the week,
    ///     cycling through the week as needed (Sunday to Saturday).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="count" /> is less than or equal to 0.
    /// </exception>
    /// <remarks>
    ///     This method returns a sequence of <see cref="DayOfWeek" /> values, beginning with <paramref name="startDay" /> and
    ///     including the next <paramref name="count" /> days, wrapping around if necessary (e.g., after Saturday, it returns
    ///     Sunday).
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     // Generates the next 3 days starting from Monday.
    ///     IEnumerable<DayOfWeek> days = DayOfWeek.Monday.GetNextDays(3);
    ///     // Result: Monday, Tuesday, Wednesday
    /// 
    ///     // Generates a full week starting from Thursday.
    ///     IEnumerable<DayOfWeek> week = DayOfWeek.Thursday.GetNextDays();
    ///     // Result: Thursday, Friday, Saturday, Sunday, Monday, Tuesday, Wednesday
    ///     ]]></code>
    /// </example>
    public static IEnumerable<DayOfWeek> GetNextDays(this DayOfWeek startDay, int count = 7)
    {
        Guard.Minimum(count, 1);
        return Enumerable.Range(0, count).Select(i => (DayOfWeek)(((int)startDay + i) % 7));
    }
}