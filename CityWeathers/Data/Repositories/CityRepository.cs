using CityWeathers.Data.DbContexts;
using CityWeathers.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CityWeathers.Data.Repositories;

public class CityRepository : ICityRepository
{
    private WeatherDbContext _context;

    public CityRepository(WeatherDbContext weatherDbContext)
    {
        _context = weatherDbContext;
    }
    
    public async Task<City?> GetCityByNameAsync(string cityName)
    {
        return await _context.Cities.Where(city => city.Name == cityName).FirstOrDefaultAsync();
    }
}