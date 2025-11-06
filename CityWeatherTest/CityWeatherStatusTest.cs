using CityWeathers.Application.Dtos.WeatherService;
using CityWeathers.Application.Services;
using CityWeathers.Data.Entity;
using CityWeathers.Data.Repositories;
using CityWeathers.Infrustructure.Weather;
using CityWeathers.Presentation.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityWeatherTest;

public class CityWeatherStatusTest
{
      [Fact]
    public async Task GetCityWeather_Returns_Combined_Weather_And_Pollution_Data()
    {
        var city = new City 
        { 
            Id = 1, 
            Name = "Tehran", 
            Latitude = 35.6892m, 
            Longitude = 51.3890m, 
            Code = "tehran" 
        };

        var mockCityRepo = new Mock<ICityRepository>();
        mockCityRepo.Setup(r => r.GetCityByNameAsync("Tehran", CancellationToken.None)).ReturnsAsync(city);
        var mockCityData = new Mock<ICityDataRepository>();

        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService.Setup(s => s.GetCityWeatherAsync(city.Id, city.Latitude, city.Longitude, CancellationToken.None))
            .ReturnsAsync(new CityWeatherDto
            {
                TemperatureCelsius = 25.5,
                HumidityPercent = 45,
                WindSpeedMetersPerSecond = 3.2,
                Latitude = city.Latitude,
                Longitude = city.Longitude
            });

        mockWeatherService.Setup(s => s.GetCityPollutantAsync(city.Id, city.Latitude, city.Longitude, CancellationToken.None))
            .ReturnsAsync(new CityPollutantDto
            {
                AirQualityIndex = 85.0,
                MajorPollutantsJson = null
            });

        var mockLogger = new Mock<ILogger<WeatherAppService>>();
        var service = new WeatherAppService(mockWeatherService.Object, mockCityRepo.Object, mockLogger.Object, mockCityData.Object);

        // Act
        var result = await service.GetCityWeather("Tehran",CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TemperatureCelsius.Should().Be(25.5);
        result.HumidityPercent.Should().Be(45);
        result.WindSpeedMetersPerSecond.Should().Be(3.2);
        result.AirQualityIndex.Should().Be(85.0);
        result.Latitude.Should().Be(city.Latitude);
        result.Longitude.Should().Be(city.Longitude);
    }
    
    [Fact]
    public async Task GetCityWeather_Throws_NotFoundException_When_City_Does_Not_Exist()
    {
        // Arrange
        var mockCityRepo = new Mock<ICityRepository>();
        mockCityRepo.Setup(r => r.GetCityByNameAsync("UnknownCity", CancellationToken.None)).ReturnsAsync((City?)null);
        var mockCityData = new Mock<ICityDataRepository>();
    
        var mockWeatherService = new Mock<IWeatherService>();
        var mockLogger = new Mock<ILogger<WeatherAppService>>();
        var service = new WeatherAppService(mockWeatherService.Object, mockCityRepo.Object, mockLogger.Object, mockCityData.Object);
    
        // Act
        var act = async () => await service.GetCityWeather("UnknownCity", CancellationToken.None);
    
        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("city UnknownCity not found");
    }
    
    [Fact]
    public async Task GetCityWeather_Throws_ThirdPartyServiceNotAvailable_When_Both_Services_Fail()
    {
        // Arrange
        var city = new City { Id = 1, Name = "Tehran", Latitude = 35.6892m, Longitude = 51.3890m, Code = "THR" };
    
        var mockCityRepo = new Mock<ICityRepository>();
        mockCityRepo.Setup(r => r.GetCityByNameAsync("Tehran", CancellationToken.None)).ReturnsAsync(city);
    
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService.Setup(s => s.GetCityWeatherAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(), CancellationToken.None))
            .ThrowsAsync(new Exception("Weather service failed"));
        mockWeatherService.Setup(s => s.GetCityPollutantAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(), CancellationToken.None))
            .ThrowsAsync(new Exception("Pollution service failed"));
    
        var mockCityData = new Mock<ICityDataRepository>();
        
        var mockLogger = new Mock<ILogger<WeatherAppService>>();
        var service = new WeatherAppService(mockWeatherService.Object, mockCityRepo.Object, mockLogger.Object, mockCityData.Object);
    
        // Act
        var act = async () => await service.GetCityWeather("Tehran", CancellationToken.None);
    
        // Assert
        await act.Should().ThrowAsync<ThirdPartyServiceNotAvailable>()
            .WithMessage("Third party service not available");
    }
}