namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given enumerable is not <c>null</c> and contains at least one element.
    ///     If the enumerable is <c>null</c> or empty, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="value">The enumerable to be checked for <c>null</c> or emptiness.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the enumerable is <c>null</c> or empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null and non-empty enumerable.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is <c>null</c> or empty.</exception>
    /// <remarks>
    ///     This method is useful for validating enumerable arguments to ensure they are neither <c>null</c> nor empty,
    ///     thus avoiding common errors related to collection handling.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotNullOrEmpty(myEnumerable, nameof(myEnumerable));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt")]
    public static IEnumerable<T> NotNullOrEmpty<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        if (value != null && value.Any())
        {
            // ReSharper disable once PossibleMultipleEnumeration
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{argumentName}' cannot be null or empty."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, argumentName);
    }
}