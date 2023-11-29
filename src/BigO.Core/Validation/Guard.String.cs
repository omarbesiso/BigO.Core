using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given string is not <c>null</c> or empty. If the string is <c>null</c> or empty, an
    ///     <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The string to be checked for <c>null</c> or empty.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is <c>null</c> or empty. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null and non-empty string.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is <c>null</c> or empty.</exception>
    /// <remarks>
    ///     This method is useful for validating string arguments to ensure they are neither <c>null</c> nor empty,
    ///     thus avoiding common errors related to string handling.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotNullOrEmpty(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrEmpty(string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (!string.IsNullOrEmpty(value))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null or empty."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string is not <c>null</c>, empty, or composed only of whitespace characters.
    ///     If the string is <c>null</c>, empty, or whitespace, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The string to be checked for <c>null</c>, empty, or whitespace.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is <c>null</c>, empty, or whitespace.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null, non-empty, and non-whitespace string.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is <c>null</c>, empty, or whitespace.</exception>
    /// <remarks>
    ///     This method is useful for validating string arguments to ensure they are meaningful and not just empty or
    ///     whitespace,
    ///     thus ensuring more robust input validation.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotNullOrWhiteSpace(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrWhiteSpace(string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null, empty, or composed only of whitespace."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string does not exceed a specified maximum length.
    ///     If the string's length is greater than the maximum, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is.
    /// </summary>
    /// <param name="value">The string to be checked for length. Can be <c>null</c>.</param>
    /// <param name="maxLength">The maximum allowable length of the string.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string exceeds the maximum length.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if its length does not exceed the maximum, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the string's length exceeds the maximum length and the string is not
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating string length constraints in method arguments.
    ///     For checking <c>null</c> or empty strings, consider using other guard methods like <c>NotNull</c> or
    ///     <c>NotNullOrEmpty</c>.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.MaxLength(myString, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? MaxLength(string? value, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (value == null)
        {
            // Returns the value because if we need to check for null then we should use NotNull() instead.
            return value;
        }

        if (value.Length <= maxLength)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The length of '{argumentName}' cannot exceed {maxLength} characters."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string meets a specified minimum length requirement.
    ///     If the string's length is less than the minimum, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is.
    /// </summary>
    /// <param name="value">The string to be checked for minimum length. Can be <c>null</c>.</param>
    /// <param name="minLength">The minimum allowable length of the string.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is shorter than the minimum length.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if its length meets or exceeds the minimum, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the string's length is less than the minimum length and the string is not
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating string length constraints, especially in ensuring a string is not too short.
    ///     For checking <c>null</c> or empty strings, consider using other guard methods like <c>NotNull</c> or
    ///     <c>NotNullOrEmpty</c>.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.MinLength(myString, 5, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? MinLength(string? value, int minLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (value == null)
        {
            // Returns the value because if we need to check for null then we should use NotNull() instead.
            return value;
        }

        if (value.Length >= minLength)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The length of '{argumentName}' must be at least {minLength} characters."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string is of a specified exact length.
    ///     If the string's length does not match the exact length, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be checked for the exact length. Can be <c>null</c>.</param>
    /// <param name="exactLength">The exact length the string must have.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string does not have the exact length.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if its length matches the exact length, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the string's length does not match the exact length and the string is not
    ///     <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating string length constraints, especially when an exact length is required.
    ///     For checking <c>null</c> or empty strings, or strings with minimum or maximum lengths, consider using other guard
    ///     methods.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.ExactLength(myString, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? ExactLength(string? value, int exactLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (value == null)
        {
            // Returns the value because if we need to check for null then we should use NotNull() instead.
            return value;
        }

        if (value.Length == exactLength)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The length of '{argumentName}' must be exactly {exactLength} characters."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string's length is within a specified range.
    ///     If the string's length is outside the specified minimum and maximum length, an <see cref="ArgumentException" /> is
    ///     thrown.
    ///     Validates that the minimum and maximum length parameters are logically consistent.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be checked for length within the specified range. Can be <c>null</c>.</param>
    /// <param name="minLength">The minimum allowable length of the string.</param>
    /// <param name="maxLength">The maximum allowable length of the string.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string's length is outside the specified range.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if its length is within the specified range, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown under the following conditions:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>If the string's length is less than the specified minimum length.</description>
    ///         </item>
    ///         <item>
    ///             <description>If the string's length is greater than the specified maximum length.</description>
    ///         </item>
    ///         <item>
    ///             <description>If the maximum length specified is less than or equal to 0.</description>
    ///         </item>
    ///         <item>
    ///             <description>If the minimum length specified is less than 0.</description>
    ///         </item>
    ///         <item>
    ///             <description>If the minimum length specified is greater than the maximum length specified.</description>
    ///         </item>
    ///     </list>
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating string length constraints where both a minimum and a maximum length are
    ///     required.
    ///     For checking <c>null</c> or empty strings, or strings with a specific length, consider using other guard methods.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.StringLengthWithinRange(myString, 5, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? StringLengthWithinRange(string? value, int minLength, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value == null)
        {
            return value;
        }

        if (maxLength <= 0)
        {
            throw new ArgumentException("The maximum length specified cannot be less than or equal to 0.",
                nameof(maxLength));
        }

        if (minLength < 0)
        {
            throw new ArgumentException("The minimum length specified cannot be less than 0.",
                nameof(minLength));
        }

        if (maxLength < minLength)
        {
            throw new ArgumentException(
                "The minimum length specified cannot be greater than the maximum length specified.",
                nameof(minLength));
        }

        if (value.Length >= minLength && value.Length <= maxLength)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The length of '{argumentName}' must be between {minLength} and {maxLength} characters."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string matches a specified regular expression pattern.
    ///     If the string does not match the pattern, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated against the regular expression. Can be <c>null</c>.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string does not match the regular expression.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if it matches the regular expression, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">Thrown if the string does not match the regular expression and is not <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for validating strings against specific patterns, such as formats for phone numbers, emails,
    ///     etc.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.MatchesRegex(myString, @"^\d{4}-\d{3}-\d{4}$", nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? MatchesRegex(string? value, string pattern,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value == null)
        {
            return value;
        }

        if (Regex.IsMatch(value, pattern))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' does not match the required pattern."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string is a valid email address.
    ///     If the string is not a valid email, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated as an email address. Can be <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is not a valid email address.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if it is a valid email address, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">Thrown if the string is not a valid email address and is not <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for validating strings that are expected to represent an email address.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.IsValidEmail(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? EmailAddress(string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value == null)
        {
            return value;
        }

        if (MailAddress.TryCreate(value, out _))
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is not a valid email address."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }

    /// <summary>
    ///     Ensures that the given string is a valid URL.
    ///     If the string is not a valid URL with HTTP or HTTPS scheme, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated as a URL. Can be <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is not a valid URL.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The string if it is a valid URL, or <c>null</c> if the input is <c>null</c>.</returns>
    /// <exception cref="ArgumentException">Thrown if the string is not a valid URL and is not <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for validating strings that are expected to represent a URL.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Url(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    public static string? Url(string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value == null)
        {
            return value;
        }

        var isValidUrl = Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (isValidUrl)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' is not a valid URL."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }
}