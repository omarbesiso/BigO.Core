using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BigO.Core.Serialization;

/// <summary>
///     A custom JSON converter for serializing and deserializing <see cref="TimeOnly" /> objects.
/// </summary>
[PublicAPI]
public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";

    /// <summary>
    ///     Reads and converts the JSON to type <see cref="TimeOnly" />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString()!, TimeFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
    }
}