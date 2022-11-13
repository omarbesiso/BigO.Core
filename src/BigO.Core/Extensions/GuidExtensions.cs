using JetBrains.Annotations;

namespace BigO.Core.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for working with <see cref="Guid" /> objects.
/// </summary>
[PublicAPI]
public static class GuidExtensions
{
    /// <summary>Indicates if <see cref="Guid" /> value value is empty.</summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns><c>true</c> if the <paramref name="value" /> is empty, otherwise <c>false</c>.</returns>
    public static bool IsEmpty(this Guid value)
    {
        return value == Guid.Empty;
    }

    /// <summary>Indicates if <see cref="Guid" /> value value is not empty.</summary>
    /// <param name="value">The value to be checked.</param>
    /// <returns><c>true</c> if the <paramref name="value" /> is not empty, otherwise <c>false</c>.</returns>
    public static bool IsNotEmpty(this Guid value)
    {
        return value != Guid.Empty;
    }
}