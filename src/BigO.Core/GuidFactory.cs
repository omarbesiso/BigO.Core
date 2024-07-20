using BigO.Core.Validation;

namespace BigO.Core;

/// <summary>
///     Contains utility methods for creating <see cref="Guid" /> instances.
/// </summary>
[PublicAPI]
public static class GuidFactory
{
    /// <summary>
    ///     Generates a new sequential <see cref="Guid" /> based on Ulid.
    /// </summary>
    /// <returns>A new sequential <see cref="Guid" /> value.</returns>
    /// <remarks>
    ///     This method generates a new <see cref="Guid" /> value that is based on Ulid (Universally Unique Lexicographically
    ///     Sortable Identifier).
    ///     The generated <see cref="Guid" /> values are unique, sequential, and sortable, which is useful for
    ///     database indexing scenarios. The Guid is derived from an Ulid, ensuring better distribution and sortability
    ///     compared to the previous implementation.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="NewSequentialGuid" /> method to generate a new sequential
    ///     <see cref="Guid" /> value.
    ///     <code><![CDATA[
    /// var newGuid = GuidFactory.NewSequentialGuid();
    /// Console.WriteLine(newGuid);
    /// ]]></code>
    /// </example>
    public static Guid NewSequentialGuid()
    {
        return Ulid.NewUlid().ToGuid();
    }

    /// <summary>
    ///     Generates an array of new sequential <see cref="Guid" /> values based on Ulid.
    /// </summary>
    /// <param name="count">The number of sequential <see cref="Guid" /> values to generate.</param>
    /// <returns>An array of new sequential <see cref="Guid" /> values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than or equal to zero.</exception>
    /// <remarks>
    ///     This method generates an array of new <see cref="Guid" /> values, each based on a unique Ulid.
    ///     The generated <see cref="Guid" /> values are unique, sequential, and sortable, which is useful for
    ///     database indexing scenarios.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="NewSequentialGuids(int)" /> method to generate an array
    ///     of new sequential <see cref="Guid" /> values.
    ///     <code><![CDATA[
    /// var newGuids = GuidFactory.NewSequentialGuids(5);
    /// foreach (var guid in newGuids)
    /// {
    ///     Console.WriteLine(guid);
    /// }
    /// ]]></code>
    /// </example>
    public static Guid[] NewSequentialGuids(int count)
    {
        Guard.Minimum(count, 1);
        var guids = new Guid[count];
        for (var i = 0; i < count; i++)
        {
            guids[i] = NewSequentialGuid();
        }

        return guids;
    }
}