namespace CityWeathers.Application.Dtos.WeatherService;

public class CityWeatherDto
{
    public double? TemperatureCelsius { get; set; }
    public int? HumidityPercent { get; set; }
    public double? WindSpeedMetersPerSecond { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}