namespace BigO.Core.Validation;

public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given enumerable property is not <c>null</c> and contains at least one element.
    ///     If the enumerable is <c>null</c> or empty, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="value">The enumerable property to be checked for <c>null</c> or emptiness.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the enumerable is <c>null</c> or empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null and non-empty enumerable property.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is <c>null</c> or empty.</exception>
    /// <remarks>
    ///     This method is useful for validating enumerable properties to ensure they are neither <c>null</c> nor empty,
    ///     thus avoiding common errors related to collection handling.
    /// </remarks>
    [ContractAnnotation("value:null => halt")]
    public static IEnumerable<T> NotNullOrEmpty<T>(
        [System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration]
        IEnumerable<T>? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        if (value != null && value.Any())
        {
            // ReSharper disable once PossibleMultipleEnumeration
            return value;
        }

        var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
            ? $"The value of '{propertyName}' cannot be null or empty."
            : exceptionMessage;

        throw new ArgumentException(errorMessage, propertyName);
    }
}