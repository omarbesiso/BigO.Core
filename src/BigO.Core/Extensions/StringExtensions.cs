using System.Net.Mail;
using System.Text;
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
}