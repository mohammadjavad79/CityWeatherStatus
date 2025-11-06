using CityWeathers.Core.Application;
using CityWeathers.Core.Middlewares;
using CityWeathers.Core.Services.Weather;
using CityWeathers.Data.DbContexts;
using CityWeathers.Data.Repositories;
using CityWeathers.Data.Seeder;
using CityWeathers.Services.Weather;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//this is for use app setting json files in program.cs
builder.Configuration.AddJsonFile("appsettings.json", false, true);

#region Configure Serilog

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/warnings-.txt",
        restrictedToMinimumLevel: LogEventLevel.Warning,
        rollingInterval: RollingInterval.Day
    )
    .WriteTo.File(
        "logs/debugs-.txt",
        restrictedToMinimumLevel: LogEventLevel.Debug,
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();
#endregion

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    });

#region Add Services
builder.Services.AddHttpClient<IWeatherService, OpenWeatherService>();
builder.Services.AddScoped<IWeatherAppService, WeatherAppService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityDataRepository, CityDataRepository>();
builder.Services.AddDbContext<WeatherDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["Databases:Sqlite:ConnectionString"]);
});
builder.Host.UseSerilog(); // this is for add serilog to the container

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region this is for seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    context.Database.Migrate();
    DataSeeder.SeedCitiesFromJson(context);
}
#endregion

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();


app.Run();