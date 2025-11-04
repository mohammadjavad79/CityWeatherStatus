namespace CityWeathers.Core.Dtos.ApiLog;

public class OpenWeatherResponseLogDto
{
    public string Url { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}