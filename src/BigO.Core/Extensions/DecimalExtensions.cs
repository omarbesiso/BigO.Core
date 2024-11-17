using System.Globalization;
using System.Text;
using BigO.Core.Factories;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="decimal" /> objects.
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
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="cultureName" /> parameter is <c>null</c> or empty.</exception>
    /// <exception cref="CultureNotFoundException">Thrown when the culture specified by the <paramref name="cultureName" /> parameter is not found.</exception>
    /// <example>
    ///     <code>
    /// decimal value = 1234.56m;
    /// string currencyString = value.ToCurrencyString("en-US");
    /// // Output: "$1,234.56"
    /// 
    /// string currencyStringFr = value.ToCurrencyString("fr-FR");
    /// // Output: "1 234,56 €"
    /// </code>
    /// </example>
    public static string ToCurrencyString(this decimal value, string cultureName = "en-US")
    {
        var culture = CultureInfoFactory.Create(cultureName);
        return string.Format(culture, "{0:C}", value);
    }

    /// <summary>
    ///     Converts a decimal value to a percentage string.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="decimalPlaces">The number of decimal places to include in the percentage string. Default is 2.</param>
    /// <param name="cultureName">The name of the culture to use for the percentage string formatting. Default is "en-US".</param>
    /// <returns>A string representing the given decimal value as a percentage in the specified culture.</returns>
    public static string ToPercentageString(this decimal value, int decimalPlaces = 2, string cultureName = "en-US")
    {
        var culture = CultureInfoFactory.Create(cultureName);
        var format = "P" + decimalPlaces;
        return value.ToString(format, culture);
    }

    /// <summary>
    ///     Rounds a decimal value to a specified number of decimal places.
    /// </summary>
    /// <param name="value">The decimal value to round.</param>
    /// <param name="decimalPlaces">The number of decimal places to round to.</param>
    /// <returns>The rounded decimal value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal RoundTo(this decimal value, int decimalPlaces)
    {
        return Math.Round(value, decimalPlaces);
    }

    /// <summary>
    ///     Converts a decimal value to its word representation.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>A string representing the given decimal value in words.</returns>
    /// <example>
    ///     <code>
    /// decimal value = 1234.56m;
    /// string words = value.ToWords();
    /// // Output: "one thousand two hundred thirty-four and fifty-six cents"
    /// </code>
    /// </example>
    public static string ToWords(this decimal value)
    {
        if (value == 0)
        {
            return "zero";
        }

        var integerPart = (long)Math.Floor(value);
        var fractionalPart = (long)((value - integerPart) * 100);

        var words = new StringBuilder();
        words.Append(NumberToWords(integerPart));

        if (fractionalPart <= 0)
        {
            return words.ToString();
        }

        words.Append(" and ");
        words.Append(NumberToWords(fractionalPart));
        words.Append(" cents");

        return words.ToString();
    }

    private static string NumberToWords(long number)
    {
        switch (number)
        {
            case 0:
                return "zero";
            case < 0:
                return "minus " + NumberToWords(Math.Abs(number));
        }

        var words = "";

        if (number / 1000000 > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number <= 0)
        {
            return words.Trim();
        }

        if (words != "")
        {
            words += "and ";
        }

        var unitsMap = new[]
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven",
            "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
        };
        var tensMap = new[]
            { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        if (number < 20)
        {
            words += unitsMap[number];
        }
        else
        {
            words += tensMap[number / 10];
            if (number % 10 > 0)
            {
                words += "-" + unitsMap[number % 10];
            }
        }

        return words.Trim();
    }

    /// <summary>
    ///     Checks if a decimal value is a whole number.
    /// </summary>
    /// <param name="value">The decimal value to check.</param>
    /// <returns><c>true</c> if the decimal value is a whole number; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWholeNumber(this decimal value)
    {
        return value == Math.Truncate(value);
    }

    /// <summary>
    ///     Gets the absolute value of a decimal.
    /// </summary>
    /// <param name="value">The decimal value to get the absolute value of.</param>
    /// <returns>The absolute value of the decimal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal Abs(this decimal value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    ///     Converts a nullable decimal value to a nullable double.
    /// </summary>
    /// <param name="value">The nullable decimal value to convert.</param>
    /// <returns>
    ///     A nullable double representing the converted value if <paramref name="value" /> has a value; otherwise, <c>null</c>
    ///     .
    /// </returns>
    /// <remarks>
    ///     This method converts the given nullable decimal value to a nullable double, which might result in a loss of
    ///     precision
    ///     since a double has less precision compared to a decimal. Use this method when the higher range of a double
    ///     is required over the precision of a decimal.
    /// </remarks>
    /// <example>
    ///     The following example shows how to use the <see cref="ToDouble" /> method to convert a nullable decimal value to a
    ///     nullable double.
    ///     <code>
    /// decimal? value = 1234.56m;
    /// double? doubleValue = value.ToDouble();
    /// Console.WriteLine("Double value: {0}", doubleValue);
    /// 
    /// decimal? nullValue = null;
    /// double? nullDoubleValue = nullValue.ToDouble();
    /// Console.WriteLine("Double value: {0}", nullDoubleValue);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? ToDouble(this decimal? value)
    {
        return value.HasValue ? (double?)value.Value : null;
    }
}