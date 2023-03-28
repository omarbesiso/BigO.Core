using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BigO.Core.Extensions;
using JetBrains.Annotations;

// ReSharper disable InvertIf

namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating method arguments.
/// </summary>
[PublicAPI]
[DebuggerStepThrough]
public static class Guard
{
    /// <summary>
    ///     This method ensures that the given value is not <c>null</c>. If the value is <c>null</c>, it throws an
    ///     <see cref="ArgumentNullException" />.
    /// </summary>
    /// <param name="value">The value to be checked for <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if the
    ///     value is <c>null</c>.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom exception message to be used if the value is <c>null</c>. If this is not
    ///     provided, a default message will be used.
    /// </param>
    /// <returns>The given value if it is not <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the given value is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that required method arguments are not <c>null</c>. It can also be used to
    ///     ensure that variables that must not be <c>null</c> are not <c>null</c>.
    /// </remarks>
    [ContractAnnotation("value: null => halt; value:notnull => notnull")]
    public static object NotNull([System.Diagnostics.CodeAnalysis.NotNull] object? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(argumentName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     This method ensures that the given string value is not <c>null</c> or empty. If the value is <c>null</c>, it throws
    ///     an <see cref="ArgumentNullException" />. If the value is empty, it throws an <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="value">The string value to be checked for <c>null</c> or empty.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if the
    ///     value is <c>null</c> or empty.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom exception message to be used if the value is <c>null</c> or empty. If this is
    ///     not provided, a default message will be used.
    /// </param>
    /// <returns>The given string value if it is not <c>null</c> or empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the given string value is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if the given string value is empty.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that required method arguments are not <c>null</c> or empty. It can also be used
    ///     to ensure that string variables that must not be <c>null</c> or empty are not <c>null</c> or empty.
    /// </remarks>
    [ContractAnnotation("value: null => halt")]
    public static string NotNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(argumentName, errorMessage);
        }

        if (value.Length == 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of {argumentName} cannot be empty."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Verifies that the provided <see cref="IEnumerable{T}" /> is not <c>null</c> or empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <see cref="IEnumerable{T}" />.</typeparam>
    /// <param name="value">The <see cref="IEnumerable{T}" /> to check.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if the check fails.
    ///     This parameter is optional. If not provided, the name of the <see cref="IEnumerable{T}" /> parameter will be used.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The message to include in the exception if the check fails. This parameter is optional.
    ///     If not provided, a default message will be used.
    /// </param>
    /// <returns>The provided <see cref="IEnumerable{T}" /> if it is not <c>null</c> or empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided <see cref="IEnumerable{T}" /> is <c>null</c> or empty.</exception>
    [ContractAnnotation("value: null => halt")]
    public static IEnumerable<T> NotNullOrEmpty<T>([System.Diagnostics.CodeAnalysis.NotNull] IEnumerable<T>? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value.IsNullOrEmpty())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(argumentName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentNullException" /> if the specified value is <c>null</c>, an empty string, or consists
    ///     only of white space characters.
    /// </summary>
    /// <param name="value">The value to check for <c>null</c>, an empty string, or white space characters.</param>
    /// <param name="argumentName">
    ///     The name of the argument that is <c>null</c>, an empty string, or consists only of white
    ///     space characters. This will be included in the exception message if the value is <c>null</c>, an empty string, or
    ///     consists only of white space characters.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional message to include in the exception. If this is not specified, a default
    ///     message will be used.
    /// </param>
    /// <returns>
    ///     The non-<c>null</c>, non-empty, and non-white-space string value that was passed as the
    ///     <paramref name="value" /> parameter.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="value" /> parameter is <c>null</c>, an empty string, or
    ///     consists only of white space characters.
    /// </exception>
    [ContractAnnotation("value: null => halt")]
    public static string NotNullOrWhiteSpace([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' cannot be null."
                : exceptionMessage;

            throw new ArgumentNullException(argumentName, errorMessage);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of {argumentName} cannot be empty or consist only of white-space characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the specified string does not exceed the maximum length specified.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="maxLength">The maximum allowed length of the string.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This is used for the exception message when the
    ///     string exceeds the maximum length. This parameter is optional and can be provided automatically by the compiler
    ///     when calling the method.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to use when the string exceeds the maximum length. If this
    ///     parameter is <c>null</c>, a default exception message will be used.
    /// </param>
    /// <returns>The original string if its length is valid.</returns>
    /// <exception cref="ArgumentException">Thrown if the length of the string exceeds the maximum length specified.</exception>
    /// <exception cref="ArgumentException">Thrown if the maximum length specified is less than or equal to 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that string arguments passed to a method are not too long.
    ///     If the string is null, an <see cref="ArgumentNullException" /> is thrown.
    ///     If the maximum length specified is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the length of the string exceeds the maximum length specified, an <see cref="ArgumentException" /> is thrown.
    ///     The exception message used can either be a default message or a custom message provided through the
    ///     <paramref name="exceptionMessage" /> parameter.
    /// </remarks>
    public static string MaxLength(string value, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "A null string value cannot be checked for maximum length.");
        }

        if (maxLength <= 0)
        {
            throw new ArgumentException("The maximum length specified cannot be less than or equal to 0.",
                nameof(maxLength));
        }

        if (value.Length > maxLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{argumentName}' cannot exceed {maxLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the specified string meets the minimum length specified.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="minLength">The minimum required length of the string.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This is used for the exception message when the
    ///     string does not meet the minimum length. This parameter is optional and can be provided automatically by the
    ///     compiler when calling the method.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to use when the string does not meet the minimum length.
    ///     If this parameter is <c>null</c>, a default exception message will be used.
    /// </param>
    /// <returns>
    ///     The original string if its length is valid, or the original string if the string is null
    ///     otherwise, an <see cref="ArgumentException" /> is thrown.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the length of the string is less than the minimum length specified.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="minLength" /> parameter is less than or
    ///     equal to 0.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that string arguments passed to a method are not shorter than a certain length.
    ///     If the string is null, the method returns the original string without throwing an exception.
    ///     If the minimum length specified is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the length of the string is less than the minimum length specified, an <see cref="ArgumentException" /> is
    ///     thrown.
    ///     The exception message used can either be a default message or a custom message provided through the
    ///     <paramref name="exceptionMessage" /> parameter.
    /// </remarks>
    public static string MinLength(string value, int minLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "A null string value cannot be checked for minimum length.");
        }

        if (minLength <= 0)
        {
            throw new ArgumentException("The minimum length specified cannot be less than or equal to 0.",
                nameof(minLength));
        }

        if (value.Length < minLength)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The length of string '{argumentName}' cannot be less than {minLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentException" /> if the length of the specified string is not within the specified range.
    /// </summary>
    /// <param name="value">The string to check for length.</param>
    /// <param name="minLength">The minimum length that the string is required to be.</param>
    /// <param name="maxLength">The maximum length that the string is allowed to be.</param>
    /// <param name="argumentName">
    ///     The name of the argument whose length is being checked. This will be included in the
    ///     exception message if the value is too short or too long.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional message to include in the exception. If this is not specified, a default
    ///     message will be used.
    /// </param>
    /// <returns>The original string if its length is valid, or the original string if the string is null.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the length of the string is less than the minimum length specified or
    ///     greater than the maximum length specified.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown if the minimum length specified is less than or equal to 0.</exception>
    /// <exception cref="ArgumentException">Thrown if the maximum length specified is less than or equal to 0.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the minimum length specified is greater than the maximum length
    ///     specified.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that string arguments passed to a method are within a certain range of lengths.
    ///     If the string is null, , an <see cref="ArgumentException" /> is thrown.
    ///     If the minimum length specified is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the maximum length specified is less than or equal to 0, an <see cref="ArgumentException" /> is thrown.
    ///     If the minimum length specified is greater than the maximum length specified, an <see cref="ArgumentException" />
    ///     is thrown.
    ///     If the length of the string is less than the minimum length specified or greater than the maximum length specified,
    ///     an <see cref="ArgumentException" /> is thrown.
    ///     The exception message used can either be a default message or a custom message provided through the
    ///     <paramref name="exceptionMessage" /> parameter.
    /// </remarks>
    public static string StrengthLength(string value, int minLength, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "A null string value cannot be checked for string length.");
        }

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
                ? $"The length of string '{argumentName}' cannot exceed {maxLength} characters and cannot be less than {minLength} characters."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the specified string is a valid email address.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This is used for the exception message when the
    ///     string is not a valid email address. This parameter is optional and can be provided automatically
    ///     by the compiler when calling the method.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to use when the string is not a valid email address. If
    ///     this parameter is <c>null</c>, a default exception message will be used.
    /// </param>
    /// <returns>The original string if it is a valid email address; otherwise, an <see cref="ArgumentException" /> is thrown.</returns>
    /// <exception cref="ArgumentException">Thrown if the string is not a valid email address.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the string is null or whitespace.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that string arguments passed to a method are valid email addresses.
    ///     If the string is null or whitespace, an <see cref="ArgumentNullException" /> is thrown.
    ///     If the string is not a valid email address, an <see cref="ArgumentException" /> is thrown.
    ///     The exception message used can either be a default message or a custom message provided through the
    ///     <paramref name="exceptionMessage" /> parameter.
    /// </remarks>
    public static string Email(string value, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "The email value to check cannot be null or whitespace.");
        }

        if (!value.IsValidEmail())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' is not a valid email address."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the value of a variable is within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="minimum">
    ///     The minimum value in the range. The value of <paramref name="value" /> must be greater than or
    ///     equal to this value.
    /// </param>
    /// <param name="maximum">
    ///     The maximum value in the range. The value of <paramref name="value" /> must be less than or equal
    ///     to this value.
    /// </param>
    /// <param name="argumentName">
    ///     The name of the argument being validated. This will be included in the exception message if
    ///     the validation fails.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to include in the exception if the validation fails. If
    ///     not provided, a default message will be used.
    /// </param>
    /// <returns>The value of <paramref name="value" /> if it is within the specified range.</returns>
    /// <exception cref="ArgumentException">Thrown if the value of <paramref name="value" /> is not within the specified range.</exception>
    [ContractAnnotation("value: null => halt;")]
    public static T Range<T>(T value, T minimum, T maximum,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (!value.IsBetween(minimum, maximum))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' is not in the range from {minimum} to {maximum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Validates that the value of the specified argument is greater than or equal to the specified minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the argument to validate.</typeparam>
    /// <param name="value">The value of the argument to validate.</param>
    /// <param name="minimum">The minimum value that the argument's value should be greater than or equal to.</param>
    /// <param name="argumentName">
    ///     The name of the argument. This will be used in the exception message if the argument's value is less than the
    ///     specified minimum value.
    /// </param>
    /// <param name="exceptionMessage">
    ///     The error message to include in the exception if the argument's value is less than the specified minimum value.
    ///     If not provided, a default error message will be used.
    /// </param>
    /// <returns>The value of the argument, if it is greater than or equal to the specified minimum value.</returns>
    /// <exception cref="System.ArgumentException">
    ///     Thrown if the value of the argument is less than the specified minimum value.
    /// </exception>
    public static T Minimum<T>(T value, T minimum, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' is less than the specified minimum value {minimum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a generic type <typeparamref name="T" /> is less than or equal to a specified maximum
    ///     value.
    /// </summary>
    /// <typeparam name="T">The type of the value to compare. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="maximum">The maximum value that <paramref name="value" /> can have.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's argument expression.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it is less than or equal to <paramref name="maximum" />.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is greater than <paramref name="maximum" />.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a value passed as an argument to a method is within a certain range.
    ///     If <paramref name="value" /> is greater than <paramref name="maximum" />, an <see cref="ArgumentException" /> is
    ///     thrown.
    ///     The exception message will include the name of the argument and the specified maximum value, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static T Maximum<T>(T value, T maximum, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(maximum) > 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' is greater than the specified maximum value {maximum}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the provided string value matches the specified regular expression pattern.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <param name="regexOptions">The regular expression options to use.</param>
    /// <param name="argumentName">The name of the argument being checked. This is used when generating the exception message.</param>
    /// <param name="exceptionMessage">
    ///     The custom exception message to use if the string value does not match the pattern. If
    ///     <c>null</c>, a default message is used.
    /// </param>
    /// <returns>The original string value if it matches the pattern.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="value" /> is null or whitespace,
    ///     <paramref name="pattern" /> is null or whitespace, or <paramref name="value" /> does not match the pattern.
    /// </exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method matches a certain pattern.
    ///     If <paramref name="value" /> does not match the specified <paramref name="pattern" />, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the argument, the specified pattern, and a description of the error,
    ///     unless a custom exception message is provided.
    /// </remarks>
    public static string Regex(string value, string pattern, RegexOptions regexOptions = RegexOptions.None,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                $"The {nameof(value)} to be checked against the pattern cannot be null or whitespace.",
                nameof(value));
        }

        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentException($"The {nameof(pattern)} cannot be null or whitespace.", nameof(pattern));
        }

        var regex = new Regex(pattern, regexOptions);

        if (!regex.IsMatch(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' does not match the specified Regex pattern: {pattern}."
                : exceptionMessage;

            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a string is a valid website URL.
    /// </summary>
    /// <param name="value">The string value to validate as a website URL.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's argument expression.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it is a valid website URL.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is not a valid website URL.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="value" /> is <c>null</c> or whitespace.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method is a properly formatted
    ///     URL for a website.
    ///     If <paramref name="value" /> is not a valid website URL, an <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the argument and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static string Url(string value, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "The URL value to check cannot be null or whitespace.");
        }

        if (!value.IsValidWebsiteUrl())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' is not a valid website URL."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a <see cref="Guid" /> is not empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid" /> value to validate as not empty.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's argument expression.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it is not empty.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a <see cref="Guid" /> value passed as an argument to a method is not empty.
    ///     If <paramref name="value" /> is empty, an <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the argument and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static Guid NotEmpty(Guid value, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value.IsEmpty())
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' cannot be Empty."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that the value of a generic type <typeparamref name="T" /> satisfies a specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    /// <param name="value">The value to validate using the specified <paramref name="predicate" />.</param>
    /// <param name="predicate">
    ///     A function to test the value for a condition. The function should return <c>true</c> if the
    ///     value satisfies the condition, and <c>false</c> otherwise.
    /// </param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's argument expression.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it satisfies the specified <paramref name="predicate" />.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="value" /> does not satisfy the specified
    ///     <paramref name="predicate" />.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a value passed as an argument to a method satisfies a certain condition.
    ///     If <paramref name="value" /> does not satisfy the specified <paramref name="predicate" />, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the argument and a description of the error, unless a custom
    ///     exception message is provided.
    /// </remarks>
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
        if (!predicate(value))
        {
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{argumentName}' does not satisfy the specified {nameof(predicate)}."
                : exceptionMessage;
            throw new ArgumentException(errorMessage, argumentName);
        }

        return value;
    }
}