using CloudWeather.Temperature.Api.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Temperature.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : ControllerBase
{
    [HttpGet("observation/{zip}")]
    public async Task<IActionResult> GetTemperatureByZip(string zip, [FromQuery] int? days, TempDbContext db)
    {
        if (days == null)
        {
            return BadRequest();
        }

        var startDate = DateTime.UtcNow.AddDays(-days.Value);
        var result = await db.Temperature
                            .Where(temp => temp.ZipCode == zip)
                            .ToListAsync();

        return Ok(result);
    }
}