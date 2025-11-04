using System.Text.Json;

namespace CityWeathers.Core.Dtos.WeatherService;

public class CityPollutantDto
{
    public double? AirQualityIndex { get; set; }
    public JsonElement? MajorPollutantsJson { get; set; }      
}