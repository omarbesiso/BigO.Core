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

    /// <summary>
    ///     Compares this instance with a specified <see cref="EmailAddress" /> and indicates whether this instance precedes,
    ///     follows, or appears in the same position in the sort order.
    /// </summary>
    /// <param name="other">An <see cref="EmailAddress" /> to compare.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared.
    ///     The return value has these meanings: Less than zero: This instance precedes <paramref name="other" /> in the sort
    ///     order.
    ///     Zero: This instance occurs in the same position in the sort order as <paramref name="other" />.
    ///     Greater than zero: This instance follows <paramref name="other" /> in the sort order.
    /// </returns>
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
        Guard.NotNullOrWhiteSpace(email);
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
    ///     Performs an implicit conversion from <see cref="EmailAddress" /> to
    ///     <see cref="string" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string?(EmailAddress? emailAddress)
    {
        return emailAddress?.Value;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="EmailAddress" /> to
    ///     <see cref="string" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="string" /> to
    ///     <see cref="EmailAddress" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator EmailAddress?(string? emailAddress)
    {
        return emailAddress is not null ? Create(emailAddress) : default;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="string" /> to
    ///     <see cref="EmailAddress" />.
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
    /// <returns>
    ///     <c>true</c> if the specified string is a valid email address; otherwise, <c>false</c>.
    /// </returns>
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
    ///         <item>
    ///             <description>The local part of the email address (before the '@' symbol).</description>
    ///         </item>
    ///         <item>
    ///             <description>The full domain part of the email address (after the '@' symbol).</description>
    ///         </item>
    ///         <item>
    ///             <description>The second-level domain (SLD), which is typically the main domain name.</description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 The top-level domain (TLD), which could be a standard TLD (e.g., .com) or a country-code TLD
    ///                 (ccTLD) (e.g., .co.uk).
    ///             </description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <exception cref="FormatException">
    ///     Thrown if the email address is not in a valid format. This can occur if:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>The email address does not contain exactly one '@' symbol.</description>
    ///         </item>
    ///         <item>
    ///             <description>The domain part does not contain at least one '.' symbol.</description>
    ///         </item>
    ///         <item>
    ///             <description>The domain part has an invalid structure (e.g., missing SLD or TLD).</description>
    ///         </item>
    ///     </list>
    /// </exception>
    /// <example>
    ///     For an email address like "user@mail.example.co.uk", the method returns:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>LocalPart: "user"</description>
    ///         </item>
    ///         <item>
    ///             <description>Domain: "mail.example.co.uk"</description>
    ///         </item>
    ///         <item>
    ///             <description>SecondLevelDomain: "example"</description>
    ///         </item>
    ///         <item>
    ///             <description>TopLevelDomain: "co.uk"</description>
    ///         </item>
    ///     </list>
    /// </example>
    public (string LocalPart, string Domain, string SecondLevelDomain, string TopLevelDomain) GetParts()
    {
        var parts = Value.Split('@');
        if (parts.Length != 2)
        {
            throw new FormatException("The email address must contain exactly one '@' symbol.");
        }

        var localPart = parts[0];
        var domain = parts[1];

        var domainParts = domain.Split('.');
        if (domainParts.Length < 2)
        {
            throw new FormatException("The domain part must contain at least one '.' symbol.");
        }

        string topLevelDomain;
        string secondLevelDomain;

        switch (domainParts.Length)
        {
            case 2:
                // Example: example.com
                secondLevelDomain = domainParts[0];
                topLevelDomain = domainParts[1];
                break;
            case 3:
                // Example: example.co.uk
                secondLevelDomain = domainParts[1];
                topLevelDomain = $"{domainParts[1]}.{domainParts[2]}";
                break;
            default:
                // Example: mail.example.co.uk
                secondLevelDomain = domainParts[^3];
                topLevelDomain = $"{domainParts[^2]}.{domainParts[^1]}";
                break;
        }

        return (localPart, domain, secondLevelDomain, topLevelDomain);
    }

    /// <summary>
    ///     Checks if the email address is from a specific domain.
    /// </summary>
    /// <param name="domain">The domain to check against.</param>
    /// <returns><c>true</c> if the email address is from the specified domain; otherwise, <c>false</c>.</returns>
    public bool IsFromDomain(string domain)
    {
        return Value.EndsWith($"@{domain}", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Gets the username part of the email address (before the '@' symbol).
    /// </summary>
    /// <returns>The username part of the email address.</returns>
    public string GetUsername()
    {
        var parts = Value.Split('@');
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid email format.");
        }

        return parts[0];
    }
}