using System.Text;
using JetBrains.Annotations;

// ReSharper disable InvalidXmlDocComment

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="StringBuilder" /> objects.
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
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Appends a character to the end of a <see cref="StringBuilder" /> object until its length reaches the specified
    ///     target length.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> object to which to append the character.</param>
    /// <param name="targetLength">The target length to which to append the character.</param>
    /// <param name="charToAppend">The character to append.</param>
    /// <returns>The modified <see cref="StringBuilder" /> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    public static StringBuilder AppendCharToLength(this StringBuilder stringBuilder, int targetLength,
        char charToAppend)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Reduces the length of the specified StringBuilder to the specified maximum length.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder to reduce in length.</param>
    /// <param name="maxLength">The maximum length to reduce the StringBuilder to.</param>
    /// <returns>The modified StringBuilder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="maxLength" /> is less than 0.</exception>
    /// <remarks>
    ///     If <paramref name="maxLength" /> is 0, this method clears the StringBuilder. If <paramref name="maxLength" /> is
    ///     greater than 0, this method sets the length of the StringBuilder to the specified maximum length.
    /// </remarks>
    public static StringBuilder ReduceToLength(this StringBuilder stringBuilder, int maxLength)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Reverses the characters in the specified <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to reverse.</param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method converts the <see cref="StringBuilder" /> to a string, reverses the characters in the string using the
    ///     <see cref="Array.Reverse(Array)" /> method, and then clears the <see cref="StringBuilder" /> and appends the
    ///     reversed characters to it.
    /// </remarks>
    public static StringBuilder Reverse(this StringBuilder stringBuilder)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Ensures that the specified <see cref="StringBuilder" /> starts with the specified prefix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to modify.</param>
    /// <param name="prefix">The prefix to ensure the <see cref="StringBuilder" /> starts with.</param>
    /// <param name="stringComparison">
    ///     The <see cref="StringComparison" /> to use when comparing the prefix and the
    ///     <see cref="StringBuilder" />. Default is <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="stringBuilder" /> or <paramref name="prefix" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <remarks>
    ///     If the <see cref="StringBuilder" /> is empty or already starts with the prefix (according to the specified
    ///     <paramref name="stringComparison" />), this method returns the <see cref="StringBuilder" /> as is. Otherwise, it
    ///     inserts the prefix at the beginning of the <see cref="StringBuilder" />.
    /// </remarks>
    public static StringBuilder EnsureStartsWith(this StringBuilder stringBuilder, string prefix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentNullException(nameof(prefix), "The prefix cannot be null or whitespace.");
        }

        if (stringBuilder.Length == 0 || stringBuilder.ToString().StartsWith(prefix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Insert(0, prefix);
        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that the specified <see cref="StringBuilder" /> ends with the specified suffix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to modify.</param>
    /// <param name="suffix">The suffix to ensure the <see cref="StringBuilder" /> ends with.</param>
    /// <param name="stringComparison">
    ///     The <see cref="StringComparison" /> to use when comparing the suffix and the
    ///     <see cref="StringBuilder" />. Default is <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="stringBuilder" /> or <paramref name="suffix" /> is
    ///     <c>null</c> or whitespace.
    /// </exception>
    /// <remarks>
    ///     If the <see cref="StringBuilder" /> already ends with the suffix (according to the specified
    ///     <paramref name="stringComparison" />), this method returns the <see cref="StringBuilder" /> as is. Otherwise, it
    ///     appends the suffix to the end of the <see cref="StringBuilder" />.
    /// </remarks>
    public static StringBuilder EnsureEndsWith(this StringBuilder stringBuilder, string suffix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(suffix))
        {
            throw new ArgumentNullException(nameof(suffix),
                $"The {nameof(stringBuilder)} cannot be null or whitespace.");
        }

        if (stringBuilder.ToString().EndsWith(suffix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Append(suffix);
        return stringBuilder;
    }

    /// <summary>
    ///     Appends multiple items to the specified <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to modify.</param>
    /// <param name="withNewLine">Indicates whether to append each item with a new line. Default is <c>false</c>.</param>
    /// <param name="items">The items to append to the <see cref="StringBuilder" />.</param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     If <paramref name="items" /> is <c>null</c> or empty, this method returns the <see cref="StringBuilder" /> as is.
    ///     Otherwise, it appends each item in <paramref name="items" /> to the <see cref="StringBuilder" />, with an optional
    ///     new line between each item depending on the value of <paramref name="withNewLine" />.
    /// </remarks>
    public static StringBuilder AppendMultiple(this StringBuilder stringBuilder, bool withNewLine = false,
        params string[]? items)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Removes all occurrences of a specified character from the specified <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to modify.</param>
    /// <param name="characterToBeRemoved">The character to remove from the <see cref="StringBuilder" />.</param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method iterates through each character in the <see cref="StringBuilder" /> and removes any occurrences of
    ///     <paramref name="characterToBeRemoved" />.
    /// </remarks>
    public static StringBuilder RemoveAllOccurrences(this StringBuilder stringBuilder, char characterToBeRemoved)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Trims leading and trailing whitespace from the specified <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> to modify.</param>
    /// <returns>The modified <see cref="StringBuilder" />.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method removes any leading and trailing whitespace from the <see cref="StringBuilder" /> by iterating through
    ///     each character and checking if it is a whitespace character.
    /// </remarks>
    public static StringBuilder Trim(this StringBuilder stringBuilder)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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
    ///     Appends the string returned by processing a composite format string, which contains one or more placeholders that
    ///     are replaced by the string representation of a corresponding argument, to this instance.
    ///     A line terminator is also appended after the last format item.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to which this method appends the format string.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="items">An object array that contains zero or more objects to format.</param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="stringBuilder" /> cannot be <c>null</c>.
    ///     <para>-or-</para>
    ///     <paramref name="format" /> cannot be <c>null</c> or whitespace.
    /// </exception>
    public static void AppendFormatLine(this StringBuilder stringBuilder, string format, params object[] items)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(format))
        {
            throw new ArgumentNullException(nameof(format), $"The {nameof(format)} cannot be null or whitespace.");
        }

        stringBuilder.AppendFormat(format, items);
        stringBuilder.AppendLine();
    }

    /// <summary>
    ///     Appends the specified number of new line characters to the end of the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance.</param>
    /// <param name="numberOfLines">
    ///     The number of new line characters to append to the end of the <see cref="StringBuilder" />
    ///     instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="stringBuilder" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="numberOfLines" /> is less than 1.
    /// </exception>
    public static void AppendMultipleLines(this StringBuilder stringBuilder, int numberOfLines)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

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