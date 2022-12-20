using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void IsGuid_ReturnsFalseForEmptyString()
    {
        var result = "".IsGuid();
        Assert.False(result);
    }

    [Fact]
    public void IsGuid_ReturnsFalseForWhiteSpaceString()
    {
        var result = "   ".IsGuid();
        Assert.False(result);
    }

    [Fact]
    public void IsGuid_ReturnsFalseForInvalidGuid()
    {
        var result = "invalid".IsGuid();
        Assert.False(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForDashedGuid()
    {
        var result = "12345678-1234-1234-1234-123456789abc".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForBracedGuid()
    {
        var result = "{12345678-1234-1234-1234-123456789abc}".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForParenthesizedGuid()
    {
        var result = "(12345678-1234-1234-1234-123456789abc)".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForUppercaseGuid()
    {
        var result = "12345678-1234-1234-1234-123456789ABC".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForLowercaseGuid()
    {
        var result = "12345678-1234-1234-1234-123456789abc".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsGuid_ReturnsTrueForDigitsOnlyGuid()
    {
        var result = "12345678123412341234123456789abc".IsGuid();
        Assert.True(result);
    }

    [Fact]
    public void IsValidEmail_ReturnsTrueForValidEmail()
    {
        const string email = "test@example.com";
        var result = email.IsValidEmail();

        Assert.True(result);
    }

    [Fact]
    public void IsValidEmail_ReturnsFalseForNullEmail()
    {
        string email = null!;
        var result = email.IsValidEmail();

        Assert.False(result);
    }


    [Fact]
    public void IsValidEmail_ReturnsFalseForEmptyEmail()
    {
        const string email = "";
        var result = email.IsValidEmail();

        Assert.False(result);
    }

    [Fact]
    public void IsValidEmail_ReturnsFalseForWhitespaceEmail()
    {
        const string email = "   ";
        var result = email.IsValidEmail();

        Assert.False(result);
    }

    [Fact]
    public void IsValidEmail_ReturnsFalseForInvalidEmail()
    {
        const string email = "invalid";
        var result = email.IsValidEmail();

        Assert.False(result);
    }


    [Fact]
    public void IsValidWebsiteUrl_ReturnsTrueForValidHttpUrl()
    {
        const string url = "http://example.com";
        var result = url.IsValidWebsiteUrl();

        Assert.True(result);
    }

    [Fact]
    public void IsValidWebsiteUrl_ReturnsTrueForValidHttpsUrl()
    {
        const string url = "https://example.com";
        var result = url.IsValidWebsiteUrl();

        Assert.True(result);
    }

    [Fact]
    public void IsValidWebsiteUrl_ReturnsFalseForInvalidUrl()
    {
        var url = "invalid";
        var result = url.IsValidWebsiteUrl();

        Assert.False(result);
    }

    [Fact]
    public void IsValidWebsiteUrl_ReturnsFalseForNullUrl()
    {
        string url = null!;
        var result = url.IsValidWebsiteUrl();

        Assert.False(result);
    }

    [Fact]
    public void IsValidWebsiteUrl_ReturnsFalseForEmptyUrl()
    {
        var url = "";
        var result = url.IsValidWebsiteUrl();

        Assert.False(result);
    }

    [Fact]
    public void IsValidWebsiteUrl_ReturnsFalseForWhitespaceUrl()
    {
        const string url = "   ";
        var result = url.IsValidWebsiteUrl();

        Assert.False(result);
    }

    [Fact]
    public void RemoveCharacters_ShouldReturnOriginalString_WhenInputIsNull()
    {
        // Arrange
        string value = null!;
        char[]? charactersToExclude = null;

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RemoveCharacters_ShouldReturnOriginalString_WhenCharactersToExcludeIsNull()
    {
        // Arrange
        const string value = "abc";
        char[]? charactersToExclude = null;

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Equal("abc", result);
    }

    [Fact]
    public void RemoveCharacters_ShouldReturnEmptyString_WhenInputIsEmpty()
    {
        // Arrange
        var value = "";
        char[] charactersToExclude = { 'a', 'b', 'c' };

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void RemoveCharacters_ShouldRemoveAllExcludedCharacters_WhenAllCharactersAreExcluded()
    {
        // Arrange
        var value = "abc";
        char[] charactersToExclude = { 'a', 'b', 'c' };

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void RemoveCharacters_ShouldRemoveExcludedCharacters_WhenSomeCharactersAreExcluded()
    {
        // Arrange
        var value = "abc";
        char[] charactersToExclude = { 'b', 'c' };

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Equal("a", result);
    }

    [Fact]
    public void RemoveCharacters_ShouldReturnOriginalString_WhenNoCharactersAreExcluded()
    {
        // Arrange
        var value = "abc";
        char[] charactersToExclude = { 'd', 'e', 'f' };

        // Act
        var result = value.RemoveCharacters(charactersToExclude);

        // Assert
        Assert.Equal("abc", result);
    }

    [Fact]
    public void Shuffle_ShouldReturnOriginalString_WhenInputIsNull()
    {
        // Arrange
        string value = null!;

        // Act
        var result = value.Shuffle();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Shuffle_ShouldReturnOriginalString_WhenInputIsEmpty()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.Shuffle();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void Shuffle_ShouldReturnOriginalString_WhenInputIsWhitespace()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.Shuffle();

        // Assert
        Assert.Equal("   ", result);
    }

    [Fact]
    public void Shuffle_ShouldReturnShuffledString_WhenInputIsNotEmpty()
    {
        // Arrange
        const string value = "abcdefghijklmno";

        // Act
        var result = value.Shuffle();

        // Assert
        Assert.NotEqual(value, result);
        Assert.Equal(value.Length, result.Length);
        Assert.True(result.ToCharArray().All(c => value.Contains(c)));
    }

    [Theory]
    [InlineData("value", 1, -1, "replaceWith")]
    [InlineData("value", 1, 6, "replaceWith")]
    public void Stuff_ShouldThrowArgumentOutOfRangeException_WhenLengthIsOutOfRange(string value, int start, int length,
        string replaceWith)
    {
        // Act and Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.Stuff(start, length, replaceWith));
    }

    [Fact]
    public void Stuff_ShouldReturnStuffedString_WhenInputIsValid()
    {
        // Arrange
        const string value = "abcdef";
        const int start = 2;
        const int length = 2;
        const string replaceWith = "xyz";

        // Act
        var result = value.Stuff(start, length, replaceWith);

        // Assert
        Assert.Equal("axyzdef", result);
    }
}