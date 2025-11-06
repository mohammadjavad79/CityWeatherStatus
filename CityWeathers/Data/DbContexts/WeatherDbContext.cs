using CityWeathers.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CityWeathers.Data.DbContexts;

public class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<CityData> CityData { get; set; } = null!;
}