using Microsoft.AspNetCore.Mvc;

namespace CloudWeather.Precipitation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecipitationController : ControllerBase
{
    [HttpGet("{zip}")]
    public IActionResult GetObservation(string zip, [FromQuery] int? days)
    {
        return Ok(zip);
    }
}