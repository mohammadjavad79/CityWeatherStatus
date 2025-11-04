using CityWeathers.Core.Dtos.ApiResponse;
using CityWeathers.Core.Dtos.WeatherService;
using CityWeathers.Core.Exceptions;
using CityWeathers.Core.Services.Weather;
using CityWeathers.Data.Entity;
using CityWeathers.Data.Repositories;

namespace CityWeathers.Core.Services.Application;

public class WeatherAppService : IWeatherAppService
{
    private readonly IWeatherService _weatherService;
    private readonly ICityRepository _cityRepository;
    private readonly ILogger<WeatherAppService> _logger;
    private readonly ICityDataRepository _cityDataRepository;

    public WeatherAppService(
        IWeatherService weatherService,
        ICityRepository cityRepository,
        ILogger<WeatherAppService> logger,
        ICityDataRepository cityDataRepository
        )
    {
        _weatherService = weatherService;
        _cityRepository = cityRepository;
        _logger = logger;
        _cityDataRepository = cityDataRepository;
    }
    
    public async Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName)
    {
        var city = await GetCityOrThrowAsync(cityName);
        
        var (cityWeather, weatherFailed) = await TryGetCityWeatherAsync(city);
        var (cityPollutant, pollutionFailed) = await TryGetCityPollutantAsync(city);
        
        await StoreCityDataAsync(cityWeather, cityPollutant, city);
        
        if (weatherFailed && pollutionFailed)
        {
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
        
        return MapToResponse(cityWeather, cityPollutant);
    }

    private async Task StoreCityDataAsync(CityWeatherDto? cityWeatherDto, CityPollutantDto? cityPollutantDto, City city)
    {
        var cityData = new CityData()
        {
            Temperature = cityWeatherDto?.TemperatureCelsius,
            Humidity = cityWeatherDto?.HumidityPercent,
            WindSpeed = cityWeatherDto?.WindSpeedMetersPerSecond,
            AirQuality = cityPollutantDto?.AirQualityIndex,
            Pollutants = cityPollutantDto?.MajorPollutantsJson.ToString() ?? string.Empty,
            CityId = city.Id
        };

        await _cityDataRepository.StoreAsync(cityData);
    }
    
    private async Task<City> GetCityOrThrowAsync(string cityName)
    {
        var city = await _cityRepository.GetCityByNameAsync(cityName);

        if (city == null)
        {
            _logger.LogWarning($"city {cityName} not found");
            
            throw new NotFoundException($"city {cityName} not found");
        }

        return city;
    }

    private async Task<(CityWeatherDto? weather, bool failed)> TryGetCityWeatherAsync(City city)
    {
        try
        {
            var cityWeather = await _weatherService.GetCityWeatherAsync(city.Id, city.Latitude, city.Longitude);
            return (cityWeather, false);
        }
        catch (Exception exception)
        {
            return (null, true);
        }
    }

    private async Task<(CityPollutantDto? weather, bool faild)> TryGetCityPollutantAsync(City city)
    {
        try
        {
            var cityPollutant = await _weatherService.GetCityPollutantAsync(city.Id, city.Latitude, city.Longitude);
            return (cityPollutant, false);
        }
        catch (Exception exception)
        {
            return (null, true);
        }
    }
    
    private GetCityWeatherStatusResponseDto MapToResponse(CityWeatherDto? weather, CityPollutantDto? pollution)
    {
        return new GetCityWeatherStatusResponseDto
        {
            TemperatureCelsius = weather?.TemperatureCelsius,
            HumidityPercent = weather?.HumidityPercent,
            WindSpeedMetersPerSecond = weather?.WindSpeedMetersPerSecond,
            Latitude = weather?.Latitude,
            Longitude = weather?.Longitude,
            AirQualityIndex = pollution?.AirQualityIndex,
            MajorPollutantsJson = pollution?.MajorPollutantsJson
        };
    }
}