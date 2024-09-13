using System.Diagnostics.CodeAnalysis;
using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="bool" /> objects.
/// </summary>
[PublicAPI]
public static class BooleanExtensions
{
    /// <summary>
    ///     Returns a custom string representation of the boolean value, based on the specified true and false values.
    /// </summary>
    /// <param name="source">The boolean value to convert to a string.</param>
    /// <param name="trueValue">The string representation of true.</param>
    /// <param name="falseValue">The string representation of false.</param>
    /// <returns>The custom string representation of the boolean value.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="trueValue" /> or
    ///     <paramref name="falseValue" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method allows the developer to convert a boolean value to a
    ///     string representation using custom true and false values.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <c>ToCustomString</c> extension method:
    ///     <code><![CDATA[
    ///     bool b = true;
    ///     string str = b.ToCustomString("Yes", "No"); // str = "Yes"
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCustomString(this bool source, string trueValue, string falseValue)
    {
        Guard.NotNull(trueValue);
        Guard.NotNull(falseValue);

        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     Converts a boolean value to a byte value.
    /// </summary>
    /// <param name="source">The boolean value to convert to a byte.</param>
    /// <returns>The byte representation of the boolean value, where true is 1 and false is 0.</returns>
    /// <remarks>
    ///     This method converts a boolean value to a byte value, where true is
    ///     represented as 1 and false is represented as 0.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     bool b = true;
    ///     byte value = b.ToByte();  // value = 1
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte(this bool source)
    {
        return source ? (byte)1 : (byte)0;
    }

    /// <summary>
    ///     Converts a boolean value to an integer value.
    /// </summary>
    /// <param name="source">The boolean value to convert to an integer.</param>
    /// <returns>The integer representation of the boolean value, where true is 1 and false is 0.</returns>
    /// <remarks>
    ///     This method converts a boolean value to an integer value, where true is
    ///     represented as 1 and false is represented as 0.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     bool b = false;
    ///     int value = b.ToInt32();  // value = 0
    ///     ]]></code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(this bool source)
    {
        return source ? 1 : 0;
    }
}