using System.Globalization;
using System.Text.Json;
using CityWeathers.Core.Dtos.WeatherService;
using CityWeathers.Core.Exceptions;
using CityWeathers.Services.Weather;
using Microsoft.AspNetCore.WebUtilities;

namespace CityWeathers.Core.Services.Weather;

public class OpenWeatherService : IWeatherService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenWeatherService> _logger;
    
    public OpenWeatherService(
        IConfiguration configuration,
        HttpClient httpClient,
        ILogger<OpenWeatherService> logger
    )
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<CityWeatherDto> GetCityWeatherAsync(int cityId, decimal latitude, decimal longitude, CancellationToken cancellationToken)
    {
        var url = Createurl(_configuration["Services:Weather:OpenWeather:CityWeatherUrl"], latitude, longitude);

        try
        { 
            var response = await _httpClient.GetAsync(url, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            
            ThrowExceptionOnFailedResponse(response, "Open Weather Weather Service Response is " + responseBody);

            return MapToCityWeatherDto(responseBody, latitude, longitude);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
    }
    
    public async Task<CityPollutantDto> GetCityPollutantAsync(int cityId, decimal latitude, decimal longitude, CancellationToken cancellationToken)
    { 
        var url = Createurl(_configuration["Services:Weather:OpenWeather:CityAirPollutantUrl"], latitude, longitude);
        
        try
        { 
            var response = await _httpClient.GetAsync(url, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            
            ThrowExceptionOnFailedResponse(response, "Open Weather Pollutant Service Response is " + responseBody);

            return MapToCityPollutantDto(responseBody);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
    }
    
    private string Createurl(string? url, decimal latitude, decimal longitude)
    {
        var apiKey = _configuration["Services:Weather:OpenWeather:ApiKey"];
        var baseUrl = _configuration["Services:Weather:OpenWeather:BaseDomainUrl"] + url;

        var queryParam = new Dictionary<string, string?>()
        {
            ["lat"] = latitude.ToString(CultureInfo.InvariantCulture),
            ["lon"] = longitude.ToString(CultureInfo.InvariantCulture),
            ["appid"] = apiKey!
        };
        
        return QueryHelpers.AddQueryString(baseUrl, queryParam);
    }
    
    private void ThrowExceptionOnFailedResponse(HttpResponseMessage response, string message)
    {
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(message);
                
            throw new ThirdPartyServiceNotAvailable("Third part service not available");
        }
    }
    
    private CityWeatherDto MapToCityWeatherDto(string responseBody, decimal latitude,  decimal longitude)
    {
        var weatherData = JsonSerializer.Deserialize<OpenWeatherGetCityWeatherResponseDto>(
            responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (weatherData == null)
        {
            _logger.LogError("data could not be deserialized to OpenWeatherGetCityResponseDto" + responseBody);

            throw new ThirdPartyServiceNotAvailable("Third part service not available");
        }
            
        return new CityWeatherDto
        {
            TemperatureCelsius = weatherData.Main?.Temp - 273.15,
            HumidityPercent = weatherData.Main?.Humidity,
            WindSpeedMetersPerSecond = weatherData.Wind?.Speed,
            Latitude = latitude,
            Longitude = longitude
        };
    }

    private CityPollutantDto MapToCityPollutantDto(string responseBody)
    {
        
        var airPollutantData = JsonSerializer.Deserialize<OpenWeatherGetCityPollutantResponseDto>(
            responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (airPollutantData == null)
        {
            _logger.LogError("data could not be deserialized to OpenWeatherGetCityPollutantResponseDto" + responseBody);

            throw new ThirdPartyServiceNotAvailable("Third part service not available");
        }
            
        return new CityPollutantDto
        {
            AirQualityIndex = airPollutantData.List[0]?.Main?.Aqi,
            MajorPollutantsJson = airPollutantData.List[0]?.Components
        };
    }
}