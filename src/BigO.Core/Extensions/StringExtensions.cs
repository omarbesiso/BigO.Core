using System.Net.Mail;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="string" /> objects.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Determines whether the specified <see cref="string" /> value is a valid <see cref="Guid" />.
    /// </summary>
    /// <param name="value">The <see cref="string" /> value to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified value is a valid <see cref="Guid" />; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method uses the <see cref="Guid.TryParse(ReadOnlySpan{char}, out Guid)" /> and
    ///     <see cref="Guid.TryParseExact(string, string, out Guid)" /> methods to check whether the
    ///     specified value is a valid <see cref="Guid" />. If either method returns <c>true</c>, the
    ///     <see cref="IsGuid" /> method will also return <c>true</c>. If both methods return <c>false</c>,
    ///     the <see cref="IsGuid" /> method will return <c>false</c>.
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
    ///     Determines whether the specified string is a valid email address.
    /// </summary>
    /// <param name="value">The string to be checked for a valid email address.</param>
    /// <returns><c>true</c> if the specified string is a valid email address; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method uses the <see cref="MailAddress.TryCreate(string, out MailAddress)" /> method to check if the given
    ///     string is a valid email address. It returns <c>true</c> if the string is not <c>null</c> or whitespace and is a
    ///     valid email address according to the <see cref="MailAddress" /> class. Otherwise, it returns <c>false</c>.
    /// </remarks>
    public static bool IsValidEmail(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value) && MailAddress.TryCreate(value, out _);
    }

    /// <summary>
    ///     Determines whether the specified value is a valid website URL.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>true if the specified value is a valid website URL; otherwise, false.</returns>
    /// <remarks>
    ///     This method checks if the specified value is a valid absolute URI with either the "http" or "https" scheme.
    ///     If the value is <c>null</c> or whitespace, this method returns false.
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
    ///     Determines whether the specified string consists of only whitespace characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the specified string consists of only whitespace characters; otherwise, false.</returns>
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
}