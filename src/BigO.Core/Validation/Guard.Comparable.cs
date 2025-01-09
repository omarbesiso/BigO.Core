namespace BigO.Core.Validation;

/// <summary>
///     A static class providing guard methods for validating arguments against certain conditions,
///     specifically using types that implement <see cref="IComparable{T}" />.
/// </summary>
public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given <paramref name="value" /> does not exceed a specified <paramref name="maxValue" />.
    ///     If <paramref name="value" /> exceeds <paramref name="maxValue" />, an <see cref="ArgumentOutOfRangeException" /> is
    ///     thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="paramName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    ///     Automatically provided when using <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value exceeds the maximum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The original <paramref name="value" /> if it does not exceed <paramref name="maxValue" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value" /> exceeds <paramref name="maxValue" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Maximum<T>(
        T value,
        T maxValue,
        [CallerArgumentExpression(nameof(value))]
        string paramName = "",
        string? exceptionMessage = null
    )
        where T : IComparable<T>
    {
        if (value.CompareTo(maxValue) <= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{paramName}' cannot exceed {maxValue}."
            : exceptionMessage;

        ThrowHelper.ThrowArgumentOutOfRangeException(paramName, errorMessage);

        return value;
    }

    /// <summary>
    ///     Ensures that the given <paramref name="value" /> does not fall below a specified <paramref name="minValue" />.
    ///     If <paramref name="value" /> is less than <paramref name="minValue" />, an
    ///     <see cref="ArgumentOutOfRangeException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="paramName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    ///     Automatically provided when using <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value falls below the minimum.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The original <paramref name="value" /> if it does not fall below <paramref name="minValue" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="value" /> is less than
    ///     <paramref name="minValue" />.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Minimum<T>(
        T value,
        T minValue,
        [CallerArgumentExpression(nameof(value))]
        string paramName = "",
        string? exceptionMessage = null
    )
        where T : IComparable<T>
    {
        if (value.CompareTo(minValue) >= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{paramName}' cannot be less than {minValue}."
            : exceptionMessage;

        ThrowHelper.ThrowArgumentOutOfRangeException(paramName, errorMessage);

        return value;
    }

    /// <summary>
    ///     Ensures that the given <paramref name="value" /> falls within the specified
    ///     <paramref name="minValue" /> and <paramref name="maxValue" />.
    ///     Also validates that <paramref name="minValue" /> is not greater than <paramref name="maxValue" />.
    ///     If <paramref name="value" /> is outside the specified range, an <see cref="ArgumentOutOfRangeException" /> is
    ///     thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="minValue">The minimum allowable value.</param>
    /// <param name="maxValue">The maximum allowable value.</param>
    /// <param name="paramName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    ///     Automatically provided when using <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value is outside the specified range.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The original <paramref name="value" /> if it falls within the specified range.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="minValue" /> is greater than <paramref name="maxValue" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="value" /> is outside the specified range.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T WithinRange<T>(
        T value,
        T minValue,
        T maxValue,
        [CallerArgumentExpression(nameof(value))]
        string paramName = "",
        string? exceptionMessage = null
    )
        where T : IComparable<T>
    {
        if (minValue.CompareTo(maxValue) > 0)
        {
            ThrowHelper.ThrowArgumentException(
                nameof(minValue),
                "The minimum value specified cannot be greater than the maximum value specified."
            );
        }

        if (value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0)
        {
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{paramName}' must be between '{minValue}' and '{maxValue}'."
            : exceptionMessage;

        ThrowHelper.ThrowArgumentOutOfRangeException(paramName, errorMessage);

        return value;
    }
}