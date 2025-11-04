using CityWeathers.Core.Dtos.WeatherService;

namespace CityWeathers.Core.Services.Weather;

public interface IWeatherService
{
    public Task<CityWeatherDto> GetCityWeatherAsync(int cityId, decimal latitude, decimal longitude);
    
    public Task<CityPollutantDto> GetCityPollutantAsync(int cityId, decimal latitude, decimal longitude);
}