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
    ///     Creates a dictionary of the names and values of an enumeration with optional descriptions.
    /// </summary>
    /// <typeparam name="T">
    ///     The enumeration type to convert. Must be a <c>struct</c> that implements
    ///     <see cref="IConvertible" />.
    /// </typeparam>
    /// <returns>
    ///     A <see cref="Dictionary{TKey, TValue}" /> with keys representing the enumeration descriptions (or names if no
    ///     description is available) and values representing the enumeration values as strings.
    /// </returns>
    /// <exception cref="System.ArgumentException">Thrown when the type of T is not an enum.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// public enum Colors
    /// {
    ///     [Description("Red Color")]
    ///     Red,
    ///     [Description("Green Color")]
    ///     Green,
    ///     [Description("Blue Color")]
    ///     Blue
    /// }
    /// 
    /// var colorList = ToDictionary<Colors>();
    /// 
    /// // colorList contains the following key-value pairs:
    /// // "Red Color" => "Red", "Green Color" => "Green", "Blue Color" => "Blue"
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method creates a <see cref="Dictionary{TKey, TValue}" /> containing the names and values of the specified
    ///     enumeration type <typeparamref name="T" />. If the enumeration members have a <see cref="DescriptionAttribute" />,
    ///     the attribute's description is used as the key. If no description is provided, the enumeration name is used as the
    ///     key. The values are the enumeration member names. The method is useful when creating a human-readable
    ///     representation of the enumeration.
    /// </remarks>
    public static Dictionary<string, string> ToDictionary<T>() where T : struct, IConvertible
    {
        var enumType = typeof(T);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("The type of T must be an enum.");
        }

        var result = new Dictionary<string, string>();
        var names = Enum.GetNames(enumType);
        var values = Enum.GetValues(enumType);

        for (var i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = (Enum)values.GetValue(i)!;
            var memInfo = enumType.GetMember(name);

            var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = descriptionAttributes.Length > 0
                ? ((DescriptionAttribute)descriptionAttributes.First()).Description
                : name;

            result.Add(description, value.ToString());
        }

        return result;
    }


    /// <summary>
    ///     Retrieves the description of an enumeration member from its <see cref="DescriptionAttribute" /> or returns the
    ///     enumeration member's name if no description is available.
    /// </summary>
    /// <param name="value">The enumeration member whose description is to be retrieved.</param>
    /// <returns>
    ///     The description from the <see cref="DescriptionAttribute" /> or the enumeration member's name if no
    ///     description is available.
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// public enum Colors
    /// {
    ///     [Description("Red Color")]
    ///     Red,
    ///     [Description("Green Color")]
    ///     Green,
    ///     [Description("Blue Color")]
    ///     Blue
    /// }
    /// 
    /// string redDescription = Colors.Red.GetEnumDescription();
    /// 
    /// // redDescription contains the string "Red Color".
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method is useful for retrieving human-readable descriptions of enumeration members if they have a
    ///     <see cref="DescriptionAttribute" />. If no description is provided, the enumeration member's name is returned. This
    ///     can be helpful when displaying the enumeration in a user interface or when generating reports.
    /// </remarks>
    public static string GetEnumDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());

        if (fi == null)
        {
            return value.ToString();
        }

        var descriptionAttribute = fi.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

        return descriptionAttribute?.Description ?? value.ToString();
    }

    /// <summary>
    ///     Retrieves the display name of an enumeration member from its <see cref="DisplayAttribute" /> or returns an empty
    ///     string if no display name is available.
    /// </summary>
    /// <param name="value">The enumeration member whose display name is to be retrieved.</param>
    /// <returns>
    ///     The display name from the <see cref="DisplayAttribute" /> or an empty string if no display name is available.
    ///     Returns <c>null</c> if the value is <c>null</c>.
    /// </returns>
    /// <example>
    ///     <code><![CDATA[
    /// public enum Colors
    /// {
    ///     [Display(Name = "Red Color")]
    ///     Red,
    ///     [Display(Name = "Green Color")]
    ///     Green,
    ///     [Display(Name = "Blue Color")]
    ///     Blue
    /// }
    /// 
    /// string? redDisplayName = Colors.Red.GetEnumDisplay();
    /// 
    /// // redDisplayName contains the string "Red Color".
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method is useful for retrieving human-readable display names of enumeration members if they have a
    ///     <see cref="DisplayAttribute" />. If no display name is provided, an empty string is returned. This can be helpful
    ///     when displaying the enumeration in a user interface or when generating reports. If the input value is <c>null</c>,
    ///     the method returns <c>null</c>.
    /// </remarks>
    public static string? GetEnumDisplay(this Enum? value)
    {
        if (value == null)
        {
            return null;
        }

        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();

        return memberInfo?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? string.Empty;
    }
}