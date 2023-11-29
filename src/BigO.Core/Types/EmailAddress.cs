using System.Net.Mail;
using System.Text;
using BigO.Core.Validation;

namespace BigO.Core.Types;

/// <summary>
///     Represents a type to use when specifying an email address.
/// </summary>
[PublicAPI]
public readonly record struct EmailAddress : IEquatable<string>, IComparable, IComparable<string>,
    IComparable<EmailAddress>
{
    public const string EmailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                     + "@"
                                     + @"((([\w]+([-\w]*[\w]+)*\.)+[a-zA-Z]+)|"
                                     + @"((([01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]).){3}[01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]))\z";

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmailAddress" /> struct.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if the provided value is not a valid email address.
    /// </exception>
    public EmailAddress(string email)
    {
        Guard.NotNullOrWhiteSpace(email);
        Guard.Email(email);

        Value = SanitizeEmailAddress(email);
    }

    /// <summary>
    ///     The string value of the email address.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Compares this instance with a specified <see cref="object" /> and indicates whether this instance precedes,
    ///     follows, or appears in the same position in the sort order.
    /// </summary>
    /// <param name="obj">An <see cref="object" /> to compare, or null.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared.
    ///     The return value has these meanings: Less than zero: This instance precedes <paramref name="obj" /> in the sort
    ///     order.
    ///     Zero: This instance occurs in the same position in the sort order as <paramref name="obj" />.
    ///     Greater than zero: This instance follows <paramref name="obj" /> in the sort order or <paramref name="obj" /> is
    ///     null.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     <paramref name="obj" /> is not the same type as this instance.
    /// </exception>
    public int CompareTo(object? obj)
    {
        return obj is EmailAddress other ? CompareTo(other) : 1;
    }

    /// <summary>
    ///     Compares this instance with a specified <see cref="EmailAddress" /> object and indicates whether this instance
    ///     precedes, follows, or appears in the same position in the sort order.
    /// </summary>
    /// <param name="other">An <see cref="EmailAddress" /> to compare with this instance.</param>
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
    ///     Compares this instance with a specified string and indicates whether this instance precedes, follows, or appears in
    ///     the same position in the sort order.
    /// </summary>
    /// <param name="other">A string to compare with this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared.
    ///     The return value has these meanings: Less than zero: This instance precedes <paramref name="other" /> in the sort
    ///     order.
    ///     Zero: This instance occurs in the same position in the sort order as <paramref name="other" />.
    ///     Greater than zero: This instance follows <paramref name="other" /> in the sort order, or <paramref name="other" />
    ///     is null.
    /// </returns>
    public int CompareTo(string? other)
    {
        return string.CompareOrdinal(Value, other);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="EmailAddress" /> instance is equal to the current instance.
    /// </summary>
    /// <param name="other">The <see cref="EmailAddress" /> instance to compare with the current instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="EmailAddress" /> is equal to the current instance; otherwise,
    ///     <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     Equality is based on the string value of the email address. This method compares the 'Value' property of the
    ///     current instance
    ///     with that of the specified instance, ignoring differences in case and leading/trailing whitespace.
    /// </remarks>
    public bool Equals(EmailAddress other)
    {
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Indicates whether this instance and a specified string represent the same value.
    /// </summary>
    /// <param name="other">The string to compare to this instance.</param>
    public bool Equals(string? other)
    {
        return string.Equals(Value, other, StringComparison.OrdinalIgnoreCase);
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
    public static string SanitizeEmailAddress(string emailAddress)
    {
        return emailAddress.Trim().ToLowerInvariant();
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Returns a string that represents the email address.
    /// </summary>
    /// <returns>The email address represented by this value object.</returns>
    public override string ToString()
    {
        return Value;
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
        return emailAddress is not null ? new EmailAddress(emailAddress) : default;
    }

    /// <summary>
    ///     Performs an implicit conversion from <see cref="string" /> to
    ///     <see cref="EmailAddress" />.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator EmailAddress(string emailAddress)
    {
        return new EmailAddress(emailAddress);
    }
}