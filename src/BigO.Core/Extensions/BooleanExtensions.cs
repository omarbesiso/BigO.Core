using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="bool" /> objects.
/// </summary>
[PublicAPI]
public static class BooleanExtensions
{
    /// <summary>
    ///     Converts the given boolean value to a string using the specified values for true and false.
    /// </summary>
    /// <param name="source">The boolean value to convert.</param>
    /// <param name="trueValue">The string to use for true.</param>
    /// <param name="falseValue">The string to use for false.</param>
    /// <returns>The string representation of the boolean value.</returns>
    /// <remarks>
    ///     This method converts the given boolean value to a string using the specified values for true and false.
    ///     If <paramref name="source" /> is true, it returns <paramref name="trueValue" />. Otherwise, it returns
    ///     <paramref name="falseValue" />.
    /// </remarks>
    public static string ToString(this bool source, string trueValue, string falseValue)
    {
        return source ? trueValue : falseValue;
    }

    /// <summary>
    ///     Converts the given boolean value to a byte.
    /// </summary>
    /// <param name="source">The boolean value to convert.</param>
    /// <returns>A byte representing the boolean value. 1 for true, 0 for false.</returns>
    /// <exception cref="OverflowException">
    ///     Thrown if the converted value is less than <see cref="byte.MinValue" /> or greater
    ///     than <see cref="byte.MaxValue" />.
    /// </exception>
    /// <remarks>
    ///     This method converts the given boolean value to a byte using the <see cref="Convert.ToByte(bool)" /> method.
    ///     If <paramref name="source" /> is true, it returns 1. Otherwise, it returns 0.
    /// </remarks>
    public static byte ToBit(this bool source)
    {
        return Convert.ToByte(source);
    }
}