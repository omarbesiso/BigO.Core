using System.Diagnostics.CodeAnalysis;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class DictionaryExtensionsTests
{
    [Fact]
    public void ToSortedDictionary_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.ToSortedDictionary());
    }

    [Fact]
    public void ToSortedDictionary_DictionaryIsEmpty_ReturnsEmptySortedDictionary()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();

        // Act
        var result = dictionary.ToSortedDictionary();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToSortedDictionary_DictionaryHasOneKeyValuePair_ReturnsSortedDictionaryWithOneKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };

        // Act
        var result = dictionary.ToSortedDictionary();

        // Assert
        Assert.Single(result);
        Assert.Equal("value", result[1]);
    }

    [Fact]
    public void ToSortedDictionary_DictionaryHasMultipleKeyValuePairs_ReturnsSortedDictionaryWithMultipleKeyValuePairs()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 3, "third" },
            { 1, "first" },
            { 2, "second" }
        };

        // Act
        var result = dictionary.ToSortedDictionary();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("first", result[1]);
        Assert.Equal("second", result[2]);
        Assert.Equal("third", result[3]);
    }

    [Fact]
    public void ToSortedDictionary2_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;
        IComparer<int> comparer = Comparer<int>.Default;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.ToSortedDictionary(comparer));
    }

    [Fact]
    public void ToSortedDictionary2_DictionaryIsEmpty_ReturnsEmptySortedDictionary()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        IComparer<int> comparer = Comparer<int>.Default;

        // Act
        var result = dictionary.ToSortedDictionary(comparer);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToSortedDictionary2_DictionaryHasOneKeyValuePair_ReturnsSortedDictionaryWithOneKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };
        IComparer<int> comparer = Comparer<int>.Default;

        // Act
        var result = dictionary.ToSortedDictionary(comparer);

        // Assert
        Assert.Single(result);
        Assert.Equal("value", result[1]);
    }

    [Fact]
    public void
        ToSortedDictionary_DictionaryHasMultipleKeyValuePairsSortedInDescendingOrder_ReturnsSortedDictionaryWithMultipleKeyValuePairs()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 3, "third" },
            { 1, "first" },
            { 2, "second" }
        };
        IComparer<int> comparer = Comparer<int>.Create((x, y) => y.CompareTo(x));

        // Act
        var result = dictionary.ToSortedDictionary(comparer);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("third", result[3]);
        Assert.Equal("second", result[2]);
        Assert.Equal("first", result[1]);
    }
}