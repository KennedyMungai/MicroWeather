using CloudWeather.Precipitation.Api.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CloudWeather.Precipitation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecipitationController : ControllerBase
{
    private readonly PrecipDbContext _context;

    public PrecipitationController(PrecipDbContext context)
    {
        _context = context;
    }

    [HttpGet("{zip}")]
    public IActionResult GetObservation(string zip, [FromQuery] int? days)
    {
        return Ok(zip);
    }
}