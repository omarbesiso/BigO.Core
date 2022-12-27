using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ComparableExtensionsTests
{
    public static IEnumerable<object[]> IsBetweenTestData()
    {
        yield return new object[] { 1, 0, 2, true, true };
        yield return new object[] { 2, 0, 2, true, true };
        yield return new object[] { 3, 0, 2, true, false };
        yield return new object[] { 1, 0, 2, false, true };
        yield return new object[] { 2, 0, 2, false, false };
        yield return new object[] { 3, 0, 2, false, false };
    }

    [Theory]
    [MemberData(nameof(IsBetweenTestData))]
    public void IsBetween_ValueIsBetweenBounds_ReturnsTrue<T>(T? value, T lowerBoundary, T upperBoundary,
        bool isBoundaryInclusive, bool expected)
        where T : IComparable<T>
    {
        // Act
        var result = value.IsBetween(lowerBoundary, upperBoundary, isBoundaryInclusive);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> LimitTestData()
    {
        yield return new object[] { 1, 2, 1 };
        yield return new object[] { 3, 2, 2 };
        yield return new object[] { "a", "b", "a" };
        yield return new object[] { "c", "b", "b" };
    }

    [Theory]
    [MemberData(nameof(LimitTestData))]
    public void Limit_ValueIsGreaterThanMaximum_ReturnsMaximum<T>(T value, T maximum, T expected)
        where T : IComparable<T>
    {
        // Act
        var result = value.Limit(maximum);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> LimitTestData2()
    {
        yield return new object[] { 1, 0, 2, 1 };
        yield return new object[] { 3, 0, 2, 2 };
        yield return new object[] { -1, 0, 2, 0 };
        yield return new object[] { "a", "b", "c", "b" };
        yield return new object[] { "d", "b", "c", "c" };
        yield return new object[] { "a", "b", "a", "b" };
    }

    [Theory]
    [MemberData(nameof(LimitTestData2))]
    public void Limit2_ValueIsBetweenMinimumAndMaximum_ReturnsValue<T>(T value, T minimum, T maximum, T expected)
        where T : IComparable
    {
        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Limit2_ValueIsLessThanMinimum_ReturnsMinimum()
    {
        // Arrange
        var value = -1;
        var minimum = 0;
        var maximum = 2;

        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(minimum, result);
    }

    [Fact]
    public void Limit2_ValueIsGreaterThanMaximum_ReturnsMaximum()
    {
        // Arrange
        var value = 3;
        var minimum = 0;
        var maximum = 2;

        // Act
        var result = value.Limit(minimum, maximum);

        // Assert
        Assert.Equal(maximum, result);
    }
}