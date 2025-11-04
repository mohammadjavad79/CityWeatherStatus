using CityWeathers.Core.Dtos.ApiResponse;
using CityWeathers.Core.Dtos.WeatherService;
using CityWeathers.Core.Exceptions;
using CityWeathers.Core.Services.Weather;
using CityWeathers.Data.Entity;
using CityWeathers.Data.Repositories;

namespace CityWeathers.Core.Services.Application;

public class WeatherAppService : IWeatherAppService
{
    private IWeatherService _weatherService;
    private ICityRepository _cityRepository;
    private ILogger<WeatherAppService> _logger;

    public WeatherAppService(
        IWeatherService weatherService,
        ICityRepository cityRepository,
        ILogger<WeatherAppService> logger
        )
    {
        _weatherService = weatherService;
        _cityRepository = cityRepository;
        _logger = logger;
    }
    
    public async Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName)
    {
        var city = await GetCityOrThrowAsync(cityName);
        
        var (cityWeather, weatherFailed) = await TryGetCityWeatherAsync(city);
        var (cityPollutant, pollutionFailed) = await TryGetCityPollutantAsync(city);

        if (weatherFailed && pollutionFailed)
        {
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
        
        return MapToResponse(cityWeather, cityPollutant);
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