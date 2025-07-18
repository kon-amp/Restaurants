using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase {
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService) {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpPost("generate")]
    public IActionResult Generate([FromQuery] int maxResults, [FromBody] TemperatureRequest tempParams)
    {
        string? error;
        if (maxResults < 0) {
            error = "Max results cannot be smaller than 0";
            return BadRequest(error);
        }
        if (tempParams.MaxTempRange < tempParams.MinTempRange) {
            error = "Max temp cannot be smaller than min temperature";
            return BadRequest(error);
        }

        var result = _weatherForecastService.Get(maxResults, tempParams.MinTempRange, tempParams.MaxTempRange);

        return Ok(result);
    }
}
