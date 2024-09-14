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
    ///     Ensures that the given <paramref name="value"/> is not <c>null</c>. If the value is <c>null</c>, an
    ///     <see cref="ArgumentNullException"/> is thrown with an optional custom exception message.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked. Must be a reference type or nullable type.</typeparam>
    /// <param name="value">The value to check for <c>null</c>.</param>
    /// <param name="argumentName">
    ///     The name of the argument being checked, automatically populated via 
    ///     <see cref="CallerArgumentExpressionAttribute"/> for better error reporting.
    /// </param>
    /// <param name="exceptionMessage">
    ///     Optional. Custom exception message to provide more context. If not specified, a default message will be used.
    /// </param>
    /// <returns>The validated <paramref name="value"/>, guaranteed to be non-null.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="value"/> is <c>null</c>, with the provided <paramref name="argumentName"/> and 
    ///     <paramref name="exceptionMessage"/> in the exception message.
    /// </exception>
    /// <remarks>
    ///     This method simplifies null checks in methods and improves code readability and robustness. It is 
    ///     particularly useful for validating arguments passed to methods and constructors.
    /// </remarks>
    /// <example>
    ///     <code><![CDATA[
    ///     // Throws if myVariable is null
    ///     Guard.NotNull(myVariable, nameof(myVariable));
    ///     ]]></code>
    /// </example>
    [ContractAnnotation("value:null => halt; value:notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] T? value,
        [CallerArgumentExpression(nameof(value))] string argumentName = "",
        string? exceptionMessage = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNullException(argumentName, exceptionMessage);
        }

        return value;
    }
}