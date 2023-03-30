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
    ///     Determines whether the <see cref="System.Text.StringBuilder" /> is empty, optionally counting white-space
    ///     characters.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="System.Text.StringBuilder" /> to check for emptiness.</param>
    /// <param name="countWhiteSpace">True to count white-space characters; otherwise, false. Default is false.</param>
    /// <returns>true if the <see cref="System.Text.StringBuilder" /> is empty; otherwise, false.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     This extension method can be used to determine whether a <see cref="System.Text.StringBuilder" /> instance is
    ///     empty.
    ///     By default, only non-white-space characters are considered when checking for emptiness, but if the
    ///     <paramref name="countWhiteSpace" /> parameter is set to true,
    ///     white-space characters are also counted. The method returns true if the <see cref="System.Text.StringBuilder" /> is
    ///     empty; otherwise, it returns false.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="IsEmpty(StringBuilder, bool)" /> method to determine
    ///     whether a StringBuilder instance is empty.
    ///     <code><![CDATA[
    /// var sb = new StringBuilder("Hello, world!");
    /// bool isEmpty = sb.IsEmpty(); // false
    /// bool isWhiteSpace = sb.IsEmpty(true); // false
    /// 
    /// sb.Clear();
    /// isEmpty = sb.IsEmpty(); // true
    /// isWhiteSpace = sb.IsEmpty(true); // true
    /// ]]></code>
    /// </example>
    public static bool IsEmpty(this StringBuilder stringBuilder, bool countWhiteSpace = false)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        // Early return if stringBuilder is empty and whitespace should not be counted
        if (stringBuilder.Length == 0 && !countWhiteSpace)
        {
            return true;
        }

        return countWhiteSpace ? string.IsNullOrWhiteSpace(stringBuilder.ToString()) : stringBuilder.Length == 0;
    }

    /// <summary>
    ///     Appends a character to the StringBuilder until it reaches the specified target length.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder instance.</param>
    /// <param name="targetLength">The desired length of the StringBuilder.</param>
    /// <param name="charToAppend">The character to append.</param>
    /// <returns>The modified StringBuilder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     If the current length of the <paramref name="stringBuilder" /> is already greater than or equal to
    ///     <paramref name="targetLength" />,
    ///     this method will not append anything and return the original <paramref name="stringBuilder" /> instance. Otherwise,
    ///     it appends <paramref name="charToAppend" />
    ///     to the <paramref name="stringBuilder" /> until it reaches the desired <paramref name="targetLength" />.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use <see cref="AppendCharToLength" /> to append a specific character to a
    ///     StringBuilder until it reaches a specified length:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("Hello");
    /// sb.AppendCharToLength(10, 'o');
    /// Console.WriteLine(sb.ToString()); // Output: "Helloooooo"
    /// sb.AppendCharToLength(5, '!');
    /// Console.WriteLine(sb.ToString()); // Output: "Helloooooo"
    /// sb.AppendCharToLength(7, ' ');
    /// Console.WriteLine(sb.ToString()); // Output: "Helloooooo   "
    /// ]]></code>
    /// </example>
    public static StringBuilder AppendCharToLength(this StringBuilder stringBuilder, int targetLength,
        char charToAppend)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        var currentLength = stringBuilder.Length;
        if (currentLength >= targetLength)
        {
            return stringBuilder;
        }

        stringBuilder.Append(charToAppend, targetLength - currentLength);
        return stringBuilder;
    }

    /// <summary>
    ///     Reduces the length of the StringBuilder to the specified maximum length.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder instance.</param>
    /// <param name="maxLength">The maximum length of the StringBuilder.</param>
    /// <returns>The modified StringBuilder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stringBuilder" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="maxLength" /> is less than zero.</exception>
    /// <remarks>
    ///     This method reduces the length of the <paramref name="stringBuilder" /> to the specified
    ///     <paramref name="maxLength" />.
    ///     If <paramref name="maxLength" /> is less than or equal to zero, this method clears the
    ///     <paramref name="stringBuilder" />. If <paramref name="maxLength" /> is greater than the current length of the
    ///     <paramref name="stringBuilder" />,
    ///     this method does not modify the <paramref name="stringBuilder" />.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use <see cref="ReduceToLength" /> to reduce the length of a StringBuilder
    ///     to a specified maximum length:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("Hello World!");
    /// sb.ReduceToLength(5);
    /// Console.WriteLine(sb.ToString()); // Output: "Hello"
    /// sb.ReduceToLength(0);
    /// Console.WriteLine(sb.ToString()); // Output: ""
    /// sb.ReduceToLength(10);
    /// Console.WriteLine(sb.ToString()); // Output: ""
    /// ]]></code>
    /// </example>
    public static StringBuilder ReduceToLength(this StringBuilder stringBuilder, int maxLength)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        if (maxLength < 0)
        {
            throw new ArgumentException("The maxlength cannot be less then 0.", nameof(maxLength));
        }

        if (maxLength == 0)
        {
            stringBuilder.Clear();
        }
        else
        {
            stringBuilder.Length = maxLength;
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Reverses the characters in the StringBuilder.
    /// </summary>
    /// <param name="stringBuilder">The StringBuilder instance.</param>
    /// <returns>The modified StringBuilder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     This method reverses the order of characters in the <paramref name="stringBuilder" />.
    ///     If the <paramref name="stringBuilder" /> is empty, this method returns the original
    ///     <paramref name="stringBuilder" /> instance.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use <see cref="Reverse" /> to reverse the characters in a StringBuilder:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("Hello World!");
    /// sb.Reverse();
    /// Console.WriteLine(sb.ToString()); // Output: "!dlroW olleH"
    /// sb.Reverse();
    /// Console.WriteLine(sb.ToString()); // Output: "Hello World!"
    /// ]]></code>
    /// </example>
    public static StringBuilder Reverse(this StringBuilder stringBuilder)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        var length = stringBuilder.Length;
        if (length == 0)
        {
            return stringBuilder;
        }

        for (int i = 0, j = length - 1; i < j; i++, j--)
        {
            (stringBuilder[i], stringBuilder[j]) = (stringBuilder[j], stringBuilder[i]);
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that the <see cref="StringBuilder" /> instance starts with the specified prefix string using the specified
    ///     comparison rules.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be modified.</param>
    /// <param name="prefix">The prefix string to ensure the <paramref name="stringBuilder" /> instance starts with.</param>
    /// <param name="stringComparison">
    ///     One of the enumeration values that determines the rules for comparing the prefix string
    ///     and the <paramref name="stringBuilder" /> instance. The default value is
    ///     <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>The <see cref="StringBuilder" /> instance with the ensured prefix string.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="stringBuilder" /> or
    ///     <paramref name="prefix" /> is null or empty.
    /// </exception>
    /// <remarks>
    ///     This method checks if the <paramref name="stringBuilder" /> instance already starts with the specified prefix
    ///     string. If it doesn't, the prefix string will be inserted at the beginning of the <paramref name="stringBuilder" />
    ///     instance. The comparison between the prefix string and the <paramref name="stringBuilder" /> instance is performed
    ///     using the specified comparison rules, which are set to <see cref="StringComparison.InvariantCulture" /> by default.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="EnsureStartsWith" /> method to ensure that a
    ///     <see cref="StringBuilder" /> instance starts with a specified prefix string:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("fox jumps over the lazy dog");
    /// sb.EnsureStartsWith("the ");
    /// Console.WriteLine(sb.ToString()); // Output: "the fox jumps over the lazy dog"
    /// ]]></code>
    /// </example>
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

        var prefixLength = prefix.Length;
        if (stringBuilder.Length < prefixLength ||
            !stringBuilder.ToString(0, prefixLength).Equals(prefix, stringComparison))
        {
            stringBuilder.Insert(0, prefix);
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that the <see cref="StringBuilder" /> instance ends with the specified suffix string using the specified
    ///     comparison rules.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be modified.</param>
    /// <param name="suffix">The suffix string to ensure the <paramref name="stringBuilder" /> instance ends with.</param>
    /// <param name="stringComparison">
    ///     One of the enumeration values that determines the rules for comparing the suffix string
    ///     and the <paramref name="stringBuilder" /> instance. The default value is
    ///     <see cref="StringComparison.InvariantCulture" />.
    /// </param>
    /// <returns>The <see cref="StringBuilder" /> instance with the ensured suffix string.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="stringBuilder" /> or
    ///     <paramref name="suffix" /> is null or empty.
    /// </exception>
    /// <remarks>
    ///     This method checks if the <paramref name="stringBuilder" /> instance already ends with the specified suffix string.
    ///     If it doesn't, the suffix string will be appended to the end of the <paramref name="stringBuilder" /> instance. The
    ///     comparison between the suffix string and the <paramref name="stringBuilder" /> instance is performed using the
    ///     specified comparison rules, which are set to <see cref="StringComparison.InvariantCulture" /> by default.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="EnsureEndsWith" /> method to ensure that a
    ///     <see cref="StringBuilder" /> instance ends with a specified suffix string:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("the quick brown fox jumps over the lazy ");
    /// sb.EnsureEndsWith("dog");
    /// Console.WriteLine(sb.ToString()); // Output: "the quick brown fox jumps over the lazy dog"
    /// ]]></code>
    /// </example>
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

        var suffixLength = suffix.Length;
        var startIndex = stringBuilder.Length - suffixLength;

        if (startIndex >= 0 &&
            stringBuilder.ToString(startIndex, suffixLength).Equals(suffix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Append(suffix);
        return stringBuilder;
    }

    /// <summary>
    ///     Appends multiple strings to the <see cref="StringBuilder" /> instance, optionally inserting a new line between each
    ///     string.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append the strings to.</param>
    /// <param name="withNewLine">Whether to insert a new line between each string. The default value is <c>false</c>.</param>
    /// <param name="items">The strings to append to the <paramref name="stringBuilder" /> instance.</param>
    /// <returns>The <see cref="StringBuilder" /> instance with the appended strings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     This method appends the specified strings to the <paramref name="stringBuilder" /> instance, optionally inserting a
    ///     new line between each string. If <paramref name="items" /> is null or empty, the <paramref name="stringBuilder" />
    ///     instance remains unchanged. Empty or null strings in <paramref name="items" /> are skipped.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="AppendMultiple" /> method to append multiple strings
    ///     to a <see cref="StringBuilder" /> instance:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder();
    /// sb.AppendMultiple(true, "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog");
    /// Console.WriteLine(sb.ToString());
    /// // Output:
    /// // The
    /// // quick
    /// // brown
    /// // fox
    /// // jumps
    /// // over
    /// // the
    /// // lazy
    /// // dog
    /// ]]></code>
    /// </example>
    public static StringBuilder AppendMultiple(this StringBuilder stringBuilder, bool withNewLine = false,
        params string[]? items)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        if (items == null || items.Length == 0)
        {
            return stringBuilder;
        }

        if (withNewLine)
        {
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                stringBuilder.Append(item);
                stringBuilder.Append(Environment.NewLine);
            }
        }
        else
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    stringBuilder.Append(item);
                }
            }
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Removes all occurrences of the specified character from the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to remove the specified character from.</param>
    /// <param name="characterToBeRemoved">The character to be removed from the <paramref name="stringBuilder" /> instance.</param>
    /// <returns>The <see cref="StringBuilder" /> instance with the specified character removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     This method removes all occurrences of the specified character from the <paramref name="stringBuilder" /> instance.
    ///     The length of the <paramref name="stringBuilder" /> instance will be updated accordingly.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="RemoveAllOccurrences" /> method to remove all
    ///     occurrences of a specified character from a <see cref="StringBuilder" /> instance:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("The quick brown fox jumps over the lazy dog");
    /// sb.RemoveAllOccurrences('o');
    /// Console.WriteLine(sb.ToString()); // Output: "The quick brwn fx jumps ver the lazy dg"
    /// ]]></code>
    /// </example>
    public static StringBuilder RemoveAllOccurrences(this StringBuilder stringBuilder, char characterToBeRemoved)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        var length = stringBuilder.Length;
        var newIndex = 0;

        for (var i = 0; i < length; i++)
        {
            if (stringBuilder[i] != characterToBeRemoved)
            {
                stringBuilder[newIndex++] = stringBuilder[i];
            }
        }

        stringBuilder.Length = newIndex;
        return stringBuilder;
    }

    /// <summary>
    ///     Removes all leading and trailing white-space characters from the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">
    ///     The <see cref="StringBuilder" /> instance to remove the leading and trailing white-space
    ///     characters from.
    /// </param>
    /// <returns>The <see cref="StringBuilder" /> instance with the leading and trailing white-space characters removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
    /// <remarks>
    ///     This method removes all leading and trailing white-space characters from the <paramref name="stringBuilder" />
    ///     instance. The length of the <paramref name="stringBuilder" /> instance will be updated accordingly.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="Trim" /> method to remove all leading and trailing
    ///     white-space characters from a <see cref="StringBuilder" /> instance:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder("    The quick brown fox jumps over the lazy dog    ");
    /// sb.Trim();
    /// Console.WriteLine(sb.ToString()); // Output: "The quick brown fox jumps over the lazy dog"
    /// ]]></code>
    /// </example>
    public static StringBuilder Trim(this StringBuilder stringBuilder)
    {
        if (stringBuilder == null)
        {
            throw new ArgumentNullException(nameof(stringBuilder), $"The {nameof(stringBuilder)} cannot be null.");
        }

        var startIndex = 0;
        var endIndex = stringBuilder.Length - 1;

        while (startIndex <= endIndex && char.IsWhiteSpace(stringBuilder[startIndex]))
        {
            startIndex++;
        }

        while (endIndex >= startIndex && char.IsWhiteSpace(stringBuilder[endIndex]))
        {
            endIndex--;
        }

        if (startIndex > 0 || endIndex < stringBuilder.Length - 1)
        {
            stringBuilder.Remove(endIndex + 1, stringBuilder.Length - endIndex - 1);
            stringBuilder.Remove(0, startIndex);
        }

        return stringBuilder;
    }

    /// <summary>
    ///     Appends a formatted string, followed by a line terminator, to the end of the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append the formatted string to.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="items">An object array that contains zero or more objects to format.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="stringBuilder" /> or <paramref name="format" /> is
    ///     null or white-space.
    /// </exception>
    /// <remarks>
    ///     This method appends a formatted string, followed by a line terminator, to the end of the
    ///     <paramref name="stringBuilder" /> instance using the specified <paramref name="format" /> and
    ///     <paramref name="items" />. If <paramref name="items" /> is null or empty, the <paramref name="format" /> string is
    ///     appended as is. The line terminator used is the newline character ("\n") for all platforms.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="AppendFormatLine" /> method to append a formatted
    ///     string, followed by a line terminator, to a <see cref="StringBuilder" /> instance:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder();
    /// sb.AppendFormatLine("The value of pi is approximately {0:F2}", Math.PI);
    /// Console.WriteLine(sb.ToString()); // Output: "The value of pi is approximately 3.14\n"
    /// ]]></code>
    /// </example>
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
    ///     Appends the specified number of newlines to the end of the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append the newlines to.</param>
    /// <param name="numberOfLines">The number of newlines to append to the end of the <see cref="StringBuilder" /> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="numberOfLines" /> is less than 1.</exception>
    /// <remarks>
    ///     This method appends the specified number of newlines to the end of the <paramref name="stringBuilder" /> instance.
    ///     The number of newlines is specified by the <paramref name="numberOfLines" /> parameter. The newline character used
    ///     is the newline character ("\n") for all platforms.
    /// </remarks>
    /// <example>
    ///     The following example demonstrates how to use the <see cref="AppendMultipleLines" /> method to append the specified
    ///     number of newlines to a <see cref="StringBuilder" /> instance:
    ///     <code><![CDATA[
    /// StringBuilder sb = new StringBuilder();
    /// sb.Append("Hello");
    /// sb.AppendMultipleLines(2);
    /// sb.Append("World");
    /// Console.WriteLine(sb.ToString()); // Output: "Hello\n\nWorld"
    /// ]]></code>
    /// </example>
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