using System.Security.Cryptography;
using System.Text;

namespace BigO.Core;

/// <summary>
///     Thread-safe random number and string generator.
/// </summary>
[PublicAPI]
public static class RandomGenerator
{
    private const string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = @"!#$%&*+-/:;<=>?@[\]^_`{|}~";

    // In .NET 6+, Random.Shared is thread-safe.
    // It provides a convenient static instance for non-cryptographic random usage.
    private static Random RandomSeed => Random.Shared;

    #region Random Boolean

    /// <summary>
    ///     Generates a random boolean value.
    /// </summary>
    /// <returns>A random boolean value.</returns>
    public static bool RandomBool()
    {
        // 50% chance for true, 50% chance for false
        return RandomSeed.NextDouble() >= 0.5;
    }

    #endregion

    #region Random Email

    /// <summary>
    ///     Generates a random email address with random local part, domain, and TLD.
    /// </summary>
    /// <param name="localPartLength">Length of the local part (before '@'). Must be at least 1.</param>
    /// <param name="domainLength">Length of the domain part (between '@' and '.'). Must be at least 1.</param>
    /// <param name="topLevelDomains">
    ///     An array of TLDs to choose from (e.g., "com", "org", "net").
    ///     If null or empty, defaults to "com".
    /// </param>
    /// <returns>A random email address.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="localPartLength" /> or
    ///     <paramref name="domainLength" /> is less than 1.
    /// </exception>
    public static string RandomEmail(int localPartLength = 10, int domainLength = 10, params string[]? topLevelDomains)
    {
        if (localPartLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(localPartLength),
                "The local part length must be at least 1.");
        }

        if (domainLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(domainLength),
                "The domain length must be at least 1.");
        }

        if (topLevelDomains == null || topLevelDomains.Length == 0)
        {
            topLevelDomains = ["com"];
        }

        // Generate a random local part (only letters & digits by default).
        var localPart = RandomString(localPartLength, includeSpecialCharacters: false).ToLowerInvariant();

        // Generate a random domain (only letters & digits by default).
        var domain = RandomString(domainLength, includeSpecialCharacters: false).ToLowerInvariant();

        // Select a random TLD from the provided list.
        var tld = RandomElement(topLevelDomains);

        return $"{localPart}@{domain}.{tld}";
    }

    #endregion

    #region Random Integers

    /// <summary>
    ///     Generates a random integer between 0 and <see cref="int.MaxValue" /> (inclusive).
    /// </summary>
    /// <param name="allowNegative">
    ///     If true, allows negative integers by using the range [<see cref="int.MinValue" />,
    ///     <see cref="int.MaxValue" />].
    /// </param>
    /// <returns>A random integer.</returns>
    public static int RandomInt(bool allowNegative = false)
    {
        return allowNegative
            ? RandomInt(int.MinValue, int.MaxValue)
            : RandomInt(0, int.MaxValue);
    }

    /// <summary>
    ///     Generates a random integer between specified bounds (inclusive).
    /// </summary>
    /// <param name="min">The minimum integer to generate (inclusive).</param>
    /// <param name="max">The maximum integer to generate (inclusive).</param>
    /// <returns>A random integer between <paramref name="min" /> and <paramref name="max" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="max" /> is less than <paramref name="min" />.</exception>
    public static int RandomInt(int min, int max)
    {
        if (max < min)
        {
            throw new ArgumentOutOfRangeException(nameof(max),
                $"The maximum value '{max}' must be greater than or equal to the minimum value '{min}'.");
        }

        // Use NextDouble to avoid overflow issues with Next(..., max+1).
        // This yields a uniform distribution in the inclusive range [min, max].
        var range = (long)max - min + 1; // can be up to 2^32 for int range; still safe for double precision
        var sample = RandomSeed.NextDouble(); // in [0, 1)
        // Multiply the range by sample => in [0, range)
        // Floor to get an integral offset => in [0, range-1]
        var offset = (long)Math.Floor(sample * range);

        return (int)(min + offset);
    }

    #endregion

    #region Random Double

    /// <summary>
    ///     Generates a random double between 0 and <see cref="int.MaxValue" /> (inclusive), rounded to 2 decimal places.
    /// </summary>
    /// <param name="allowNegative">
    ///     If true, allows negative numbers by using the range [<see cref="int.MinValue" />,
    ///     <see cref="int.MaxValue" />].
    /// </param>
    /// <returns>A random double.</returns>
    public static double RandomNumber(bool allowNegative = false)
    {
        return RandomNumber(2, allowNegative ? int.MinValue : 0);
    }

    /// <summary>
    ///     Generates a random double between <paramref name="min" /> and <paramref name="max" /> (inclusive),
    ///     rounded to <paramref name="digits" /> decimal places.
    /// </summary>
    /// <param name="digits">Number of digits to round to (defaults to 2).</param>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive). Defaults to <see cref="int.MaxValue" />.</param>
    /// <returns>A random double in the inclusive range [<paramref name="min" />, <paramref name="max" />].</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="max" /> is less than <paramref name="min" />.</exception>
    public static double RandomNumber(int digits = 2, int min = 0, int max = int.MaxValue)
    {
        if (max < min)
        {
            throw new ArgumentOutOfRangeException(nameof(max),
                $"The maximum value '{max}' must be greater than or equal to the minimum value '{min}'.");
        }

        // Uniform distribution in [min, max].
        var range = (double)max - min;
        var value = RandomSeed.NextDouble() * range + min;
        return Math.Round(value, digits);
    }

    #endregion

    #region Random DateTime

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object between the year 1900 and "now".
    /// </summary>
    /// <returns>A random <see cref="DateTime" /> in the range [1900-01-01, DateTime.Now].</returns>
    public static DateTime RandomDate()
    {
        return RandomDate(new DateTime(1900, 1, 1), DateTime.Now);
    }

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object within the specified date and time range.
    /// </summary>
    /// <param name="fromDate">The minimum <see cref="DateTime" /> generation boundary (inclusive).</param>
    /// <param name="toDate">The maximum <see cref="DateTime" /> generation boundary (exclusive by default).</param>
    /// <param name="fromTime">The minimum time of day.</param>
    /// <param name="toTime">The maximum time of day.</param>
    /// <returns>A random <see cref="DateTime" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="fromDate" /> is greater than <paramref name="toDate" />
    ///     or <paramref name="fromTime" /> is greater than <paramref name="toTime" />.
    /// </exception>
    public static DateTime RandomDate(DateTime fromDate, DateTime toDate, TimeSpan fromTime, TimeSpan toTime)
    {
        if (fromDate >= toDate)
        {
            throw new ArgumentOutOfRangeException(nameof(toDate),
                "The 'toDate' must be greater than the 'fromDate'.");
        }

        if (fromTime > toTime)
        {
            throw new ArgumentOutOfRangeException(nameof(toTime),
                "The 'toTime' must be greater than the 'fromTime'.");
        }

        var randomDate = RandomDate(fromDate, toDate);
        // Generate random time between fromTime and toTime
        var timeRange = toTime - fromTime;
        var randomTimeSpan = new TimeSpan((long)(RandomSeed.NextDouble() * timeRange.Ticks));

        return randomDate.Date + fromTime + randomTimeSpan;
    }

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object within the specified date range.
    /// </summary>
    /// <param name="from">The minimum <see cref="DateTime" /> boundary (inclusive).</param>
    /// <param name="to">The maximum <see cref="DateTime" /> boundary.</param>
    /// <returns>A random <see cref="DateTime" /> in the range [<paramref name="from" />, <paramref name="to" />).</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="from" /> is greater than or equal to
    ///     <paramref name="to" />.
    /// </exception>
    public static DateTime RandomDate(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentOutOfRangeException(nameof(to),
                $"The 'to' date '{to}' must be greater than the 'from' date '{from}'.");
        }

        var range = to.Ticks - from.Ticks;
        // Random double in [0, 1)
        var randomFraction = RandomSeed.NextDouble();
        var randomTicks = (long)(range * randomFraction);

        return from.AddTicks(randomTicks);
    }

    #endregion

    #region Random String

    /// <summary>
    ///     Generates a random string based on various character-inclusion flags and optional exclusions.
    ///     Uses a cryptographically secure random number generator for character selection.
    /// </summary>
    /// <param name="stringSize">The size of the generated string (at least 1).</param>
    /// <param name="includeLowerCaseCharacters">Whether to include lowercase letters.</param>
    /// <param name="includeUpperCaseCharacters">Whether to include uppercase letters.</param>
    /// <param name="includeDigits">Whether to include digit characters.</param>
    /// <param name="includeSpecialCharacters">Whether to include special characters (e.g., !@#$...).</param>
    /// <param name="charactersToExclude">Optional array of characters to exclude from the generated string.</param>
    /// <returns>A random string of length <paramref name="stringSize" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="stringSize" /> is less than 1.</exception>
    /// <exception cref="ArgumentException">Thrown if no character sets remain after exclusions.</exception>
    public static string RandomString(
        int stringSize,
        bool includeLowerCaseCharacters = true,
        bool includeUpperCaseCharacters = true,
        bool includeDigits = true,
        bool includeSpecialCharacters = true,
        params char[]? charactersToExclude)
    {
        if (stringSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(stringSize),
                "The string size must be at least 1.");
        }

        var allowedCharacters = BuildAllowedCharacters(
            includeLowerCaseCharacters,
            includeUpperCaseCharacters,
            includeDigits,
            includeSpecialCharacters,
            charactersToExclude);

        if (string.IsNullOrEmpty(allowedCharacters))
        {
            throw new ArgumentException("At least one type of character must be included.");
        }

        // We use a cryptographically secure approach for strings.
        // 1 char = 2 bytes in Unicode, so we generate stringSize * 2 bytes.
        var bytes = new byte[stringSize * sizeof(char)];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new char[stringSize];
        for (var i = 0; i < stringSize; i++)
        {
            // Convert 2 bytes into a ushort, then mod by allowedCharacters.Length
            var value = BitConverter.ToUInt16(bytes, i * 2);
            result[i] = allowedCharacters[value % allowedCharacters.Length];
        }

        return new string(result);
    }

    /// <summary>
    ///     Builds the allowed characters string based on the specified options and exclusions.
    /// </summary>
    /// <param name="includeLowerCase">Include lowercase characters if true.</param>
    /// <param name="includeUpperCase">Include uppercase characters if true.</param>
    /// <param name="includeDigits">Include digits if true.</param>
    /// <param name="includeSpecial">Include special characters if true.</param>
    /// <param name="excludeChars">Optional array of characters to exclude.</param>
    /// <returns>A string containing all allowed characters.</returns>
    private static string BuildAllowedCharacters(
        bool includeLowerCase,
        bool includeUpperCase,
        bool includeDigits,
        bool includeSpecial,
        char[]? excludeChars)
    {
        var allowedChars = new StringBuilder(
            LowerCaseCharacters.Length +
            UpperCaseCharacters.Length +
            Digits.Length +
            SpecialCharacters.Length);

        if (includeLowerCase)
        {
            allowedChars.Append(LowerCaseCharacters);
        }

        if (includeUpperCase)
        {
            allowedChars.Append(UpperCaseCharacters);
        }

        if (includeDigits)
        {
            allowedChars.Append(Digits);
        }

        if (includeSpecial)
        {
            allowedChars.Append(SpecialCharacters);
        }

        var allowedCharsString = allowedChars.ToString();

        // Remove excluded chars, if any
        return excludeChars is { Length: > 0 }
            ? new string(allowedCharsString.Except(excludeChars).ToArray())
            : allowedCharsString;
    }

    #endregion

    #region Random Element from Collections

    /// <summary>
    ///     Selects a random element from the specified array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to select a random element from.</param>
    /// <returns>A random element from <paramref name="array" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="array" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="array" /> is empty.</exception>
    public static T RandomElement<T>(T[]? array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), "Array cannot be null.");
        }

        if (array.Length == 0)
        {
            throw new ArgumentException("Array cannot be empty.", nameof(array));
        }

        // Use the inclusive integer method: index in [0, array.Length - 1]
        var index = RandomInt(0, array.Length - 1);
        return array[index];
    }

    /// <summary>
    ///     Selects a random element from the specified list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to select a random element from.</param>
    /// <returns>A random element from <paramref name="list" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="list" /> is empty.</exception>
    public static T RandomElement<T>(List<T> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list), "List cannot be null.");
        }

        if (list.Count == 0)
        {
            throw new ArgumentException("List cannot be empty.", nameof(list));
        }

        // Use the inclusive integer method: index in [0, list.Count - 1]
        var index = RandomInt(0, list.Count - 1);
        return list[index];
    }

    #endregion
}