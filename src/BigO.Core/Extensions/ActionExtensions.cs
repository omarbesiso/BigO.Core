using System.Diagnostics;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Action" /> objects.
/// </summary>
[PublicAPI]
public static class ActionExtensions
{
    /// <summary>
    ///     Runs the specified <see cref="Action" /> asynchronously in a background thread.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
    /// <returns>The started <see cref="Task" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action" /> is <c>null</c>.</exception>
    public static async Task RunAsynchronously(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        await Task.Factory.StartNew(action);
    }

    /// <summary>
    ///     Executes the specified <see cref="Action" /> and measures the time of execution.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed and measured.</param>
    /// <returns>A <see cref="TimeSpan" /> object representing the time elapsed time for execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action" /> is <c>null</c>. </exception>
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var stopwatch = Stopwatch.StartNew();

        action.Invoke();

        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}