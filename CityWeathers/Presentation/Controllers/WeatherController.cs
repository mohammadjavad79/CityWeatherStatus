using CityWeathers.Application.Dtos.ApiRequest;
using CityWeathers.Application.Dtos.ApiResponse;
using CityWeathers.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityWeathers.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherAppService _weatherAppService;

    public WeatherController(IWeatherAppService weatherAppService)
    {
        _weatherAppService = weatherAppService;
    }
    
    [HttpGet]
    public async Task<ActionResult<GetCityWeatherStatusResponseDto>> Get([FromQuery] GetCityWeatherRequestDto getCityWeatherRequest, CancellationToken cancellationToken)
    {
        var result = await _weatherAppService.GetCityWeather(getCityWeatherRequest.Name, cancellationToken);
        
        return Ok(result);
    }
}