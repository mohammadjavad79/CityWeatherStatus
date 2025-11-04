using System.ComponentModel.DataAnnotations;

namespace CityWeathers.Core.Dtos.ApiRequest;

public class GetCityWeatherRequestDto
{
    [Required(ErrorMessage = "City name is required")]
    [MaxLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;
}