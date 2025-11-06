using System.Text.Json;
using System.Text.Json.Serialization;
using CityWeathers.Helpers.Converters;

namespace CityWeathers.Application.Dtos.ApiResponse;

public class GetCityWeatherStatusResponseDto
{
    [JsonConverter(typeof(JsonRound2Converter))]
    public double? TemperatureCelsius { get; set; }
    
    public int? HumidityPercent { get; set; }
    
    [JsonConverter(typeof(JsonRound2Converter))]
    public double? WindSpeedMetersPerSecond { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }    
    
    [JsonConverter(typeof(JsonRound2Converter))]
    public double? AirQualityIndex { get; set; }
    public JsonElement? MajorPollutantsJson { get; set; }      
}