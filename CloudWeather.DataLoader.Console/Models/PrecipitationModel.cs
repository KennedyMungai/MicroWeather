namespace CloudWeather.DataLoader.Console.Models;

internal class PrecipitationModel
{
    public DateTime CreatedOn { get; set; }
    public decimal AmountByInches { get; set; }
    public string WeatherType { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}