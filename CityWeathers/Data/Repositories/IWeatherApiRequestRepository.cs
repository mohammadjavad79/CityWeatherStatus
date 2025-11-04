using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public interface IWeatherApiRequestRepository
{
    public Task<WeatherApiRequest> Create(WeatherApiRequest weatherApiRequest);
}