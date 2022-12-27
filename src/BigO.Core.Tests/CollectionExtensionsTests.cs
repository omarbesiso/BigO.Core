using System.Collections.ObjectModel;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class CollectionExtensionsTests
{
    public static IEnumerable<object[]> RemoveWhereTestData()
    {
        yield return new object[] { new List<int> { 1, 2, 3, 4, 5 }, (Predicate<int>)(x => x % 2 == 0), 2 };
        yield return new object[] { new List<int> { 1, 2, 3, 4, 5 }, (Predicate<int>)(x => x > 5), 0 };
        yield return new object[]
            { new List<string> { "a", "b", "c", "d", "e" }, (Predicate<string>)(x => x == "b" || x == "d"), 2 };
        yield return new object[]
            { new List<string> { "a", "b", "c", "d", "e" }, (Predicate<string>)(x => x == "f"), 0 };
        yield return new object[] { new Collection<int> { 1, 2, 3, 4, 5 }, (Predicate<int>)(x => x % 2 == 0), 2 };
        yield return new object[] { new Collection<int> { 1, 2, 3, 4, 5 }, (Predicate<int>)(x => x > 5), 0 };
        yield return new object[]
            { new Collection<string> { "a", "b", "c", "d", "e" }, (Predicate<string>)(x => x == "b" || x == "d"), 2 };
        yield return new object[]
            { new Collection<string> { "a", "b", "c", "d", "e" }, (Predicate<string>)(x => x == "f"), 0 };
    }

    [Theory]
    [MemberData(nameof(RemoveWhereTestData))]
    public void RemoveWhere_PredicateMatchingElements_ReturnsNumberOfRemovedElements<T>(ICollection<T> collection,
        Predicate<T> predicate, int expected)
    {
        // Act
        var result = collection.RemoveWhere(predicate);

        // Assert
        Assert.Equal(expected, result);
        foreach (var item in collection)
        {
            Assert.True(!predicate(item));
        }
    }

    [Fact]
    public void RemoveWhere_CollectionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(x => x > 0));
        Assert.Equal("collection", exception.ParamName);
        Assert.Equal($"The {nameof(collection)} cannot be null. (Parameter '{nameof(collection)}')", exception.Message);
    }

    [Fact]
    public void RemoveWhere_PredicateIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var collection = new List<int>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(null!));
        Assert.Equal("predicate", exception.ParamName);
        Assert.Equal("The predicate cannot be null. (Parameter 'predicate')", exception.Message);
    }

    public static IEnumerable<object[]> AddUniqueTestData()
    {
        yield return new object[] { new List<int>(), 1, true };
        yield return new object[] { new List<int> { 1 }, 1, false };
        yield return new object[] { new List<int> { 1 }, 2, true };
        yield return new object[] { new List<string> { "a", "b", "c" }, "d", true };
        yield return new object[] { new List<string> { "a", "b", "c" }, "b", false };
    }

    [Theory]
    [MemberData(nameof(AddUniqueTestData))]
    public void AddUnique_UniqueValueAdded_ReturnsTrue<T>(ICollection<T> collection, T value, bool expected)
    {
        // Act
        var result = collection.AddUnique(value);

        // Assert
        Assert.Equal(expected, result);
        if (expected)
        {
            Assert.Contains(value, collection);
        }
    }

    [Fact]
    public void AddUnique_CollectionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => collection.AddUnique(1));
        Assert.Equal(nameof(collection), exception.ParamName);
        Assert.Equal($"The {nameof(collection)} cannot be null. (Parameter '{nameof(collection)}')", exception.Message);
    }

    public static IEnumerable<object[]> AddIfTestData()
    {
        yield return new object[] { new List<int> { 1, 2, 3, 4, 5 }, (Func<int, bool>)(x => x % 2 == 0), 6, true };
        yield return new object[] { new List<int> { 1, 2, 3, 4, 5 }, (Func<int, bool>)(x => x % 2 == 0), 5, false };
        yield return new object[]
            { new List<string> { "a", "b", "c", "d", "e" }, (Func<string, bool>)(x => x.Length > 0), "f", true };
        yield return new object[]
            { new List<string> { "a", "b", "c", "d", "e" }, (Func<string, bool>)(x => x.Length > 2), "e", false };
        yield return new object[]
            { new Collection<int> { 1, 2, 3, 4, 5 }, (Func<int, bool>)(x => x % 2 == 0), 6, true };
        yield return new object[]
            { new Collection<int> { 1, 2, 3, 4, 5 }, (Func<int, bool>)(x => x % 2 == 0), 5, false };
        yield return new object[]
            { new Collection<string> { "a", "b", "c", "d", "e" }, (Func<string, bool>)(x => x.Length > 0), "f", true };
        yield return new object[]
            { new Collection<string> { "a", "b", "c", "d", "e" }, (Func<string, bool>)(x => x.Length > 2), "e", false };
    }

    [Theory]
    [MemberData(nameof(AddIfTestData))]
    public void AddIf_PredicateMatchingValue_AddsValueToCollection<T>(ICollection<T> collection,
        Func<T, bool> predicate, T value, bool expected)
    {
        // Act
        var result = collection.AddIf(predicate, value);

        // Assert
        Assert.Equal(expected, result);
        if (expected)
        {
            Assert.Contains(value, collection);
        }
    }

    [Fact]
    public void AddIf_CollectionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => collection.AddIf(x => x > 0, 5));
        Assert.Equal("collection", exception.ParamName);
        Assert.Equal($"The {nameof(collection)} cannot be null. (Parameter '{nameof(collection)}')", exception.Message);
    }

    [Fact]
    public void AddIf_PredicateIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3, 4, 5 };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => collection.AddIf(null!, 5));
        Assert.Equal("predicate", exception.ParamName);
        Assert.Equal("The predicate cannot be null. (Parameter 'predicate')", exception.Message);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, 0)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, 3)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 3, 4, 5 }, 2)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, 2)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5, 6, 7, 8 }, 5)]
    public void AddUniqueRange_AddsValuesToCollection(int[] initialValues, int[] valuesToAdd, int expectedCount)
    {
        // Arrange
        var collection = new List<int>(initialValues);

        // Act
        var result = collection.AddUniqueRange(valuesToAdd);

        // Assert
        Assert.Equal(expectedCount, result);
        Assert.Equal(initialValues.Concat(valuesToAdd).Distinct().Count(), collection.Count);
    }

    [Fact]
    public void AddUniqueRange_ThrowsArgumentNullException_WhenCollectionIsNull()
    {
        // Arrange
        ICollection<int> collection = null!;
        var values = new[] { 1, 2, 3 };

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => collection.AddUniqueRange(values));
    }

    [Fact]
    public void AddUniqueRange_ReturnsZero_WhenValuesAreNull()
    {
        // Arrange
        var collection = new List<int> { 1, 2, 3 };
        IEnumerable<int> values = null!;

        // Act
        var result = collection.AddUniqueRange(values);

        // Assert
        Assert.Equal(0, result);
        Assert.Equal(3, collection.Count);
    }

    [Fact]
    public void ContainsAny_WithNullCollection_ThrowsException()
    {
        // Arrange
        ICollection<int> collection = null!;

        // Act
        // ReSharper disable once ConvertToLocalFunction
        Action action = () => collection.ContainsAny(1, 2, 3);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void ContainsAny_WithEmptyCollection_ReturnsFalse()
    {
        // Arrange
        ICollection<int> collection = new Collection<int>();

        // Act
        var result = collection.ContainsAny(1, 2, 3);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_WithEmptyValues_ReturnsFalse()
    {
        // Arrange
        ICollection<int> collection = new Collection<int> { 1, 2, 3 };

        // Act
        var result = collection.ContainsAny();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_WithNoMatch_ReturnsFalse()
    {
        // Arrange
        ICollection<int> collection = new Collection<int> { 1, 2, 3 };

        // Act
        var result = collection.ContainsAny(4, 5, 6);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsAny_WithMatch_ReturnsTrue()
    {
        // Arrange
        ICollection<int> collection = new Collection<int> { 1, 2, 3 };

        // Act
        var result = collection.ContainsAny(2, 4, 6);

        // Assert
        Assert.True(result);
    }
}