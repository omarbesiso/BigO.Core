using System.Diagnostics;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Action" /> objects.
/// </summary>
[PublicAPI]
public static class ActionExtensions
{
    /// <summary>
    ///     Executes the specified <see cref="Action" /> asynchronously using a <see cref="Task" />, and supports cancellation.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
    ///     The default is <see cref="CancellationToken.None" />.
    /// </param>
    /// <remarks>
    ///     This extension method is designed to run a given <see cref="Action" /> asynchronously with support for
    ///     cancellation.
    ///     Keep in mind that this method is most suitable for use cases where you need to run small or simple actions
    ///     asynchronously. For more complex scenarios, consider using <see cref="Task.Run(Action, CancellationToken)" />
    ///     directly, or create
    ///     a dedicated <see cref="Task" />-based method.
    /// </remarks>
    /// <example>
    ///     Here's an example of how to use this extension method:
    ///     <code>
    /// <![CDATA[
    /// using System;
    /// using System.Threading;
    /// using System.Threading.Tasks;
    /// 
    /// public class Program
    /// {
    ///     public static async Task Main()
    ///     {
    ///         var cancellationTokenSource = new CancellationTokenSource();
    ///         Action printHelloWorld = () => Console.WriteLine("Hello, World!");
    ///         
    ///         // Cancel the task after 1 second
    ///         cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));
    ///         
    ///         try
    ///         {
    ///             await printHelloWorld.RunAsynchronously(cancellationTokenSource.Token);
    ///         }
    ///         catch (OperationCanceledException)
    ///         {
    ///             Console.WriteLine("The operation was canceled.");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="action" /> parameter is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task RunAsynchronously(this Action action, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(action);
        await Task.Run(action, cancellationToken);
    }

    /// <summary>
    ///     Executes the specified <see cref="Action" /> and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed.</param>
    /// <returns>A <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="action" />.</returns>
    /// <remarks>
    ///     This extension method is designed to execute a given <see cref="Action" /> and measure its execution time.
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        Guard.NotNull(action);

        var startTime = Stopwatch.GetTimestamp();

        action.Invoke();

        return Stopwatch.GetElapsedTime(startTime);
    }

    /// <summary>
    ///     Executes the specified <see cref="Action" /> asynchronously, and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
    ///     The default is <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TimeSpan}" /> representing the elapsed time for the execution of the
    ///     <paramref name="action" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="action" /> parameter is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TimeSpan> ExecuteAndTimeAsync(this Action action,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(action);

        var startTime = Stopwatch.GetTimestamp();

        await Task.Run(action, cancellationToken);

        return Stopwatch.GetElapsedTime(startTime);
    }
}