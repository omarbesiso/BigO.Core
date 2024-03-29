﻿using BigO.Core.Validation;

namespace BigO.Core.Tests;

public class GuardTest
{
    [Theory]
    [InlineData(null, "value", "The value of 'value' cannot be null.")]
    [InlineData(null, "", "The value of '' cannot be null.")]
    [InlineData(null, "", "Custom exception message")]
    public void NotNull_ThrowsArgumentNullException_ForNullValue(object value, string argumentName,
        string exceptionMessage)
    {
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => Guard.NotNull(value, argumentName, exceptionMessage));
    }

    [Theory]
    [InlineData("not null", "value")]
    [InlineData(1, "")]
    public void NotNull_ReturnsValue_ForNonNullValue(object value, string argumentName)
    {
        // Act
        var result = Guard.NotNull(value, argumentName);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("not null", "value")]
    [InlineData("not empty", "")]
    public void NotNullOrEmpty_ReturnsValue_ForNonNullOrEmptyValue(string value, string argumentName)
    {
        // Act
        var result = Guard.NotNullOrEmpty(value, argumentName);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("foo")]
    [InlineData(" bar ")]
    public void NotNullOrWhiteSpace_ShouldNotThrowException_WhenValueIsNotNullOrWhiteSpace(string value)
    {
        // Arrange

        // Act
        var result = Guard.NotNullOrWhiteSpace(value);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("test", 5)]
    public void MaxLength_ValidInput_ReturnsInput(string value, int maxLength)
    {
        // Act
        var result = Guard.MaxLength(value, maxLength);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("test", 3)]
    [InlineData("test", 2)]
    public void MaxLength_InvalidInput_ThrowsArgumentException(string value, int maxLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Guard.MaxLength(value, maxLength));
    }

    [Fact]
    public void MaxLength_InvalidMaxLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Guard.MaxLength("test", 0));
        Assert.Throws<ArgumentException>(() => Guard.MaxLength("test", -1));
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("test", 3)]
    public void MinLength_ValidInput_ReturnsInput(string value, int minLength)
    {
        // Act
        var result = Guard.MinLength(value, minLength);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("test", 1, 5)]
    [InlineData("test", 2, 5)]
    [InlineData("test", 3, 5)]
    [InlineData("test", 4, 5)]
    public void StrengthLength_ValidInput_ReturnsInput(string value, int minLength, int maxLength)
    {
        // Act
        var result = Guard.StringLengthWithinRange(value, minLength, maxLength);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void StrengthLength_InvalidMinMaxLength_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 5, 4));
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 5, 3));
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 4, 3));
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 3, 3));
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 3, 2));
        Assert.Throws<ArgumentException>(() => Guard.StringLengthWithinRange("test", 2, 2));
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("test.test@example.com")]
    [InlineData("test@sub.example.com")]
    public void Email_ValidEmail_DoesNotThrowException(string email)
    {
        // Act
        Guard.Email(email);
    }

    [Theory]
    [InlineData("valid1email.com", "Custom exception message")]
    [InlineData("another1email.com", "Another custom exception message")]
    public void Email_InvalidEmailWithCustomExceptionMessage_ThrowsArgumentExceptionWithCustomMessage(string email,
        string exceptionMessage)
    {
        // Act
        var ex = Assert.Throws<ArgumentException>(() => Guard.Email(email, exceptionMessage: exceptionMessage));

        // Assert
        Assert.Contains(exceptionMessage, ex.Message);
    }

    [Theory]
    [InlineData(5, 1, 10, "argName")]
    [InlineData(5, 5, 10, "argName")]
    [InlineData(7, 6, 10, "argName")]
    public void Range_ValidInput_DoesNotThrowException(int value, int min, int max, string argName)
    {
        // Act
        Guard.WithinRange(value, min, max, argName);
    }

    [Theory]
    [InlineData(5, 5, "argName")]
    [InlineData(int.MinValue, int.MinValue, "argName")]
    [InlineData(int.MaxValue, 9, "argName")]
    public void Minimum_ValidInput_DoesNotThrowException(int value, int min, string argName)
    {
        // Act
        Guard.Minimum(value, min, argName);
    }

    [Theory]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)]
    [InlineData(int.MinValue, 10, int.MinValue)]
    [InlineData(2, 100, 2)]
    public void Maximum_ValidValues_ReturnsCorrectValue(int value, int maximum, int expected)
    {
        // Act
        var result = Guard.Maximum(value, maximum);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("value", "pattern", "The value of 'value' does not match the specified Regex pattern: pattern.")]
    public void Regex_ValueDoesNotMatchPattern_ThrowsArgumentException(string value, string pattern,
        string exceptionMessage)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => Guard.MatchesRegex(value, pattern, exceptionMessage: exceptionMessage));
    }

    [Theory]
    [InlineData("value", "value")]
    public void Regex_ValueMatchesPattern_ReturnsValue(string value, string pattern)
    {
        // Arrange
        // Act
        var result = Guard.MatchesRegex(value, pattern);

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData("http://www.google.com", "value")]
    [InlineData("https://www.google.com", "value")]
    public void Url_ValidUrl_DoesNotThrowException(string url, string argumentName)
    {
        // Act
        Guard.Url(url, argumentName);
    }

    [Theory]
    [InlineData("invalid", "value")]
    [InlineData("http:google.com", "value")]
    [InlineData("http:/google.com", "value")]
    public void Url_InvalidUrl_ThrowsArgumentException(string url, string argumentName)
    {
        // Act and Assert
        Assert.Throws<ArgumentException>(() => Guard.Url(url, argumentName));
    }

    [Theory]
    [InlineData("{00000000-0000-0000-0000-000000000000}", "value")]
    public void NotEmpty_EmptyGuid_ThrowsArgumentException(string value, string argumentName)
    {
        // Arrange
        var guid = Guid.Parse(value);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => Guard.NotEmpty(guid, argumentName));
    }

    [Theory]
    [InlineData("{12345678-1234-1234-1234-123456781234}", "value")]
    [InlineData("{87654321-4321-4321-4321-876543211234}", "value")]
    public void NotEmpty_NonEmptyGuid_DoesNotThrowException(string value, string argumentName)
    {
        // Arrange
        var guid = Guid.Parse(value);

        // Act
        Guard.NotEmpty(guid, argumentName);
    }

    [Fact]
    public void Requires_ValidInput_ReturnsInput()
    {
        // Arrange
        var value = 5;
        Predicate<int> predicate = x => x > 0;
        var argumentName = "value";
        var exceptionMessage = "The value must be greater than 0.";

        // Act
        var result = Guard.Requires(value, predicate, argumentName, exceptionMessage);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void Requires_InvalidInput_ThrowsArgumentException()
    {
        // Arrange
        var value = 5;
        Predicate<int> predicate = x => x < 0;
        var argumentName = "value";
        var exceptionMessage = "The value must be greater than 0.";

        // Act
        Action action = () => Guard.Requires(value, predicate, argumentName, exceptionMessage);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}