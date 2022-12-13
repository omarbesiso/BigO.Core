using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="IComparable" /> objects.
/// </summary>
[PublicAPI]
public static class ComparableExtensions
{
    /// <summary>
    ///     Determines if the specified <paramref name="value" /> is equal to one of the specified boundaries or exists in the
    ///     range provided.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object to compare. This type parameter is contravariant.That is, you can use either the
    ///     type you specified or any type that is less derived.
    /// </typeparam>
    /// <param name="value">The <paramref name="value" /> to be checked against the range.</param>
    /// <param name="lowerBoundary">The <paramref name="lowerBoundary" /> boundary of the range.</param>
    /// <param name="upperBoundary">The <paramref name="upperBoundary" /> boundary of the range.</param>
    /// <returns><c>true</c> if the <paramref name="value" /> value falls within the specified range, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value" /> is <c>null</c>.</exception>
    public static bool IsBetween<T>(this T value, T lowerBoundary, T upperBoundary) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.CompareTo(lowerBoundary) >= 0 && value.CompareTo(upperBoundary) <= 0;
    }

    /// <summary>
    ///     Limits the <paramref name="value" /> to a  specified <paramref name="maximum" /> value.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object to compare. This type parameter is contravariant.That is, you can use either the
    ///     type you specified or any type that is less derived.
    /// </typeparam>
    /// <param name="value">The value to be limited.</param>
    /// <param name="maximum">The maximum limit of the value.</param>
    /// <remarks>
    ///     This is useful in instances such as feeding a progress bar with values from a source
    ///     which eventually might exceed an expected maximum.
    /// </remarks>
    /// <returns>
    ///     If the <paramref name="value" /> does not exceed the limit then the value will be returned. Otherwise, the
    ///     limit will be returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value" /> is <c>null</c>.</exception>
    public static T Limit<T>(this T value, T maximum) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.CompareTo(maximum) < 1 ? value : maximum;
    }

    /// <summary>
    ///     Limits the <paramref name="value" /> to a specified <paramref name="minimum" /> and <paramref name="maximum" />
    ///     values.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of object to compare. This type parameter is contravariant.That is, you can use either the
    ///     type you specified or any type that is less derived.
    /// </typeparam>
    /// <param name="value">The value to be limited.</param>
    /// <param name="minimum">The minimum limit of the value.</param>
    /// <param name="maximum">The maximum limit of the value.</param>
    /// <returns>The value, truncated by a minimum or maximum boundary.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value" /> is <c>null</c>.</exception>
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