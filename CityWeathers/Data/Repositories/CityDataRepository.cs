using CityWeathers.Data.DbContexts;
using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public class CityDataRepository : ICityDataRepository
{
    private readonly WeatherDbContext _context;

    public CityDataRepository(WeatherDbContext weatherDbContext)
    {
        _context = weatherDbContext;
    }
    
    public async Task<CityData> StoreAsync(CityData cityData)
    {
        _context.CityData.Add(cityData);

        await _context.SaveChangesAsync();

        return cityData;
    }
}