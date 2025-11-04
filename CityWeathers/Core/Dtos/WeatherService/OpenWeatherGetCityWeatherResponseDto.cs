using System.Text.Json.Serialization;

namespace CityWeathers.Core.Dtos.WeatherService;

public class OpenWeatherGetCityWeatherResponseDto
{
    [JsonPropertyName("coord")]
    public CoordDto? Coord { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDto>? Weather { get; set; }

    [JsonPropertyName("base")]
    public string? Base { get; set; }

    [JsonPropertyName("main")]
    public MainDto? Main { get; set; }

    [JsonPropertyName("visibility")]
    public int? Visibility { get; set; }

    [JsonPropertyName("wind")]
    public WindDto? Wind { get; set; }

    [JsonPropertyName("clouds")]
    public CloudsDto? Clouds { get; set; }

    [JsonPropertyName("dt")]
    public long? Dt { get; set; }

    [JsonPropertyName("sys")]
    public SysDto? Sys { get; set; }

    [JsonPropertyName("timezone")]
    public int? Timezone { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("cod")]
    public int? Cod { get; set; }

    public class CoordDto
    {
        [JsonPropertyName("lon")]
        public decimal? Lon { get; set; }

        [JsonPropertyName("lat")]
        public decimal? Lat { get; set; }
    }

    public class WeatherDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
    }

    public class MainDto
    {
        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double? FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double? TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double? TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public int? Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int? Humidity { get; set; }

        [JsonPropertyName("sea_level")]
        public int? SeaLevel { get; set; }

        [JsonPropertyName("grnd_level")]
        public int? GrndLevel { get; set; }
    }

    public class WindDto
    {
        [JsonPropertyName("speed")]
        public double? Speed { get; set; }

        [JsonPropertyName("deg")]
        public int? Deg { get; set; }

        [JsonPropertyName("gust")]
        public double? Gust { get; set; }
    }

    public class CloudsDto
    {
        [JsonPropertyName("all")]
        public int? All { get; set; }
    }

    public class SysDto
    {
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("sunrise")]
        public long? Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long? Sunset { get; set; }
    }
}
