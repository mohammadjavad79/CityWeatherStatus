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
    
    public async Task AddAsync(CityData cityData)
    {
        await _context.CityData.AddAsync(cityData);

        await _context.SaveChangesAsync();
    }
}