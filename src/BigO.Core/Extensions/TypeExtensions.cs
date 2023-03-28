using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using JetBrains.Annotations;

// ReSharper disable InvalidXmlDocComment


namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Type" /> objects.
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<Type, object?> DefaultValues = new();

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
    ///     Returns the default value for the specified type.
    /// </summary>
    /// <typeparam name="T">The type to get the default value for.</typeparam>
    /// <returns>The default value for the specified type.</returns>
    /// <remarks>
    ///     This method uses the default keyword to return the default value for the specified type.
    ///     The default value for a reference type is <c>null</c>, and the default value for a value type is the value produced
    ///     by the value type's default constructor.
    /// </remarks>
    public static T? DefaultValue<T>()
    {
        return default;
    }

    /// <summary>
    ///     Returns the default value for the specified type.
    /// </summary>
    /// <param name="type">The type to get the default value for.</param>
    /// <returns>The default value for the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses a <see cref="ConcurrentDictionary{TKey,TValue}" /> to cache default values for each type,
    ///     and uses the <see cref="Type.GetTypeInfo" /> method and a series of type checks and typecasts to determine the
    ///     default value for the specified type.
    ///     The default value for a reference type is <c>null</c>, and the default value for a value type is the value produced
    ///     by the value type's default constructor.
    ///     If the type is not one of the explicitly checked value types, the method uses the
    ///     <see cref="FormatterServices.GetUninitializedObject(Type)" /> method to return an uninitialized object of the
    ///     specified type.
    /// </remarks>
    public static object? DefaultValue(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (DefaultValues.TryGetValue(type, out var value))
        {
            return value;
        }

        if (type.GetTypeInfo().IsValueType)
        {
            if (type == typeof(bool))
            {
                value = false;
            }
            else if (type == typeof(byte))
            {
                value = (byte)0;
            }
            else if (type == typeof(char))
            {
                value = '\0';
            }
            else if (type == typeof(decimal))
            {
                value = (decimal)0;
            }
            else if (type == typeof(double))
            {
                value = (double)0;
            }
            else if (type == typeof(float))
            {
                value = (float)0;
            }
            else if (type == typeof(int))
            {
                value = 0;
            }
            else if (type == typeof(long))
            {
                value = (long)0;
            }
            else if (type == typeof(sbyte))
            {
                value = (sbyte)0;
            }
            else if (type == typeof(short))
            {
                value = (short)0;
            }
            else if (type == typeof(uint))
            {
                value = (uint)0;
            }
            else if (type == typeof(ulong))
            {
                value = (ulong)0;
            }
            else if (type == typeof(ushort))
            {
                value = (ushort)0;
            }
            else
            {
                value = FormatterServices.GetUninitializedObject(type);
            }
        }
        else
        {
            value = null;
        }

        DefaultValues[type] = value;
        return value;
    }

    /// <summary>
    ///     Gets the name or an alias for the specified type.
    /// </summary>
    /// <param name="type">The type to get the name or alias for.</param>
    /// <returns>The name or alias for the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method uses a <see cref="Dictionary{TKey,TValue}" /> to map commonly used types to their corresponding
    ///     aliases.
    ///     If the specified type is not in the dictionary, the method returns the value of the <see cref="Type.Name" />
    ///     property.
    /// </remarks>
    public static string GetNameOrAlias(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (!TypeAlias.TryGetValue(type, out var result))
        {
            result = type.Name;
        }

        return result;
    }

    /// <summary>
    ///     Determines whether the specified source is of a nullable type.
    /// </summary>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="source">The source object to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified source is of a nullable type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown if the specified source is <c>null</c> and the type of the source
    ///     object is a value type.
    /// </exception>
    public static bool IsNullable(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the specified source object is of a nullable type.
    /// </summary>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="source">The source object to check for nullability.</param>
    /// <returns>True if the source object is of a nullable type, false otherwise.</returns>
    /// <exception cref="System.ArgumentNullException">
    ///     Thrown if the <paramref name="source" /> is <c>null</c> and the type <typeparamref name="T" /> is a value type.
    /// </exception>
    public static bool IsOfNullableType<T>(this T source)
    {
        var type = source != null ? source.GetType() : typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the specified type is a nullable type.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <returns>True if the type is a nullable type; otherwise, false.</returns>
    /// <remarks>
    ///     A nullable type is a type that can represent a value or <c>null</c>.
    ///     This method checks if the specified type is a nullable type by using the
    ///     <see cref="Nullable.GetUnderlyingType(Type)" /> method.
    /// </remarks>
    public static bool IsOfNullableType<T>()
    {
        var type = typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the specified type is a numeric type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="includeNullableTypes">
    ///     A flag indicating whether nullable types should be included in the check.
    ///     If this flag is set to true, nullable numeric types will be considered numeric.
    ///     If this flag is set to false, nullable numeric types will not be considered numeric.
    /// </param>
    /// <returns>True if the type is a numeric type; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="type" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks if the specified type is a numeric type by using the
    ///     <see cref="Type.GetTypeCode(Type)" /> method and a switch statement.
    ///     Numeric types include byte, decimal, double, int16, int32, int64, sbyte, single, uint16, uint32, and uint64.
    /// </remarks>
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
    ///     Determines whether the specified type is an open generic type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is an open generic type; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="type" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     An open generic type is a generic type that is not closed over specific type arguments.
    ///     For example, the type `List&lt;&gt;` is an open generic type, while the type `List&lt;int&gt;` is a closed generic
    ///     type.
    ///     This method checks if the specified type is an open generic type by using the
    ///     <see cref="Type.GetTypeInfo()" /> method and the
    ///     <see cref="TypeInfo.IsGenericTypeDefinition" /> property.
    /// </remarks>
    public static bool IsOpenGeneric(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsGenericTypeDefinition;
    }

    /// <summary>
    ///     Determines whether the specified type has the specified attribute.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="attributeType">The attribute type to check for.</param>
    /// <returns>True if the type has the attribute; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="type" /> or <paramref name="attributeType" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks if the specified type has the specified attribute by using the
    ///     <see cref="Type.GetTypeInfo()" /> method and the
    ///     <see cref="TypeInfo.IsDefined(Type, bool)" /> method.
    ///     The second parameter of the <see cref="TypeInfo.IsDefined(Type, bool)" /> method is set to true,
    ///     which indicates that the search should include inherited attributes.
    /// </remarks>
    public static bool HasAttribute(this Type type, Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsDefined(attributeType, true);
    }

    /// <summary>
    ///     Determines whether the specified type has an attribute of type <typeparamref name="T" /> that satisfies the
    ///     specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <param name="predicate">The predicate to apply to the attribute.</param>
    /// <returns>
    ///     True if the type has an attribute of type <typeparamref name="T" /> that satisfies the predicate; otherwise,
    ///     false.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="type" /> or <paramref name="predicate" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks if the specified type has an attribute of type <typeparamref name="T" /> that satisfies the
    ///     specified predicate by using the
    ///     <see cref="Type.GetTypeInfo()" /> method and the
    ///     <see cref="TypeInfo.GetCustomAttributes{T}(bool)" /> method.
    ///     The second parameter of the <see cref="TypeInfo.GetCustomAttributes{T}(bool)" /> method is set to true,
    ///     which indicates that the search should include inherited attributes.
    ///     The resulting array of attributes is then filtered using the
    ///     <see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})" /> method and the specified
    ///     predicate.
    /// </remarks>
    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().GetCustomAttributes<T>(true).Any(predicate);
    }

    /// <summary>
    ///     Gets the <see cref="TypeCode" /> of the specified type.
    /// </summary>
    /// <param name="type">The type to get the type code for.</param>
    /// <param name="includeNullableTypes">
    ///     A flag indicating whether nullable types should be treated as their underlying types.
    ///     If this flag is set to true, the type code for a nullable type will be the type code for its underlying type.
    ///     If this flag is set to false, the type code for a nullable type will be <see cref="TypeCode.Object" />.
    /// </param>
    /// <returns>The <see cref="TypeCode" /> of the specified type.</returns>
    /// <remarks>
    ///     This method gets the <see cref="TypeCode" /> of the specified type by using the
    ///     <see cref="Type.GetTypeCode(Type)" /> method.
    ///     If the <paramref name="includeNullableTypes" /> flag is set to true and the specified type is a nullable type,
    ///     the method first gets the underlying type using the <see cref="Nullable.GetUnderlyingType(Type)" /> method
    ///     before calling the <see cref="Type.GetTypeCode(Type)" /> method.
    /// </remarks>
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