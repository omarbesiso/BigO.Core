using BigO.Core.Validation;

namespace BigO.Core.Types;

/// <summary>
///     Represents a type to use when specifying an email address.
/// </summary>
public readonly record struct EmailAddress : IEquatable<string>, IComparable, IComparable<string>,
    IComparable<EmailAddress>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmailAddress" /> struct.
    /// </summary>
    /// <param name="email">The email.</param>
    public EmailAddress(string email)
    {
        Guard.NotNullOrWhiteSpace(email);
        Guard.Email(email);

        Value = email.Trim().ToLowerInvariant();
    }

    /// <summary>
    ///     The string value of the email address.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Compares this instance to a specified object and returns an integer that indicates whether this instance is less
    ///     than, equal to, or greater than the specified object.
    /// </summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     Less than zero: This instance is less than `obj`. Zero: This instance is equal to `obj`. Greater than zero: This
    ///     instance is greater than `obj`.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the specified object is not a email address or string.</exception>
    public int CompareTo(object? obj)
    {
        return obj == null ? 1 : string.CompareOrdinal(Value, obj.ToString());
    }

    /// <summary>
    ///     Compares this instance to a specified `EmailAddress` object and returns an integer that indicates whether this
    ///     instance is less than, equal to, or greater than the specified object.
    /// </summary>
    /// <param name="other">The `EmailAddress` object to compare to this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     Less than zero: This instance is less than `other`. Zero: This instance is equal to `other`. Greater than zero:
    ///     This instance is greater than `other`.
    /// </returns>
    public int CompareTo(EmailAddress other)
    {
        return string.CompareOrdinal(Value, other.Value);
    }

    /// <summary>
    ///     Compares this instance to a specified string and returns an integer that indicates whether this instance is less
    ///     than, equal to, or greater than the specified string.
    /// </summary>
    /// <param name="other">The string to compare to this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     Less than zero: This instance is less than `other`. Zero: This instance is equal to `other`. Greater than zero:
    ///     This instance is greater than `other`.
    /// </returns>
    public int CompareTo(string? other)
    {
        return string.CompareOrdinal(Value, other);
    }

    /// <summary>
    ///     Indicates whether this instance and a specified string represent the same value.
    /// </summary>
    /// <param name="other">The string to compare to this instance.</param>
    public bool Equals(string? other)
    {
        return string.Equals(Value, other);
    }

    /// <summary>
    ///     Indicates whether this instance and a specified string represent the same value.
    /// </summary>
    /// <param name="other">The other <see cref="EmailAddress" /> instance.</param>
    /// <returns>bool.</returns>
    public bool Equals(EmailAddress? other)
    {
        return string.Equals(Value, other?.Value);
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return string.GetHashCode(Value);
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
    public static implicit operator EmailAddress(string? emailAddress)
    {
        return new EmailAddress(emailAddress!);
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>The name represented by this value object.</returns>
    public override string ToString()
    {
        return Value;
    }
}