using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility methods for working with enums.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    /// <summary>
    ///     Converts an enumeration to a <see cref="NameValueCollection" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The enumeration type to convert. Must be a <c>struct</c> that implements
    ///     <see cref="IConvertible" />.
    /// </typeparam>
    /// <exception cref="ArgumentException">Thrown when <typeparamref name="T" /> is not an enumeration.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the enum name not returned from <see cref="Enum.GetName" />.</exception>
    /// <returns>A <see cref="NameValueCollection" /> containing the names and values of the enumeration.</returns>
    public static NameValueCollection ToList<T>() where T : struct, IConvertible
    {
        var enumType = typeof(T);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("The type of T must be an enum.");
        }

        var result = new NameValueCollection();
        var values = Enum.GetValues(enumType);

        foreach (var value in values)
        {
            var memInfo = enumType.GetMember(enumType.GetEnumName(value) ?? throw new InvalidOperationException());
            var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = descriptionAttributes.Length > 0
                ? ((DescriptionAttribute)descriptionAttributes.First()).Description
                : value.ToString();
            result.Add(description, value.ToString());
        }

        return result;
    }

    /// <summary>
    ///     Gets the description of an enumeration value.
    /// </summary>
    /// <param name="value">The enumeration value to get the description of.</param>
    /// <returns>The description of <paramref name="value" />, or its string representation if no description is found.</returns>
    /// <remarks>
    ///     This method uses the <see cref="DescriptionAttribute" /> to retrieve the description associated with the
    ///     enumeration value.
    ///     If the attribute is not found, the method returns the string representation of the enumeration value by calling
    ///     <see cref="Enum.ToString()" />
    ///     It also requires that the enumeration is decorated with <see cref="DescriptionAttribute" /> and the value passed to
    ///     this method should be a valid enumeration value.
    /// </remarks>
    public static string GetEnumDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());

        if (fi == null)
        {
            return value.ToString();
        }

        if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes &&
            attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }

    /// <summary>
    ///     Returns the <see cref="DisplayAttribute.Name" /> for a given <see cref="Enum" /> value.
    /// </summary>
    /// <param name="value">The <see cref="Enum" /> value to retrieve the <see cref="DisplayAttribute.Name" /> for.</param>
    /// <returns>
    ///     The <see cref="DisplayAttribute.Name" /> for the given <see cref="Enum" /> value. If the
    ///     <paramref name="value" /> is <c>null</c>, or the <see cref="DisplayAttribute.Name" /> is not found, an empty string
    ///     is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="value" /> is <c>null</c>, the exception will be thrown.</exception>
    /// <remarks>
    ///     This extension method allows for more readable and maintainable code by returning the user-friendly name of an Enum
    ///     value, as defined by the <see cref="DisplayAttribute.Name" />.
    ///     Instead of using the string representation of the Enum value, which may be changed in the future, the user-friendly
    ///     name can be used instead.
    /// </remarks>
    public static string? DisplayName(this Enum? value)
    {
        if (value == null)
        {
            return null;
        }

        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();

        return memberInfo?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? string.Empty;
    }
}