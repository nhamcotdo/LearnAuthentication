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

    [BasicAuthFilter("Admin")]
    [HttpGet(Name = "GetWeatherForecast")]
    [MyFilter("WeatherForecastGet")]
    public IEnumerable<WeatherForecast> Get()
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
}