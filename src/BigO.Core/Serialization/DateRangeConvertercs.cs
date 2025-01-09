using System.Text.Json;
using System.Text.Json.Serialization;
using BigO.Core.Types;

namespace BigO.Core.Serialization;

/// <summary>
///     A custom JSON converter for serializing and deserializing <see cref="DateRange" /> objects.
/// </summary>
[PublicAPI]
public class DateRangeConverter : JsonConverter<DateRange>
{
    /// <summary>
    ///     Reads and converts the JSON to type <see cref="DateRange" />.
    /// </summary>
    public override DateRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = reader.GetString();

        if (string.IsNullOrWhiteSpace(raw))
        {
            // Decide if you want to treat empty as default or throw
            throw new JsonException("Unable to parse empty string as DateRange.");
        }

        if (!DateRange.TryParse(raw, out var range))
        {
            throw new JsonException($"Invalid DateRange format: {raw}");
        }

        return range;
    }

    /// <summary>
    ///     Writes a specified value as JSON.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
    {
        // This calls DateRange.ToString(), which might produce "yyyy-MM-dd-∞"
        // for open-ended ranges, or "yyyy-MM-dd-yyyy-MM-dd" otherwise.
        writer.WriteStringValue(value.ToString());
    }
}