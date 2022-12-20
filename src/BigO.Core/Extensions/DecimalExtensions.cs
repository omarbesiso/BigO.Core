using System.Globalization;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="decimal" /> objects.
/// </summary>
[PublicAPI]
public static class DecimalExtensions
{
    /// <summary>
    ///     Converts a decimal value to a currency string using a specified culture.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="cultureName">The name of the culture to use for the currency string formatting. Default is "en-US".</param>
    /// <returns>A string representing the given decimal value as a currency in the specified culture.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="cultureName" /> parameter is <c>null</c> or empty.</exception>
    /// <exception cref="CultureNotFoundException">
    ///     The culture specified by the <paramref name="cultureName" /> parameter is
    ///     not found.
    /// </exception>
    /// <remarks>
    ///     This method converts the given decimal value to a currency string using the specified culture. If no culture is
    ///     specified, or if the culture is "en-US", the invariant culture is used.
    ///     The resulting currency string is formatted according to the conventions of the specified culture.
    /// </remarks>
    public static string ToCurrencyString(this decimal value, string cultureName = "en-US")
    {
        var currentCulture = string.IsNullOrWhiteSpace(cultureName) &&
                             !cultureName.Equals("en-US", StringComparison.OrdinalIgnoreCase)
            ? CultureInfo.InvariantCulture
            : new CultureInfo(cultureName);
        return string.Format(currentCulture, "{0:C}", value);
    }
}