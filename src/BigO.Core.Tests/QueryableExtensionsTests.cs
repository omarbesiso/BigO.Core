using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class QueryableExtensionsTests
{
    [Fact]
    public void Page_ReturnsCorrectPage_ForValidInput()
    {
        // Arrange
        var source = Enumerable.Range(1, 20).AsQueryable();
        const int pageNumber = 2;
        const int pageSize = 5;

        // Act
        var result = source.Page(pageNumber, pageSize);

        // Assert
        Assert.Equal(5, result.Count());
        Assert.Equal(6, result.First());
        Assert.Equal(10, result.Last());
    }

    [Fact]
    public void Page_ThrowsArgumentNullException_ForNullSource()
    {
        // Arrange
        IQueryable<int> source = null!;
        const int pageNumber = 2;
        const int pageSize = 5;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => source.Page(pageNumber, pageSize));
    }

    [Fact]
    public void Page_ReturnsEmptyPage_ForPageNumberOutOfRange()
    {
        // Arrange
        var source = Enumerable.Range(1, 20).AsQueryable();
        const int pageNumber = 10;
        const int pageSize = 5;

        // Act
        var result = source.Page(pageNumber, pageSize);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Page_ReturnsCorrectPage_ForPageSizeGreaterThanSourceCount()
    {
        // Arrange
        var source = Enumerable.Range(1, 20).AsQueryable();
        const int pageNumber = 1;
        const int pageSize = 50;

        // Act
        var result = source.Page(pageNumber, pageSize);

        // Assert
        Assert.Equal(20, result.Count());
        Assert.Equal(1, result.First());
        Assert.Equal(20, result.Last());
    }

    [Fact]
    public void Page_ReturnsCorrectPage_ForPageNumberEqualToOne()
    {
        // Arrange
        var source = Enumerable.Range(1, 20).AsQueryable();
        const int pageNumber = 1;
        const int pageSize = 5;

        // Act
        var result = source.Page(pageNumber, pageSize);

        // Assert
        Assert.Equal(5, result.Count());
        Assert.Equal(1, result.First());
        Assert.Equal(5, result.Last());
    }
}