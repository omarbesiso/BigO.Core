using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides extension methods for enumerations, including methods to convert enumerations to dictionaries
///     and retrieve descriptions or display names of enumeration values.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> EnumDescriptionCache = new();
    private static readonly ConcurrentDictionary<Type, Dictionary<string, string?>> EnumDisplayCache = new();

    /// <summary>
    ///     Creates a dictionary of the names and values of an enumeration with optional descriptions.
    /// </summary>
    /// <typeparam name="T">The enumeration type to convert.</typeparam>
    /// <returns>
    ///     A dictionary with keys representing the enumeration descriptions (or names if no description is available)
    ///     and values representing the enumeration values as strings.
    /// </returns>
    public static Dictionary<string, string> ToDictionary<T>() where T : Enum
    {
        var enumType = typeof(T);

        return EnumDescriptionCache.GetOrAdd(enumType, _ =>
        {
            var result = new Dictionary<string, string>();
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumValue = (Enum)Enum.Parse(enumType, name);
                var description = GetEnumDescription(enumValue);

                if (!result.TryAdd(description, name))
                {
                    throw new InvalidOperationException(
                        $"Duplicate description '{description}' found in enum '{enumType.Name}'.");
                }
            }

            return result;
        });
    }

    /// <summary>
    ///     Retrieves the description of an enumeration member from its <see cref="DescriptionAttribute" /> or
    ///     returns the enumeration member's name if no description is available.
    /// </summary>
    /// <param name="value">The enumeration member whose description is to be retrieved.</param>
    /// <returns>
    ///     The description from the <see cref="DescriptionAttribute" /> or the enumeration member's name if no
    ///     description is available.
    /// </returns>
    public static string GetEnumDescription(this Enum value)
    {
        var enumType = value.GetType();
        var enumName = value.ToString();

        return EnumDescriptionCache.GetOrAdd(enumType, _ =>
            Enum.GetValues(enumType)
                .Cast<Enum>()
                .ToDictionary(e => e.ToString(), GetEnumDescriptionInternal)
        )[enumName];
    }

    private static string GetEnumDescriptionInternal(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        return fieldInfo?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }

    /// <summary>
    ///     Retrieves the display name of an enumeration member from its <see cref="DisplayAttribute" /> or returns an empty
    ///     string if no display name is available.
    /// </summary>
    /// <param name="value">The enumeration member whose display name is to be retrieved.</param>
    /// <returns>
    ///     The display name from the <see cref="DisplayAttribute" /> or an empty string if no display name is available.
    /// </returns>
    public static string GetEnumDisplay(this Enum value)
    {
        var enumType = value.GetType();
        var enumName = value.ToString();

        return EnumDisplayCache.GetOrAdd(enumType, _ => [])[enumName] ??
               InitializeEnumDisplay(enumType, value, enumName);
    }

    private static string InitializeEnumDisplay(Type enumType, Enum value, string enumName)
    {
        var fieldInfo = enumType.GetField(enumName);
        var display = fieldInfo?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? string.Empty;

        EnumDisplayCache[enumType][enumName] = display;
        return display;
    }
}