using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="bool" /> objects.
/// </summary>
[PublicAPI]
public static class BooleanExtensions
{
    /// <summary>
    ///     An extension method that show the <paramref name="trueValue" /> when the <paramref name="source" /> value is
    ///     <c>true</c>; otherwise show the falseValue.
    /// </summary>
    /// <param name="source">The <paramref name="source" /> boolean value.</param>
    /// <param name="trueValue">The string to be returned if the <paramref name="source" /> value is <c>true</c>.</param>
    /// <param name="falseValue">The string to be returned if the <paramref name="source" /> value is <c>false</c>.</param>
    /// <returns>A string that represents of the current boolean value.</returns>
    public static string ToString(this bool source, string trueValue, string falseValue)
    {
        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     An extension method that convert the <paramref name="source" /> into a binary representation.
    /// </summary>
    /// <param name="source">The <paramref name="source" /> boolean value.</param>
    /// <returns>A binary representation of this boolean value.</returns>
    public static byte ToBit(this bool source)
    {
        return Convert.ToByte(source);
    }
}