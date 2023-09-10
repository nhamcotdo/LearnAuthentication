using LearnAuthentication.Configuarations.Filters;
using LearnAuthentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnAuthentication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAuthModel _authModel;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IAuthModel authModel
    )
    {
        _logger = logger;
        _authModel = authModel;
    }

    [AuthFilter("Admin")]
    [HttpGet("GetList", Name = "GetListWeatherForecast")]
    [MyFilter("GetListWeatherForecast")]
    public IEnumerable<WeatherForecast> GetList()
    {
        _logger.LogInformation("Begin executing : GetWeatherForecast");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [AuthFilter("Nham")]
    [HttpGet("Get", Name = "GetWeatherForecast")]
    [MyFilter("WeatherForecastGet")]
    public WeatherForecast Get(int next)
    {
        _logger.LogInformation("Begin executing : WeatherForecastGet");
        return new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(next)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };
    }
}