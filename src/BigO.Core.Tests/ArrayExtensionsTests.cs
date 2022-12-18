using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ArrayExtensionsTests
{
    [Fact]
    public void Shuffle_OnNullArray_ThrowsArgumentNullException()
    {
        // Arrange
        int[] array = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => array.Shuffle());
    }

    [Fact]
    public void Shuffle_OnEmptyArray_ReturnsSameArray()
    {
        // Arrange
        var array = Array.Empty<int>();

        // Act
        var result = array.Shuffle();

        // Assert
        Assert.Same(array, result);
    }

    [Fact]
    public void Shuffle_OnArrayWithOneElement_ReturnsSameArray()
    {
        // Arrange
        int[] array = { 1 };

        // Act
        var result = array.Shuffle();

        // Assert
        Assert.Same(array, result);
    }

    [Fact]
    public void Shuffle_OnArrayWithMultipleElements_ReturnsShuffledArray()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };
        int[] array2 = { 1, 2, 3, 4, 5 };

        // Act
        var result = array.Shuffle();

        // Assert
        Assert.NotSame(array2, result);
        Assert.Equal(array2, result.OrderBy(x => x));
    }

    [Fact]
    public void Clear_OnNullArray_ThrowsArgumentNullException()
    {
        // Arrange
        int[] array = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => array.Clear(0, 0));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void Clear_OnInvalidIndexOrLength_ThrowsArgumentOutOfRangeException(int index, int length)
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act and Assert
        Assert.Throws<IndexOutOfRangeException>(() => array.Clear(index, length));
    }

    [Fact]
    public void Clear_OnValidIndexAndLength_ClearsArrayElements()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act
        array.Clear(1, 3);

        // Assert
        Assert.Equal(new[] { 1, 0, 0, 0, 5 }, array);
    }
}