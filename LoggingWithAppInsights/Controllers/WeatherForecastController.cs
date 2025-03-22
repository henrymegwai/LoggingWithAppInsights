using Microsoft.AspNetCore.Mvc;

namespace LoggingWithAppInsights.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "/GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogDebug("A debug log");
        _logger.LogTrace("A trace log");
        _logger.LogInformation("An information log");
        _logger.LogWarning("A warning log");
        _logger.LogError("An error log");
        _logger.LogCritical("A critical log");
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    
    [HttpPost(Name = "/ThrowException")]
    public IActionResult ThrowException()
    {
        try
        {
            _logger.LogInformation("Entered the try block");
            throw new AbandonedMutexException("An exception occurred");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Entered the catch block");
           _logger.LogError(ex, "Unable to complete operation");
        }
        finally
        {
            _logger.LogInformation("Entered the finally block");
        }
        
        return Ok();
    }
}
