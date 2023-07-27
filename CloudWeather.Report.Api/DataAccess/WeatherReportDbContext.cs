using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Report.Api.DataAccess;

public class WeatherReportDbContext : DbContext
{
    public WeatherReportDbContext(DbContextOptions<WeatherReportDbContext> options) : base(options)
    {
    }
    public DbSet<WeatherReport> Reports { get; set; }
}