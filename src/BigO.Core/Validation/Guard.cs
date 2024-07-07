using System.Diagnostics;

namespace BigO.Core.Validation;

/// <summary>
///     Class with validation utilities to be used in code contract fashion for validating method arguments.
/// </summary>
[PublicAPI]
[DebuggerStepThrough]
public static partial class Guard
{
    /// <summary>
    ///     Ensures that the given value is not <c>null</c>. If the value is <c>null</c>, an
    ///     <see cref="ArgumentNullException" /> is thrown.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <param name="value">The value to be checked for <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, used in the exception message for clarity.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Custom exception message if the value is <c>null</c>. If not provided, a default message is used.
    /// </param>
    /// <returns>The non-null value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     Useful in validating method arguments and ensuring non-null variables, this method simplifies null checks
    ///     and enhances code readability and robustness.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         Guard.NotNull(myVariable, nameof(myVariable));
    ///     </code>
    /// </example>
    [ContractAnnotation("value:null => halt; value:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] T? value,
        [CallerArgumentExpression(nameof(value))]
        string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNullException(argumentName, exceptionMessage);
        }

        return value;
    }
}