namespace BigO.Core.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="Guid" /> objects.
/// </summary>
[PublicAPI]
public static class GuidExtensions
{
    /// <summary>
    ///     Determines if the specified <see cref="Guid" /> value is empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid" /> value to check for emptiness.</param>
    /// <returns><c>true</c> if the <see cref="Guid" /> value is empty; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// Guid emptyGuid = Guid.Empty;
    /// bool isEmpty = emptyGuid.IsEmpty();
    /// 
    /// // isEmpty is true.
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method is useful for quickly checking if a given <see cref="Guid" /> value is equal to
    ///     <see cref="Guid.Empty" />. The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option, allowing the JIT compiler to inline the method's body
    ///     at the call site for improved performance.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Guid value)
    {
        return value == Guid.Empty;
    }

    /// <summary>
    ///     Determines if the specified <see cref="Guid" /> value is not empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid" /> value to check for non-emptiness.</param>
    /// <returns><c>true</c> if the <see cref="Guid" /> value is not empty; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// Guid nonEmptyGuid = Guid.NewGuid();
    /// bool isNotEmpty = nonEmptyGuid.IsNotEmpty();
    /// 
    /// // isNotEmpty is true.
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     This extension method is useful for quickly checking if a given <see cref="Guid" /> value is not equal to
    ///     <see cref="Guid.Empty" />. The method is marked with the <see cref="MethodImplAttribute" /> and the
    ///     <see cref="MethodImplOptions.AggressiveInlining" /> option, allowing the JIT compiler to inline the method's body
    ///     at the call site for improved performance.
    /// </remarks>
    [System.Diagnostics.Contracts.Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty(this Guid value)
    {
        return value != Guid.Empty;
    }
}