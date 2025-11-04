using System.Text.Json;
using System.Text.Json.Serialization;

namespace CityWeathers.Core.Dtos.WeatherService;

public class OpenWeatherGetCityPollutantResponseDto
{
    [JsonPropertyName("coord")]
    public CoordDto? Coord { get; set; }

    [JsonPropertyName("list")]
    public List<PollutionDataDto?> List { get; set; }

    public class CoordDto
    {
        [JsonPropertyName("lon")]
        public double? Lon { get; set; }

        [JsonPropertyName("lat")]
        public double? Lat { get; set; }
    }

    public class PollutionDataDto
    {
        [JsonPropertyName("main")]
        public MainDto? Main { get; set; }

        [JsonPropertyName("components")]
        public JsonElement? Components { get; set; }

        [JsonPropertyName("dt")]
        public long? Dt { get; set; }
    }

    public class MainDto
    {
        [JsonPropertyName("aqi")]
        public double? Aqi { get; set; }
    }
}