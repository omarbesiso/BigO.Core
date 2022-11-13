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
    ///     Determines whether the specified <see cref="StringBuilder" /> object is empty.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be checked.</param>
    /// <param name="countWhiteSpace">If <c>true</c> white space will be considered a valid character for length calculation.</param>
    /// <returns><c>true</c> if the specified string builder is empty; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Appends a character to the <see cref="StringBuilder" /> object until a specified target length is reached.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append the character to.</param>
    /// <param name="targetLength">The target length of the <see cref="StringBuilder" />.</param>
    /// <param name="charToAppend">The character to append to the <see cref="StringBuilder" />.</param>
    /// <returns>The <see cref="StringBuilder" /> with the specified target length or higher.</returns>
    /// <remarks>
    ///     If the targetLength of <see cref="StringBuilder" /> is already equal to or greater to the target length
    ///     the <see cref="StringBuilder" /> is returned without any further processing.
    /// </remarks>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Reduces the length of the <see cref="StringBuilder" /> object to the specified length if it exceeds it.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be reduced.</param>
    /// <param name="maxLength">
    ///     The target maximum length of the <see cref="StringBuilder" />. The length has to be equal to or
    ///     greater than 0.
    /// </param>
    /// <returns>The <see cref="StringBuilder" /> reduced to maximum length specified if it exceeds it.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentException"><paramref name="maxLength" /> is less than 0 (negative number).</exception>
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
    ///     Reversed the characters in a <see cref="StringBuilder" /> object.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance in which characters will be reversed.</param>
    /// <returns>The <see cref="StringBuilder" /> instance with reversed characters.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Ensures that a <see cref="StringBuilder" /> starts with a specified prefix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to check.</param>
    /// <param name="prefix">The prefix to checked or inserted.</param>
    /// <param name="stringComparison">Specifies the culture, case, and sort rules to be used.</param>
    /// <returns>A <see cref="StringBuilder" /> that starts with the specified prefix.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c></exception>
    /// <exception cref="System.ArgumentException"><paramref name="prefix" /> is <c>null</c> or empty.</exception>
    public static StringBuilder EnsureStartsWith(this StringBuilder stringBuilder, string prefix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        ArgumentException.ThrowIfNullOrEmpty(prefix);

        if (stringBuilder.ToString().StartsWith(prefix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Insert(0, prefix);
        return stringBuilder;
    }

    /// <summary>
    ///     Ensures that a <see cref="StringBuilder" /> ends with a specified suffix.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to check.</param>
    /// <param name="suffix">The suffix to checked or inserted.</param>
    /// <param name="stringComparison">Specifies the culture, case, and sort rules to be used.</param>
    /// <returns>A <see cref="StringBuilder" /> that ends with the specified suffix.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c></exception>
    /// <exception cref="System.ArgumentException"><paramref name="suffix" /> is <c>null</c> or empty.</exception>
    public static StringBuilder EnsureEndsWith(this StringBuilder stringBuilder, string suffix,
        StringComparison stringComparison = StringComparison.InvariantCulture)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);
        ArgumentException.ThrowIfNullOrEmpty(suffix);

        if (stringBuilder.ToString().EndsWith(suffix, stringComparison))
        {
            return stringBuilder;
        }

        stringBuilder.Append(suffix);
        return stringBuilder;
    }

    /// <summary>
    ///     Appends multiple strings to the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append to.</param>
    /// <param name="withNewLine">
    ///     if <c>true</c> items will be appended with a new line inserted after each format.
    /// </param>
    /// <param name="items">The items to be appended.</param>
    /// <returns>The <see cref="StringBuilder" /> with the appended items.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Removes all the occurrences of a specified character from the <see cref="StringBuilder" /> .
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be checked.</param>
    /// <param name="characterToBeRemoved">The character to be removed.</param>
    /// <returns>The <see cref="StringBuilder" />  with the specified character removed.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Trims the <see cref="StringBuilder" />.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be trimmed.</param>
    /// <returns>The trimmed <see cref="StringBuilder" />.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
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
    ///     Appends the string returned by processing a composite format string, which contains zero or more format items, to
    ///     this instance. Each format format is replaced by the string representation of a corresponding argument in a
    ///     parameter
    ///     array. Also appends a new line.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to append to.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="items">An array of objects to format.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// ///
    /// <exception cref="System.ArgumentNullException">
    ///     <paramref name="format" /> is <c>null</c> or consist only of whitespace
    ///     characters.
    /// </exception>
    /// <exception cref="System.ArgumentNullException"><paramref name="items" /> is <c>null</c>.</exception>
    /// <exception cref="T:System.FormatException">
    ///     <paramref name="format" /> is invalid. -or- The index of a format item is
    ///     less than 0 (zero), or greater than or equal to the length of the <paramref name="items" /> array.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///     The length of the expanded string would exceed
    ///     <see cref="P:System.Text.StringBuilder.MaxCapacity" />.
    /// </exception>
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
    ///     Appends multiple lines to the <see cref="StringBuilder" /> instance.
    /// </summary>
    /// <param name="stringBuilder">The <see cref="StringBuilder" /> instance to be appended to.</param>
    /// <param name="numberOfLines">The number of lines to be appended.</param>
    /// <exception cref="System.ArgumentNullException"><paramref name="stringBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="System.ArgumentException"><paramref name="numberOfLines" /> is less than 1.</exception>
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