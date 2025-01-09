namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given <paramref name="value" /> satisfies the specified <paramref name="predicate" />.
    ///     If it does not satisfy the predicate, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="predicate">A predicate that the value must satisfy.</param>
    /// <param name="paramName">
    ///     The name of the parameter being checked, used in the exception message for clarity.
    ///     Automatically provided via <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value does not satisfy the predicate.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The <paramref name="value" /> if it satisfies the predicate.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the value does not satisfy the predicate.
    /// </exception>
    /// <remarks>
    ///     Useful for validating that a value meets a custom condition defined by the predicate.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Requires(myValue, x => x > 0, nameof(myValue), "Value must be greater than 0");
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Requires<T>(
        T value,
        Predicate<T> predicate,
        [CallerArgumentExpression(nameof(value))]
        string paramName = "",
        string? exceptionMessage = null)
    {
        // Ensure the predicate is not null
        NotNull(predicate, nameof(predicate));

        // Check if the value satisfies the predicate
        if (predicate(value))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{paramName}' does not meet the required condition."
            : exceptionMessage;

        ThrowHelper.ThrowArgumentException(paramName, errorMessage);

        return value;
    }
}