using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Guid" /> objects.
/// </summary>
[PublicAPI]
public static class GuidExtensions
{
    /// <summary>
    /// Determines whether the specified <see cref="Guid"/> is empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="Guid"/> is empty; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///   A <see cref="Guid"/> is considered empty if it has a value of <c>Guid.Empty</c>.
    /// </remarks>
    public static bool IsEmpty(this Guid value)
    {
        return value == Guid.Empty;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Guid"/> is not empty.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="Guid"/> is not empty; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///   A <see cref="Guid"/> is considered not empty if it has a value other than <c>Guid.Empty</c>.
    /// </remarks>
    public static bool IsNotEmpty(this Guid value)
    {
        return value != Guid.Empty;
    }
}