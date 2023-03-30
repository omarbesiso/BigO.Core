using System.Net.Mail;
using System.Text;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="string" /> objects.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
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
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value" /> is null.</exception>
    /// <remarks>
    ///     This method uses the <see cref="MailAddress" /> class to validate whether the specified string is a valid email
    ///     address.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use <see cref="IsValidEmail(string)" /> method.
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
    ///     Determines whether the specified string consists of whitespace characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the specified string consists of whitespace characters; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
    /// <remarks>
    ///     This method checks whether a string consists of only whitespace characters. It considers a character to be a
    ///     whitespace
    ///     character if <see cref="char.IsWhiteSpace(char)" /> returns true.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use <see cref="IsWhiteSpace(string)" /> method to check if a string consists
    ///     of
    ///     whitespace characters:
    ///     <code><![CDATA[
    /// string str1 = "   ";
    /// string str2 = "  example  ";
    /// 
    /// bool result1 = str1.IsWhiteSpace(); // True
    /// bool result2 = str2.IsWhiteSpace(); // False
    /// ]]></code>
    /// </example>
    public static bool IsWhiteSpace(this string value)
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Capitalizes the first letter of each word in the given input string.
    /// </summary>
    /// <param name="input">The input string to capitalize.</param>
    /// <returns>
    ///     A new string with the first letter of each word capitalized or the original string if it is <c>null</c> or
    ///     whitespace.
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// string input = "hello world";
    /// string capitalized = CapitalizeFirstLetterOfWords(input); // Returns "Hello World"
    /// 
    /// string emptyInput = "";
    /// string result = CapitalizeFirstLetterOfWords(emptyInput); // Returns ""
    /// 
    /// string nullInput = null;
    /// string nullResult = CapitalizeFirstLetterOfWords(nullInput); // Returns null
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="CapitalizeFirstLetterOfWords" /> method iterates through the input string and capitalizes the first
    ///     letter of each word. If the input string is <c>null</c> or contains only whitespace, the method returns the
    ///     original string without making any changes. This method does not handle complex capitalization rules, such as those
    ///     found in proper nouns or acronyms. It only capitalizes the first letter of each word separated by whitespace.
    /// </remarks>
    public static string? CapitalizeFirstLetterOfWords(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        var sb = new StringBuilder(input.Length);
        var capitalizeNext = true;

        foreach (var currentChar in input)
        {
            if (char.IsWhiteSpace(currentChar))
            {
                capitalizeNext = true;
                sb.Append(currentChar);
            }
            else if (capitalizeNext)
            {
                sb.Append(char.ToUpper(currentChar));
                capitalizeNext = false;
            }
            else
            {
                sb.Append(currentChar);
            }
        }

        return sb.ToString();
    }
}