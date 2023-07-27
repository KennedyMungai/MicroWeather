using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Temperature.Api.DataAccess;

public class TempDbContext : DbContext
{
    public TempDbContext(DbContextOptions<TempDbContext> options) : base(options)
    {

    }

    public DbSet<Temperature> Temperature { get; set; }
}