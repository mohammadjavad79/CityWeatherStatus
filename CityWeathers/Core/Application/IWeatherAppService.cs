using CityWeathers.Core.Dtos.ApiResponse;

namespace CityWeathers.Core.Application;

public interface IWeatherAppService
{
    public Task<GetCityWeatherStatusResponseDto> GetCityWeather(string cityName, CancellationToken cancellationToken);
}