using System.Text.Json;
using System.Text.Json.Serialization;

namespace CityWeathers.Helpers.Converters;

public class JsonRound2Converter : JsonConverter<double?>
{
    public override double? Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
        => r.TokenType == JsonTokenType.Null ? (double?)null : r.GetDouble();

    public override void Write(Utf8JsonWriter w, double? v, JsonSerializerOptions o)
    {
        if (v.HasValue) w.WriteNumberValue(Math.Round(v.Value, 2));
        else w.WriteNullValue();
    }
}