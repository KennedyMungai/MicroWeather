using CloudWeather.Report.Api.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace CloudWeather.Report.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherReportController : ControllerBase
{
    [HttpGet("weather-report/{zip}")]
    public async Task<IActionResult> GetWeatherReportByZipCode(string zip, [FromQuery] int? days, IWeatherReportAggregator weatherAgg)
    {
        if (days is null)
        {
            return BadRequest("Please provide the 'days' query parameter");
        }

        var report = await weatherAgg.BuildReport(zip, days.Value);

        return Ok(report);
    }
}
