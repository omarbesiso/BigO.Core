using System.Diagnostics;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for measuring the execution time of actions and functions.
/// </summary>
[PublicAPI]
public static class ActionExtensions
{
    /// <summary>
    ///     Executes the specified <see cref="Action" /> and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed.</param>
    /// <returns>
    ///     A <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="action" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="action" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        Guard.NotNull(action);

        var startTime = GetTimestamp();
        action.Invoke();
        return GetElapsedTime(startTime);
    }

    /// <summary>
    ///     Executes the specified asynchronous function and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="func">The asynchronous function to be executed.</param>
    /// <returns>
    ///     A <see cref="Task{TimeSpan}" /> representing the asynchronous operation, with the result being a
    ///     <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="func" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TimeSpan> ExecuteAndTimeAsync(this Func<Task> func)
    {
        Guard.NotNull(func);

        var startTime = GetTimestamp();
        await func().ConfigureAwait(false);
        return GetElapsedTime(startTime);
    }

    /// <summary>
    ///     Executes the specified <see cref="Action" /> asynchronously and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TimeSpan}" /> representing the asynchronous operation, with the result being a
    ///     <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="action" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="action" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TimeSpan> ExecuteAndTimeAsync(this Action action,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(action);

        var startTime = GetTimestamp();
        await Task.Run(action, cancellationToken).ConfigureAwait(false);
        return GetElapsedTime(startTime);
    }

    /// <summary>
    ///     Executes the specified <see cref="Func{TResult}" /> and measures the elapsed time it takes to complete.
    /// </summary>
    /// <typeparam name="TResult">The return type of the function.</typeparam>
    /// <param name="func">The function to be executed.</param>
    /// <returns>
    ///     A tuple containing the result of the function and a <see cref="TimeSpan" /> representing the elapsed time for the
    ///     execution.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TResult Result, TimeSpan ElapsedTime) ExecuteAndTime<TResult>(this Func<TResult> func)
    {
        Guard.NotNull(func);

        var startTime = GetTimestamp();
        var result = func();
        var elapsedTime = GetElapsedTime(startTime);
        return (result, elapsedTime);
    }

    /// <summary>
    ///     Executes the specified asynchronous function and measures the elapsed time it takes to complete.
    /// </summary>
    /// <typeparam name="TResult">The return type of the asynchronous function.</typeparam>
    /// <param name="func">The asynchronous function to be executed.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation, with the result being a tuple containing the result
    ///     of the function and a <see cref="TimeSpan" /> representing the elapsed time for the execution.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<(TResult Result, TimeSpan ElapsedTime)> ExecuteAndTimeAsync<TResult>(
        this Func<Task<TResult>> func)
    {
        Guard.NotNull(func);

        var startTime = GetTimestamp();
        var result = await func().ConfigureAwait(false);
        var elapsedTime = GetElapsedTime(startTime);
        return (result, elapsedTime);
    }

    /// <summary>
    ///     Executes the specified asynchronous function returning a <see cref="ValueTask{TResult}" /> and measures the elapsed
    ///     time it takes to complete.
    /// </summary>
    /// <typeparam name="TResult">The return type of the asynchronous function.</typeparam>
    /// <param name="func">The asynchronous function to be executed.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation, with the result being a tuple containing the result
    ///     of the function and a <see cref="TimeSpan" /> representing the elapsed time for the execution.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<(TResult Result, TimeSpan ElapsedTime)> ExecuteAndTimeAsync<TResult>(
        this Func<ValueTask<TResult>> func)
    {
        Guard.NotNull(func);

        var startTime = GetTimestamp();
        var result = await func().ConfigureAwait(false);
        var elapsedTime = GetElapsedTime(startTime);
        return (result, elapsedTime);
    }

    // Private helper methods to abstract version-specific implementations

    /// <summary>
    ///     Gets the current timestamp.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GetTimestamp()
    {
        return Stopwatch.GetTimestamp();
    }

    /// <summary>
    ///     Calculates the elapsed time from the given start timestamp.
    /// </summary>
    /// <param name="startTimestamp">The start timestamp.</param>
    /// <returns>The elapsed <see cref="TimeSpan" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TimeSpan GetElapsedTime(long startTimestamp)
    {
#if NET7_0_OR_GREATER
        return Stopwatch.GetElapsedTime(startTimestamp);
#else
        var endTimestamp = Stopwatch.GetTimestamp();
        var elapsedTicks = endTimestamp - startTimestamp;
        var elapsedTime = TimeSpan.FromSeconds((double)elapsedTicks / Stopwatch.Frequency);
        return elapsedTime;
#endif
    }
}