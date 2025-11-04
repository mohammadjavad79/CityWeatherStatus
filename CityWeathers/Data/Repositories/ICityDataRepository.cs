using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public interface ICityDataRepository
{
    public Task<CityData> StoreAsync(CityData cityData);
}