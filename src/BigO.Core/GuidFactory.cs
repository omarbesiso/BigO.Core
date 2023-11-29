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
        var counter = Interlocked.Increment(ref _counter);

        // Handle overflow. Reset to DateTime.UtcNow.Ticks if counter overflows.
        if (counter < 0)
        {
            Interlocked.Exchange(ref _counter, DateTime.UtcNow.Ticks);
            counter = Interlocked.Increment(ref _counter);
        }

        var randomBytes = Guid.NewGuid().ToByteArray();
        var counterBytes = BitConverter.GetBytes(counter);

        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(counterBytes);
        }

        // Copy the counter bytes into the GUID. Avoid overwriting the GUID version and variant bits.
        // Typically, the last 6 bytes of the GUID can be safely modified.
        randomBytes[10] = counterBytes[1];
        randomBytes[11] = counterBytes[0];
        randomBytes[12] = counterBytes[7];
        randomBytes[13] = counterBytes[6];
        randomBytes[14] = counterBytes[5];
        randomBytes[15] = counterBytes[4];

        return new Guid(randomBytes);
    }
}