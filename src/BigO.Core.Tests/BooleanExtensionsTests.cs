using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class BooleanExtensionsTests
{
    [Theory]
    [InlineData(true, "true", "false", "true")]
    [InlineData(false, "true", "false", "false")]
    public void ToString_ReturnsExpectedResult(bool source, string trueValue, string falseValue, string expectedResult)
    {
        // Act
        var result = source.ToString(trueValue, falseValue);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void ToBit_ReturnsExpectedResult(bool source, byte expectedResult)
    {
        // Act
        var result = source.ToByte();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}