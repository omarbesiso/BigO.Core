using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BigO.Core.Extensions;
using JetBrains.Annotations;

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
    ///     Enforces that a property value is not null.
    /// </summary>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value if it passes validation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
    [ContractAnnotation("value: null => halt; value:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object NotNull([System.Diagnostics.CodeAnalysis.NotNull] object? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (value == null)
        {
            ThrowHelper.ThrowArgumentNullException(propertyName, exceptionMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a property string value is not null or empty.
    /// </summary>
    /// <param name="value">The property string value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The string value if it passes validation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c> or empty.</exception>
    [ContractAnnotation("value: null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            ThrowHelper.ThrowArgumentNullException(propertyName, exceptionMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a property string value is not null, empty or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The property string value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The string value if it passes validation.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="value" /> is <c>null</c> or consists only of white-space
    ///     characters.
    /// </exception>
    [ContractAnnotation("value: null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrWhiteSpace([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            ThrowHelper.ThrowArgumentNullException(propertyName, exceptionMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that the length of a <see cref="string" /> property value does not exceed a specified maximum
    ///     number of characters.
    /// </summary>
    /// <param name="value">The property string value to be checked.</param>
    /// <param name="maxLength">The maximum length allowed for the <see cref="string" /> value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="maxLength" /> is less than or equal 0.
    ///     (and/or)
    ///     The length of <paramref name="value" /> exceeds the <paramref name="maxLength" /> of
    ///     characters specified.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string MaxLength(string value, int maxLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        ValidationHelper.ValidateMaxLength(value, maxLength, propertyName, exceptionMessage);
        return value;
    }

    /// <summary>
    ///     Enforces that the length of a <see cref="string" /> property value has at least a specified minimum
    ///     number of characters.
    /// </summary>
    /// <param name="value">The string property value to be checked.</param>
    /// <param name="minLength">The minimum length allowed for the <see cref="string" /> value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="minLength" /> is less than or equal 0.
    ///     (and/or)
    ///     The length of <paramref name="value" /> is below the <paramref name="minLength" /> of
    ///     characters specified.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string MinLength(string value, int minLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        ValidationHelper.ValidateMinLength(value, minLength, propertyName, exceptionMessage);
        return value;
    }

    /// <summary>
    ///     Enforces that the length of a <see cref="string" /> property value has at least a specified minimum
    ///     number of characters and does not exceed a specified maximum number of characters.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <param name="minLength">The minimum length allowed for the <see cref="string" /> value.</param>
    /// <param name="maxLength">The maximum length allowed for the <see cref="string" /> value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException">
    ///     The <paramref name="maxLength" /> value is less than or equal to 0.
    ///     (and/or)
    ///     The <paramref name="minLength" /> value is less than 0.
    ///     (and/or)
    ///     The <paramref name="minLength" /> value is less than The <paramref name="maxLength" /> specified.
    ///     (and/or)
    ///     <paramref name="value" /> length is less than the <paramref name="minLength" /> specified or exceeds the
    ///     <paramref name="maxLength" /> specified.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string StrengthLength(string value, int minLength, int maxLength,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        ValidationHelper.ValidateStrengthLength(value, minLength, maxLength, propertyName, exceptionMessage);
        return value;
    }

    /// <summary>
    ///     Enforces that a <see cref="string" /> property value represents a valid email.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> is not a valid email.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="value" /> is <c>null</c> or consists only of white-space
    ///     characters.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Email(string value, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        if (!value.IsValidEmail())
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not a valid email address."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Ensures that a value falls within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" /> that implements <see cref="IComparable{T}" /></typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minimum">The minimum value of the specified range.</param>
    /// <param name="maximum">The maximum value of the specified range.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> does not fall in the specified range.</exception>
    [ContractAnnotation("value: null => halt;")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Range<T>(T value, T minimum, T maximum,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (!value.IsBetween(minimum, maximum))
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not in the range from {minimum} to {maximum}."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a value is not less than a specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" /> that implements <see cref="IComparable{T}" /></typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minimum">The minimum against with the <paramref name="value" /> is checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> is less than the specified minimum value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Minimum<T>(T value, T minimum, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0)
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is less than the specified minimum value {minimum}."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a value is not greater than a specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" /> that implements <see cref="IComparable{T}" /></typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="maximum">The maximum value against with the <paramref name="value" /> is checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> is greater than the specified maximum value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Maximum<T>(T value, T maximum, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(maximum) > 0)
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is greater than the specified maximum value {maximum}."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a <see cref="string" /> value matches a specified regular expression pattern.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="pattern">The regular expression used for evaluation of the value.</param>
    /// <param name="regexOptions">(Optional) Provides enumerated values to use to set regular expression options.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="value" /> does not match the specified
    ///     <paramref name="pattern" />.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="pattern" /> is <c>null</c> or consists only of white-space
    ///     characters.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' does not match the specified Regex pattern: {pattern}."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a <see cref="string" /> value represents a valid website URL.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> is not a valid website URL.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Url(string value, [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (!value.IsValidWebsiteUrl())
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' is not a valid website URL."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }

    /// <summary>
    ///     Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> is empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid NotEmpty(Guid value, [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        {
            if (!value.IsEmpty())
            {
                var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                    ? $"The value of '{propertyName}' cannot be Empty."
                    : exceptionMessage;
                ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
            }

            return value;
        }
    }

    /// <summary>
    ///     Enforces that the provided value to satisfy a specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value to be tested.</typeparam>
    /// <param name="value">The string value to be checked.</param>
    /// <param name="predicate">The predicate sued to validate the value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="exceptionMessage">The optional exception message.</param>
    /// <returns>The value that has been successfully checked.</returns>
    /// <exception cref="ArgumentException"><paramref name="value" /> does not satisfy the specified predicate.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerMemberName] string propertyName = "", string? exceptionMessage = null)
    {
        if (!predicate(value))
        {
            var errorMessage = !string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The value of '{propertyName}' does not satisfy the specified {nameof(predicate)}."
                : exceptionMessage;
            ThrowHelper.ThrowArgumentException(propertyName, errorMessage);
        }

        return value;
    }
}