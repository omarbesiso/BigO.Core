namespace BigO.Core.Validation;

public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property Guid is not empty.
    ///     If the Guid is empty, an <see cref="ArgumentException" /> is thrown.
    /// </summary>
    /// <param name="value">The Guid to be checked.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the Guid is empty.
    ///     If not provided, a default message is used.
    /// </param>
    /// <returns>The non-empty Guid.</returns>
    /// <exception cref="ArgumentException">Thrown if the Guid is empty.</exception>
    /// <remarks>
    ///     This method is useful for validating Guid properties to ensure they are not empty.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guid myGuid = Guid.NewGuid();
    ///         PropertyGuard.NotEmpty(myGuid, nameof(myGuid));
    ///     </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid NotEmpty(Guid value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotEmpty(value, propertyName, exceptionMessage);
    }
}