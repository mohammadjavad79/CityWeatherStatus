using CityWeathers.Core.Dtos.ApiResponse;

namespace CityWeathers.Core.Services.Application;

public interface IWeatherAppService
{
    public Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName);
}