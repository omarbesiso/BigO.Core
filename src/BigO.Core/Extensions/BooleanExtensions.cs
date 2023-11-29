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
    ///     Returns a string representation of the boolean value, based on the specified true and false values.
    /// </summary>
    /// <remarks>
    ///     This method is an extension method for boolean types. It allows the developer to convert a boolean value to a
    ///     string representation using custom true and false values.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the ToString() extension method:
    ///     <code>
    /// <![CDATA[
    ///     bool b = true;
    ///     string str = b.ToString("Yes", "No"); // str = "Yes"
    /// ]]>
    /// </code>
    /// </example>
    /// <param name="source">The boolean value to convert to a string.</param>
    /// <param name="trueValue">The string representation of true.</param>
    /// <param name="falseValue">The string representation of false.</param>
    /// <returns>The string representation of the boolean value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either trueValue or falseValue is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(this bool source, string trueValue, string falseValue)
    {
        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     Converts a boolean value to a byte value.
    /// </summary>
    /// <remarks>
    ///     This method is an extension method for boolean types. It converts a boolean value to a byte value, where true is
    ///     represented as 1 and false is represented as 0.
    /// </remarks>
    /// <example>
    ///     <code>
    /// <![CDATA[
    ///     bool b = true;
    ///     byte value = b.ToByte();  // value = 1
    /// ]]>
    /// </code>
    /// </example>
    /// <param name="source">The boolean value to convert to a byte.</param>
    /// <returns>The byte representation of the boolean value, where true is 1 and false is 0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToByte(this bool source)
    {
        return source ? One : Zero;
    }
}