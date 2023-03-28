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
    ///     Executes the specified <see cref="Action" /> asynchronously using a <see cref="Task" />.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
    /// <remarks>
    ///     This extension method is designed to run a given <see cref="Action" /> asynchronously using aggressive inlining,
    ///     which means that the method is optimized for performance.
    ///     Keep in mind that this method is most suitable for use cases where you need to run small or simple actions
    ///     asynchronously. For more complex scenarios, consider using <see cref="Task.Run(Action)" /> directly, or create
    ///     a dedicated <see cref="Task" />-based method.
    /// </remarks>
    /// <example>
    ///     Here's an example of how to use this extension method:
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// using System.Threading.Tasks;
    /// 
    /// public class Program
    /// {
    ///     public static async Task Main()
    ///     {
    ///         Action printHelloWorld = () => Console.WriteLine("Hello, World!");
    ///         await printHelloWorld.RunAsynchronously();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="action" /> parameter is <c>null</c>.</exception>
    /// <exception cref="TaskSchedulerException">
    ///     Thrown when the <see cref="Task" /> is not able to be queued to the default
    ///     scheduler. This typically occurs when a <see cref="TaskScheduler" /> is unable to queue a <see cref="Task" />.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the <see cref="Task" /> has been disposed or the
    ///     <see cref="TaskScheduler" /> is unavailable.
    /// </exception>
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
    ///     Executes the specified <see cref="Action" /> and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed.</param>
    /// <returns>A <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="action" />.</returns>
    /// <remarks>
    ///     This extension method is designed to execute a given <see cref="Action" /> and measure its execution time using
    ///     aggressive inlining,
    ///     which means that the method is optimized for performance.
    ///     Keep in mind that this method is most suitable for use cases where you need to measure the time it takes to execute
    ///     small or simple actions.
    ///     For more complex scenarios, consider using the <see cref="Stopwatch" /> class directly.
    /// </remarks>
    /// <example>
    ///     Here's an example of how to use this extension method:
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// using System.Diagnostics;
    /// 
    /// public class Program
    /// {
    ///     public static void Main()
    ///     {
    ///         Action printHelloWorld = () => Console.WriteLine("Hello, World!");
    ///         TimeSpan elapsedTime = printHelloWorld.ExecuteAndTime();
    ///         Console.WriteLine($"Elapsed time: {elapsedTime.TotalMilliseconds} ms");
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="action" /> parameter is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <see cref="Stopwatch" /> methods are called in an incorrect
    ///     order, such as calling <see cref="Stopwatch.GetElapsedTime(long)" /> before <see cref="Stopwatch.GetTimestamp" />.
    /// </exception>
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