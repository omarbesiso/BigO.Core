using System.Collections;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void AddUnique_OnNullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.AddUnique(0));
    }

    [Theory]
    [InlineData(new int[] { }, 0, true)]
    [InlineData(new[] { 1, 2, 3 }, 0, true)]
    [InlineData(new[] { 1, 2, 3 }, 3, false)]
    public void AddUnique_ReturnsExpectedResult(int[] collection, int value, bool expectedResult)
    {
        // Arrange
        var list = new List<int>(collection);

        // Act
        var result = list.AddUnique(value);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddUniqueRange_OnNullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.AddUniqueRange(new[] { 1, 2, 3 }));
    }

    [Theory]
    [InlineData(new int[] { }, new int[] { }, 0)]
    [InlineData(new int[] { }, new[] { 1, 2, 3 }, 3)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, 3)]
    public void AddUniqueRange_ReturnsExpectedResult(int[] collection, int[] values, int expectedResult)
    {
        // Arrange
        var list = new List<int>(collection);

        // Act
        var result = list.AddUniqueRange(values);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RemoveWhere_OnNullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(x => x > 0));
    }

    [Fact]
    public void RemoveWhere_OnNullPredicate_ThrowsArgumentNullException()
    {
        // Arrange
        var collection = new List<int>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(null!));
    }

    [Theory]
    [ClassData(typeof(RemoveWhereTestData))]
    public void RemoveWhere_ReturnsExpectedResult(int[] collection, Predicate<int> predicate, int expectedResult)
    {
        // Arrange
        var list = new List<int>(collection);

        // Act
        var result = list.RemoveWhere(predicate);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddIf_ReturnsTrue_WhenPredicateIsTrue()
    {
        // Arrange
        var collection = new List<int>();
        var predicate = new Func<int, bool>(x => x > 0);
        var value = 1;

        // Act
        var result = collection.AddIf(predicate, value);

        // Assert
        Assert.True(result);
        Assert.Contains(value, collection);
        Assert.Single(collection); // Additional assert to check the collection's count after the successful addition
    }

    [Fact]
    public void AddIf_ReturnsFalse_WhenPredicateIsFalse()
    {
        // Arrange
        var collection = new List<int>();
        var predicate = new Func<int, bool>(x => x > 0);
        var value = -1;

        // Act
        var result = collection.AddIf(predicate, value);

        // Assert
        Assert.False(result);
        Assert.DoesNotContain(value, collection);
        Assert.Empty(collection); // Additional assert to check the collection's count after the unsuccessful addition
    }

    [Fact]
    public void AddIf_ThrowsArgumentNullException_WhenCollectionIsNull()
    {
        // Arrange
        ICollection<int> collection = null!;
        var predicate = new Func<int, bool>(x => x > 0);
        var value = 1;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.AddIf(predicate, value));
    }

    [Fact]
    public void AddIf_ThrowsArgumentNullException_WhenPredicateIsNull()
    {
        // Arrange
        var collection = new List<int>();
        Func<int, bool> predicate = null!;
        var value = 1;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.AddIf(predicate, value));
    }

    [Fact]
    public void AddIf_AddsValueToEmptyCollection_WhenPredicateIsTrue()
    {
        // Arrange
        var collection = new List<int>();
        var predicate = new Func<int, bool>(x => x > 0);
        var value = 1;

        // Act
        var result = collection.AddIf(predicate, value);

        // Assert
        Assert.True(result);
        Assert.Contains(value, collection);
        Assert.Single(collection);
    }

    [Fact]
    public void AddIf_AddsValueToNonEmptyCollection_WhenPredicateIsTrue()
    {
        // Arrange
        var collection = new List<int> { 0, 1, 2 };
        var predicate = new Func<int, bool>(x => x > 0);
        var value = 3;

        // Act
        var result = collection.AddIf(predicate, value);

        // Assert
        Assert.True(result);
        Assert.Contains(value, collection);
        Assert.Equal(4,
            collection.Count); // Additional assert to check the collection's count after the successful addition
    }

    [Fact]
    public void ContainsAny_ReturnsTrue_WhenCollectionContainsAnyOfTheValues()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };
        var values = new[] { 2, 4, 6 };

        // Act
        var result = collection.ContainsAny(values);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsAny_ReturnsFalse_WhenCollectionDoesNotContainAnyOfTheValues()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };
        var values = new[] { 4, 5, 6 };

        // Act
        var result = collection.ContainsAny(values);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_ReturnsFalse_WhenCollectionIsEmpty()
    {
        // Arrange
        var collection = new List<int>();
        var values = new[] { 1, 2, 3 };

        // Act
        var result = collection.ContainsAny(values);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_ThrowsArgumentNullException_WhenCollectionIsNull()
    {
        // Arrange
        ICollection<int> collection = null!;
        var values = new[] { 1, 2, 3 };

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.ContainsAny(values));
    }

    private class RemoveWhereTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new int[] { }, (Predicate<int>)(x => x > 0), 0 };
            yield return new object[] { new[] { 1, 2, 3 }, (Predicate<int>)(x => x > 0), 3 };
            yield return new object[] { new[] { 1, 2, 3 }, (Predicate<int>)(x => x < 0), 0 };
            yield return new object[] { new[] { 1, 2, 3 }, (Predicate<int>)(x => x % 2 == 0), 1 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

