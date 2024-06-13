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
    ///     The first 10 bytes of the generated <see cref="Guid" /> are random, while the remaining 6 bytes are
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

        // Handle overflow without locking.
        if (counter < 0)
        {
            Interlocked.CompareExchange(ref _counter, DateTime.UtcNow.Ticks, counter);
            counter = Interlocked.Increment(ref _counter);
        }

        var randomBytes = Guid.NewGuid().ToByteArray();
        var counterBytes = BitConverter.GetBytes(counter);

        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(counterBytes);
        }

        // Copy the counter bytes into the last 6 bytes of the GUID to maintain uniqueness
        Array.Copy(counterBytes, 0, randomBytes, 10, 6);

        return new Guid(randomBytes);
    }
}