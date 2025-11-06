using CityWeathers.Application.Dtos.ApiResponse;

namespace CityWeathers.Application.Services;

public interface IWeatherAppService
{
    public Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName, CancellationToken cancellationToken);
}