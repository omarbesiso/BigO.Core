using System.Net.Mail;
using System.Text;
using BigO.Core.Extensions;
using BigO.Core.Validation;

namespace BigO.Core.Types;

/// <summary>
///     Represents a type to use when specifying an email address.
/// </summary>
[PublicAPI]
public readonly record struct EmailAddress : IComparable<EmailAddress>
{
    /// <summary>
    ///     The maximum length of an email according to the RFC 5321.
    /// </summary>
    public const int MaxLength = 254;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmailAddress" /> struct.
    /// </summary>
    /// <param name="value">The email value.</param>
    private EmailAddress(string value)
    {
        Value = value;
    }

    /// <summary>
    ///     The string value of the email address.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public int CompareTo(EmailAddress other)
    {
        return string.CompareOrdinal(Value, other.Value);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="EmailAddress" /> struct after validation and sanitization.
    /// </summary>
    /// <param name="email">The email address to create.</param>
    /// <returns>A new instance of <see cref="EmailAddress" />.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the provided value is not a valid email address.
    /// </exception>
    public static EmailAddress Create(string email)
    {
        // Example usage of .NET 7+ specialized throw method (optional)
        // #if NET7_0_OR_GREATER
        // ArgumentNullException.ThrowIfNullOrWhiteSpace(email);
        // #else
        Guard.NotNullOrWhiteSpace(email);
        // #endif

        Guard.MaxLength(email, MaxLength);
        Guard.EmailAddress(email);

        var sanitizedEmail = SanitizeEmailAddress(email);
        return new EmailAddress(sanitizedEmail);
    }

    /// <summary>
    ///     Converts this <see cref="EmailAddress" /> instance to a <see cref="MailAddress" />.
    /// </summary>
    /// <returns>A <see cref="MailAddress" /> that represents the current email address.</returns>
    public MailAddress ToMailAddress()
    {
        return new MailAddress(Value);
    }

    /// <summary>
    ///     Converts this <see cref="EmailAddress" /> instance to a <see cref="MailAddress" />, with a display name.
    /// </summary>
    /// <param name="displayName">The display name to be used with the email address.</param>
    /// <returns>A <see cref="MailAddress" /> that represents the current email address with the specified display name.</returns>
    public MailAddress ToMailAddress(string? displayName)
    {
        return new MailAddress(Value, displayName);
    }

    /// <summary>
    ///     Converts this <see cref="EmailAddress" /> instance to a <see cref="MailAddress" />, with a display name and
    ///     encoding.
    /// </summary>
    /// <param name="displayName">The display name to be used with the email address.</param>
    /// <param name="displayNameEncoding">The encoding to be used for the display name.</param>
    /// <returns>
    ///     A <see cref="MailAddress" /> that represents the current email address with the specified display name and
    ///     encoding.
    /// </returns>
    public MailAddress ToMailAddress(string? displayName, Encoding? displayNameEncoding)
    {
        return new MailAddress(Value, displayName, displayNameEncoding);
    }

    /// <summary>
    ///     Sanitizes the given email address by trimming whitespace and converting it to lower case.
    /// </summary>
    /// <param name="emailAddress">The email address to sanitize.</param>
    /// <returns>The sanitized email address.</returns>
    private static string SanitizeEmailAddress(string emailAddress)
    {
        return emailAddress.Trim().ToLowerInvariant();
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="EmailAddress" /> to <see cref="string" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string?(EmailAddress? emailAddress)
    {
        return emailAddress?.Value;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="EmailAddress" /> to <see cref="string" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="string" /> to <see cref="EmailAddress" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator EmailAddress?(string? emailAddress)
    {
        return emailAddress is not null ? Create(emailAddress) : default;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="string" /> to <see cref="EmailAddress" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator EmailAddress(string emailAddress)
    {
        return Create(emailAddress);
    }

    /// <summary>
    ///     Parses the specified email address string and returns an <see cref="EmailAddress" /> instance.
    /// </summary>
    /// <param name="email">The email address string to parse.</param>
    /// <returns>An <see cref="EmailAddress" /> instance representing the specified email address.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the provided value is not a valid email address.
    /// </exception>
    public static EmailAddress Parse(string email)
    {
        return Create(email);
    }

    /// <summary>
    ///     Returns a string that represents the current email address.
    /// </summary>
    /// <returns>The string representation of the email address.</returns>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    ///     Determines whether the specified string is a valid email address.
    /// </summary>
    /// <param name="email">The email address string to validate.</param>
    /// <returns><c>true</c> if the specified string is a valid email address; otherwise, <c>false</c>.</returns>
    public static bool IsEmailAddressValid(string email)
    {
        return email.IsValidEmail();
    }

    /// <summary>
    ///     Gets the local part, domain, second-level domain (SLD), and top-level domain (TLD) of the email address.
    /// </summary>
    /// <returns>
    ///     A tuple containing:
    ///     <list type="bullet">
    ///         <item>The local part of the email address (before the '@' symbol).</item>
    ///         <item>The full domain part of the email address (after the '@' symbol).</item>
    ///         <item>The second-level domain (SLD), which is typically the main domain name.</item>
    ///         <item>The top-level domain (TLD), which could be a standard TLD (e.g., .com) or a ccTLD (e.g., .co.uk).</item>
    ///     </list>
    /// </returns>
    /// <exception cref="FormatException">
    ///     Thrown if the email address format is invalid (e.g., missing '@', missing '.', etc.).
    /// </exception>
    public (string LocalPart, string Domain, string SecondLevelDomain, string TopLevelDomain) GetParts()
    {
        // Use a helper method for clarity
        var (localPart, domain) = SplitEmail(Value);
        var (sld, tld) = ExtractDomainParts(domain);
        return (localPart, domain, sld, tld);
    }

    /// <summary>
    ///     Checks if the email address is from a specific domain.
    /// </summary>
    /// <param name="domain">The domain to check against (e.g., "example.com").</param>
    /// <returns><c>true</c> if the email address is from the specified domain; otherwise, <c>false</c>.</returns>
    public bool IsFromDomain(string domain)
    {
        // Current implementation:
        // return Value.EndsWith($"@{domain}", StringComparison.OrdinalIgnoreCase);

        // Optional stricter match example:
        var parts = SplitEmail(Value);
        return parts.domain.Equals(domain, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Gets the username part of the email address (before the '@' symbol).
    /// </summary>
    /// <returns>The username part of the email address.</returns>
    /// <exception cref="FormatException">If the email format is invalid.</exception>
    public string GetUsername()
    {
        var (localPart, _) = SplitEmail(Value);
        return localPart;
    }

    #region Private Helpers

    /// <summary>
    ///     Splits an email into local part and domain.
    /// </summary>
    /// <param name="email">The full email address.</param>
    /// <returns>A tuple of (localPart, domain).</returns>
    /// <exception cref="FormatException">If '@' is missing or there's more than one '@'.</exception>
    private static (string localPart, string domain) SplitEmail(string email)
    {
        var parts = email.Split('@');
        if (parts.Length != 2)
        {
            throw new FormatException("The email address must contain exactly one '@' symbol.");
        }

        return (parts[0], parts[1]);
    }

    /// <summary>
    ///     Extracts the SLD and TLD from the domain.
    /// </summary>
    /// <param name="domain">The domain part of the email.</param>
    /// <returns>A tuple of (SecondLevelDomain, TopLevelDomain).</returns>
    /// <exception cref="FormatException">If the domain is missing '.' or is otherwise invalid.</exception>
    private static (string sld, string tld) ExtractDomainParts(string domain)
    {
        var domainParts = domain.Split('.');
        if (domainParts.Length < 2)
        {
            throw new FormatException("The domain part must contain at least one '.' symbol.");
        }

        // Pattern matching on domainParts.Length for clarity
        return domainParts.Length switch
        {
            // e.g., example.com
            2 => (domainParts[0], domainParts[1]),
            // e.g., example.co.uk
            3 => (domainParts[1], $"{domainParts[1]}.{domainParts[2]}"),
            // e.g., mail.example.co.uk or more subdomains
            _ =>
            (
                domainParts[^3], // second-level domain (e.g., "example")
                $"{domainParts[^2]}.{domainParts[^1]}" // top-level domain (e.g., "co.uk")
            )
        };
    }

    #endregion
}