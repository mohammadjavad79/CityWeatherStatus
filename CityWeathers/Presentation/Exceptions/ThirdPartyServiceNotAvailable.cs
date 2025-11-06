namespace CityWeathers.Presentation.Exceptions;

public class ThirdPartyServiceNotAvailable : Exception
{
    public int ErrorCode { get; } = 503;
    
    public ThirdPartyServiceNotAvailable(string message) : base(message) {}
}