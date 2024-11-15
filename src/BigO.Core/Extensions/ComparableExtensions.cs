﻿using BigO.Core.Validation;

namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="IComparable{T}" /> objects.
/// </summary>
[PublicAPI]
public static class ComparableExtensions
{
    /// <summary>
    ///     Determines whether the specified value falls within a certain range.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="lowerBoundary">The lower boundary of the range.</param>
    /// <param name="upperBoundary">The upper boundary of the range.</param>
    /// <param name="isBoundaryInclusive">
    ///     Specifies whether the boundaries are inclusive. If <c>true</c>, the method checks if the value is greater than or
    ///     equal to the lower boundary
    ///     and less than or equal to the upper boundary; otherwise, it checks if the value is strictly greater than the lower
    ///     boundary and strictly less than the upper boundary.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the value is within the range; <c>false</c> otherwise or if the value is <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either the lower boundary or the upper boundary is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method provides a generic way to check if a value of any comparable type falls within a specific range.
    ///     It can be used with numeric types, dates, and other comparable types.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int? number = 5;
    ///     bool result = number.IsBetween(1, 10);
    ///     // result is true
    ///
    ///     result = number.IsBetween(5, 5, isBoundaryInclusive: false);
    ///     // result is false
    ///     ]]></code>
    /// </example>
    public static bool IsBetween<T>(this T? value, T lowerBoundary, T upperBoundary, bool isBoundaryInclusive = true)
        where T : IComparable<T>
    {
        Guard.NotNull(lowerBoundary, nameof(lowerBoundary));
        Guard.NotNull(upperBoundary, nameof(upperBoundary));

        if (value is null)
        {
            return false;
        }

        var comparer = Comparer<T>.Default;

        return isBoundaryInclusive
            ? comparer.Compare(value, lowerBoundary) >= 0 && comparer.Compare(value, upperBoundary) <= 0
            : comparer.Compare(value, lowerBoundary) > 0 && comparer.Compare(value, upperBoundary) < 0;
    }

    /// <summary>
    ///     Limits a value to a specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to limit.</param>
    /// <param name="maximum">The maximum allowable value.</param>
    /// <returns>
    ///     The original value if it is less than or equal to the maximum; otherwise, the maximum value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if either the value or the maximum is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method compares the provided value with the maximum and returns the lesser of the two.
    ///     It's useful for enforcing upper limits on variable values, such as sizes, quantities, or other measurable
    ///     quantities.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int number = 10;
    ///     int maxLimit = 8;
    ///     int limitedNumber = number.Limit(maxLimit);
    ///     // limitedNumber is 8
    /// 
    ///     number = 6;
    ///     limitedNumber = number.Limit(maxLimit);
    ///     // limitedNumber is 6
    ///     ]]></code>
    /// </example>
    public static T Limit<T>(this T value, T maximum) where T : IComparable<T>
    {
        Guard.NotNull(value, nameof(value));
        Guard.NotNull(maximum, nameof(maximum));

        var comparer = Comparer<T>.Default;

        return comparer.Compare(value, maximum) <= 0 ? value : maximum;
    }

    /// <summary>
    ///     Limits a value to be within a specified range defined by a minimum and maximum.
    /// </summary>
    /// <typeparam name="T">The type of the value being limited. Must implement <see cref="IComparable{T}" />.</typeparam>
    /// <param name="value">The value to be limited.</param>
    /// <param name="minimum">The minimum allowable value.</param>
    /// <param name="maximum">The maximum allowable value.</param>
    /// <returns>
    ///     The minimum value if the original value is less than the minimum,
    ///     the maximum value if the original value is greater than the maximum,
    ///     or the original value if it is within the range.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the value, minimum, or maximum is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method checks whether the provided value falls within the specified range and adjusts it if necessary.
    ///     It is useful for ensuring that a variable remains within certain bounds, such as within a valid range of dates,
    ///     numbers, or other measurable quantities.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     int number = 15;
    ///     int minLimit = 10;
    ///     int maxLimit = 20;
    ///     int limitedNumber = number.Limit(minLimit, maxLimit);
    ///     // limitedNumber is 15
    ///
    ///     number = 25;
    ///     limitedNumber = number.Limit(minLimit, maxLimit);
    ///     // limitedNumber is 20
    ///     ]]></code>
    /// </example>
    public static T Limit<T>(this T value, T minimum, T maximum) where T : IComparable<T>
    {
        Guard.NotNull(value, nameof(value));
        Guard.NotNull(minimum, nameof(minimum));
        Guard.NotNull(maximum, nameof(maximum));

        var comparer = Comparer<T>.Default;

        if (comparer.Compare(value, minimum) < 0)
        {
            return minimum;
        }

        if (comparer.Compare(value, maximum) > 0)
        {
            return maximum;
        }

        return value;
    }
}