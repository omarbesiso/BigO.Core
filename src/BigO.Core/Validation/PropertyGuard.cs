using System.Diagnostics;

namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating property values.
/// </summary>
[PublicAPI]
[DebuggerStepThrough]
public static partial class PropertyGuard
{
    /// <summary>
    ///     Ensures that the given property value is not <c>null</c>. If the value is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the property value being checked.</typeparam>
    /// <param name="value">The property value to be checked for <c>null</c>.</param>
    /// <param name="propertyName">
    ///     The name of the property being checked, used in the exception message for clarity.
    ///     This is automatically captured from the caller member name.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the property value is <c>null</c>. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null property value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the property value is <c>null</c>.</exception>
    /// <remarks>
    ///     Useful in validating property values and ensuring non-null variables, this method simplifies null checks
    ///     and enhances code readability and robustness.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         PropertyGuard.NotNull(myProperty, nameof(myProperty));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt; value:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] T? value,
        [CallerMemberName] string propertyName = "",
        string? exceptionMessage = null)
    {
        return Guard.NotNull(value, propertyName, exceptionMessage);
    }
}