using CityWeathers.Data.DbContexts;
using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Repositories;

public class WeatherApiRequestRepository : IWeatherApiRequestRepository
{
    private readonly WeatherDbContext _weatherDbContext;
    
    public WeatherApiRequestRepository(WeatherDbContext weatherDbContext)
    {
        _weatherDbContext = weatherDbContext;
    }
    
    public async Task<WeatherApiRequest> StoreAsync(WeatherApiRequest weatherApiRequest)
    {
        _weatherDbContext.WeatherApiRequests.Add(weatherApiRequest);
        
        await _weatherDbContext.SaveChangesAsync();
        
        return weatherApiRequest;
    }
}