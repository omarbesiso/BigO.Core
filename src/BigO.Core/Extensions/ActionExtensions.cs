using System.Diagnostics;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Action" /> objects.
/// </summary>
[PublicAPI]
public static class ActionExtensions
{
    /// <summary>
    ///     Runs the specified action asynchronously.
    /// </summary>
    /// <param name="action">The action to run asynchronously.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method creates a new task using the <see cref="Task.Factory" /> property and starts it with the specified
    ///     action.
    /// </remarks>
    public static async Task RunAsynchronously(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        await Task.Factory.StartNew(action);
    }

    /// <summary>
    ///     Executes the specified action and returns the elapsed time.
    /// </summary>
    /// <param name="action">The action to execute and time.</param>
    /// <returns>The elapsed time of the action.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses the <see cref="Stopwatch" /> class to measure the elapsed time of the specified action. The action
    ///     is invoked using the <see cref="Action.Invoke" /> method.
    /// </remarks>
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var startTime = Stopwatch.GetTimestamp();

        action.Invoke();

        return Stopwatch.GetElapsedTime(startTime);
    }
}