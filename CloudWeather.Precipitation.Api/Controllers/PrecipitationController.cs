using CloudWeather.Precipitation.Api.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CloudWeather.Precipitation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecipitationController : ControllerBase
{
    [HttpGet("{zip}")]
    public IActionResult GetObservation(string zip, [FromQuery] int? days, PrecipDbContext db)
    {
        if (days is null)
        {
            return BadRequest("Days is required");
        }

        var startDate = DateTime.UtcNow.AddDays(days.GetValueOrDefault(7));
        var result = db.Precipitation
                            .Where(p => p.ZipCode == zip)
                            .ToList();



        return Ok(result);
    }
}