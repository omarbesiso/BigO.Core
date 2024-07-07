namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="double" /> objects.
/// </summary>
[PublicAPI]
public static class DoubleExtensions
{
    /// <summary>
    ///     Converts a nullable double value to a nullable decimal.
    /// </summary>
    /// <param name="value">The nullable double value to convert.</param>
    /// <returns>
    ///     A nullable decimal representing the converted value if <paramref name="value" /> has a value; otherwise,
    ///     <c>null</c>.
    /// </returns>
    /// <remarks>
    ///     This method converts the given nullable double value to a nullable decimal. Since a decimal has higher precision
    ///     compared to a double,
    ///     this conversion is safe and does not result in a loss of precision.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the <see cref="ToDecimal" /> method to convert a nullable double value to a
    ///     nullable decimal.
    ///     <code>
    /// double? value = 1234.56;
    /// decimal? decimalValue = value.ToDecimal();
    /// Console.WriteLine("Decimal value: {0}", decimalValue);
    /// 
    /// double? nullValue = null;
    /// decimal? nullDecimalValue = nullValue.ToDecimal();
    /// Console.WriteLine("Decimal value: {0}", nullDecimalValue);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? ToDecimal(this double? value)
    {
        return value.HasValue ? (decimal)value.Value : null;
    }
}