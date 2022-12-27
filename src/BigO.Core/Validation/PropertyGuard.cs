using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BigO.Core.Extensions;
using JetBrains.Annotations;

// ReSharper disable InvertIf

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
public static class PropertyGuard
{
    /// <summary>
    ///     Ensures that the value of an object is not null.
    /// </summary>
    /// <param name="value">The object value to validate as not null.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that an object value passed as an argument to a method is not null.
    ///     If <paramref name="value" /> is <c>null</c>, an <see cref="ArgumentNullException" /> is thrown.
    ///     The exception message will include the name of the property and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    [ContractAnnotation("value: null => halt; value:notnull => notnull")]
    public static object NotNull([System.Diagnostics.CodeAnalysis.NotNull] object? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(propertyName, exceptionMessage);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a string is not null or empty.
    /// </summary>
    /// <param name="value">The string value to validate as not null or empty.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it is not null or empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is not null or empty.
    ///     If <paramref name="value" /> is <c>null</c>, an <see cref="ArgumentNullException" /> is thrown.
    ///     If <paramref name="value" /> is empty, an <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the property and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    [ContractAnnotation("value: null => halt")]
    public static string NotNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(propertyName, errorMessage);
        }

        if (value.Length == 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of {propertyName} cannot be empty."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a string is not null, empty, or consisting only of whitespace characters.
    /// </summary>
    /// <param name="value">The string value to validate as not null, empty, or consisting only of whitespace characters.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>
    ///     The original value of <paramref name="value" /> if it is not null, empty, or consisting only of whitespace
    ///     characters.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="value" /> is empty or consisting only of whitespace
    ///     characters.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is not null, empty, or
    ///     consisting only of whitespace characters.
    ///     If <paramref name="value" /> is <c>null</c>, an <see cref="ArgumentNullException" /> is thrown.
    ///     If <paramref name="value" /> is empty or consisting only of whitespace characters, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the property and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    [ContractAnnotation("value: null => halt")]
    public static string NotNullOrWhiteSpace([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(propertyName, errorMessage);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of ;{propertyName}' cannot be empty or consist only of whitespace characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the length of a string is less than or equal to a specified maximum length.
    /// </summary>
    /// <param name="value">The string value to validate the length of.</param>
    /// <param name="maxLength">The maximum allowed length for the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>
    ///     The original value of <paramref name="value" /> if its length is less than or equal to
    ///     <paramref name="maxLength" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="maxLength" /> is less than or equal to 0, or if the
    ///     length of <paramref name="value" /> is greater than <paramref name="maxLength" />.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is not too long.
    ///     If <paramref name="maxLength" /> is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the length of <paramref name="value" /> is greater than <paramref name="maxLength" />, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the property and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static string MaxLength(string value, int maxLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (maxLength <= 0)
        {
            throw new ArgumentException("The maximum length specified cannot be less than or equal to 0.",
                nameof(maxLength));
        }

        if (value.Length > maxLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{propertyName}' cannot exceed {maxLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the length of a string is greater than or equal to a specified minimum length.
    /// </summary>
    /// <param name="value">The string value to validate the length of.</param>
    /// <param name="minLength">The minimum allowed length for the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>
    ///     The original value of <paramref name="value" /> if its length is greater than or equal to
    ///     <paramref name="minLength" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="minLength" /> is less than or equal to 0, or if the
    ///     length of <paramref name="value" /> is less than <paramref name="minLength" />.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is long enough.
    ///     If <paramref name="minLength" /> is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the length of <paramref name="value" /> is less than <paramref name="minLength" />, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the property and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static string MinLength(string value, int minLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (minLength <= 0)
        {
            throw new ArgumentException("The minimum length specified cannot be less than or equal to 0.",
                nameof(minLength));
        }

        if (value.Length > minLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{propertyName}' cannot be less than {minLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the length of a string is within a specified range.
    /// </summary>
    /// <param name="value">The string value to validate the length of.</param>
    /// <param name="minLength">The minimum allowed length for the string.</param>
    /// <param name="maxLength">The maximum allowed length for the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if its length is within the specified range.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="minLength" /> is less than or equal to 0,
    ///     <paramref name="maxLength" /> is less than or equal to 0, or if the length of <paramref name="value" /> is outside
    ///     the range specified by <paramref name="minLength" /> and <paramref name="maxLength" />.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is of a suitable length.
    ///     If <paramref name="minLength" /> is less than or equal to 0, or if <paramref name="maxLength" /> is less than or
    ///     equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If <paramref name="maxLength" /> is less than <paramref name="minLength" />, an <see cref="ArgumentException" /> is
    ///     thrown.
    ///     If the length of <paramref name="value" /> is outside the range specified by <paramref name="minLength" /> and
    ///     <paramref name="maxLength" /> , an <see cref="ArgumentException" /> is thrown with a message indicating the range
    ///     that is allowed.
    ///     If the <paramref name="exceptionMessage" /> parameter is provided, it will be used as the message for the
    ///     exception.
    ///     Otherwise, a default message will be used indicating the range that is allowed for the length of the string.
    /// </remarks>
    public static string StrengthLength(string value, int minLength, int maxLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (maxLength <= 0)
        {
            throw new ArgumentException("The maximum length specified cannot be less than or equal to 0.",
                nameof(maxLength));
        }

        if (minLength <= 0)
        {
            throw new ArgumentException("The minimum length specified cannot be less than or equal to 0.",
                nameof(minLength));
        }

        if (maxLength < minLength)
        {
            throw new ArgumentException(
                "The minimum length specified cannot be greater than the maximum length specified.");
        }

        if (value.Length > maxLength || value.Length < minLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{propertyName}' cannot exceed {maxLength} characters and cannot be less than {minLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the specified value is a valid email address.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="propertyName">
    ///     The name of the property being validated. This is used to set the <c>paramName</c>
    ///     parameter of the <see cref="ArgumentException" /> if the validation fails.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The error message to include in the <see cref="ArgumentException" /> if the validation fails. If this
    ///     is <c>null</c> or whitespace, a default error message will be used.
    /// </param>
    /// <returns>The specified value if it is a valid email address.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified value is not a valid email address.
    /// </exception>
    public static string Email(string value, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (!value.IsValidEmail())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not a valid email address."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the given value is within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minimum">The minimum value in the range.</param>
    /// <param name="maximum">The maximum value in the range.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This is used in the exception message when an
    ///     <see cref="ArgumentException" /> is thrown.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The message to include in the exception thrown if the value is not within the specified range. If this
    ///     parameter is <c>null</c>, a default exception message is used.
    /// </param>
    /// <returns>The value being checked.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the value is not within the specified range.
    /// </exception>
    [ContractAnnotation("value: null => halt;")]
    public static T Range<T>(T value, T minimum, T maximum, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (!value.IsBetween(minimum, maximum))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not in the range from {minimum} to {maximum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the given value is greater than or equal to the specified minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated.</typeparam>
    /// <param name="value">The value to be validated.</param>
    /// <param name="minimum">The minimum value that the <paramref name="value" /> must be greater than or equal to.</param>
    /// <param name="propertyName">
    ///     (Optional) The name of the property being validated. This is automatically set by the
    ///     <see cref="CallerMemberNameAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     (Optional) The custom error message to use if validation fails. If not specified, a
    ///     default error message will be used.
    /// </param>
    /// <returns>The original value if it is greater than or equal to the specified minimum value.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="value" /> is less than the specified minimum value.</exception>
    public static T Minimum<T>(T value, T minimum, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is less than the specified minimum value {minimum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the given value is less than or equal to the specified maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the value to be validated.</typeparam>
    /// <param name="value">The value to be validated.</param>
    /// <param name="maximum">The maximum value that the given value is allowed to be.</param>
    /// <param name="propertyName">
    ///     The name of the property being validated. This value is optional and is used for generating
    ///     the exception message. If not provided, the name will be retrieved using the
    ///     <see cref="CallerMemberNameAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The exception message to be used if the validation fails. If not provided, a default
    ///     message will be generated based on the validation failure.
    /// </param>
    /// <returns>The given value if it is less than or equal to the specified maximum value.</returns>
    /// <exception cref="ArgumentException">Thrown if the given value is greater than the specified maximum value.</exception>
    public static T Maximum<T>(T value, T maximum, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(maximum) > 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is greater than the specified maximum value {maximum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the specified <paramref name="value" /> matches the specified <paramref name="pattern" /> using the
    ///     specified <paramref name="regexOptions" />.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="regexOptions">The options to use when matching the pattern.</param>
    /// <param name="propertyName">
    ///     The name of the property being validated. This is automatically populated by the compiler
    ///     when the <c>CallerMemberName</c> attribute is used.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The error message to include in the exception if validation fails. If this is
    ///     <c>null</c>, a default error message will be used.
    /// </param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the value does not match the pattern or if the pattern is null or
    ///     whitespace.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="Regex" /> class to match the pattern. If the value does not match the pattern, an
    ///     <see cref="ArgumentException" /> is thrown.
    /// </remarks>
    public static string Regex(string value, string pattern, RegexOptions regexOptions = RegexOptions.None,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentException($"The {nameof(pattern)} cannot be null or whitespace.", nameof(pattern));
        }

        var regex = new Regex(pattern, regexOptions);

        if (!regex.IsMatch(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' does not match the specified Regex pattern: {pattern}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the specified value is a valid website URL.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked. This will be used in the exception message if the check fails.
    ///     This value is optional and can be provided automatically by using the <see cref="CallerMemberNameAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception that is thrown if the check fails.
    ///     This value is optional and a default message will be used if not provided.
    /// </param>
    /// <returns>The value if it is a valid website URL.</returns>
    /// <exception cref="ArgumentException">Thrown if the value is not a valid website URL.</exception>
    /// <remarks>
    ///     A valid website URL is defined as a string that starts with "http://" or "https://" and is a valid domain name.
    ///     The domain name can contain letters, digits, hyphens and periods, but must not start or end with a period.
    ///     If the <paramref name="value" /> is <c>null</c> or an empty string, it will be considered invalid.
    /// </remarks>
    public static string Url(string value, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (!value.IsValidWebsiteUrl())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not a valid website URL."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the specified <see cref="Guid" /> is not empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid" /> value to check.</param>
    /// <param name="propertyName">
    ///     The name of the property being validated. This will be used in the exception message if the
    ///     validation fails. This parameter is optional and can be specified as <c>null</c>.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to use if the validation fails. This parameter is optional
    ///     and can be specified as <c>null</c>.
    /// </param>
    /// <returns>The input value if it is not empty.</returns>
    /// <exception cref="ArgumentException">Thrown if the input value is empty.</exception>
    public static Guid NotEmpty(Guid value, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (value.IsEmpty())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' cannot be Empty."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }

    /// <summary>
    ///     Validates the specified value using the provided <paramref name="predicate" />.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="predicate">The predicate to use for validation.</param>
    /// <param name="propertyName">
    ///     The name of the property being validated. This will be set automatically by the calling
    ///     member.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Optional custom exception message to use if the value does not pass validation. If not
    ///     specified, a default message will be used.
    /// </param>
    /// <exception cref="ArgumentException">Thrown if the value does not pass validation.</exception>
    /// <returns>The original value if it passes validation.</returns>
    /// <remarks>
    ///     This method is intended to be used as a way to validate values using a custom predicate.
    ///     It can be useful in situations where a more specific validation is needed that is not covered by the other methods
    ///     in this class.
    ///     The <paramref name="predicate" /> parameter should be a function that takes a single parameter of type
    ///     <typeparamref name="T" /> and returns a boolean indicating whether the value is valid.
    ///     If the value is not valid, an <see cref="ArgumentException" /> will be thrown.
    ///     If a custom exception message is provided, it will be used as the message for the exception. Otherwise, a default
    ///     message will be used.
    /// </remarks>
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (!predicate(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' does not satisfy the specified {nameof(predicate)}."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, propertyName);
        }

        return value;
    }
}