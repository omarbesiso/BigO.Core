using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    /// <param name="action">The action to run asynchronously. Cannot be <c>null</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method takes an <see cref="Action" /> delegate as an input parameter and runs it asynchronously using the
    ///     <see cref="Task.Run(System.Action)" /> method.
    ///     The <see cref="Task.Run(System.Action)" /> method is a convenient way to start a new task and run it
    ///     asynchronously. It returns a
    ///     <see cref="Task" /> object that represents the asynchronous operation, which can be used to track the progress of
    ///     the operation and wait for it to complete if necessary.
    ///     By using the <c>await</c> keyword, the <c>RunAsynchronously</c> method ensures that the calling code can run
    ///     asynchronously without being blocked. This can be useful for improving the performance and responsiveness of a
    ///     program by allowing it to perform other tasks while waiting for the action to complete.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task RunAsynchronously(this Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), $"The {nameof(action)} cannot be null.");
        }

        await Task.Run(action);
    }

    /// <summary>
    ///     Executes the specified action and returns the elapsed time.
    /// </summary>
    /// <param name="action">The action to execute. Cannot be <c>null</c>.</param>
    /// <returns>The elapsed time in <see cref="TimeSpan" /> format.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method takes an <see cref="Action" /> delegate as an input parameter and executes it using the
    ///     <see cref="Action.Invoke" /> method.
    ///     It uses the <see cref="Stopwatch" /> class to measure the elapsed time between the start and end of the action. The
    ///     start time is recorded using the <see cref="Stopwatch.GetTimestamp" /> method, and the elapsed time is calculated
    ///     using the <see cref="Stopwatch.GetElapsedTime(long)" /> method, which takes the start time as an input parameter.
    ///     The elapsed time is returned as a <see cref="TimeSpan" /> object, which represents a time interval.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), $"The {nameof(action)} cannot be null.");
        }

        var startTime = Stopwatch.GetTimestamp();

        action.Invoke();

        return Stopwatch.GetElapsedTime(startTime);
    }
}