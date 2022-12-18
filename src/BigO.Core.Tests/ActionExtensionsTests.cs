using BigO.Core.Extensions;

namespace BigO.Core.Tests;

public class ActionExtensionsTests
{
    [Fact]
    public async void RunAsynchronously_ShouldInvokeAction()
    {
        // Arrange
        var actionInvoked = false;
        Action action = () => actionInvoked = true;

        // Act
        await action.RunAsynchronously();

        // Assert
        Assert.True(actionInvoked);
    }

    [Fact]
    public async void RunAsynchronously_ShouldThrowArgumentNullException_IfActionIsNull()
    {
        // Arrange
        Action action = null!;

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => action.RunAsynchronously());
    }

    [Fact]
    public static void ExecuteAndTime_ReturnsExpectedTimeSpan()
    {
        // Arrange
        var action = () =>
        {
            // Sleep for 1 second to simulate execution time
            Thread.Sleep(1000);
        };

        // Act
        var elapsed = action.ExecuteAndTime();

        // Assert
        Assert.True(elapsed >= TimeSpan.FromSeconds(1) && elapsed <= TimeSpan.FromSeconds(1.1),
            $"Expected elapsed time to be between 1 and 1.1 seconds, but got {elapsed.TotalSeconds} seconds");
    }

    [Fact]
    public static void ExecuteAndTime_ThrowsException_ForNullAction()
    {
        // Arrange
        Action action = null!;

        // Act and assert
        Assert.Throws<ArgumentNullException>(() => action.ExecuteAndTime());
    }
}

