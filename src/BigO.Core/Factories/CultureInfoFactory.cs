using System.Globalization;
using BigO.Core.Validation;

namespace BigO.Core.Factories;

internal static class CultureInfoFactory
{
    /// <summary>
    ///     Creates a <see cref="CultureInfo" /> object using the specified culture name.
    ///     If the culture name is <c>null</c>, empty, or consists only of white-space characters,
    ///     an <see cref="ArgumentNullException" /> or <see cref="ArgumentException" /> is thrown.
    ///     If the specified culture is not found, a <see cref="CultureNotFoundException" /> is thrown.
    /// </summary>
    /// <param name="cultureName">
    ///     The name of the culture to create. It must be a valid culture name string that is neither null nor empty.
    /// </param>
    /// <returns>
    ///     A <see cref="CultureInfo" /> object representing the specified culture.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="cultureName" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="cultureName" /> is empty or contains only white-space characters.
    /// </exception>
    /// <exception cref="CultureNotFoundException">
    ///     Thrown if the specified culture is not found.
    /// </exception>
    /// <remarks>
    ///     This method is useful for creating a <see cref="CultureInfo" /> object from a culture name string.
    ///     It validates that the culture name is not null, empty, or white-space, and ensures that the culture exists.
    /// </remarks>
    /// <example>
    ///     <code>
    ///         var culture = CultureInfoFactory.Create("en-US");
    ///     </code>
    /// </example>
    [ContractAnnotation("cultureName:null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CultureInfo Create(string cultureName)
    {
        Guard.NotNull(cultureName);

        if (string.IsNullOrWhiteSpace(cultureName))
        {
            throw new ArgumentException("Culture name cannot be empty or consist only of white-space characters.",
                nameof(cultureName));
        }

        try
        {
            return new CultureInfo(cultureName);
        }
        catch (CultureNotFoundException ex)
        {
            throw new CultureNotFoundException($"The culture specified by '{cultureName}' is not found.", ex);
        }
    }
}