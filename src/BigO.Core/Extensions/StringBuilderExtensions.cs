using System.Text;
using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="StringBuilder" /> objects.
/// </summary>
[PublicAPI]
public static class StringBuilderExtensions
{
    /// <summary>
    ///     Determines whether the specified <see cref="StringBuilder" /> is empty.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to check.</param>
    /// <param name="countWhiteSpace">
    ///     A flag that indicates whether white space should be considered when determining if the <see cref="StringBuilder" />
    ///     is empty.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="StringBuilder" /> is empty; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="stringBuilder" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     If <paramref name="countWhiteSpace" /> is <c>false</c>, the <see cref="StringBuilder" /> is considered empty if its
    ///     <see cref="StringBuilder.Length" /> property is 0. If <paramref name="countWhiteSpace" /> is <c>true</c>, the
    ///     <see cref="StringBuilder" /> is considered empty if its <see cref="StringBuilder.ToString" /> method returns
    ///     <c>null</c> or
    ///     a string that consists only of white space.
    /// </remarks>
    public static bool IsEmpty(this StringBuilder stringBuilder, bool countWhiteSpace = false)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        switch (countWhiteSpace)
        {
            case false when stringBuilder.Length == 0:
            case true when string.IsNullOrWhiteSpace(stringBuilder.ToString()):
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Appends a character to the end of a <see cref="StringBuilder" /> object until its length
    ///     reaches the specified target length.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to which to append the character.</param>
    /// <param name="targetLength">The target length to which to append the character.</param>
    /// <param name="charToAppend">The character to append.</param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    public static StringBuilder AppendCharToLength(this StringBuilder stringBuilder, int targetLength,
        char charToAppend)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        var stringBuilderLength = stringBuilder.Length;
        if (stringBuilderLength >= targetLength)
        {
            return stringBuilder;
        }

        while (stringBuilderLength < targetLength)
        {
            stringBuilder.Append(charToAppend);
            stringBuilderLength++;
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Reduces the length of a <see cref="StringBuilder" /> object to the specified maximum length.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to reduce in length.</param>
    /// <param name="maxLength">The maximum length to which to reduce the <see cref="StringBuilder" /> object.</param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="maxLength" /> is less than 0.</exception>
    public static StringBuilder ReduceToLength(this StringBuilder stringBuilder, int maxLength)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        switch (maxLength)
        {
            case < 0:
                throw new ArgumentException("The maxlength cannot be less then 0.", nameof(maxLength));
            case 0:
                stringBuilder.Clear();
                return stringBuilder;
            default:
                stringBuilder.Length = maxLength;
                return stringBuilder;
        }
    }

    /// <summary>
    ///     Reverses the characters in a <see cref="StringBuilder" /> object.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to reverse.</param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    public static StringBuilder Reverse(this StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        if (stringBuilder.Length == 0)
        {
            return stringBuilder;
        }

        var originalString = stringBuilder.ToString();

        stringBuilder.Clear();

        var chars = originalString.ToCharArray();
        Array.Reverse(chars);

        foreach (var c in chars)
        {
            stringBuilder.Append(c);
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that a <see cref="StringBuilder" /> object starts with a specified prefix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to modify.</param>
    /// <param name="prefix">
    ///     The prefix to add to the start of the <see cref="StringBuilder" /> object, if it does not already
    ///     start with it.
    /// </param>
    /// <param name="stringComparison">
    ///     The string comparison method to use when comparing the prefix to the start of the
    ///     <see cref="StringBuilder" /> object.
    /// </param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="prefix" /> is <c>null</c> or whitespace.</exception>
    public static StringBuilder EnsureStartsWith(this StringBuilder stringBuilder, string prefix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentNullException(nameof(prefix), "The prefix cannot be null or whitespace.");
        }

        if (stringBuilder.ToString().StartsWith(prefix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Insert(0, prefix);
        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that a <see cref="StringBuilder" /> object ends with a specified suffix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to modify.</param>
    /// <param name="suffix">
    ///     The suffix to add to the end of the <see cref="StringBuilder" /> object, if it does not already
    ///     end with it.
    /// </param>
    /// <param name="stringComparison">
    ///     The string comparison method to use when comparing the suffix to the end of the
    ///     <see cref="StringBuilder" /> object.
    /// </param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="suffix" /> is <c>null</c> or whitespace.</exception>
    public static StringBuilder EnsureEndsWith(this StringBuilder stringBuilder, string suffix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        if (string.IsNullOrWhiteSpace(suffix))
        {
            throw new ArgumentNullException(nameof(suffix), "The suffix cannot be null or whitespace.");
        }

        if (stringBuilder.ToString().EndsWith(suffix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Append(suffix);
        return stringBuilder;
    }

    /// <summary>
    ///     Appends the given string items to the end of the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append the items to.</param>
    /// <param name="withNewLine">Indicates whether a new line should be added after each item. Default value is <c>false</c>.</param>
    /// <param name="items">The string items to append. This parameter is optional and can be <c>null</c>.</param>
    /// <returns>The <see cref="StringBuilder" /> with the items appended to it.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     If <paramref name="items" /> is <c>null</c> or empty, the method returns the <paramref name="stringBuilder" />
    ///     without modifying it.
    ///     If any of the items in <paramref name="items" /> are <c>null</c> or empty, they are ignored.
    /// </remarks>
    public static StringBuilder AppendMultiple(this StringBuilder stringBuilder, bool withNewLine = false,
        params string[]? items)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        if (items == null || !items.Any())
        {
            return stringBuilder;
        }

        foreach (var item in items.Where(item => !string.IsNullOrEmpty(item)))
        {
            if (withNewLine)
            {
                stringBuilder.AppendLine(item);
            }
            else
            {
                stringBuilder.Append(item);
            }
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Removes all occurrences of the given character from the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to remove the characters from.</param>
    /// <param name="characterToBeRemoved">The character to remove from the <paramref name="stringBuilder" />.</param>
    /// <returns>The <see cref="StringBuilder" /> with all occurrences of the given character removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method iterates through the <paramref name="stringBuilder" /> and removes any occurrences of the given
    ///     character.
    /// </remarks>
    public static StringBuilder RemoveAllOccurrences(this StringBuilder stringBuilder, char characterToBeRemoved)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        for (var i = 0; i < stringBuilder.Length;)
        {
            if (stringBuilder[i] == characterToBeRemoved)
            {
                stringBuilder.Remove(i, 1);
            }
            else
            {
                i++;
            }
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Removes all leading and trailing white space characters from the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to remove the white space characters from.</param>
    /// <returns>The <see cref="StringBuilder" /> with all leading and trailing white space characters removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method iterates through the <paramref name="stringBuilder" /> and removes all leading and trailing white space
    ///     characters.
    ///     If the <paramref name="stringBuilder" /> is empty or consists only of white space characters, it is returned
    ///     without modification.
    /// </remarks>
    public static StringBuilder Trim(this StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        if (stringBuilder.Length == 0)
        {
            return stringBuilder;
        }

        var length = 0;
        var num2 = stringBuilder.Length;
        while (char.IsWhiteSpace(stringBuilder[length]) && length < num2)
        {
            length++;
        }

        if (length > 0)
        {
            stringBuilder.Remove(0, length);
            num2 = stringBuilder.Length;
        }

        length = num2 - 1;
        while (char.IsWhiteSpace(stringBuilder[length]) && length > -1)
        {
            length--;
        }

        if (length < num2 - 1)
        {
            stringBuilder.Remove(length + 1, num2 - length - 1);
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Appends a formatted string followed by a new line to the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append the formatted string to.</param>
    /// <param name="format">The composite format string.</param>
    /// <param name="items">The objects to format.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="stringBuilder" /> or <paramref name="format" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <exception cref="FormatException">Thrown if the format of <paramref name="format" /> is invalid.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the format of <paramref name="format" /> is invalid and
    ///     specifies a replacement index that is greater than or equal to the number of objects in <paramref name="items" />.
    /// </exception>
    /// <remarks>
    ///     This method uses the <see cref="StringBuilder.AppendFormat(string, object[])" /> method to append a formatted
    ///     string to the <paramref name="stringBuilder" />.
    ///     It then appends a new line to the end of the formatted string.
    /// </remarks>
    public static void AppendFormatLine(this StringBuilder stringBuilder, string format, params object[] items)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        if (string.IsNullOrWhiteSpace(format))
        {
            throw new ArgumentNullException(nameof(format));
        }

        stringBuilder.AppendFormat(format, items);
        stringBuilder.AppendLine();
    }

    /// <summary>
    ///     Appends a specified number of new line characters to the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to append the new line characters to.</param>
    /// <param name="numberOfLines">The number of new line characters to append. Default value is 1.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="numberOfLines" /> is less than 1.</exception>
    /// <remarks>
    ///     This method uses the <see cref="StringBuilder.AppendLine()" /> method to append a new line character to the
    ///     <paramref name="stringBuilder" />.
    ///     It repeats this process for the number of times specified by <paramref name="numberOfLines" />.
    /// </remarks>
    public static void AppendLine(this StringBuilder stringBuilder, int numberOfLines = 1)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        if (numberOfLines < 1)
        {
            throw new ArgumentException("The number of lines cannot be less than 1.", nameof(numberOfLines));
        }

        for (var i = 0; i < numberOfLines; i++)
        {
            stringBuilder.AppendLine();
        }
    }
}