using System.Text;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        var result = stringBuilder.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForEmptyStringBuilder_WhenCountingWhiteSpace()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        var result = stringBuilder.IsEmpty(true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForNonEmptyStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");

        // Act
        var result = stringBuilder.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_ForNonEmptyStringBuilder_WhenCountingWhiteSpace()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");

        // Act
        var result = stringBuilder.IsEmpty(true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_ForStringBuilderWithOnlyWhiteSpace()
    {
        // Arrange
        var stringBuilder = new StringBuilder("   ");

        // Act
        var result = stringBuilder.IsEmpty(true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_ThrowsArgumentNullException_ForNullStringBuilder()
    {
        // Arrange
        StringBuilder stringBuilder = null!;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.IsEmpty());
        Assert.Throws<ArgumentNullException>(() => stringBuilder.IsEmpty(true));
    }


    [Fact]
    public void AppendCharToLength_ReturnsStringBuilder_ForValidInput()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        var targetLength = 10;
        var charToAppend = 'x';

        // Act
        var result = stringBuilder.AppendCharToLength(targetLength, charToAppend);

        // Assert
        Assert.Equal(stringBuilder, result);
    }

    [Fact]
    public void AppendCharToLength_AppendsChars_ToReachTargetLength()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        var targetLength = 10;
        var charToAppend = 'x';

        // Act
        stringBuilder.AppendCharToLength(targetLength, charToAppend);

        // Assert
        Assert.Equal(targetLength, stringBuilder.Length);
        Assert.Equal(new string(charToAppend, targetLength - 5), stringBuilder.ToString().Substring(5));
    }

    [Fact]
    public void AppendCharToLength_DoesNotAppendChars_ForStringBuilderLongerThanTargetLength()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var targetLength = 5;
        var charToAppend = 'x';

        // Act
        stringBuilder.AppendCharToLength(targetLength, charToAppend);

        // Assert
        Assert.Equal(11, stringBuilder.Length);
    }

    [Fact]
    public void AppendCharToLength_ReturnsStringBuilder_ForStringBuilderEqualToTargetLength()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var targetLength = 11;
        var charToAppend = 'x';

        // Act
        var result = stringBuilder.AppendCharToLength(targetLength, charToAppend);

        // Assert
        Assert.Equal(stringBuilder, result);
    }

    [Fact]
    public void AppendCharToLength_ThrowsArgumentNullException_ForNullStringBuilder()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        var targetLength = 10;
        var charToAppend = 'x';

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.AppendCharToLength(targetLength, charToAppend));
    }

    [Fact]
    public void ReduceToLength_ReturnsStringBuilder_ForValidInput()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var maxLength = 5;

        // Act
        var result = stringBuilder.ReduceToLength(maxLength);

        // Assert
        Assert.Equal(stringBuilder, result);
    }

    [Fact]
    public void ReduceToLength_ReducesStringBuilder_ToMaxLength()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var maxLength = 5;

        // Act
        stringBuilder.ReduceToLength(maxLength);

        // Assert
        Assert.Equal(maxLength, stringBuilder.Length);
        Assert.Equal("hello", stringBuilder.ToString());
    }

    [Fact]
    public void ReduceToLength_ClearsStringBuilder_ForMaxLengthEqualToZero()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var maxLength = 0;

        // Act
        stringBuilder.ReduceToLength(maxLength);

        // Assert
        Assert.True(stringBuilder.IsEmpty());
    }

    [Fact]
    public void ReduceToLength_ThrowsArgumentException_ForMaxLengthLessThanZero()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var maxLength = -1;

        // Act and assert
        Assert.Throws<ArgumentException>(() => stringBuilder.ReduceToLength(maxLength));
    }

    [Fact]
    public void ReduceToLength_ThrowsArgumentNullException_ForNullStringBuilder()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        var maxLength = 5;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.ReduceToLength(maxLength));
    }

    [Fact]
    public void Reverse_ReturnsStringBuilder_ForValidInput()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");

        // Act
        var result = stringBuilder.Reverse();

        // Assert
        Assert.Equal(stringBuilder, result);
    }

    [Fact]
    public void Reverse_ReversesStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");

        // Act
        stringBuilder.Reverse();

        // Assert
        Assert.Equal("dlrow olleh", stringBuilder.ToString());
    }

    [Fact]
    public void Reverse_DoesNotModifyStringBuilder_ForEmptyStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder();

        // Act
        stringBuilder.Reverse();

        // Assert
        Assert.True(stringBuilder.IsEmpty());
    }

    [Fact]
    public void Reverse_ThrowsArgumentNullException_ForNullStringBuilder()
    {
        // Arrange
        StringBuilder stringBuilder = null!;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.Reverse());
    }

    [Fact]
    public void EnsureStartsWith_ReturnsStringBuilder_ForValidInput()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var prefix = "Hello";

        // Act
        var result = stringBuilder.EnsureStartsWith(prefix);

        // Assert
        Assert.Equal(stringBuilder, result);
    }

    [Fact]
    public void EnsureStartsWith_AddsPrefix_ToStringBuilder_IfStringBuilderDoesNotStartWithPrefix()
    {
        // Arrange
        var stringBuilder = new StringBuilder("world");
        var prefix = "Hello ";

        // Act
        stringBuilder.EnsureStartsWith(prefix);

        // Assert
        Assert.Equal("Hello world", stringBuilder.ToString());
    }

    [Fact]
    public void EnsureStartsWith_DoesNotAddPrefix_ToStringBuilder_IfStringBuilderAlreadyStartsWithPrefix()
    {
        // Arrange
        var stringBuilder = new StringBuilder("Hello world");
        var prefix = "Hello";

        // Act
        stringBuilder.EnsureStartsWith(prefix);

        // Assert
        Assert.Equal("Hello world", stringBuilder.ToString());
    }

    [Fact]
    public void EnsureStartsWith_ThrowsArgumentException_ForNullOrEmptyPrefix()
    {
        // Arrange
        var stringBuilder = new StringBuilder("Hello world");
        string prefix = null!;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureStartsWith(prefix));
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureStartsWith(""));
    }

    [Fact]
    public void EnsureStartsWith_ThrowsArgumentNullException_ForNullStringBuilder()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        const string prefix = "Hello";
        // Act and assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureStartsWith(prefix));
    }

    [Fact]
    public void EnsureEndsWith_GivenNullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        var suffix = "suffix";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureEndsWith(suffix));
    }

    [Fact]
    public void EnsureEndsWith_GivenNullSuffix_ThrowsArgumentNullException()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        string suffix = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureEndsWith(suffix));
    }

    [Fact]
    public void EnsureEndsWith_GivenEmptySuffix_ThrowsArgumentException()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var suffix = "";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.EnsureEndsWith(suffix));
    }

    [Fact]
    public void EnsureEndsWith_GivenStringBuilderEndingWithSuffix_ReturnsSameStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        const string suffix = "lo";

        // Act
        var result = stringBuilder.EnsureEndsWith(suffix);

        // Assert
        Assert.Same(stringBuilder, result);
    }

    [Fact]
    public void EnsureEndsWith_GivenStringBuilderNotEndingWithSuffix_AppendsSuffixToStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        const string suffix = "world";

        // Act
        stringBuilder.EnsureEndsWith(suffix);

        // Assert
        Assert.Equal("helloworld", stringBuilder.ToString());
    }

    [Fact]
    public void EnsureEndsWith_GivenStringBuilderNotEndingWithSuffixIgnoreCase_AppendsSuffixToStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        const string suffix = "WORLD";

        // Act
        stringBuilder.EnsureEndsWith(suffix, StringComparison.InvariantCultureIgnoreCase);

        // Assert
        Assert.Equal("helloWORLD", stringBuilder.ToString());
    }

    [Fact]
    public void AppendMultiple_GivenNullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        string[] items = { "item1", "item2" };

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.AppendMultiple(items: items));
    }

    [Fact]
    public void AppendMultiple_GivenEmptyArray_ReturnsSameStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        var items = Array.Empty<string>();

        // Act
        var result = stringBuilder.AppendMultiple(items: items);

        // Assert
        Assert.Same(stringBuilder, result);
    }

    [Fact]
    public void AppendMultiple_GivenNullArray_ReturnsSameStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        string[]? items = null;

        // Act
        var result = stringBuilder.AppendMultiple(items: items);

        // Assert
        Assert.Same(stringBuilder, result);
    }

    [Fact]
    public void AppendMultiple_GivenArrayWithNullOrEmptyValues_DoesAppendValues()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        string[] items = { "item1", null!, "item2", "", "item3" };

        // Act
        stringBuilder.AppendMultiple(items: items);

        // Assert
        Assert.Equal("helloitem1item2item3", stringBuilder.ToString());
    }

    [Fact]
    public void AppendMultiple_GivenArrayWithValues_AppendsValuesToStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        string[] items = { "item1", "item2", "item3" };

        // Act
        stringBuilder.AppendMultiple(items: items);

        // Assert
        Assert.Equal("helloitem1item2item3", stringBuilder.ToString());
    }

    [Fact]
    public void AppendMultiple_GivenArrayWithValuesAndWithNewLine_AppendsValuesToStringBuilderWithNewLine()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        string[] items = { "item1", "item2", "item3" };

        // Act
        stringBuilder.AppendMultiple(true, items);

        // Assert
        Assert.Equal("helloitem1\r\nitem2\r\nitem3\r\n", stringBuilder.ToString());
    }

    [Fact]
    public void RemoveAllOccurrences_GivenNullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder stringBuilder = null!;
        var characterToBeRemoved = 'x';

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => stringBuilder.RemoveAllOccurrences(characterToBeRemoved));
    }

    [Fact]
    public void RemoveAllOccurrences_GivenStringBuilderWithoutCharacter_ReturnsSameStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        const char characterToBeRemoved = 'x';

        // Act
        var result = stringBuilder.RemoveAllOccurrences(characterToBeRemoved);

        // Assert
        Assert.Same(stringBuilder, result);
    }

    [Fact]
    public void RemoveAllOccurrences_GivenStringBuilderWithOneCharacter_RemovesCharacterFromStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello");
        var characterToBeRemoved = 'l';

        // Act
        stringBuilder.RemoveAllOccurrences(characterToBeRemoved);

        // Assert
        Assert.Equal("heo", stringBuilder.ToString());
    }

    [Fact]
    public void RemoveAllOccurrences_GivenStringBuilderWithMultipleCharacters_RemovesAllCharactersFromStringBuilder()
    {
        // Arrange
        var stringBuilder = new StringBuilder("hello world");
        var characterToBeRemoved = 'l';

        // Act
        stringBuilder.RemoveAllOccurrences(characterToBeRemoved);

        // Assert
        Assert.Equal("heo word", stringBuilder.ToString());
    }

    [Fact]
    public void Trim_NullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder sb = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.Trim());
    }

    [Fact]
    public void Trim_EmptyStringBuilder_ReturnsSameStringBuilder()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        var result = sb.Trim();

        // Assert
        Assert.Same(sb, result);
    }

    [Fact]
    public void Trim_StringBuilderWithNoLeadingOrTrailingWhitespace_ReturnsSameStringBuilder()
    {
        // Arrange
        var sb = new StringBuilder("test");

        // Act
        var result = sb.Trim();

        // Assert
        Assert.Same(sb, result);
    }

    [Fact]
    public void Trim_StringBuilderWithLeadingWhitespace_RemovesLeadingWhitespace()
    {
        // Arrange
        var sb = new StringBuilder("  test");

        // Act
        var result = sb.Trim();

        // Assert
        Assert.Equal("test", result.ToString());
    }

    [Fact]
    public void Trim_StringBuilderWithTrailingWhitespace_RemovesTrailingWhitespace()
    {
        // Arrange
        var sb = new StringBuilder("test  ");

        // Act
        var result = sb.Trim();

        // Assert
        Assert.Equal("test", result.ToString());
    }

    [Fact]
    public void Trim_StringBuilderWithLeadingAndTrailingWhitespace_RemovesLeadingAndTrailingWhitespace()
    {
        // Arrange
        var sb = new StringBuilder("  test  ");

        // Act
        var result = sb.Trim();

        // Assert
        Assert.Equal("test", result.ToString());
    }

    [Fact]
    public void AppendFormatLine_NullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder sb = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.AppendFormatLine("format", "item1"));
    }

    [Fact]
    public void AppendFormatLine_NullFormat_ThrowsArgumentNullException()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.AppendFormatLine(null!, "item1"));
    }

    [Fact]
    public void AppendFormatLine_EmptyFormat_ThrowsArgumentNullException()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.AppendFormatLine("", "item1"));
    }

    [Fact]
    public void AppendFormatLine_WhitespaceFormat_ThrowsArgumentNullException()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.AppendFormatLine(" ", "item1"));
    }

    [Fact]
    public void AppendFormatLine_FormatWithOneItem_AppendsFormattedStringWithLineBreak()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        sb.AppendFormatLine("format {0}", "item1");

        // Assert
        Assert.Equal("format item1\r\n", sb.ToString());
    }

    [Fact]
    public void AppendFormatLine_FormatWithMultipleItems_AppendsFormattedStringWithLineBreak()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        sb.AppendFormatLine("format {0} {1}", "item1", "item2");

        // Assert
        Assert.Equal("format item1 item2\r\n", sb.ToString());
    }

    [Fact]
    public void AppendLine_NullStringBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        StringBuilder sb = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => sb.AppendLine(1));
    }

    [Fact]
    public void AppendLine_NumberOfLinesLessThanOne_ThrowsArgumentException()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act and Assert
        Assert.Throws<ArgumentException>(() => sb.AppendLine(0));
        Assert.Throws<ArgumentException>(() => sb.AppendLine(-1));
    }

    [Fact]
    public void AppendLine_NumberOfLinesOne_AppendsOneLineBreak()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        sb.AppendLine(1);

        // Assert
        Assert.Equal("\r\n", sb.ToString());
    }

    [Fact]
    public void AppendLine_NumberOfLinesTwo_AppendsTwoLineBreaks()
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        sb.AppendLine(2);

        // Assert
        Assert.Equal("\r\n\r\n", sb.ToString());
    }
}
