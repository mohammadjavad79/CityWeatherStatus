namespace CityWeathers.Core.Dtos.ApiLog;

public class OpenWeatherRequestLogDto
{
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string? RequestBody { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}