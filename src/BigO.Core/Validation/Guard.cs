using System.Diagnostics;
using BigO.Core.Extensions;

namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating method arguments.
/// </summary>
[PublicAPI]
[DebuggerStepThrough]
public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given value is not <c>null</c>. If the value is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <param name="value">The value to be checked for <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value is <c>null</c>. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     Useful in validating method arguments and ensuring non-null variables, this method simplifies null checks
    ///     and enhances code readability and robustness.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotNull(myVariable, nameof(myVariable));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt; value:notnull => notnull")]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] T value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value != null)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null."
            : exceptionMessage;

        throw new ArgumentNullException(argumentName, errorMessage);
    }

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
    /// <exception cref="ArgumentException">Thrown if the value does not satisfy the predicate.</exception>
    /// <remarks>
    ///     This method is useful for validating that a value meets a custom condition defined by the predicate.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Requires(myValue, x => x > 0, nameof(myValue), "Value must be greater than 0");
    ///     </code>
    /// </example>
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (predicate(value))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' does not meet the required condition."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Validates that a given string is a valid email address.
    /// </summary>
    /// <param name="value">The string to validate as an email address.</param>
    /// <param name="argumentName">The name of the argument being checked. Used in the exception message for clarity.</param>
    /// <param name="exceptionMessage">
    ///     Optional. A custom exception message to be used if the value is not a valid email
    ///     address. If not provided, a default message is used.
    /// </param>
    /// <returns>The original value if it is a valid email address.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the provided value is not a valid email address.
    /// </exception>
    /// <remarks>
    ///     This method checks whether the provided string meets the criteria for a valid email address.
    ///     If the string is null, it is returned as is, assuming that nullability should be checked separately.
    ///     If the string is not a valid email address and an exception message is provided, that message is used; otherwise, a
    ///     default message is used.
    ///     The method relies on an internal 'IsValidEmail' function to determine email validity.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         string email = "example@domain.com";
    ///         string validEmail = Guard.Email(email, nameof(email));
    ///     </code>
    ///     This example shows how to validate that a string is a valid email address.
    /// </example>
    public static string? Email(string? value, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract    
        if (value == null)
        {
            return value;
        }

        if (value.IsValidEmail())
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is not a valid email address."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }
}