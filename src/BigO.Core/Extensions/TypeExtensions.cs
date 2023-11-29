using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

// ReSharper disable InvalidXmlDocComment


namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Type" /> objects.
/// </summary>
[PublicAPI]
public static class TypeExtensions
{
    private static readonly Type ExtensionsClassType = typeof(TypeExtensions);
    private static readonly MethodInfo DefaultValueMethod = ExtensionsClassType.GetMethod(nameof(DefaultValue))!;
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

        value = DefaultValueMethod.MakeGenericMethod(type).Invoke(null, null);

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
    public static string GetTypeAsString(this Type type)
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
    ///     Determines whether the specified <paramref name="type" /> is nullable.
    /// </summary>
    /// <param name="type">The type to check for nullability.</param>
    /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// Type nullableIntType = typeof(int?);
    /// bool isNullable = nullableIntType.IsNullable(); // true
    /// 
    /// Type nonNullableIntType = typeof(int);
    /// bool isNotNullable = nonNullableIntType.IsNullable(); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method checks if the provided <paramref name="type" /> is a nullable value type. Reference types (except for
    ///     Nullable
    ///     <T>
    ///         itself) are not considered nullable by this method. To check for a nullable value type, the method inspects
    ///         whether the type is a generic instantiation of Nullable<T>.
    /// </remarks>
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
    ///     Determines whether the specified source object is of a nullable type.
    /// </summary>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="source">The source object to check for nullability.</param>
    /// <returns><c>true</c> if the specified source object is of a nullable type; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// int? nullableInt = 5;
    /// bool isNullableType = nullableInt.IsOfNullableType(); // true
    /// 
    /// int nonNullableInt = 5;
    /// bool isNotNullableType = nonNullableInt.IsOfNullableType(); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method checks if the provided <paramref name="source" /> object is of a nullable value type. If the
    ///     <paramref name="source" /> object is <c>null</c>, the method checks the underlying type <typeparamref name="T" />
    ///     instead. Reference types (except for Nullable
    ///     <T>
    ///         itself) are not considered nullable by this method. To check for a nullable value type, the method inspects
    ///         whether the type is a generic instantiation of Nullable<T>.
    /// </remarks>
    public static bool IsOfNullableType<T>()
    {
        var type = typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the provided <paramref name="type" /> is a numeric type, optionally including nullable numeric
    ///     types.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> to check if it is a numeric type.</param>
    /// <param name="includeNullableTypes">
    ///     A <c>bool</c> value indicating whether nullable numeric types should be considered
    ///     as numeric. The default value is <c>true</c>.
    /// </param>
    /// <returns><c>true</c> if the <paramref name="type" /> is a numeric type, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="type" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// var isIntNumeric = typeof(int).IsNumeric(); // true
    /// var isNullableIntNumeric = typeof(int?).IsNumeric(); // true
    /// var isStringNumeric = typeof(string).IsNumeric(); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This method checks if the provided <paramref name="type" /> is one of the following numeric types:
    ///     <see cref="Byte" />, <see cref="Decimal" />, <see cref="Double" />, <see cref="Int16" />, <see cref="Int32" />,
    ///     <see cref="Int64" />, <see cref="SByte" />, <see cref="Single" />, <see cref="UInt16" />, <see cref="UInt32" />,
    ///     and <see cref="UInt64" />. If the <paramref name="includeNullableTypes" /> parameter is set to <c>true</c>, the
    ///     method will also consider the nullable versions of these numeric types as valid numeric types. Otherwise, it will
    ///     exclude them from the check.
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
    ///     Determines whether the specified <see cref="Type" /> represents an open generic type.
    /// </summary>
    /// <param name="type">The type to check for being an open generic type.</param>
    /// <returns><c>true</c> if the specified type is an open generic type; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is <c>null</c>.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// bool isOpenGeneric = typeof(Dictionary<,>).IsOpenGeneric(); // true
    /// 
    /// bool isNotOpenGeneric = typeof(Dictionary<string, int>).IsOpenGeneric(); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     An open generic type is a generic type definition, such as
    ///     <c>
    ///         Dictionary<,></c>
    ///     , that has not been provided with specific type arguments. This method checks if the provided
    ///     <paramref name="type" /> represents an open generic type by examining the
    ///     <see cref="TypeInfo.IsGenericTypeDefinition" /> property of the type's <see cref="TypeInfo" />.
    /// </remarks>
    public static bool IsOpenGeneric(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsGenericTypeDefinition;
    }

    /// <summary>
    ///     Determines whether the specified <see cref="Type" /> has a specified attribute type.
    /// </summary>
    /// <param name="type">The type to check for the presence of the attribute type.</param>
    /// <param name="attributeType">The attribute type to look for.</param>
    /// <returns><c>true</c> if the specified type has the attribute type; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="type" /> or <paramref name="attributeType" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <example>
    ///     <code><![CDATA[
    /// [Serializable]
    /// public class MyClass { }
    /// 
    /// bool hasAttribute = typeof(MyClass).HasAttribute(typeof(SerializableAttribute)); // true
    /// 
    /// bool noAttribute = typeof(MyClass).HasAttribute(typeof(ObsoleteAttribute)); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="HasAttribute" /> method checks if the provided <paramref name="type" /> is decorated with the
    ///     specified <paramref name="attributeType" />. This is done by calling the
    ///     <see cref="MemberInfo.IsDefined(Type, bool)" /> method on the type's <see cref="TypeInfo" />.
    /// </remarks>
    public static bool HasAttribute(this Type type, Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetTypeInfo().IsDefined(attributeType, true);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="Type" /> has an attribute of type <typeparamref name="T" /> that
    ///     satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The attribute type to look for.</typeparam>
    /// <param name="type">The type to check for the presence of the attribute.</param>
    /// <param name="predicate">A function to test each attribute for a condition.</param>
    /// <returns>
    ///     <c>true</c> if the specified type has an attribute of type <typeparamref name="T" /> that satisfies the
    ///     predicate; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="type" /> or <paramref name="predicate" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <example>
    ///     <code><![CDATA[
    /// [Obsolete("This class is obsolete.")]
    /// public class MyClass { }
    /// 
    /// bool hasObsoleteAttribute = typeof(MyClass).HasAttribute<ObsoleteAttribute>(attr => attr.Message == "This class is obsolete."); // true
    /// 
    /// bool noObsoleteAttribute = typeof(MyClass).HasAttribute<ObsoleteAttribute>(attr => attr.Message == "This class is not supported."); // false
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="HasAttribute{T}" /> method checks if the provided <paramref name="type" /> is decorated with an
    ///     attribute of type <typeparamref name="T" /> that satisfies the specified <paramref name="predicate" />. This is
    ///     done by calling the <see cref="TypeInfo.GetCustomAttributes{T}(bool)" /> method
    ///     method.
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