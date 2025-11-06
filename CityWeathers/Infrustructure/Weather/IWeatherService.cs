using CityWeathers.Application.Dtos.WeatherService;

namespace CityWeathers.Infrustructure.Weather;

public interface IWeatherService
{
    public Task<CityWeatherDto> GetCityWeatherAsync(int cityId, decimal latitude, decimal longitude, CancellationToken cancellationToken);
    
    public Task<CityPollutantDto> GetCityPollutantAsync(int cityId, decimal latitude, decimal longitude, CancellationToken cancellationToken);
}