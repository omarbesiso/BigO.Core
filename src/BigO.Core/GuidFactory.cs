using JetBrains.Annotations;

namespace BigO.Core;

/// <summary>
///     Contains utility methods for creating <see cref="Guid" /> instances.
/// </summary>
[PublicAPI]
public static class GuidFactory
{
    private static long _counter = DateTime.UtcNow.Ticks;

    /// <summary>
    ///     Generates a new sequential <see cref="Guid" /> based on the current timestamp and a counter.
    /// </summary>
    /// <returns>A new sequential <see cref="Guid" /> value.</returns>
    /// <remarks>
    ///     This method generates a new <see cref="Guid" /> value that is based on the current timestamp and a counter.
    ///     The first 6 bytes of the generated <see cref="Guid" /> are random, while the remaining 10 bytes are
    ///     derived from the current timestamp and a counter that is incremented on each call to this method. This
    ///     ensures that the generated <see cref="Guid" /> values are unique and sequential, which can be useful for
    ///     certain types of database indexing scenarios.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="NewSequentialGuid" /> method to generate a new sequential
    ///     <see cref="Guid" /> value.
    ///     <code><![CDATA[
    /// var newGuid = NewSequentialGuid();
    /// Console.WriteLine(newGuid);
    /// ]]></code>
    /// </example>
    public static Guid NewSequentialGuid()
    {
        //var counter = Stopwatch.GetTimestamp();
        //var guidBytes = Guid.NewGuid().ToByteArray();
        //var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref counter));

        var counter = Interlocked.Increment(ref _counter);
        var guidBytes = Guid.NewGuid().ToByteArray();
        var counterBytes = BitConverter.GetBytes(counter);

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