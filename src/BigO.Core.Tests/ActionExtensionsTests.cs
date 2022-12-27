using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ActionExtensionsTests
{
    [Fact]
    public async Task RunAsynchronously_ThrowsArgumentNullException_WhenActionIsNull()
    {
        // Arrange
        Action action = null!;

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => action.RunAsynchronously());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task RunAsynchronously_RunsActionAsynchronously_WhenActionIsNotNull(int value)
    {
        // Arrange
        var result = 0;
        Action action = () => result = value;

        // Act
        await action.RunAsynchronously();

        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ExecuteAndTime_ActionExecuted_ReturnsCorrectTimeSpan(int delay)
    {
        // Arrange
        var action = new Action(() => Thread.Sleep(delay * 1000));

        // Act
        var result = action.ExecuteAndTime();

        // Assert
        Assert.Equal(delay, result.Seconds);
    }

    [Fact]
    public void ExecuteAndTime_ActionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Action action = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => action.ExecuteAndTime());
        Assert.Equal(nameof(action), exception.ParamName);
        Assert.Equal(string.Format("The {0} cannot be null. (Parameter '{0}')", nameof(action)), exception.Message);
    }
}