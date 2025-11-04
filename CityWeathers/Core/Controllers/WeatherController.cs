using CityWeathers.Core.Dtos.ApiRequest;
using CityWeathers.Core.Dtos.ApiResponse;
using CityWeathers.Core.Services.Application;
using Microsoft.AspNetCore.Mvc;

namespace CityWeathers.Core.Controllers;

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
    public async Task<ActionResult<GetCityWeatherStatusResponseDto>> Get([FromQuery] GetCityWeatherRequestDto getCityWeatherRequest)
    {
        var result = await _weatherAppService.GetCityWeather(getCityWeatherRequest.Name);
        
        return Ok(result);
    }
}