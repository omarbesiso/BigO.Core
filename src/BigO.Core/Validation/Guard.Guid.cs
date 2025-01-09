namespace BigO.Core.Validation;

public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given <paramref name="value" /> is not <see cref="Guid.Empty" />.
    ///     If the value is <see cref="Guid.Empty" />, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The GUID to be checked.</param>
    /// <param name="paramName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    ///     Automatically provided via <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the GUID is empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-empty GUID.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is <see cref="Guid.Empty" />.</exception>
    /// <remarks>
    ///     This method is useful for validating GUID arguments to ensure they are not empty.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guid myGuid = Guid.NewGuid();
    ///         Guard.NotEmpty(myGuid, nameof(myGuid));
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid NotEmpty(
        Guid value,
        [CallerArgumentExpression(nameof(value))]
        string paramName = "",
        string? exceptionMessage = null)
    {
        if (value == Guid.Empty)
        {
            // Determine the error message to use
            var errorMessage = string.IsNullOrWhiteSpace(exceptionMessage)
                ? $"The GUID '{paramName}' cannot be empty."
                : exceptionMessage;

            ThrowHelper.ThrowArgumentException(paramName, errorMessage);
        }

        return value;
    }
}