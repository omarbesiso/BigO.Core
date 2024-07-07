// ReSharper disable InvertIf

namespace BigO.Core.Validation;

public partial class Guard
{
    /// <summary>
    ///     Ensures that the given value satisfies a specified predicate.
    ///     If the value does not satisfy the predicate, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="predicate">The predicate that the value must satisfy.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value does not satisfy the predicate.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The value if it satisfies the predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="predicate" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if the value does not satisfy the predicate.</exception>
    /// <remarks>
    ///     This method is useful for validating that a value meets a custom condition defined by the predicate.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Requires(myValue, x => x > 0, nameof(myValue), "Value must be greater than 0");
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        // Ensure the predicate is not null
        NotNull(predicate, nameof(predicate));

        // Check if the value satisfies the predicate
        if (!predicate(value))
        {
            // Determine the error message to use
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' does not meet the required condition."
                : exceptionMessage;

            // Throw the ArgumentException with the appropriate message
            ThrowHelper.ThrowArgumentException(argumentName, errorMessage);
        }

        // Return the value if it satisfies the predicate
        return value;
    }
}