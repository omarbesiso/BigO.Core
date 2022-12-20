using System.Net.Mail;
using System.Text;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IComparable{T}" /> objects.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Determines whether the given string is a valid <see cref="Guid" />.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid <see cref="Guid" />; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method uses the <see cref="Guid.TryParse(ReadOnlySpan{char}, out Guid)" /> and
    ///     <see cref="Guid.TryParseExact(string, string, out Guid)" /> methods to check if the given string is a valid
    ///     <see cref="Guid" />.
    ///     If the string is <c>null</c> or empty, the method returns <c>false</c>.
    /// </remarks>
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
    ///     Determines whether the given string is a valid email address.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid email address; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method uses the <see cref="MailAddress.TryCreate(string, out MailAddress)" /> method to check if the given
    ///     string is a valid email address.
    ///     If the string is <c>null</c> or empty, the method returns <c>false</c>.
    /// </remarks>
    public static bool IsValidEmail(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value) && MailAddress.TryCreate(value, out _);
    }

    /// <summary>
    ///     Determines whether the given string is a valid URL for a website.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid URL for a website; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method uses the <see cref="Uri.TryCreate(string, UriKind, out Uri)" /> method to check if the given string is
    ///     a valid URL for a website.
    ///     The URL must have the scheme "http" or "https".
    ///     If the string is <c>null</c> or empty, the method returns <c>false</c>.
    /// </remarks>
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
    ///     Removes specified characters from the string.
    /// </summary>
    /// <param name="value">The string to remove the characters from.</param>
    /// <param name="charactersToExclude">The characters to remove from the string.</param>
    /// <returns>The string with the specified characters removed.</returns>
    /// <remarks>
    ///     This method iterates through the <paramref name="value" /> string and removes any occurrences of the specified
    ///     characters.
    ///     If the <paramref name="value" /> string is <c>null</c> or empty, or if <paramref name="charactersToExclude" /> is
    ///     <c>null</c> or empty, the method returns the original string.
    /// </remarks>
    public static string RemoveCharacters(this string value, params char[]? charactersToExclude)
    {
        if (string.IsNullOrEmpty(value) || charactersToExclude.IsNullOrEmpty())
        {
            return value;
        }

        var originalStringSpan = value.AsSpan();
        var excludedCharacterSpan = charactersToExclude.AsSpan();

        StringBuilder builder = new();

        foreach (var character in originalStringSpan)
        {
            if (!excludedCharacterSpan.Contains(character))
            {
                builder.Append(character);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    ///     Shuffles the characters in the string.
    /// </summary>
    /// <param name="value">The string to shuffle.</param>
    /// <returns>The shuffled string.</returns>
    /// <remarks>
    ///     This method converts the <paramref name="value" /> string to a character array and shuffles the characters using
    ///     the Fisher-Yates shuffle algorithm.
    ///     It then rebuilds the string using the shuffled character array.
    ///     If the <paramref name="value" /> string is <c>null</c> or empty, the method returns the original string.
    /// </remarks>
    public static string Shuffle(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var shuffledCharacterArray = value.ToCharArray().Shuffle();

        StringBuilder builder = new();

        foreach (var character in shuffledCharacterArray)
        {
            builder.Append(character);
        }

        return builder.ToString();
    }

    /// <summary>
    ///     Replaces a specified number of characters in the string with a specified replacement string.
    /// </summary>
    /// <param name="value">The string to modify.</param>
    /// <param name="start">The starting position of the characters to replace. The first character is 1.</param>
    /// <param name="length">The number of characters to replace.</param>
    /// <param name="replaceWith">The replacement string.</param>
    /// <returns>The modified string.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="value" /> or <paramref name="replaceWith" /> is
    ///     <c>null</c> or empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="start" /> is less than 1 or greater than the length of the <paramref name="value" />
    ///     string,
    ///     or if <paramref name="length" /> is less than 0 or the sum of <paramref name="start" /> and
    ///     <paramref name="length" /> is greater than the length of the <paramref name="value" /> string.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="StringBuilder.Replace(string, string, int, int)" /> method to replace a specified
    ///     number of characters in the <paramref name="value" /> string with the <paramref name="replaceWith" /> string.
    /// </remarks>
    public static string Stuff(this string value, int start, int length, string replaceWith)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (string.IsNullOrWhiteSpace(replaceWith))
        {
            throw new ArgumentNullException(nameof(replaceWith));
        }

        if (start < 1 || start > value.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start),
                $"The value of {nameof(start)} cannot be less than 1 or grater than {nameof(length)}.");
        }

        if (length < 0 || start + length - 1 > value.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length),
                $"The value of {nameof(length)} cannot be less than 0 and the {nameof(start)} + {nameof(length)} cannot be longer than the value string in length.");
        }

        var sb = new StringBuilder(value);
        sb.Replace(value.Substring(start - 1, length), replaceWith, start - 1, length);

        return sb.ToString();
    }
}