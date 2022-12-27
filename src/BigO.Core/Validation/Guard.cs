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
                ? $"The value of {argumentName} cannot be empty or consist only of whitespace characters."
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
    /// <returns>
    ///     The input string if it does not exceed the maximum length specified; otherwise, an
    ///     <see cref="ArgumentException" /> is thrown.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the length of the string exceeds the maximum length specified.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="maxLength" /> parameter is less than or
    ///     equal to 0.
    /// </exception>
    public static string MaxLength(string value, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
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
    ///     The input string if it meets the minimum length specified; otherwise, an <see cref="ArgumentException" /> is
    ///     thrown.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the length of the string is less than the minimum length specified.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="minLength" /> parameter is less than or
    ///     equal to 0.
    /// </exception>
    public static string MinLength(string value, int minLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
    {
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
    /// <returns>The string value that was passed as the <paramref name="value" /> parameter.</returns>
    /// <exception cref="ArgumentException">
    ///     If the length of the <paramref name="value" /> parameter is not within the
    ///     specified range of <paramref name="minLength" /> and <paramref name="maxLength" />.
    /// </exception>
    public static string StrengthLength(string value, int minLength, int maxLength,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "", string? exceptionMessage = null)
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
    ///     string is not a valid email address. This parameter is optional and can be provided automatically by the compiler
    ///     when calling the method.
    /// </param>
    /// <param name="exceptionMessage">
    ///     An optional exception message to use when the string is not a valid email address. If
    ///     this parameter is <c>null</c>, a default exception message will be used.
    /// </param>
    /// <returns>The input string if it is a valid email address; otherwise, an <see cref="ArgumentException" /> is thrown.</returns>
    /// <exception cref="ArgumentException">Thrown if the string is not a valid email address.</exception>
    public static string Email(string value, [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
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
    ///     Ensures that the value of a string matches a specified regular expression pattern.
    /// </summary>
    /// <param name="value">The string value to match against the pattern.</param>
    /// <param name="pattern">The regular expression pattern to match against <paramref name="value" />.</param>
    /// <param name="regexOptions">
    ///     Options to use when creating the <see cref="Regex" /> object to match the pattern. This
    ///     parameter is optional and has a default value of <see cref="RegexOptions.None" />.
    /// </param>
    /// <param name="argumentName">
    ///     The name of the argument being checked. This will be used in the exception message if an
    ///     exception is thrown. This parameter is optional and is provided by the caller's argument expression.
    /// </param>
    /// <param name="exceptionMessage">
    ///     A custom message to include in the exception if one is thrown. This parameter is
    ///     optional.
    /// </param>
    /// <returns>The original value of <paramref name="value" /> if it matches the specified <paramref name="pattern" />.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="pattern" /> is <c>null</c> or whitespace, or if
    ///     <paramref name="value" /> does not match the specified pattern.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is useful for ensuring that a string value passed as an argument to a method matches a certain pattern.
    ///     If <paramref name="value" /> does not match the specified <paramref name="pattern" />, an
    ///     <see cref="ArgumentException" /> is thrown.
    ///     The exception message will include the name of the argument, the specified pattern, and a description of the error,
    ///     unless a custom exception message is provided.
    /// </remarks>
    public static string Regex(string value, string pattern, RegexOptions regexOptions = RegexOptions.None,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
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
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
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