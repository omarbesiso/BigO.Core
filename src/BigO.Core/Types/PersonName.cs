using System.Text;
using BigO.Core.Extensions;
using BigO.Core.Validation;

namespace BigO.Core.Types;

/// <summary>
///     Represents a person's name, including title, first name, middle name, and last name.
/// </summary>
[PublicAPI]
public readonly record struct PersonName : IComparable<PersonName>
{
    private const int MaximumNameLength = 100;

    /// <summary>
    ///     Initializes a new instance of the PersonName struct with first name, last name, and optionally middle name and
    ///     title.
    /// </summary>
    /// <param name="firstName">The first name of the person.</param>
    /// <param name="lastName">The last name of the person.</param>
    /// <param name="middleName">The middle name of the person (optional).</param>
    /// <param name="title">The title of the person (optional).</param>
    /// <exception cref="ArgumentException">Thrown when first name or last name is null, empty, or whitespace.</exception>
    public PersonName(string firstName, string lastName, string? middleName = null,
        PersonTitle title = PersonTitle.Unknown)
    {
        Title = title; // Title is optional and nullable

        Guard.NotNullOrWhiteSpace(firstName, nameof(firstName));
        FirstName = CapitalizeAndTrim(firstName);

        if (!string.IsNullOrWhiteSpace(middleName))
        {
            Guard.MaxLength(middleName, MaximumNameLength, nameof(middleName));
            MiddleName = CapitalizeAndTrim(middleName);
        }
        else
        {
            MiddleName = null;
        }

        Guard.NotNullOrWhiteSpace(lastName, nameof(lastName));
        LastName = CapitalizeAndTrim(lastName);
    }

    public PersonName(string firstName, string middleName, string lastName) : this(firstName, lastName, middleName,
        PersonTitle.Unknown)
    {
    }

    public PersonName(PersonTitle title, string firstName, string lastName) : this(firstName, lastName, title: title)
    {
    }

    public PersonName(PersonTitle title, string firstName, string middleName, string lastName) :
        this(firstName, lastName, middleName, title)
    {
    }

    /// <summary>
    ///     Gets the title of the person.
    /// </summary>
    public PersonTitle Title { get; }

    /// <summary>
    ///     Gets the first name of the person.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    ///     Gets the optional middle name of the person.
    /// </summary>
    public string? MiddleName { get; }

    /// <summary>
    ///     Gets the last name of the person.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    ///     Gets the full name of the person, including the title if known.
    /// </summary>
    public string FullName => GetFullName();

    /// <summary>
    ///     Compares this instance with another PersonName instance and returns an integer that indicates whether this instance
    ///     precedes, follows, or appears in the same position in the sort order as the other instance.
    /// </summary>
    /// <param name="other">The PersonName to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(PersonName other)
    {
        return string.CompareOrdinal(FullName, other.FullName);
    }

    private static string CapitalizeAndTrim(string value)
    {
        var trimmedValue = value.Trim();
        return char.ToUpper(trimmedValue[0]) + trimmedValue[1..].ToLowerInvariant();
    }

    private string GetFullName()
    {
        StringBuilder sb = new();

        if (Title != PersonTitle.Unknown)
        {
            sb.Append(Title.GetEnumDisplay());
            sb.Append(' ');
        }

        sb.Append(FirstName);
        sb.Append(' ');

        if (!string.IsNullOrWhiteSpace(MiddleName))
        {
            sb.Append(MiddleName);
            sb.Append(' ');
        }

        sb.Append(LastName);

        return sb.ToString();
    }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return FullName;
    }

    /// <summary>
    ///     Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Title, FirstName, MiddleName, LastName);
    }
}