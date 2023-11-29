namespace BigO.Core.Validation;

public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property value does not exceed a specified maximum value.
    ///     If the value exceeds the maximum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value exceeds the maximum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it does not exceed the maximum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the property value exceeds the maximum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value does not surpass a defined upper limit.
    /// </remarks>
    public static T Maximum<T>(T value, T maxValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable
    {
        if (value.CompareTo(maxValue) <= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' cannot exceed {maxValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(propertyName, errorMessage);
    }

    /// <summary>
    ///     Ensures that the given property value does not fall below a specified minimum value.
    ///     If the property value is less than the minimum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value falls below the minimum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The property value if it is not less than the minimum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the property value is less than the minimum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a property value does not fall below a defined lower limit.
    /// </remarks>
    public static T Minimum<T>(T value, T minValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable
    {
        if (value.CompareTo(minValue) >= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' cannot be less than {minValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(propertyName, errorMessage);
    }


    /// <summary>
    ///     Ensures that the given property value falls within a specified range.
    ///     Validates that the minimum and maximum range parameters are logically consistent.
    ///     If the property value is outside the specified minimum and maximum range, an
    ///     <see cref="ArgumentOutOfRangeException" /> is
    ///     thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The property value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value is outside the range.
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
    public static T WithinRange<T>(T value, T minValue, T maxValue,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null) where T : IComparable
    {
        if (minValue.CompareTo(maxValue) > 0)
        {
            throw new ArgumentException(
                "The minimum value specified cannot be greater than the maximum value specified.",
                nameof(minValue));
        }

        if (value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' must be between {minValue} and {maxValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(propertyName, errorMessage);
    }
}