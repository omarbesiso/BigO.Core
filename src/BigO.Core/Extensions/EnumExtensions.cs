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
    /// <exception cref="ArgumentException">Thrown when T is not an enum.</exception>
    public static Dictionary<string, string> ToDictionary<T>() where T : Enum
    {
        var enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("T must be an enum type.", nameof(T));
        }

        if (EnumDescriptionCache.TryGetValue(enumType, out var cachedDict))
        {
            return cachedDict;
        }

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

        EnumDescriptionCache[enumType] = result;
        return result;
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
        return EnumDescriptionCache.GetOrAdd(enumType,
            _ => Enum.GetValues(enumType).Cast<Enum>().ToDictionary(e => e.ToString(), GetEnumDescriptionInternal))[
            value.ToString()];
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
        if (EnumDisplayCache.TryGetValue(enumType, out var cachedDict) &&
            cachedDict.TryGetValue(value.ToString(), out var cachedDisplay))
        {
            return cachedDisplay ?? string.Empty;
        }

        var fieldInfo = enumType.GetField(value.ToString());
        var display = fieldInfo?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? string.Empty;

        if (!EnumDisplayCache.ContainsKey(enumType))
        {
            EnumDisplayCache[enumType] = new Dictionary<string, string?>();
        }

        EnumDisplayCache[enumType][value.ToString()] = display;
        return display;
    }
}