namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating property values.
/// </summary>
public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property value satisfies a specified predicate.
    ///     If the value does not satisfy the predicate, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="predicate">The predicate that the property value must satisfy.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value does not satisfy the predicate.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it satisfies the predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="predicate" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if the property value does not satisfy the predicate.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value meets a custom condition defined by the predicate.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.Requires(myProperty, x => x > 0, nameof(myProperty), "Value must be greater than 0");
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Requires<T>(T value, Predicate<T> predicate,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.Requires(value, predicate, propertyName, exceptionMessage);
    }
}