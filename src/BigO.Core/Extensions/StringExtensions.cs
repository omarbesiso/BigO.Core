using System.Net.Mail;
using System.Text;
using BigO.Core.Validation;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility methods for working with strings.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Indicates whether the specified string represents a valid <see cref="Guid" />.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <returns><c>true</c> if the string represents a valid <see cref="Guid" />; otherwise <c>false</c>.</returns>
    public static bool IsGuid(this string value)
    {
        return !string.IsNullOrWhiteSpace(value) && Guid.TryParse(value.AsSpan(), out _);
    }

    /// <summary>
    ///     Indicates whether the specified string represents a valid email.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <returns><c>true</c> if <paramref name="value" /> is a valid email; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value) && MailAddress.TryCreate(value, out _);
    }

    /// <summary>
    ///     Indicates whether the specified string represents a valid website URL.
    /// </summary>
    /// <param name="value">The string value to be checked.</param>
    /// <returns><c>true</c> if <paramref name="value" /> is a valid URL; otherwise, <c>false</c>.</returns>
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
    ///     Removes a specified set of characters from a string.
    /// </summary>
    /// <param name="value">The string from which characters are to eb removed.</param>
    /// <param name="charactersToExclude">The set of characters to be removed.</param>
    /// <returns>
    ///     If the string is empty or the list of characters to be excluded is null or empty then the original string is
    ///     returned, otherwise a string with the specified characters removed is returned.
    /// </returns>
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
    ///     Shuffles/Scrambles the characters in a string.
    /// </summary>
    /// <param name="value">The value of the string to be shuffled.</param>
    /// <returns>The shuffled string. If the string is <c>null</c> or empty then the string is returned as is.</returns>
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
    ///     Replicates the logic of T-SQL STUFF function. The STUFF function inserts a string into another string. It deletes a
    ///     specified length of characters in the first string at the start position and then inserts the second string into
    ///     the first string at the start position.
    /// </summary>
    /// <param name="value">The string value to be apply the replacement to.</param>
    /// <param name="start">The starting position for the replacement.</param>
    /// <param name="length">The number of characters to replace.</param>
    /// <param name="replaceWith">The string to insert at the specified position.</param>
    /// <returns>
    ///     The string  after removing the specified number of characters starting at the specified position and inserting
    ///     the replacement string at that position..
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     start - The value of {nameof(start)} cannot be less than 1 or
    ///     grater than {nameof(length)}.
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    ///     length - The value of {nameof(length)} cannot be less than 0 and
    ///     the {nameof(start)} + {nameof(length)} cannot be longer than the value string in length.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="replaceWith" /> is <c>null</c>.</exception>
    public static string Stuff(this string value, int start, int length, string replaceWith)
    {
        Guard.NotNull(value);
        Guard.NotNull(replaceWith);

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