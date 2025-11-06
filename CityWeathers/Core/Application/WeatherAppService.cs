using CityWeathers.Core.Dtos.ApiResponse;
using CityWeathers.Core.Dtos.WeatherService;
using CityWeathers.Core.Exceptions;
using CityWeathers.Data.Entity;
using CityWeathers.Data.Repositories;
using CityWeathers.Services.Weather;

namespace CityWeathers.Core.Application;

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
    
    public async Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName, CancellationToken cancellationToken)
    {
        var city = await GetCityOrThrowAsync(cityName, cancellationToken);
        
        Result<CityWeatherDto> cityWeather = await TryGetCityWeatherAsync(city, cancellationToken);
        Result<CityPollutantDto> cityPollutant = await TryGetCityPollutantAsync(city, cancellationToken);
        
        await StoreCityDataAsync(cityWeather.Value, cityPollutant.Value, city);
        
        if (! cityWeather.IsSuccess && ! cityPollutant.IsSuccess)
        {
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
        
        return MapToResponse(cityWeather.Value, cityPollutant.Value);
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

        await _cityDataRepository.AddAsync(cityData);
    }
    
    private async Task<City> GetCityOrThrowAsync(string cityName, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetCityByNameAsync(cityName, cancellationToken);

        if (city == null)
        {
            _logger.LogWarning($"city {cityName} not found");
            
            throw new NotFoundException($"city {cityName} not found");
        }

        return city;
    }

    private async Task<Result<CityWeatherDto>> TryGetCityWeatherAsync(City city, CancellationToken cancellationToken)
    {
        try
        {
            var cityWeather = await _weatherService.GetCityWeatherAsync(city.Id, city.Latitude, city.Longitude, cancellationToken);

            return Result<CityWeatherDto>.Success(cityWeather);
        }
        catch (Exception exception)
        {
            return Result<CityWeatherDto>.Failure("City Weather Has Exception or Error");
        }
    }

    private async Task<Result<CityPollutantDto>> TryGetCityPollutantAsync(City city, CancellationToken cancellationToken)
    {
        try
        {
            var cityPollutant = await _weatherService.GetCityPollutantAsync(city.Id, city.Latitude, city.Longitude, cancellationToken);

            return Result<CityPollutantDto>.Success(cityPollutant);
        }
        catch (Exception exception)
        {
            return Result<CityPollutantDto>.Failure("City Pollutant Has Exception or Error");
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