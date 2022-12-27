using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ArrayExtensionsTests
{
    public static IEnumerable<object[]> ShuffleTestData()
    {
        yield return new object[] { new[] { 1, 2, 3, 4, 5 } };
        yield return new object[] { new[] { "a", "b", "c", "d", "e" } };
        yield return new object[] { new[] { 1.5, 2.5, 3.5, 4.5, 5.5 } };
    }

    [Theory]
    [MemberData(nameof(ShuffleTestData))]
    public void Shuffle_ArrayShuffled_ReturnsShuffledArray<T>(T[] array)
    {
        // Arrange
        var originalArray = (T[])array.Clone();

        // Act
        var result = array.Shuffle();

        // Assert
        Assert.NotEqual(originalArray, result);
        Assert.Equal(originalArray.Length, result.Length);
        Assert.Contains(result, x => originalArray.Contains(x));
    }

    [Fact]
    public void Shuffle_ArrayIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        int[] array = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => array.Shuffle());
        Assert.Equal(nameof(array), exception.ParamName);
        Assert.Equal(string.Format("The {0} cannot be null. (Parameter '{0}')", nameof(array)), exception.Message);
    }

    public static IEnumerable<object[]> ClearTestData()
    {
        yield return new object[] { new int[5], 0, 3 };
        yield return new object[] { new string[7], 1, 5 };
        yield return new object[] { new double[10], 3, 2 };
    }

    [Theory]
    [MemberData(nameof(ClearTestData))]
    public void Clear_ArrayCleared_ArrayCleared<T>(T[] array, int index, int length)
    {
        // Arrange
        var originalArray = (T[])array.Clone();

        // Act
        array.Clear(index, length);

        // Assert
        for (var i = 0; i < index; i++)
        {
            Assert.Equal(originalArray[i], array[i]);
        }

        for (var i = index; i < index + length; i++)
        {
            Assert.Equal(default, array[i]);
        }

        for (var i = index + length; i < array.Length; i++)
        {
            Assert.Equal(originalArray[i], array[i]);
        }
    }

    [Fact]
    public void Clear_ArrayIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        int[] array = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => array.Clear(0, 0));
        Assert.Equal(nameof(array), exception.ParamName);
        Assert.Equal(string.Format("The {0} cannot be null. (Parameter '{0}')", nameof(array)), exception.Message);
    }
}