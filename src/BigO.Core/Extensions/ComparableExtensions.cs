using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IComparable{T}" /> objects.
/// </summary>
[PublicAPI]
public static class ComparableExtensions
{
    /// <summary>
    ///     Determines whether the value is between the lower and upper boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value and the boundaries. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to check. Can be <c>null</c>.</param>
    /// <param name="lowerBoundary">The lower boundary. Cannot be <c>null</c>.</param>
    /// <param name="upperBoundary">The upper boundary. Cannot be <c>null</c>.</param>
    /// <param name="isBoundaryInclusive">Whether or not to include the boundaries in the comparison. Default is true.</param>
    /// <returns>True if the value is between the lower and upper boundaries, false if not.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="lowerBoundary" /> is <c>null</c>.
    ///     Thrown if <paramref name="upperBoundary" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method determines whether the value is between the lower and upper boundaries.
    ///     If the value is <c>null</c>, it returns false.
    ///     If the value is not <c>null</c>, it compares the value to the lower and upper boundaries using the
    ///     <see cref="IComparable{T}.CompareTo" /> method.
    ///     If <paramref name="isBoundaryInclusive" /> is true, it returns true if the value is greater than or equal to the
    ///     lower boundary and less than or equal to the upper boundary.
    ///     If <paramref name="isBoundaryInclusive" /> is false, it returns true if the value is greater than the lower
    ///     boundary and less than the upper boundary.
    ///     If the value is not between the boundaries, it returns false.
    /// </remarks>
    public static bool IsBetween<T>(this T? value, T lowerBoundary, T upperBoundary, bool isBoundaryInclusive = true)
        where T : IComparable<T>
    {
        if (lowerBoundary == null)
        {
            throw new ArgumentNullException(nameof(lowerBoundary), $"The {nameof(lowerBoundary)} cannot be null.");
        }

        if (upperBoundary == null)
        {
            throw new ArgumentNullException(nameof(upperBoundary), $"The {nameof(upperBoundary)} cannot be null.");
        }

        if (value == null)
        {
            return false;
        }

        if (isBoundaryInclusive)
        {
            return value.CompareTo(lowerBoundary) >= 0 && value.CompareTo(upperBoundary) <= 0;
        }

        return value.CompareTo(lowerBoundary) > 0 && value.CompareTo(upperBoundary) < 0;
    }

    /// <summary>
    ///     Limits the value to the maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value and the maximum. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to check. Cannot be <c>null</c>.</param>
    /// <param name="maximum">The maximum value. Cannot be <c>null</c>.</param>
    /// <returns>The value if it is less than or equal to the maximum, the maximum if the value is greater than the maximum.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="value" /> is <c>null</c>.
    ///     Thrown if <paramref name="maximum" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method limits the value to the maximum.
    ///     It compares the value to the maximum using the <see cref="IComparable{T}.CompareTo" /> method.
    ///     If the value is less than or equal to the maximum, it returns the value.
    ///     If the value is greater than the maximum, it returns the maximum.
    /// </remarks>
    public static T Limit<T>(this T value, T maximum) where T : IComparable<T>
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), $"The {nameof(value)} cannot be null.");
        }

        if (maximum == null)
        {
            throw new ArgumentNullException(nameof(maximum), $"The {nameof(maximum)} cannot be null.");
        }

        return value.CompareTo(maximum) < 1 ? value : maximum;
    }

    /// <summary>
    ///     Limits the value to the minimum and maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value, minimum, and maximum. Must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The value to check. Cannot be <c>null</c>.</param>
    /// <param name="minimum">The minimum value. Cannot be <c>null</c>.</param>
    /// <param name="maximum">The maximum value. Cannot be <c>null</c>.</param>
    /// <returns>
    ///     The value if it is within the minimum and maximum, the minimum if the value is less than the minimum, the
    ///     maximum if the value is greater than the maximum.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="value" /> is <c>null</c>.
    ///     Thrown if <paramref name="minimum" /> is <c>null</c>.
    ///     Thrown if <paramref name="maximum" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method limits the value to the minimum and maximum.
    ///     It compares the value to the minimum and maximum using the <see cref="IComparable.CompareTo" /> method.
    ///     If the value is within the minimum and maximum, it returns the value.
    ///     If the value is less than the minimum, it returns the minimum.
    ///     If the value is greater than the maximum, it returns the maximum.
    /// </remarks>
    public static T Limit<T>(this T value, T minimum, T maximum) where T : IComparable
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), $"The {nameof(value)} cannot be null.");
        }

        if (maximum == null)
        {
            throw new ArgumentNullException(nameof(maximum), $"The {nameof(maximum)} cannot be null.");
        }

        if (minimum == null)
        {
            throw new ArgumentNullException(nameof(minimum), $"The {nameof(minimum)} cannot be null.");
        }

        if (value.CompareTo(minimum) < 0)
        {
            return minimum;
        }

        return value.CompareTo(maximum) > 0 ? maximum : value;
    }
}