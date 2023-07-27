using CloudWeather.Precipitation.Api.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Precipitation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrecipitationController : ControllerBase
{
    [HttpGet("observation/{zip}")]
    public async Task<IActionResult> GetObservation(string zip, [FromQuery] int? days, PrecipDbContext db)
    {
        if (days is null)
        {
            return BadRequest("Days is required");
        }

        var startDate = DateTime.UtcNow.AddDays(days.GetValueOrDefault(7));
        var result = await db.Precipitation
                            .Where(p => p.ZipCode == zip)
                            .ToListAsync();

        return Ok(result);
    }

    [HttpPost("observation")]
    public async Task<IActionResult> PostObservation(PrecipitationModel precipitation, PrecipDbContext db)
    {
        precipitation.CreatedOn = precipitation.CreatedOn.ToUniversalTime();
        await db.Precipitation.AddAsync(precipitation);
        await db.SaveChangesAsync();
        return Ok();
    }
}