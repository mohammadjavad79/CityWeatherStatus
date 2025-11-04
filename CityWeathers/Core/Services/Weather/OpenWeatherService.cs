using System.Globalization;
using System.Text.Json;
using CityWeathers.Core.Dtos.ApiLog;
using CityWeathers.Core.Dtos.WeatherService;
using CityWeathers.Core.Exceptions;
using CityWeathers.Data.Entity;
using CityWeathers.Data.Repositories;
using Microsoft.AspNetCore.WebUtilities;

namespace CityWeathers.Core.Services.Weather;

public class OpenWeatherService : IWeatherService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IWeatherApiRequestRepository _weatherApiRequestRepository;
    private readonly ILogger<OpenWeatherService> _logger;
    
    public OpenWeatherService(
        IConfiguration configuration,
        HttpClient httpClient,
        IWeatherApiRequestRepository weatherApiRequestRepository,
        ILogger<OpenWeatherService> logger
    )
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _weatherApiRequestRepository = weatherApiRequestRepository;
        _logger = logger;
    }
    
    public async Task<CityWeatherDto> GetCityWeatherAsync(int cityId, decimal latitude, decimal longitude)
    {
        var url = PrepareUrl(_configuration["Services:Weather:OpenWeather:CityWeatherUrl"], latitude, longitude);

        var requestLog = PrepareRequestLogDto(url,  "Get");

        try
        { 
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.ReasonPhrase + ", Open Weather Weather Service Response is " + responseBody);

                await _weatherApiRequestRepository.Create(
                    PrepareForLog(
                        cityId, 
                        requestLog, 
                        PrepareResponseLogDto(url, (int) response.StatusCode, responseBody)
                        )
                    );
                
                throw new ThirdPartyServiceNotAvailable("Third part service not available");
            }
            
            await _weatherApiRequestRepository.Create(
                PrepareForLog(
                    cityId, 
                    requestLog, 
                    PrepareResponseLogDto(url, (int) response.StatusCode, responseBody)
                )
            );
            
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            await _weatherApiRequestRepository.Create(PrepareForLog(cityId, requestLog , PrePareExceptionErrorLog(e)));
            
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
    }

    public async Task<CityPollutantDto> GetCityPollutantAsync(int cityId, decimal latitude, decimal longitude)
    { 
        var url = PrepareUrl(_configuration["Services:Weather:OpenWeather:CityAirPollutantUrl"], latitude, longitude);
        
        var requestLog = PrepareRequestLogDto(url,  "Get");

        try
        { 
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                
                _logger.LogError(response.ReasonPhrase + ", Open Weather Pollutant Service Response is " + responseBody);

                await _weatherApiRequestRepository.Create(
                    PrepareForLog(
                        cityId, 
                        requestLog, 
                        PrepareResponseLogDto(url, (int) response.StatusCode, responseBody)
                        )
                    );
                
                throw new ThirdPartyServiceNotAvailable("Third part service not available");
            }
            
            await _weatherApiRequestRepository.Create(
                PrepareForLog(
                    cityId, 
                    requestLog, 
                    PrepareResponseLogDto(url, (int) response.StatusCode, responseBody)
                )
            );
            
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            await _weatherApiRequestRepository.Create(PrepareForLog(cityId, requestLog , PrePareExceptionErrorLog(e)));
            
            throw new ThirdPartyServiceNotAvailable("Third party service not available");
        }
    }

    
    private string PrePareExceptionErrorLog(Exception exception)
    {
        var errorLog = new
        {
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            Type = exception.GetType().Name,
            Time = DateTime.UtcNow
        };

        return JsonSerializer.Serialize(errorLog);
    }
    
    private WeatherApiRequest PrepareForLog(int cityId, string requestLog, string responseLog)
    {
        return new WeatherApiRequest()
        {
            CityId = cityId,
            RequestJson = requestLog,
            ResponseJson = responseLog
        };
    }
    
    private string PrepareUrl(string? url, decimal latitude, decimal longitude)
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

    private string PrepareRequestLogDto(string url, string method, string? requestBody = null)
    {
        var request = new OpenWeatherRequestLogDto()
        {
            Url = url,
            Method = method,
            RequestBody = requestBody
        };
        
        return JsonSerializer.Serialize(request);
    }

    private string PrepareResponseLogDto(string url, int statusCode, string? responseBody = null)
    {
        var response = new OpenWeatherResponseLogDto()
        {
            Url = url,
            StatusCode = statusCode,
            ResponseBody = responseBody
        };
        
        return JsonSerializer.Serialize(response);
    }
}