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
    ///     Convert a decimal to string formatted in the currency format of the specified culture.
    /// </summary>
    /// <param name="value">The value to be formatted.</param>
    /// <param name="cultureName">Name of the culture to be used when formatting the value.</param>
    /// <returns>The <paramref name="value" /> represented in currency format according to the specified culture.</returns>
    public static string ToCurrencyString(this decimal value, string cultureName = "en-US")
    {
        if (string.IsNullOrWhiteSpace(cultureName))
        {
            throw new ArgumentException("The culture name cannot be null or whitespace.", nameof(cultureName));
        }

        var currentCulture = new CultureInfo(cultureName);
        return string.Format(currentCulture, "{0:C}", value);
    }
}