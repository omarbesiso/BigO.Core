using System.Diagnostics;
using BigO.Core.Extensions;

namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating properties.
/// </summary>
/// <remarks>
///     The class uses <seealso cref="System.Runtime.CompilerServices" /> to detect the name of the property being
///     validated.
/// </remarks>
[PublicAPI]
[DebuggerStepThrough]
public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property value is not <c>null</c>. If the value is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked.</typeparam>
    /// <param name="value">The property value to be checked for <c>null</c>.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value is <c>null</c>. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null property value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the property value is <c>null</c>.</exception>
    /// <remarks>
    ///     Useful in validating property values and ensuring non-null variables, this method simplifies null checks
    ///     and enhances code readability and robustness.
    /// </remarks>
    [ContractAnnotation("value:null => halt; value:notnull => notnull")]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] T value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (value != null)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' cannot be null."
            : exceptionMessage;

        throw new ArgumentNullException(propertyName, errorMessage);
    }

    /// <summary>
    ///     Ensures that the property value satisfies a specified predicate.
    ///     If the value does not satisfy the predicate, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="predicate">The predicate that the property value must satisfy.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value does not satisfy the predicate.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it satisfies the predicate.</returns>
    /// <exception cref="ArgumentException">Thrown if the property value does not satisfy the predicate.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value meets a custom condition defined by the predicate.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Requires(MyProperty, x => x > 0, nameof(MyProperty), "Property value must be greater than 0");
    ///     </code>
    /// </example>
    public static T Requires<T>(T value, Func<T, bool> predicate,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (predicate(value))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' does not meet the required condition."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, propertyName);
    }

    /// <summary>
    ///     Validates that a given string is a valid email address.
    /// </summary>
    /// <param name="value">The string to validate as an email address.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. Used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
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
    public static string? Email(string? value, [CallerMemberName] string propertyName = "",
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
            ? $"The value of '{propertyName}' is not a valid email address."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, propertyName);
    }
}