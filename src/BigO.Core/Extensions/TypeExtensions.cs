using System.Reflection;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Type" /> objects.
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    private static readonly Dictionary<Type, string> TypeAlias = new()
    {
        { typeof(bool), "bool" },
        { typeof(bool?), "bool?" },
        { typeof(byte), "byte" },
        { typeof(byte?), "byte?" },
        { typeof(char), "char" },
        { typeof(char?), "char?" },
        { typeof(decimal), "decimal" },
        { typeof(decimal?), "decimal?" },
        { typeof(double), "double" },
        { typeof(double?), "double?" },
        { typeof(float), "float" },
        { typeof(float?), "float?" },
        { typeof(int), "int" },
        { typeof(int?), "int?" },
        { typeof(long), "long" },
        { typeof(long?), "long?" },
        { typeof(object), "object" },
        { typeof(sbyte), "sbyte" },
        { typeof(sbyte?), "sbyte?" },
        { typeof(short), "short" },
        { typeof(short?), "short?" },
        { typeof(string), "string" },
        { typeof(uint), "uint" },
        { typeof(uint?), "uint?" },
        { typeof(ulong), "ulong" },
        { typeof(ulong?), "ulong?" },
        { typeof(Guid), "Guid" },
        { typeof(Guid?), "Guid?" },
        { typeof(DateTime), "DateTime" },
        { typeof(DateTime?), "DateTime?" },
        { typeof(void), "void" }
    };

    /// <summary>
    ///     Returns the default value for a specified <see cref="Type" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to return the default value for.</param>
    /// <returns>The default value.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
    public static object? DefaultValue(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(type) == null)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    /// <summary>
    ///     Get the .Net type name or alias if applicable. For example an typeof(int) will return "int" instead of "Int32".
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The type name or alias.</returns>
    public static string GetNameOrAlias(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!TypeAlias.TryGetValue(type, out var result))
        {
            result = type.Name;
        }

        return result;
    }

    /// <summary>
    ///     Determines whether the specified type is nullable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     If you want to determine whether an instance is of a nullable value type, don't use the Object.GetType method to
    ///     get a Type instance to be tested with the preceding code. When you call the Object.GetType method on an instance of
    ///     a nullable value type, the instance is boxed to Object. As boxing of a non-null instance of a nullable value type
    ///     is equivalent to boxing of a value of the underlying type, GetType returns a Type instance that represents the
    ///     underlying type of a nullable value type.
    /// </remarks>
    public static bool IsNullable(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the type of the specified object is nullable.
    /// </summary>
    /// <typeparam name="T">The type to be checked</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns><c>true</c> if the specified object's type is nullable; otherwise, <c>false</c>.</returns>
    public static bool IsOfNullableType<T>(T source)
    {
        var type = source != null ? source.GetType() : typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the specified type is nullable.
    /// </summary>
    /// <typeparam name="T">The type to be checked</typeparam>
    /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
    public static bool IsOfNullableType<T>()
    {
        var type = typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the specified type is numeric.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="includeNullableTypes">if set to <c>true</c> nullable types will be checked. Default is <c>true</c>.</param>
    /// <returns><c>true</c> if the specified type is numeric; otherwise, <c>false</c>.</returns>
    public static bool IsNumeric(this Type? type, bool includeNullableTypes = true)
    {
        ArgumentNullException.ThrowIfNull(type);

        var typeCode = GetTypeCode(type, includeNullableTypes);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (typeCode)
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Determines whether the specified type is an open generic one.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <returns><c>true</c> if the specified type is an open generic one; otherwise, <c>false</c>.</returns>
    public static bool IsOpenGeneric(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsGenericTypeDefinition;
    }

    /// <summary>
    ///     Determines whether the specified type has the specified attribute.
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <param name="attributeType">Type of the attribute.</param>
    /// <returns><c>true</c> if the specified type has the specified attribute; otherwise, <c>false</c>.</returns>
    public static bool HasAttribute(this Type type, Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsDefined(attributeType, true);
    }

    /// <summary>
    ///     Determines whether the specified type has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <param name="type">The type of the attribute.</param>
    /// <param name="predicate">The predicate used to check attribute existence.</param>
    /// <returns><c>true</c> if the specified type has the specified attribute; otherwise, <c>false</c>.</returns>
    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().GetCustomAttributes<T>(true).Any(predicate);
    }

    /// <summary>
    ///     Gets the type code of a specified <see cref="Type" />.
    /// </summary>
    /// <param name="type">The type to get the byte code for.</param>
    /// <param name="includeNullableTypes">if set to <c>true</c> will check for the underlying type in nullable types.</param>
    /// <returns>Type.</returns>
    private static TypeCode GetTypeCode(Type type, bool includeNullableTypes)
    {
        Type? typeToCheck;

        if (includeNullableTypes)
        {
            typeToCheck = type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        }
        else
        {
            typeToCheck = type;
        }

        return Type.GetTypeCode(typeToCheck);
    }
}