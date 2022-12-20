using System.Globalization;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class DecimalExtensionsTests
{
    [Theory]
    [InlineData(10.00, "en-US", "$10.00")]
    [InlineData(10.00, "fr-FR", "10,00 €")]
    [InlineData(10.00, "ja-JP", "￥10")]
    public void ToCurrencyString_ReturnsCorrectCurrencyString_ForDifferentCultureNames(decimal value,
        string cultureName, string expectedResult)
    {
        // Act
        var result = value.ToCurrencyString(cultureName);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ToCurrencyString_ReturnsCorrectCurrencyString_ForInvariantCulture()
    {
        // Arrange
        var value = 10.00m;

        // Act
        var result = value.ToCurrencyString(CultureInfo.InvariantCulture.Name);

        // Assert
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, "{0:C}", value), result);
    }
}