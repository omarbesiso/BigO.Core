using BigO.Core.Extensions;

namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given <paramref name="collection" /> is not <c>null</c>.
    ///     If <paramref name="collection" /> is <c>null</c>, an <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection being checked.</typeparam>
    /// <param name="collection">The collection to be checked for <c>null</c>.</param>
    /// <param name="paramName">
    ///     The name of the parameter being checked, used in the exception message for clarity.
    ///     Automatically populated via <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the collection is <c>null</c>.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     Useful for validating method arguments and ensuring non-null collections.
    ///     This method does not enumerate the collection.
    /// </remarks>
    /// <example>
    ///     <code>
    /// Guard.NotNull(myCollection, nameof(myCollection));
    /// </code>
    /// </example>
    [ContractAnnotation("collection:null => halt; collection:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> NotNull<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? collection,
        [CallerArgumentExpression(nameof(collection))]
        string paramName = "",
        string? exceptionMessage = null)
    {
        if (collection is null)
        {
            ThrowHelper.ThrowArgumentNullException(paramName, exceptionMessage);
        }

        return collection;
    }

    /// <summary>
    ///     Ensures that the given collection is not <c>null</c> and contains at least one element.
    ///     If the collection is <c>null</c> or empty, an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be checked for <c>null</c> or emptiness.</param>
    /// <param name="paramName">
    ///     The name of the parameter being checked, used in the exception message for clarity.
    ///     Automatically populated via <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the collection is <c>null</c> or empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null, non-empty collection.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="collection" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="collection" /> contains no elements.
    /// </exception>
    /// <remarks>
    ///     This method checks for null and then verifies that at least one element exists.
    ///     It short-circuits as soon as it finds one element, so it does not iterate the entire collection.
    /// </remarks>
    /// <example>
    ///     <code>
    /// Guard.NotNullOrEmpty(myCollection, nameof(myCollection));
    /// </code>
    /// </example>
    [ContractAnnotation("collection:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> NotNullOrEmpty<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? collection,
        [CallerArgumentExpression(nameof(collection))]
        string paramName = "",
        string? exceptionMessage = null)
    {
        NotNull(collection, paramName, exceptionMessage);

        if (collection.IsEmpty())
        {
            return collection;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{paramName}' cannot be empty."
            : exceptionMessage;

        ThrowHelper.ThrowArgumentException(paramName, errorMessage);

        return collection;
    }
}