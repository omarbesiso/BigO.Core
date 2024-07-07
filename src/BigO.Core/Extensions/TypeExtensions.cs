using System.Collections.Concurrent;
using System.Reflection;
using BigO.Core.Validation;

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
        { typeof(DateOnly), "DateOnly" },
        { typeof(DateOnly?), "DateOnly?" },
        { typeof(TimeOnly), "TimeOnly" },
        { typeof(TimeOnly?), "TimeOnly?" },
        { typeof(void), "void" }
    };

    /// <summary>
    ///     Returns the default value for the specified type.
    /// </summary>
    /// <typeparam name="T">The type to get the default value for.</typeparam>
    /// <returns>The default value for the specified type.</returns>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? DefaultValue<T>()
    {
        return default;
    }

    /// <summary>
    ///     Returns the default value for the specified type <paramref name="type" />.
    /// </summary>
    /// <param name="type">The type to get the default value for.</param>
    /// <returns>
    ///     The default value for the specified type. For reference types and nullable value types, this will be
    ///     <c>null</c>. For non-nullable value types, this will be the value produced by the type's default constructor.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
    public static object? DefaultValue(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (DefaultValues.TryGetValue(type, out var value))
        {
            return value;
        }

        try
        {
            value = DefaultValueMethod.MakeGenericMethod(type).Invoke(null, null);
            DefaultValues[type] = value;
            return value;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error obtaining default value for type {type.FullName}", ex);
        }
    }

    /// <summary>
    ///     Asynchronously returns the default value for the specified type <paramref name="type" />.
    /// </summary>
    /// <param name="type">The type to get the default value for.</param>
    /// <returns>
    ///     The default value for the specified type. For reference types and nullable value types, this will be
    ///     <c>null</c>. For non-nullable value types, this will be the value produced by the type's default constructor.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
    public static async Task<object?> DefaultValueAsync(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return await Task.Run(() => DefaultValue(type));
    }

    /// <summary>
    ///     Gets the name or an alias for the specified type.
    /// </summary>
    /// <param name="type">The type to get the name or alias for.</param>
    /// <returns>The name or alias for the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetTypeAsString(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return TypeAlias.TryGetValue(type, out var result) ? result : type.Name;
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="type" /> is nullable.
    /// </summary>
    /// <param name="type">The type to check for nullability.</param>
    /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="type" /> is <c>null</c>.</exception>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullable(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Nullable.GetUnderlyingType(type) != null;
    }

    /// <summary>
    ///     Determines whether the type of the specified source object is a nullable type.
    /// </summary>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="source">The source object to check for nullability.</param>
    /// <returns><c>true</c> if the type of the source object is a nullable type; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <typeparamref name="T" /> is a value type and
    ///     <paramref name="source" /> is <c>null</c>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfNullableType<T>(this T source)
    {
        if (source == null && typeof(T).IsValueType)
        {
            throw new ArgumentNullException(nameof(source), "Source cannot be null when T is a value type.");
        }

        return typeof(T).IsNullable();
    }

    /// <summary>
    ///     Determines whether the specified type <typeparamref name="T" /> is a nullable type.
    /// </summary>
    /// <typeparam name="T">The type to check for nullability.</typeparam>
    /// <returns><c>true</c> if the specified type <typeparamref name="T" /> is a nullable type; otherwise, <c>false</c>.</returns>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOfNullableType<T>()
    {
        return typeof(T).IsNullable();
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
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNumeric(this Type type, bool includeNullableTypes = true)
    {
        Guard.NotNull(type);

        var typeCode = GetTypeCode(type, includeNullableTypes);

        if (type.IsArray || type.IsEnum)
        {
            return false;
        }

        return typeCode switch
        {
            TypeCode.Byte or TypeCode.Decimal or TypeCode.Double or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64
                or TypeCode.SByte or TypeCode.Single or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => true,
            _ => false
        };
    }

    /// <summary>
    ///     Determines whether the specified <see cref="Type" /> represents an open generic type.
    /// </summary>
    /// <param name="type">The type to check for being an open generic type.</param>
    /// <returns><c>true</c> if the specified type is an open generic type; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is <c>null</c>.</exception>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOpenGeneric(this Type type)
    {
        Guard.NotNull(type);
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
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAttribute(this Type type, Type attributeType)
    {
        Guard.NotNull(type);
        Guard.NotNull(attributeType);
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
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
    {
        Guard.NotNull(type);
        Guard.NotNull(predicate);
        return type.GetTypeInfo().GetCustomAttributes<T>(true).Any(predicate);
    }

    /// <summary>
    ///     Gets the <see cref="TypeCode" /> of the specified type.
    /// </summary>
    /// <param name="type">The type to get the type code for.</param>
    /// <param name="includeNullableTypes">
    ///     A flag indicating whether nullable types should be treated as their underlying
    ///     types. If this flag is set to true, the type code for a nullable type will be the type code for its underlying
    ///     type. If this flag is set to false, the type code for a nullable type will be <see cref="TypeCode.Object" />.
    /// </param>
    /// <returns>The <see cref="TypeCode" /> of the specified type.</returns>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TypeCode GetTypeCode(Type type, bool includeNullableTypes)
    {
        var typeToCheck = includeNullableTypes && type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
        return Type.GetTypeCode(typeToCheck);
    }
}