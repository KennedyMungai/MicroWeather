using System.ComponentModel.DataAnnotations;

namespace CloudWeather.Precipitation.Api.DataAccess;

public class Precipitation
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal AmountInches { get; set; }
    public string WeatherType { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}