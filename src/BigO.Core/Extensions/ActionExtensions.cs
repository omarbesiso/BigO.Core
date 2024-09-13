using System.Diagnostics;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Action" /> and <see cref="Func{TResult}" />
///     objects.
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
    /// <remarks>
    ///     This extension method is designed to execute a given <see cref="Action" /> and measure its execution time.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="action" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan ExecuteAndTime(this Action action)
    {
        Guard.NotNull(action);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        action.Invoke();
        return Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
#endif
    }

    /// <summary>
    ///     Executes the specified asynchronous function and measures the elapsed time it takes to complete.
    /// </summary>
    /// <param name="func">The asynchronous function to be executed.</param>
    /// <returns>
    ///     A <see cref="Task{TimeSpan}" /> representing the asynchronous operation, with the result being a
    ///     <see cref="TimeSpan" /> representing the elapsed time for the execution of the <paramref name="func" />.
    /// </returns>
    /// <remarks>
    ///     This extension method is designed to execute a given asynchronous function and measure its execution time.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TimeSpan> ExecuteAndTimeAsync(this Func<Task> func)
    {
        Guard.NotNull(func);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        await func();
        return Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            await func();
            stopwatch.Stop();
            return stopwatch.Elapsed;
#endif
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
    /// <remarks>
    ///     This extension method is designed to execute a given <see cref="Action" /> asynchronously and measure its execution
    ///     time.
    ///     The action is run on a separate thread using <see cref="Task.Run(Action, CancellationToken)" />.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="action" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TimeSpan> ExecuteAndTimeAsync(this Action action,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(action);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        await Task.Run(action, cancellationToken);
        return Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            await Task.Run(action, cancellationToken);
            stopwatch.Stop();
            return stopwatch.Elapsed;
#endif
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
    /// <remarks>
    ///     This extension method is designed to execute a given function and measure its execution time.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (TResult Result, TimeSpan ElapsedTime) ExecuteAndTime<TResult>(this Func<TResult> func)
    {
        Guard.NotNull(func);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        var result = func();
        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            var result = func();
            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
#endif
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
    /// <remarks>
    ///     This extension method is designed to execute a given asynchronous function and measure its execution time.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<(TResult Result, TimeSpan ElapsedTime)> ExecuteAndTimeAsync<TResult>(
        this Func<Task<TResult>> func)
    {
        Guard.NotNull(func);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        var result = await func();
        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            var result = await func();
            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
#endif
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
    /// <remarks>
    ///     This extension method is designed to execute a given asynchronous function and measure its execution time.
    ///     For .NET 7 and later, it uses <c>Stopwatch.GetElapsedTime(long)</c> for better performance.
    ///     For earlier versions, it falls back to <see cref="Stopwatch.StartNew()" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="func" /> parameter is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<(TResult Result, TimeSpan ElapsedTime)> ExecuteAndTimeAsync<TResult>(
        this Func<ValueTask<TResult>> func)
    {
        Guard.NotNull(func);

#if NET7_0_OR_GREATER
        var startTime = Stopwatch.GetTimestamp();
        var result = await func();
        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
#else
            var stopwatch = Stopwatch.StartNew();
            var result = await func();
            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
#endif
        return (result, elapsedTime);
    }
}