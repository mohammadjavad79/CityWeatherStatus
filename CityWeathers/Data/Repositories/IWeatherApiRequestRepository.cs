using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public interface IWeatherApiRequestRepository
{
    public Task<WeatherApiRequest> StoreAsync(WeatherApiRequest weatherApiRequest);
}