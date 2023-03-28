using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="DayOfWeek" /> enums.
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    ///     Returns the DayOfWeek value resulting from adding a specified number of days to the current DayOfWeek.
    /// </summary>
    /// <param name="dayOfWeek">The DayOfWeek value to increment.</param>
    /// <param name="numberOfDays">The number of days to add. The default value is 1.</param>
    /// <returns>
    ///     A DayOfWeek value that is <paramref name="numberOfDays" /> days after the <paramref name="dayOfWeek" />
    ///     parameter.
    /// </returns>
    /// <remarks>
    ///     This method supports incrementing the <paramref name="dayOfWeek" /> parameter by any number of days.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the Increment method to get the DayOfWeek value for two days after
    ///     Monday.
    ///     <code><![CDATA[
    /// DayOfWeek monday = DayOfWeek.Monday;
    /// DayOfWeek wednesday = monday.Increment(2);
    /// Console.WriteLine(wednesday); // Output: Wednesday
    /// ]]></code>
    /// </example>
    public static DayOfWeek Increment(this DayOfWeek dayOfWeek, int numberOfDays = 1)
    {
        var offset = (int)dayOfWeek + numberOfDays;
        var result = (DayOfWeek)((offset % 7 + 7) % 7);
        return result;
    }
}