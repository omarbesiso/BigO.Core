namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating property values.
/// </summary>
public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property string is not <c>null</c> or empty. If the string is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown. If the string is empty, an
    ///     <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The string to be checked for <c>null</c> or empty.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is <c>null</c> or empty. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null and non-empty string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the string is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if the string is empty.</exception>
    /// <remarks>
    ///     This method is useful for validating string properties to ensure they are neither <c>null</c> nor empty,
    ///     thus avoiding common errors related to string handling.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.NotNullOrEmpty(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotNullOrEmpty(value, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string is not <c>null</c>, empty, or consists only of white-space characters. If
    ///     the string
    ///     is <c>null</c>, an <see cref="ArgumentNullException" /> is thrown. If the string is empty or consists only of
    ///     white-space characters,
    ///     an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The string to be checked for <c>null</c>, empty, or white-space.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the string is <c>null</c>, empty, or consists only of white-space. If not provided, a
    ///     default message is used.
    /// </param>
    /// <returns>The non-null and non-white-space string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the string is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the string is empty or consists only of white-space characters.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating string properties to ensure they are neither <c>null</c>, empty, nor consist
    ///     only of white-space characters, thus avoiding common errors related to string handling.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.NotNullOrWhiteSpace(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrWhitespace([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotNullOrWhiteSpace(value, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string does not exceed a specified maximum length.
    ///     If the string's length is greater than the maximum, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is.
    /// </summary>
    /// <param name="value">The string to be checked for length. Can be <c>null</c>.</param>
    /// <param name="maxLength">The maximum allowable length of the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///     This method is useful for validating string length constraints in property values.
    ///     For checking <c>null</c> or empty strings, consider using other guard methods like <c>NotNull</c> or
    ///     <c>NotNullOrEmpty</c>.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.MaxLength(myString, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MaxLength(string? value, int maxLength,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.MaxLength(value, maxLength, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string meets a specified minimum length requirement.
    ///     If the string's length is less than the minimum, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is.
    /// </summary>
    /// <param name="value">The string to be checked for minimum length. Can be <c>null</c>.</param>
    /// <param name="minLength">The minimum allowable length of the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.MinLength(myString, 5, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MinLength(string? value, int minLength,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.MinLength(value, minLength, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string is of a specified exact length.
    ///     If the string's length does not match the exact length, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be checked for the exact length. Can be <c>null</c>.</param>
    /// <param name="exactLength">The exact length the string must have.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.ExactLength(myString, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ExactLength(string? value, int exactLength,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.ExactLength(value, exactLength, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string's length is within a specified range.
    ///     If the string's length is outside the specified minimum and maximum length, an <see cref="ArgumentException" /> is
    ///     thrown.
    ///     Validates that the minimum and maximum length parameters are logically consistent.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be checked for length within the specified range. Can be <c>null</c>.</param>
    /// <param name="minLength">The minimum allowable length of the string.</param>
    /// <param name="maxLength">The maximum allowable length of the string.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.StringLengthWithinRange(myString, 5, 10, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? StringLengthWithinRange(string? value, int minLength, int maxLength,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.StringLengthWithinRange(value, minLength, maxLength, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string matches a specified regular expression pattern.
    ///     If the string does not match the pattern, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated against the regular expression. Can be <c>null</c>.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.MatchesRegex(myString, @"^\d{4}-\d{3}-\d{4}$", nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? MatchesRegex(string? value, string pattern,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.MatchesRegex(value, pattern, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string is a valid email address.
    ///     If the string is not a valid email, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated as an email address. Can be <c>null</c>.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.EmailAddress(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? EmailAddress(string? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.EmailAddress(value, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property string is a valid URL.
    ///     If the string is not a valid URL with HTTP or HTTPS scheme, an <see cref="ArgumentException" /> is thrown.
    ///     This method allows <c>null</c> values and will return them as-is, only validating non-null strings.
    /// </summary>
    /// <param name="value">The string to be validated as a URL. Can be <c>null</c>.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
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
    ///         PropertyGuard.Url(myString, nameof(myString));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? Url(string? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.Url(value, propertyName, exceptionMessage);
    }
}