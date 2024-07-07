using BigO.Core.Validation;

namespace BigO.Core;

/// <summary>
///     Contains utility methods for creating <see cref="Guid" /> instances.
/// </summary>
[PublicAPI]
public static class GuidFactory
{
    private static long _counter;

    static GuidFactory()
    {
        _counter = DateTimeOffset.UtcNow.Ticks;
    }

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
    /// var newGuid = GuidFactory.NewSequentialGuid();
    /// Console.WriteLine(newGuid);
    /// ]]></code>
    /// </example>
    public static Guid NewSequentialGuid()
    {
        // Increment the counter in a thread-safe manner
        var counter = Interlocked.Increment(ref _counter);

        // Handle overflow by resetting the counter
        if (counter < 0)
        {
            Interlocked.CompareExchange(ref _counter, DateTimeOffset.UtcNow.Ticks, counter);
            counter = Interlocked.Increment(ref _counter);
        }

        var randomBytes = Guid.NewGuid().ToByteArray();
        var counterBytes = BitConverter.GetBytes(counter);

        // Ensure the byte array is in the correct endianness
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(counterBytes);
        }

        // Copy the counter bytes into the last 6 bytes of the GUID to maintain uniqueness
        Array.Copy(counterBytes, 0, randomBytes, 10, 6);

        return new Guid(randomBytes);
    }

    /// <summary>
    ///     Generates an array of new sequential <see cref="Guid" /> values based on the current timestamp and a counter.
    /// </summary>
    /// <param name="count">The number of sequential <see cref="Guid" /> values to generate.</param>
    /// <returns>An array of new sequential <see cref="Guid" /> values.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than or equal to zero.</exception>
    /// <remarks>
    ///     This method generates an array of new <see cref="Guid" /> values, each based on the current timestamp and a
    ///     counter.
    ///     The generated <see cref="Guid" /> values are unique and sequential, which can be useful for certain types of
    ///     database indexing scenarios.
    /// </remarks>
    /// <example>
    ///     The following code demonstrates how to use the <see cref="NewSequentialGuids(int)" /> method to generate an array
    ///     of new sequential
    ///     <see cref="Guid" /> values.
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