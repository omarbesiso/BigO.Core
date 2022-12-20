using System.Collections;
using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void IsEmpty_WithEmptyList_ReturnsTrue()
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        var list = new List<int>();
        var result = list.IsEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyList_ReturnsFalse()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.IsEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_WithEmptyArray_ReturnsTrue()
    {
        var array = Array.Empty<int>();
        var result = array.IsEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyArray_ReturnsFalse()
    {
        var array = new[] { 1, 2, 3 };
        var result = array.IsEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_WithEmptyCollection_ReturnsTrue()
    {
        var collection = new TestCollection();
        var result = collection.IsEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyCollection_ReturnsFalse()
    {
        var collection = new TestCollection(new[] { 1, 2, 3 });
        var result = collection.IsEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsEmpty_WithNullCollection_ThrowsArgumentNullException()
    {
        IEnumerable<int> collection = null!;
        Assert.Throws<ArgumentNullException>(() => collection.IsEmpty());
    }

    [Fact]
    public void IsNotEmpty_WithEmptyList_ReturnsFalse()
    {
        var list = new List<int>();
        var result = list.IsNotEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_WithNonEmptyList_ReturnsTrue()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.IsNotEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsNotEmpty_WithEmptyArray_ReturnsFalse()
    {
        var array = Array.Empty<int>();
        var result = array.IsNotEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_WithNonEmptyArray_ReturnsTrue()
    {
        var array = new[] { 1, 2, 3 };
        var result = array.IsNotEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsNotEmpty_WithEmptyCollection_ReturnsFalse()
    {
        var collection = new TestCollection();
        var result = collection.IsNotEmpty();
        Assert.False(result);
    }

    [Fact]
    public void IsNotEmpty_WithNonEmptyCollection_ReturnsTrue()
    {
        var collection = new TestCollection(new[] { 1, 2, 3 });
        var result = collection.IsNotEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsNotEmpty_WithNullCollection_ThrowsArgumentNullException()
    {
        IEnumerable<int> collection = null!;
        Assert.Throws<ArgumentNullException>(() => collection.IsNotEmpty());
    }

    private class TestCollection : IEnumerable<int>
    {
        private readonly List<int> _items;

        public TestCollection()
        {
            _items = new List<int>();
        }

        public TestCollection(IEnumerable<int> items)
        {
            _items = new List<int>(items);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}