namespace BigO.Core.Types;

/// <summary>
///     Represents the result of a time range difference operation.
/// </summary>
public struct TimeRangeDiffResult
{
    /// <summary>
    ///     Gets the remaining time range when the other time range overlaps with the start or end of the current instance.
    /// </summary>
    public TimeRange? RemainingTimeRange { get; }

    /// <summary>
    ///     Gets the remaining time range before the other time range when the current instance contains the other time range.
    /// </summary>
    public TimeRange? RemainingBeforeRange { get; }

    /// <summary>
    ///     Gets the remaining time range after the other time range when the current instance contains the other time range.
    /// </summary>
    public TimeRange? RemainingAfterRange { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TimeRangeDiffResult" /> struct.
    /// </summary>
    /// <param name="remainingTimeRange">
    ///     The remaining time range when the other time range overlaps with the start or end of
    ///     the current instance.
    /// </param>
    /// <param name="remainingBeforeRange">
    ///     The remaining time range before the other time range when the current instance
    ///     contains the other time range.
    /// </param>
    /// <param name="remainingAfterRange">
    ///     The remaining time range after the other time range when the current instance
    ///     contains the other time range.
    /// </param>
    internal TimeRangeDiffResult(TimeRange? remainingTimeRange = null, TimeRange? remainingBeforeRange = null,
        TimeRange? remainingAfterRange = null)
    {
        RemainingTimeRange = remainingTimeRange;
        RemainingBeforeRange = remainingBeforeRange;
        RemainingAfterRange = remainingAfterRange;
    }
}