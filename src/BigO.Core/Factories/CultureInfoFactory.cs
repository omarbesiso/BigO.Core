using System.Globalization;
using BigO.Core.Validation;

namespace BigO.Core.Factories;

internal static class CultureInfoFactory
{
    /// <summary>
    ///     Creates a <see cref="CultureInfo" /> object using the specified culture name.
    ///     If the culture name is <c>null</c>, empty, or consists only of white-space characters, an
    ///     <see cref="ArgumentNullException" /> or <see cref="ArgumentException" /> is thrown.
    ///     If the specified culture is not found, a <see cref="CultureNotFoundException" /> is thrown.
    /// </summary>
    /// <param name="cultureName">The name of the culture to create.</param>
    /// <returns>A <see cref="CultureInfo" /> object representing the specified culture.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="cultureName" /> is <c>null</c>.</exception>
    /// <exception cref="CultureNotFoundException">Thrown if the specified culture is not found.</exception>
    /// <remarks>
    ///     This method is useful for creating a <see cref="CultureInfo" /> object from a culture name string,
    ///     ensuring that the culture name is valid and the culture exists.
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

        CultureInfo culture;
        try
        {
            culture = new CultureInfo(cultureName);
        }
        catch (CultureNotFoundException ex)
        {
            throw new CultureNotFoundException($"The culture specified by '{cultureName}' is not found.", ex);
        }

        return culture;
    }
}