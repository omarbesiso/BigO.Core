using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using BigO.Core.Types;

namespace BigO.Core.Serialization;

/// <summary>
///     A custom JSON converter for serializing and deserializing <see cref="DateOnly" /> objects.
/// </summary>
[PublicAPI]
public class DateRangeConverter : JsonConverter<DateRange>
{
    private const string Format = "yyyy-MM-dd-yyyy-MM-dd";

    /// <summary>
    ///     Reads and converts the JSON to type <see cref="DateOnly" />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override DateRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var parts = reader.GetString()!.Split("-");
        var startDate = DateOnly.ParseExact(parts[0], Format, CultureInfo.InvariantCulture);
        var endDate = DateOnly.ParseExact(parts[1], Format, CultureInfo.InvariantCulture);
        return DateRange.Create(startDate, endDate);
    }

    /// <summary>
    ///     Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}