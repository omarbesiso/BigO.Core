using System.Security.Cryptography;
using System.Text;

namespace BigO.Core;

/// <summary>
///     Thread safe random number and string generator.
/// </summary>
[PublicAPI]
public static class RandomGenerator
{
    private const string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = @"!#$%&*+-/:;<=>?@[\]^_`{|}~";
    private static Random RandomSeed => Random.Shared;

    /// <summary>
    ///     Generates a random integer between 0 and <see cref="int.MaxValue" /> inclusive.
    /// </summary>
    /// <param name="allowNegative">If true, allows negative integers.</param>
    /// <returns>A random integer.</returns>
    public static int RandomInt(bool allowNegative = false)
    {
        return RandomInt(allowNegative ? int.MinValue : 0, int.MaxValue);
    }

    /// <summary>
    ///     Generates a random integer between specified bounds.
    /// </summary>
    /// <param name="min">The minimum integer to generate.</param>
    /// <param name="max">The maximum integer to generate.</param>
    /// <returns>A random integer.</returns>
    public static int RandomInt(int min, int max)
    {
        if (max <= min)
        {
            throw new ArgumentOutOfRangeException(nameof(max),
                $"The maximum value '{max}' must be greater than the minimum value '{min}'.");
        }

        return RandomSeed.Next(min, max + 1);
    }

    /// <summary>
    ///     Generates a random number between 0 and <see cref="int.MaxValue" /> inclusive, rounded to 2 decimal places.
    /// </summary>
    /// <param name="allowNegative">If true, allows negative numbers.</param>
    /// <returns>A random number within the specified range.</returns>
    public static double RandomNumber(bool allowNegative = false)
    {
        return RandomNumber(2, allowNegative ? int.MinValue : 0);
    }

    /// <summary>
    ///     Generates a random number within the specified range.
    /// </summary>
    /// <param name="digits">The number of digits to round to. Default is 2.</param>
    /// <param name="min">The minimum number to generate.</param>
    /// <param name="max">The maximum number to generate. Default is <see cref="int.MaxValue" />.</param>
    /// <returns>A random number within the specified range.</returns>
    public static double RandomNumber(int digits = 2, int min = 0, int max = int.MaxValue)
    {
        if (max <= min)
        {
            throw new ArgumentOutOfRangeException(nameof(max),
                $"The maximum value '{max}' must be greater than the minimum value '{min}'.");
        }

        return Math.Round(RandomSeed.Next(min, max - 1) + RandomSeed.NextDouble(), digits);
    }

    /// <summary>
    ///     Generates a random boolean value.
    /// </summary>
    /// <returns>A random boolean value.</returns>
    public static bool RandomBool()
    {
        return RandomSeed.NextDouble() > 0.5;
    }

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object.
    /// </summary>
    /// <returns>A random <see cref="DateTime" />.</returns>
    public static DateTime RandomDate()
    {
        return RandomDate(new DateTime(1900, 1, 1), DateTime.Now);
    }

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object within the specified date and time range.
    /// </summary>
    /// <param name="fromDate">The minimum <see cref="System.DateTime" /> generation boundary.</param>
    /// <param name="toDate">The maximum <see cref="System.DateTime" /> generation boundary.</param>
    /// <param name="fromTime">The minimum time of day.</param>
    /// <param name="toTime">The maximum time of day.</param>
    /// <returns>A random <see cref="System.DateTime" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when 'fromDate' is greater than 'toDate' or 'fromTime' is greater
    ///     than 'toTime'.
    /// </exception>
    public static DateTime RandomDate(DateTime fromDate, DateTime toDate, TimeSpan fromTime, TimeSpan toTime)
    {
        if (fromDate >= toDate)
        {
            throw new ArgumentOutOfRangeException(nameof(toDate), "The 'toDate' must be greater than the 'fromDate'.");
        }

        if (fromTime > toTime)
        {
            throw new ArgumentOutOfRangeException(nameof(toTime), "The 'toTime' must be greater than the 'fromTime'.");
        }

        var randomDate = RandomDate(fromDate, toDate);
        var randomTimeSpan = new TimeSpan((long)(RandomSeed.NextDouble() * (toTime - fromTime).Ticks));
        return randomDate.Date + fromTime + randomTimeSpan;
    }

    /// <summary>
    ///     Generates a random <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="from">The minimum <see cref="System.DateTime" /> generation boundary.</param>
    /// <param name="to">The maximum <see cref="System.DateTime" /> generation boundary.</param>
    /// <returns>A random <see cref="System.DateTime" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when 'from' is greater than 'to'.</exception>
    public static DateTime RandomDate(DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentOutOfRangeException(nameof(to),
                $"The 'to' date '{to}' must be greater than the 'from' date '{from}'.");
        }

        var range = new TimeSpan(to.Ticks - from.Ticks);
        return from + new TimeSpan((long)(range.Ticks * RandomSeed.NextDouble()));
    }

    /// <summary>
    ///     Generates a random string complying with the specifications provided.
    /// </summary>
    /// <param name="stringSize">The size of the string. Cannot be less than 1.</param>
    /// <param name="includeLowerCaseCharacters">Includes uppercase characters in the string.</param>
    /// <param name="includeUpperCaseCharacters">Includes lowercase characters in the string.</param>
    /// <param name="includeDigits">Includes digits in the string.</param>
    /// <param name="includeSpecialCharacters">Includes special characters in the string.</param>
    /// <param name="charactersToExclude">(Optional) The characters to exclude from the </param>
    /// <returns>The random string that was generated.</returns>
    public static string RandomString(int stringSize, bool includeLowerCaseCharacters = true,
        bool includeUpperCaseCharacters = true, bool includeDigits = true,
        bool includeSpecialCharacters = true, params char[]? charactersToExclude)
    {
        if (stringSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(stringSize), "The string size must be at least 1.");
        }

        var allowedCharacters = BuildAllowedCharacters(includeLowerCaseCharacters, includeUpperCaseCharacters,
            includeDigits, includeSpecialCharacters, charactersToExclude);

        if (string.IsNullOrEmpty(allowedCharacters))
        {
            throw new ArgumentException("At least one type of character must be included.");
        }

        var bytes = new byte[stringSize * sizeof(char)];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        var result = new char[stringSize];
        for (var i = 0; i < stringSize; i++)
        {
            var value = BitConverter.ToUInt16(bytes, i * 2);
            result[i] = allowedCharacters[value % allowedCharacters.Length];
        }

        return new string(result);
    }

    /// <summary>
    ///     Builds the allowed characters string based on the specified options.
    /// </summary>
    /// <param name="includeLowerCase">Includes lowercase characters if true.</param>
    /// <param name="includeUpperCase">Includes uppercase characters if true.</param>
    /// <param name="includeDigits">Includes digit characters if true.</param>
    /// <param name="includeSpecial">Includes special characters if true.</param>
    /// <param name="excludeChars">An optional array of characters to exclude.</param>
    /// <returns>A string containing the allowed characters.</returns>
    private static string BuildAllowedCharacters(bool includeLowerCase, bool includeUpperCase,
        bool includeDigits, bool includeSpecial, char[]? excludeChars)
    {
        var allowedChars = new StringBuilder(LowerCaseCharacters.Length + UpperCaseCharacters.Length +
                                             Digits.Length + SpecialCharacters.Length);

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

        return excludeChars != null && excludeChars.Length > 0
            ? new string(allowedCharsString.Except(excludeChars).ToArray())
            : allowedCharsString;
    }


    /// <summary>
    ///     Generates a random email address using a random local part and domain.
    /// </summary>
    /// <param name="localPartLength">The length of the local part of the email address.</param>
    /// <param name="domainLength">The length of the domain part of the email address.</param>
    /// <param name="topLevelDomains">A list of top-level domains to choose from.</param>
    /// <returns>A random email address.</returns>
    public static string RandomEmail(int localPartLength = 10, int domainLength = 10,
        params string[]? topLevelDomains)
    {
        if (localPartLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(localPartLength), "The local part length must be at least 1.");
        }

        if (domainLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(domainLength), "The domain length must be at least 1.");
        }

        if (topLevelDomains == null || topLevelDomains.Length == 0)
        {
            topLevelDomains = ["com"];
        }

        // Generate a random local part for the email address
        var localPart = RandomString(localPartLength, includeSpecialCharacters: false).ToLower();

        // Generate a random domain for the email address
        var domain = RandomString(domainLength, includeSpecialCharacters: false).ToLower();

        // Select a random TLD from the provided list
        var tld = RandomElement(topLevelDomains);

        return $"{localPart}@{domain}.{tld}";
    }

    /// <summary>
    ///     Selects a random element from the specified array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to select a random element from.</param>
    /// <returns>A random element from the array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the array is empty.</exception>
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

        return array[RandomInt(0, array.Length - 1)];
    }

    /// <summary>
    ///     Selects a random element from the specified list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to select a random element from.</param>
    /// <returns>A random element from the list.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the list is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the list is empty.</exception>
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

        return list[RandomInt(0, list.Count - 1)];
    }
}