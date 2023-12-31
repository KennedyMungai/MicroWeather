namespace CloudWeather.Temperature.Api.DataAccess;

public class TemperatureModel
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal TempHighF { get; set; }
    public decimal TempLowF { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}