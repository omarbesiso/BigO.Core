using System.Globalization;
using System.Net.Mail;
using System.Text;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="string" /> objects.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Extracts all the digits from the given input string.
    /// </summary>
    /// <param name="input">The input string from which the digits are to be extracted.</param>
    /// <returns>
    ///     Returns a string containing only the digits from the input. If the input is <c>null</c>, empty, or consists
    ///     only of white-space characters, an empty string is returned.
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// string text = "Hello123World";
    /// string result = text.ExtractDigits();
    /// Console.WriteLine(result); // Outputs: "123"
    /// ]]></code>
    /// </example>
    public static string ExtractDigits(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        StringBuilder sb = new();
        foreach (var character in input.Where(char.IsDigit))
        {
            sb.Append(character);
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Determines whether the specified string value is a valid GUID.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>true if the string value is a valid GUID; otherwise, false.</returns>
    /// <remarks>
    ///     This method determines whether the specified <paramref name="value" /> is a valid GUID. A GUID is a 128-bit integer
    ///     (16 bytes) that can be used to identify objects such as database records, software components, and system
    ///     resources. A GUID can be represented as a string of 32 hexadecimal digits (0-9 and A-F) grouped in 5 sections
    ///     separated by hyphens. For example: "3F2504E0-4F89-11D3-9A0C-0305E82C3301". This method checks whether the
    ///     <paramref name="value" /> parameter can be parsed as a <see cref="Guid" /> using either
    ///     <see cref="Guid.TryParse(ReadOnlySpan{char}, out Guid)" /> or
    ///     <see cref="Guid.TryParseExact(string, string, out Guid)" /> with the "N" format specifier. If the
    ///     <paramref name="value" /> parameter is null, empty, or contains only white space characters, this method returns
    ///     false.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="IsGuid" /> method to determine whether a string value
    ///     is a valid GUID:
    ///     <code><![CDATA[
    /// string guidString = "3F2504E0-4F89-11D3-9A0C-0305E82C3301";
    /// bool isValidGuid = guidString.IsGuid();
    /// Console.WriteLine(isValidGuid); // Output: True
    /// 
    /// string invalidGuidString = "not a valid GUID";
    /// bool isInvalidGuid = invalidGuidString.IsGuid();
    /// Console.WriteLine(isInvalidGuid); // Output: False
    /// ]]></code>
    /// </example>
    public static bool IsGuid(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var guidSpan = value.AsSpan();
        return Guid.TryParse(guidSpan, out _) || Guid.TryParseExact(value, "N", out _);
    }

    /// <summary>
    ///     Determines whether the specified string is a valid email address.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if the specified string is a valid email address, otherwise false.</returns>
    /// <remarks>
    ///     This method uses the <see cref="MailAddress" /> class to validate whether the specified string is a valid email
    ///     address.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsValidEmail(string)" /> method.
    ///     <code><![CDATA[
    /// string email = "test@example.com";
    /// bool isValidEmail = email.IsValidEmail(); // returns true
    /// ]]></code>
    /// </example>
    public static bool IsValidEmail(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value) && MailAddress.TryCreate(value, out _);
    }

    /// <summary>
    ///     Determines whether the specified string is a valid website URL.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns><c>true</c> if the specified string is a valid website URL; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     A valid website URL is a non-null, non-empty string that can be parsed into a <see cref="Uri" /> object with an
    ///     absolute <see cref="UriKind" /> of either <see cref="Uri.UriSchemeHttp" /> or <see cref="Uri.UriSchemeHttps" />.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use <see cref="IsValidWebsiteUrl(string?)" /> method.
    ///     <code><![CDATA[
    /// string url = "https://www.example.com";
    /// bool isValid = url.IsValidWebsiteUrl(); // Returns true
    /// ]]></code>
    /// </example>
    public static bool IsValidWebsiteUrl(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var result = Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return result;
    }

    /// <summary>
    ///     Determines whether the specified string consists entirely of whitespace characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the specified string consists only of whitespace characters; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
    /// <remarks>
    ///     This method checks whether every character in the input string is a whitespace character, as defined by
    ///     <see cref="char.IsWhiteSpace(char)" />.
    ///     It considers various types of whitespace characters, including spaces, tabs, and newline characters.
    ///     An empty string will return <c>true</c> as it does not contain any non-whitespace characters.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use <see cref="IsWhiteSpace" /> method to check if a string consists of only
    ///     whitespace characters:
    ///     <code>
    /// string str1 = "   ";
    /// string str2 = "  example  ";
    /// 
    /// bool result1 = str1.IsWhiteSpace(); // True
    /// bool result2 = str2.IsWhiteSpace(); // False
    /// </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWhiteSpace(this string value)
    {
        Guard.NotNull(value);
        return value.All(char.IsWhiteSpace);
    }

    /// <summary>
    ///     Capitalizes the first letter of each word in the given string.
    /// </summary>
    /// <param name="input">The string to capitalize.</param>
    /// <returns>
    ///     A new string where the first letter of each word is capitalized,
    ///     or the original string if it is null or consists only of whitespace.
    /// </returns>
    /// <remarks>
    ///     This method uses the System.Globalization.TextInfo.ToTitleCase method to capitalize the first letter of each word.
    ///     A 'word' is defined as any sequence of characters preceded by whitespace or the start of the string.
    ///     The method does not change the case of any other letters and handles culture-specific rules.
    ///     If the input string is null or contains only whitespace characters, the original string is returned.
    /// </remarks>
    /// <example>
    ///     <code>
    /// string input = "hello world";
    /// string capitalized = input.CapitalizeFirstLetterOfWords(); // Returns "Hello World"
    /// 
    /// string emptyInput = "";
    /// string result = emptyInput.CapitalizeFirstLetterOfWords(); // Returns ""
    /// 
    /// string nullInput = null;
    /// string nullResult = nullInput.CapitalizeFirstLetterOfWords(); // Returns null
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? CapitalizeFirstLetterOfWords(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }

    /// <summary>
    ///     Converts the specified <see cref="string" /> into a <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="value">The <see cref="string" /> to convert.</param>
    /// <returns>A <see cref="StringBuilder" /> containing the contents of the <paramref name="value" />.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// string originalString = "Hello, world!";
    /// StringBuilder stringBuilder = originalString.ToStringBuilder();
    /// stringBuilder.Append(" This is a StringBuilder.");
    /// Console.WriteLine(stringBuilder); // Output: Hello, world! This is a StringBuilder.
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="ToStringBuilder" /> method creates a new <see cref="StringBuilder" /> instance with the contents of
    ///     the specified <see cref="string" />. If the input <paramref name="value" /> is <c>null</c>, the resulting
    ///     <see cref="StringBuilder" /> will be empty.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder ToStringBuilder(this string? value)
    {
        return new StringBuilder(value);
    }

    /// <summary>
    ///     Appends a specified character to the given <see cref="string" /> until it reaches the specified target length.
    /// </summary>
    /// <param name="value">The <see cref="string" /> to which the character will be appended.</param>
    /// <param name="targetLength">The target length of the resulting <see cref="string" />.</param>
    /// <param name="charToAppend">The character to append to the <paramref name="value" />.</param>
    /// <returns>A <see cref="string" /> with the specified character appended to reach the target length.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// string originalString = "Hello";
    /// string result = originalString.AppendCharToLength(10, '-');
    /// Console.WriteLine(result); // Output: Hello-----
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="AppendCharToLength" /> method appends the specified <paramref name="charToAppend" /> to the
    ///     <paramref name="value" /> until the resulting <see cref="string" /> reaches the specified
    ///     <paramref name="targetLength" />. If the <paramref name="value" /> is already longer than or equal to the
    ///     <paramref name="targetLength" />, the method returns the original <paramref name="value" /> without any changes.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string AppendCharToLength(this string? value, int targetLength,
        char charToAppend)
    {
        var stringBuilder = new StringBuilder(value);
        return stringBuilder.AppendCharToLength(targetLength, charToAppend).ToString();
    }

    /// <summary>
    ///     Reduces the length of the given <see cref="string" /> to the specified maximum length using the underlying
    ///     <see cref="StringBuilder" /> method.
    /// </summary>
    /// <param name="value">The <see cref="string" /> to reduce in length.</param>
    /// <param name="maxLength">The maximum length of the resulting <see cref="string" />.</param>
    /// <returns>A <see cref="string" /> with a length reduced to the specified maximum length.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="maxLength" /> is less than 0.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// string originalString = "Hello, World!";
    /// string result = originalString.ReduceToLength(5);
    /// Console.WriteLine(result); // Output: Hello
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="ReduceToLength" /> method reduces the length of the <paramref name="value" /> to the specified
    ///     <paramref name="maxLength" />. If the <paramref name="value" /> is already shorter than or equal to the
    ///     <paramref name="maxLength" />, the method returns the original <paramref name="value" /> without any changes.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReduceToLength(this string value, int maxLength)
    {
        var stringBuilder = new StringBuilder(value);
        return stringBuilder.ReduceToLength(maxLength).ToString();
    }

    /// <summary>
    ///     Ensures the given <see cref="string" /> starts with the specified prefix.
    /// </summary>
    /// <param name="value">The <see cref="string" /> to ensure starts with the prefix.</param>
    /// <param name="prefix">The prefix to check and add if needed.</param>
    /// <param name="stringComparison">
    ///     An optional <see cref="StringComparison" /> enumeration value that determines how the
    ///     comparison is performed. The default is <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>A <see cref="string" /> that starts with the specified prefix.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// string originalString = "World";
    /// string result = originalString.EnsureStartsWith("Hello, ");
    /// Console.WriteLine(result); // Output: Hello, World
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="EnsureStartsWith" /> method checks if the <paramref name="value" /> starts with the specified
    ///     <paramref name="prefix" />. If it doesn't, the method adds the <paramref name="prefix" /> to the beginning of the
    ///     <paramref name="value" />. If the <paramref name="value" /> already starts with the <paramref name="prefix" />, the
    ///     method returns the original <paramref name="value" /> without any changes.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EnsureStartsWith(this string value, string prefix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        var stringBuilder = new StringBuilder(value);
        return stringBuilder.EnsureStartsWith(prefix, stringComparison).ToString();
    }

    /// <summary>
    ///     Ensures the given <see cref="string" /> ends with the specified suffix.
    /// </summary>
    /// <param name="value">The <see cref="string" /> to ensure ends with the suffix.</param>
    /// <param name="suffix">The suffix to check and add if needed.</param>
    /// <param name="stringComparison">
    ///     An optional <see cref="StringComparison" /> enumeration value that determines how the
    ///     comparison is performed. The default is <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>A <see cref="string" /> that ends with the specified suffix.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// string originalString = "Hello";
    /// string result = originalString.EnsureEndsWith(", World");
    /// Console.WriteLine(result); // Output: Hello, World
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="EnsureEndsWith" /> method checks if the <paramref name="value" /> ends with the specified
    ///     <paramref name="suffix" />. If it doesn't, the method appends the <paramref name="suffix" /> to the
    ///     <paramref name="value" />. If the <paramref name="value" /> already ends with the <paramref name="suffix" />, the
    ///     method returns the original <paramref name="value" /> without any changes.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EnsureEndsWith(this string value, string suffix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        var stringBuilder = new StringBuilder(value);
        return stringBuilder.EnsureEndsWith(suffix, stringComparison).ToString();
    }

    /// <summary>
    ///     Determines whether the specified <see cref="string" /> can be parsed as a valid <see cref="DateTime" />.
    /// </summary>
    /// <param name="input">The <see cref="string" /> to check for a valid date representation.</param>
    /// <returns>
    ///     <c>true</c> if the <paramref name="input" /> can be parsed as a valid <see cref="DateTime" />; otherwise,
    ///     <c>false</c>.
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// string validDate = "2022-01-01";
    /// string invalidDate = "invalid-date";
    /// bool validResult = validDate.IsDateTime(); // true
    /// bool invalidResult = invalidDate.IsDateTime(); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="IsDateTime" /> method checks if the provided <paramref name="input" /> is not <c>null</c> or empty
    ///     and can be successfully parsed as a <see cref="DateTime" /> using the
    ///     <see cref="DateTime.TryParse(string, out DateTime)" /> method.
    ///     Note that this method returns <c>false</c> if the input is <c>null</c> or an empty string.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDateTime(this string? input)
    {
        return !string.IsNullOrEmpty(input) && DateTime.TryParse(input, out _);
    }

    /// <summary>
    ///     Limits the length of the text to the specified maximum length.
    ///     If the source text is shorter than or equal to the maximum length, the entire source text is returned.
    ///     Otherwise, a substring of the source text is returned with a length equal to the specified maximum length.
    /// </summary>
    /// <param name="source">The source text. If null, a null value is returned.</param>
    /// <param name="maxLength">The maximum length for the returned string.</param>
    /// <returns>
    ///     A string that represents the limited length of the source text.
    ///     If <paramref name="source" /> is null, the method returns null.
    ///     If <paramref name="source" /> is shorter than or equal to <paramref name="maxLength" />, the method returns
    ///     <paramref name="source" />.
    ///     Otherwise, the method returns a substring of <paramref name="source" /> with length equal to
    ///     <paramref name="maxLength" />.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="maxLength" /> is less than 0.
    /// </exception>
    public static string? LimitLength(this string? source, int maxLength)
    {
        Guard.Minimum(maxLength, 0);

        if (source is null || source.Length <= maxLength)
        {
            return source;
        }

        return source[..maxLength];
    }

    /// <summary>
    ///     Removes all whitespace characters from the specified string.
    /// </summary>
    /// <param name="input">The string from which to remove whitespace.</param>
    /// <returns>A new string with all whitespace characters removed from the input string.</returns>
    /// <remarks>
    ///     This method iterates through each character in the input string and appends only non-whitespace characters to a new
    ///     string.
    ///     It's useful in scenarios where whitespace is not required or should be ignored, such as processing user input or
    ///     preparing strings for comparison.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     string input = "  Hello  World  ";
    ///     string result = RemoveWhitespace(input);
    ///     // result is "HelloWorld"
    ///     ]]></code>
    /// </example>
    public static string? RemoveWhitespace(string? input)
    {
        if (string.IsNullOrEmpty(input) || !input.Any(char.IsWhiteSpace))
        {
            return input;
        }

        var stringBuilder = new StringBuilder(input.Length);
        foreach (var c in input.Where(c => !char.IsWhiteSpace(c)))
        {
            stringBuilder.Append(c);
        }

        return stringBuilder.ToString();
    }
}