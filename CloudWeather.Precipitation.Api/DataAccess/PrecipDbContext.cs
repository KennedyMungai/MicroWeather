using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Precipitation.Api.DataAccess;

public class PrecipDbContext : DbContext
{
    public PrecipDbContext(DbContextOptions<PrecipDbContext> options) : base(options)
    {

    }

    public DbSet<PrecipitationModel> Precipitation { get; set; }
}