using JetBrains.Annotations;

namespace BigO.Core;

/// <summary>
///     Contains utility methods for creating <see cref="Guid" /> instances.
/// </summary>
[PublicAPI]
public static class GuidFactory
{
    /// <summary>
    ///     <para>
    ///         Generates sequential <see cref="Guid" /> values optimized for use in Microsoft SQL server clustered
    ///         keys or indexes, yielding better performance than random values.
    ///     </para>
    ///     <para>
    ///         Although this generator achieves the same goals as SQL Server's NEWSEQUENTIALID, the algorithm used
    ///         to generate the GUIDs is different.
    ///     </para>
    ///     <para>
    ///         See <see href="https://docs.microsoft.com/sql/t-sql/functions/newsequentialid-transact-sql" />.
    ///     </para>
    /// </summary>
    /// <example>
    ///     <code>Guid sequentialGuid = GuidFactory.NewSequentialGuid();</code>
    /// </example>
    public static Guid NewSequentialGuid()
    {
        var counter = DateTime.UtcNow.Ticks;
        var guidBytes = Guid.NewGuid().ToByteArray();
        var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref counter));

        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(counterBytes);
        }

        guidBytes[08] = counterBytes[1];
        guidBytes[09] = counterBytes[0];
        guidBytes[10] = counterBytes[7];
        guidBytes[11] = counterBytes[6];
        guidBytes[12] = counterBytes[5];
        guidBytes[13] = counterBytes[4];
        guidBytes[14] = counterBytes[3];
        guidBytes[15] = counterBytes[2];

        return new Guid(guidBytes);
    }
}