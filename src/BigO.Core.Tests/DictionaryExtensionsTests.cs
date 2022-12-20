using System.Diagnostics.CodeAnalysis;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class DictionaryExtensionsTests
{
    [Fact]
    public void AddIfNotContainsKey_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;
        var key = 1;
        var value = "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, value));
    }

    [Fact]
    public void AddIfNotContainsKey_KeyNotInDictionary_AddsKeyValuePairAndReturnsTrue()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        var key = 1;
        var value = "value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, value);

        // Assert
        Assert.True(result);
        Assert.Equal(value, dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey_KeyInDictionary_DoesNotAddKeyValuePairAndReturnsFalse()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };
        const int key = 1;
        const string value = "new value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, value);

        // Assert
        Assert.False(result);
        Assert.Equal("value", dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey_KeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = null!;
        const string value = "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, value));
    }

    [Fact]
    public void AddIfNotContainsKey_ValueIsNull_AddsKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        string value = null!;

        // Act
        var result = dictionary.AddIfNotContainsKey(key, value);

        // Assert
        Assert.True(result);
        Assert.Null(dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey2_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;
        var key = 1;
        Func<string> valueFactory = () => "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }


    [Fact]
    public void AddIfNotContainsKey2_KeyNotInDictionary_AddsKeyValuePairAndReturnsTrue()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        var key = 1;
        Func<string> valueFactory = () => "value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.True(result);
        Assert.Equal("value", dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey2_KeyInDictionary_DoesNotAddKeyValuePairAndReturnsFalse()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };
        var key = 1;
        Func<string> valueFactory = () => "new value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.False(result);
        Assert.Equal("value", dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey2_KeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = null!;
        Func<string> valueFactory = () => "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }

    [Fact]
    public void AddIfNotContainsKey2_ValueFactoryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<string> valueFactory = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }

    [Fact]
    public void AddIfNotContainsKey2_ValueFactoryReturnsNull_AddsKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<string> valueFactory = () => null!;

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.True(result);
        Assert.Null(dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey2_ValueFactoryThrowsException_DoesNotAddKeyValuePairAndRethrowsException()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<string> valueFactory = () => throw new Exception("value factory error");

        // Act and Assert
        var exception = Assert.Throws<Exception>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
        Assert.Equal("value factory error", exception.Message);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void AddIfNotContainsKey3_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;
        var key = 1;
        Func<int, string> valueFactory = k => "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }

    [Fact]
    public void AddIfNotContainsKey3_KeyNotInDictionary_AddsKeyValuePairAndReturnsTrue()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<int, string> valueFactory = k => "value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.True(result);
        Assert.Equal("value", dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey3_KeyInDictionary_DoesNotAddKeyValuePairAndReturnsFalse()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };
        const int key = 1;
        Func<int, string> valueFactory = k => "new value";

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.False(result);
        Assert.Equal("value", dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey3_KeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = null!;
        Func<string, string> valueFactory = k => "value";

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }

    [Fact]
    public void AddIfNotContainsKey3_ValueFactoryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        var key = 1;
        Func<int, string> valueFactory = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
    }

    [Fact]
    public void AddIfNotContainsKey_ValueFactoryReturnsNull_AddsKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<int, string> valueFactory = k => null!;

        // Act
        var result = dictionary.AddIfNotContainsKey(key, valueFactory);

        // Assert
        Assert.True(result);
        Assert.Null(dictionary[key]);
    }

    [Fact]
    public void AddIfNotContainsKey_ValueFactoryThrowsException_DoesNotAddKeyValuePairAndRethrowsException()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;
        Func<int, string> valueFactory = k => throw new Exception("value factory error");

        // Act and Assert
        var exception = Assert.Throws<Exception>(() => dictionary.AddIfNotContainsKey(key, valueFactory));
        Assert.Equal("value factory error", exception.Message);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void RemoveIfContainsKey_DictionaryIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<int, string> dictionary = null!;
        const int key = 1;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.RemoveIfContainsKey(key));
    }

    [Fact]
    public void RemoveIfContainsKey_KeyNotInDictionary_DoesNothing()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>();
        const int key = 1;

        // Act
        dictionary.RemoveIfContainsKey(key);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void RemoveIfContainsKey_KeyInDictionary_RemovesKeyValuePair()
    {
        // Arrange
        IDictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "value" }
        };
        const int key = 1;

        // Act
        dictionary.RemoveIfContainsKey(key);

        // Assert
        Assert.DoesNotContain(key, dictionary.Keys);
    }

    [Fact]
    public void RemoveIfContainsKey_KeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.RemoveIfContainsKey(key));
    }

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