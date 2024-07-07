namespace BigO.Core.Validation;

public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given collection is not <c>null</c>. If the collection is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection being checked.</typeparam>
    /// <param name="collection">The collection to be checked for <c>null</c>.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the collection is <c>null</c>. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the collection is <c>null</c>.</exception>
    /// <remarks>
    ///     Useful in validating property values and ensuring non-null collections, this method simplifies null checks
    ///     and enhances code readability and robustness. No enumeration of the collection is performed.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.NotNull(myCollection, nameof(myCollection));
    ///     </code>
    /// </example>
    [ContractAnnotation("collection:null => halt; collection:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> NotNull<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? collection,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotNull(collection, propertyName, exceptionMessage);
    }

    /// <summary>
    ///     Ensures that the given enumerable is not <c>null</c> and contains at least one element.
    ///     If the enumerable is <c>null</c> or empty, an exception is thrown.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="value">The enumerable to be checked for <c>null</c> or emptiness.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the enumerable is <c>null</c> or empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null and non-empty enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the enumerable is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if the enumerable is empty.</exception>
    /// <remarks>
    ///     This method is useful for validating enumerable property values to ensure they are neither <c>null</c> nor empty,
    ///     thus avoiding common errors related to collection handling. It first checks if the enumerable is null and
    ///     then checks if it contains any elements without enumerating the entire collection.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.NotNullOrEmpty(myEnumerable, nameof(myEnumerable));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> NotNullOrEmpty<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotNullOrEmpty(value, propertyName, exceptionMessage);
    }
}