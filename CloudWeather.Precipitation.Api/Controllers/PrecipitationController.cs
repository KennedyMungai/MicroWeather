using Microsoft.AspNetCore.Mvc;

namespace CloudWeather.Precipitation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecipitationController : ControllerBase
{
    [HttpGet("{zip:string}")]
    public IActionResult GetObservation(string zip)
    {
        return Ok(zip);
    }
}