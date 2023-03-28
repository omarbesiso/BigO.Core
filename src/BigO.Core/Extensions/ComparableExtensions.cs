using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IComparable{T}" /> objects.
/// </summary>
[PublicAPI]
public static class ComparableExtensions
{
    /// <summary>
    ///     Determines whether the value is between the specified boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value and boundaries. It must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to check if it is between the boundaries.</param>
    /// <param name="lowerBoundary">The lower boundary.</param>
    /// <param name="upperBoundary">The upper boundary.</param>
    /// <param name="isBoundaryInclusive">Whether the boundaries are inclusive or exclusive. Default is true.</param>
    /// <returns>True if the value is between the boundaries, otherwise false.</returns>
    /// <remarks>
    ///     This method will check if the <paramref name="value" /> is between the specified <paramref name="lowerBoundary" />
    ///     and <paramref name="upperBoundary" />.
    ///     If the <paramref name="lowerBoundary" /> or <paramref name="upperBoundary" /> is null, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    ///     If the <paramref name="value" /> is null, this method will always return false.
    ///     If the <paramref name="isBoundaryInclusive" /> is true, then the comparison is inclusive of the boundaries (greater
    ///     than or equal to the lower boundary and less than or equal to the upper boundary).
    ///     If the <paramref name="isBoundaryInclusive" /> is false, then the comparison is exclusive of the boundaries
    ///     (greater than the lower boundary and less than the upper boundary).
    /// </remarks>
    /// <example>
    ///     The following code example checks if an integer is between two other integers:
    ///     <code><![CDATA[
    /// int value = 5;
    /// int lowerBoundary = 1;
    /// int upperBoundary = 10;
    /// bool isBetween = value.IsBetween(lowerBoundary, upperBoundary);
    /// Console.WriteLine(isBetween); // True
    /// isBetween = value.IsBetween(lowerBoundary, upperBoundary, isBoundaryInclusive: false);
    /// Console.WriteLine(isBetween); // False
    /// ]]></code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="lowerBoundary" /> or <paramref name="upperBoundary" /> is null.
    /// </exception>
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
    ///     Limits a value to a specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value and maximum, which must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to be limited.</param>
    /// <param name="maximum">The maximum limit for the value.</param>
    /// <returns>The original value if it's less than or equal to the maximum, otherwise the maximum value.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="value" /> or <paramref name="maximum" /> is
    ///     <c>null</c>.
    /// </exception>
    /// <example>
    ///     <code><![CDATA[
    /// int value = 42;
    /// int max = 50;
    /// int limitedValue = value.Limit(max); // limitedValue will be 42
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method can be used with any type that implements <see cref="IComparable{T}" />. It compares the
    ///     value and maximum using the <see cref="IComparable{T}.CompareTo(T)" /> method.
    ///     If either <paramref name="value" /> or <paramref name="maximum" /> is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> will be thrown.
    ///     When the value is less than or equal to the maximum, the method returns the original value, otherwise it returns
    ///     the maximum.
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
    ///     Limits a value between a specified minimum and maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value, minimum, and maximum, which must implement <see cref="IComparable" />.</typeparam>
    /// <param name="value">The value to be limited.</param>
    /// <param name="minimum">The minimum limit for the value.</param>
    /// <param name="maximum">The maximum limit for the value.</param>
    /// <returns>The limited value between the minimum and maximum, inclusive.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="value" />, <paramref name="minimum" />, or
    ///     <paramref name="maximum" /> is <c>null</c>.
    /// </exception>
    /// <example>
    ///     <code><![CDATA[
    /// int value = 42;
    /// int min = 30;
    /// int max = 50;
    /// int limitedValue = value.Limit(min, max); // limitedValue will be 42
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method can be used with any type that implements <see cref="IComparable" />. It compares the value,
    ///     minimum, and maximum using the <see cref="IComparable.CompareTo(object)" /> method.
    ///     If either <paramref name="value" />, <paramref name="minimum" />, or <paramref name="maximum" /> is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> will be thrown.
    ///     When the value is less than the minimum, the method returns the minimum; when the value is greater than the
    ///     maximum, it returns the maximum; otherwise, it returns the original value.
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