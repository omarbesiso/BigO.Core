namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating property values.
/// </summary>
public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property value does not exceed a specified maximum value.
    ///     If the value exceeds the maximum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing <see cref="IComparable{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value exceeds the maximum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it does not exceed the maximum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the property value exceeds the maximum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value does not surpass a defined upper limit.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.Maximum(myProperty, 100, nameof(myProperty));
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Maximum<T>(T value, T maxValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable<T>
    {
        return Guard.Maximum(value, maxValue, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property value does not fall below a specified minimum value.
    ///     If the value is less than the minimum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing <see cref="IComparable{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value falls below the minimum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it is not less than the minimum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the property value is less than the minimum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value does not fall below a defined lower limit.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.Minimum(myProperty, 10, nameof(myProperty));
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Minimum<T>(T value, T minValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable<T>
    {
        return Guard.Minimum(value, minValue, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given property value falls within a specified range.
    ///     Validates that the minimum and maximum range parameters are logically consistent.
    ///     If the value is outside the specified minimum and maximum range, an <see cref="ArgumentOutOfRangeException" /> is
    ///     thrown.
    ///     This method is applicable for types implementing <see cref="IComparable{t}" />.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value is outside the range.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it falls within the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the property value is outside the specified range or if the specified range is invalid.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the minimum value specified is greater than the maximum value specified.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating that a property value falls within a defined range.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.WithinRange(myProperty, 10, 20, nameof(myProperty));
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T WithinRange<T>(T value, T minValue, T maxValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable<T>
    {
        return Guard.WithinRange(value, minValue, maxValue, propertyName, exceptionMessage);
    }
}