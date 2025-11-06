using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public interface ICityRepository
{
    public Task<City?> GetCityByNameAsync(string cityName, CancellationToken cancellationToken);
}