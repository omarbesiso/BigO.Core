using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IComparable{T}" /> objects.
/// </summary>
[PublicAPI]
public static class ComparableExtensions
{
    /// <summary>
    ///     Determines whether the given nullable value is between the specified lower and upper boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value to compare. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The nullable value to compare.</param>
    /// <param name="lowerBoundary">The lower boundary to compare against.</param>
    /// <param name="upperBoundary">The upper boundary to compare against.</param>
    /// <returns>True if the value is between the lower and upper boundaries (inclusive), false otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the value is <c>null</c> and the type <typeparamref name="T" /> is a
    ///     reference type.
    /// </exception>
    /// <remarks>
    ///     If the value is <c>null</c>, the method will return false.
    /// </remarks>
    public static bool IsBetween<T>(this T? value, T lowerBoundary, T upperBoundary) where T : IComparable<T>
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (value == null)
        {
            return false;
        }

        return value.CompareTo(lowerBoundary) >= 0 && value.CompareTo(upperBoundary) <= 0;
    }

    /// <summary>
    ///     Limits the given value to the specified maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the value to compare. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to compare and potentially limit.</param>
    /// <param name="maximum">The maximum value to limit to.</param>
    /// <returns>The given value if it is less than the maximum value, the maximum value otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the value is <c>null</c> and the type <typeparamref name="T" /> is a
    ///     reference type.
    /// </exception>
    /// <remarks>
    ///     This method can be used to ensure that a value does not exceed a certain limit.
    /// </remarks>
    public static T Limit<T>(this T value, T maximum) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.CompareTo(maximum) < 1 ? value : maximum;
    }

    /// <summary>
    ///     Limits the given value to the specified minimum and maximum values.
    /// </summary>
    /// <typeparam name="T">The type of the value to compare. Must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The value to compare and potentially limit.</param>
    /// <param name="minimum">The minimum value to limit to.</param>
    /// <param name="maximum">The maximum value to limit to.</param>
    /// <returns>
    ///     The given value if it is between the minimum and maximum values (inclusive), the minimum value if it is less
    ///     than the minimum, or the maximum value if it is greater than the maximum.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the value is <c>null</c> and the type <typeparamref name="T" /> is a
    ///     reference type.
    /// </exception>
    /// <remarks>
    ///     This method can be used to ensure that a value stays within a certain range.
    /// </remarks>
    public static T Limit<T>(this T value, T minimum, T maximum) where T : IComparable
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.CompareTo(minimum) < 0)
        {
            return minimum;
        }

        return value.CompareTo(maximum) > 0 ? maximum : value;
    }
}