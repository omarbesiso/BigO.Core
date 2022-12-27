using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="bool" /> objects.
/// </summary>
[PublicAPI]
public static class BooleanExtensions
{
    private const byte Zero = 0;
    private const byte One = 1;

    /// <summary>
    ///     Converts the specified boolean value to a string.
    /// </summary>
    /// <param name="source">The boolean value to convert.</param>
    /// <param name="trueValue">The string to return if <paramref name="source" /> is <c>true</c>.</param>
    /// <param name="falseValue">The string to return if <paramref name="source" /> is <c>false</c>.</param>
    /// <returns>The string representation of the boolean value.</returns>
    /// <remarks>
    ///     This method converts the specified boolean value to a string using a ternary operator.
    ///     If <paramref name="source" /> is <c>true</c>, it returns <paramref name="trueValue" />. If
    ///     <paramref name="source" /> is <c>false</c>, it returns <paramref name="falseValue" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(this bool source, string trueValue, string falseValue)
    {
        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     Converts the specified <paramref name="source" /> value to a byte value.
    /// </summary>
    /// <param name="source">The boolean value to convert.</param>
    /// <returns>A byte value that represents the <paramref name="source" /> value.</returns>
    /// <exception cref="OverflowException">
    ///     The <paramref name="source" /> value is <c>true</c>, but the resulting value is
    ///     larger than <see cref="Byte.MaxValue" />.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="Convert.ToByte(bool)" /> method to convert the <paramref name="source" /> value to
    ///     a byte. If the <paramref name="source" /> value is <c>true</c>, the resulting byte value is 1. If the
    ///     <paramref name="source" /> value is <c>false</c>, the resulting byte value is 0.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte(this bool source)
    {
        return source ? One : Zero;
    }
}