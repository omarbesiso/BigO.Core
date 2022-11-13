using JetBrains.Annotations;

namespace BigO.Core;

/// <summary>
///     Thread safe random number and string generator.
/// </summary>
[PublicAPI]
public static class RandomGenerator
{
    private static Random RandomSeed => Random.Shared;

    /// <summary>
    ///     Generates a random integer.
    /// </summary>
    /// <param name="min">The minimum integer to generate.</param>
    /// <param name="max">The maximum integer to generate.</param>
    /// <returns>A random integer.</returns>
    /// <remarks>
    ///     The lower and upper bound are inclusive.
    /// </remarks>
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
    ///     Generates a random number.
    /// </summary>
    /// <param name="min">The minimum number to generate. Default is 0.</param>
    /// <param name="max">The maximum number to generate. Default is <see cref="int.MaxValue" />.</param>
    /// <param name="digits">The number or digits to round to. Default is 2.</param>
    /// <returns>A random number within the specified range.</returns>
    public static double RandomNumber(int digits = 2, int min = 0, int max = int.MaxValue)
    {
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
    ///     Generates a random <see cref="DateTime" /> object.
    /// </summary>
    /// <param name="from">The minimum <see cref="System.DateTime" /> generation boundary.</param>
    /// <param name="to">The maximum <see cref="System.DateTime" /> generation boundary.</param>
    /// <returns>A random <see cref="System.DateTime" />.</returns>
    public static DateTime RandomDate(DateTime from, DateTime to)
    {
        var range = new TimeSpan(to.Ticks - from.Ticks);
        return from + new TimeSpan((long)(range.Ticks * RandomSeed.NextDouble()));
    }

    /// <summary>
    ///     Generates a random string complying with the specifications provided.
    /// </summary>
    /// <param name="stringSize">The size of the string. Cannot be less than 1.</param>
    /// <param name="allowLowerCaseCharacters">Allows uppercase characters in the string.</param>
    /// <param name="allowUpperCaseCharacters">Allows lowercase characters in the string.</param>
    /// <param name="allowDigits">Allows digits in the string.</param>
    /// <param name="allowSpecialCharacters">Allows special characters in the string.</param>
    /// <param name="charactersToExclude">(Optional) The characters to exclude from the </param>
    /// <returns>The random string that was generated.</returns>
    public static string RandomString(int stringSize, bool allowLowerCaseCharacters = true,
        bool allowUpperCaseCharacters = true, bool allowDigits = true,
        bool allowSpecialCharacters = true, params char[]? charactersToExclude)
    {
        var randomStringBuilder = new RandomStringBuilder(stringSize, allowLowerCaseCharacters,
            allowUpperCaseCharacters, allowDigits, allowSpecialCharacters, charactersToExclude);
        return randomStringBuilder.Build();
    }
}