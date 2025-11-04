using System.Globalization;
using System.Text.Json;
using CityWeathers.Data.DbContexts;
using CityWeathers.Data.Entity;

namespace CityWeathers.Data.Seeder;

public class DataSeeder
{
    public static void SeedCitiesFromJson(WeatherDbContext context)
    {
        if (!context.Cities.Any())
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "cities.json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Seed file not found at {filePath}");
            }

            var json = File.ReadAllText(filePath);
            var cityList = JsonSerializer.Deserialize<List<CityJson>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (cityList != null && cityList.Count > 0)
            {
                foreach (var item in cityList)
                {
                    context.Cities.Add(new City
                    {
                        Name = item.name,
                        Code = item.code,
                        Latitude = decimal.Parse(item.lat, CultureInfo.InvariantCulture),
                        Longitude = decimal.Parse(item.lng, CultureInfo.InvariantCulture)
                    });
                }

                context.SaveChanges();
            }
        }
    }
    
    private class CityJson
    {
        public string name { get; set; } = null!;
        public string code { get; set; } = null!;
        public string lat { get; set; } = null!;
        public string lng { get; set; } = null!;
    }
}