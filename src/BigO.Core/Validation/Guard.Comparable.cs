namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given value does not exceed a specified maximum value.
    ///     If the value exceeds the maximum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value exceeds the maximum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The value if it does not exceed the maximum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value exceeds the maximum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a value does not surpass a defined upper limit.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotExceed(myValue, 100, nameof(myValue));
    ///     </code>
    /// </example>
    public static T Maximum<T>(T value, T maxValue,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null) where T : IComparable
    {
        if (value.CompareTo(maxValue) <= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot exceed {maxValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(argumentName, errorMessage);
    }

    /// <summary>
    ///     Ensures that the given value does not fall below a specified minimum value.
    ///     If the value is less than the minimum, an <see cref="ArgumentOutOfRangeException" /> is thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value falls below the minimum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The value if it is not less than the minimum.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than the minimum allowable value.</exception>
    /// <remarks>
    ///     This method is useful for validating that a value does not fall below a defined lower limit.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.Minimum(myValue, 10, nameof(myValue));
    ///     </code>
    /// </example>
    public static T Minimum<T>(T value, T minValue,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null) where T : IComparable
    {
        if (value.CompareTo(minValue) >= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be less than {minValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(argumentName, errorMessage);
    }

    /// <summary>
    ///     Ensures that the given value falls within a specified range.
    ///     Validates that the minimum and maximum range parameters are logically consistent.
    ///     If the value is outside the specified minimum and maximum range, an <see cref="ArgumentOutOfRangeException" /> is
    ///     thrown.
    ///     This method is applicable for types implementing IComparable.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement IComparable.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value is outside the range.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The value if it falls within the specified range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the value is outside the specified range or if the specified range is invalid.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the minimum value specified is greater than the maximum value specified.
    /// </exception>
    /// <remarks>
    ///     This method is useful for validating that a value falls within a defined range.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.WithinRange(myValue, 10, 20, nameof(myValue));
    ///     </code>
    /// </example>
    public static T WithinRange<T>(T value, T minValue, T maxValue,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
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
            ? $"The value of '{argumentName}' must be between {minValue} and {maxValue}."
            : exceptionMessage;

        throw new ArgumentOutOfRangeException(argumentName, errorMessage);
    }
}