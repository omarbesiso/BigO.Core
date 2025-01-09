using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="bool" /> objects.
/// </summary>
[PublicAPI]
public static class BooleanExtensions
{
    /// <summary>
    ///     Returns a custom string representation of the boolean value, with default strings if none are provided.
    ///     <para>
    ///         This is an overload of <see cref="ToCustomString(bool, string, string)" /> that allows optional parameters
    ///         and is available starting from .NET 6.
    ///     </para>
    /// </summary>
    /// <param name="source">The boolean value to convert to a string.</param>
    /// <param name="trueValue">
    ///     The string representation of <c>true</c>. Defaults to <c>"True"</c> if not provided.
    /// </param>
    /// <param name="falseValue">
    ///     The string representation of <c>false</c>. Defaults to <c>"False"</c> if not provided.
    /// </param>
    /// <returns>A string that represents the boolean value, using the specified (or default) true/false strings.</returns>
    /// <remarks>
    ///     <example>
    ///         <code><![CDATA[
    ///         bool b = true;
    ///         // Uses default "True" and "False":
    ///         string str1 = b.ToCustomString();            
    ///         
    ///         // Supply just the trueValue:
    ///         string str2 = b.ToCustomString("YES");
    /// 
    ///         // Supply both trueValue and falseValue:
    ///         string str3 = b.ToCustomString("Yes", "No"); 
    ///         ]]></code>
    ///     </example>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCustomString(this bool source, string? trueValue = "True", string? falseValue = "False")
    {
        // We only do these checks if the parameters are actually used (not default).
        Guard.NotNull(trueValue);
        Guard.NotNull(falseValue);

        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     Converts a boolean value to a byte value.
    ///     <para>
    ///         Returns <c>1</c> if <paramref name="source" /> is <c>true</c>, otherwise <c>0</c>.
    ///     </para>
    /// </summary>
    /// <param name="source">The boolean value to convert to a byte.</param>
    /// <returns>The byte representation of the boolean value, where <c>true</c> is <c>1</c> and <c>false</c> is <c>0</c>.</returns>
    /// <remarks>
    ///     <example>
    ///         <code><![CDATA[
    ///         bool b = true;
    ///         byte value = b.ToByte();  // value = 1
    ///         ]]></code>
    ///     </example>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte(this bool source)
    {
        return source ? (byte)1 : (byte)0;
    }

    /// <summary>
    ///     Converts a boolean value to an integer value.
    ///     <para>
    ///         Returns <c>1</c> if <paramref name="source" /> is <c>true</c>, otherwise <c>0</c>.
    ///     </para>
    /// </summary>
    /// <param name="source">The boolean value to convert to an integer.</param>
    /// <returns>The integer representation of the boolean value, where <c>true</c> is <c>1</c> and <c>false</c> is <c>0</c>.</returns>
    /// <remarks>
    ///     <example>
    ///         <code><![CDATA[
    ///         bool b = false;
    ///         int value = b.ToInt32();  // value = 0
    ///         ]]></code>
    ///     </example>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(this bool source)
    {
        return source ? 1 : 0;
    }
}