using System.Text.Json;
using CloudWeather.Report.Api.Config;
using CloudWeather.Report.Api.DataAccess;
using CloudWeather.Report.Api.Models;

namespace CloudWeather.Report.Api.BusinessLogic;

public class WeatherReportAggregator : IWeatherReportAggregator
{
    private readonly IHttpClientFactory _http;
    private readonly ILogger<WeatherReportAggregator> _logger;
    private readonly WeatherDataConfig _weatherDataConfig;
    private readonly WeatherReportDbContext _context;

    public WeatherReportAggregator(
        ILogger<WeatherReportAggregator> logger,
        WeatherDataConfig weatherDataConfig,
        WeatherReportDbContext context,
        IHttpClientFactory http
        )
    {
        _logger = logger;
        _weatherDataConfig = weatherDataConfig;
        _context = context;
        _http = http;
    }


    public async Task<WeatherReport> BuildWeeklyReport(string zip, int days)
    {
        var httpClient = _http.CreateClient();
        var precipData = await FetchPrecipitationData(httpClient, zip, days);
        decimal totalSnow = GetTotalSnow(precipData);
        decimal totalRain = GetTotalRain(precipData);
        _logger.LogInformation($"zip: {zip} over the last {days} days has total snow levels of: {totalSnow} and total rain levels of : {totalRain}");

        var tempData = await FetchTemperatureData(httpClient, zip, days);
        decimal avgTempHigh = tempData.Average(t => t.TempHighF);
        decimal avgTempLow = tempData.Average(t => t.TempLowF);
        _logger.LogInformation($"zip: {zip} over the last {days} days has average high of: {avgTempHigh} and average low of: {avgTempLow}");

        var weatherReport = new WeatherReport
        {
            AverageHighF = Math.Ceiling(avgTempHigh),
            AverageLowF = Math.Ceiling(avgTempLow),
            RainfallTotalInches = totalRain,
            SnowTotalInches = totalSnow,
            ZipCode = zip,
            CreatedOn = DateTime.UtcNow
        };

        // TODO: Add some caching to avoid unnecessary database requests
        _context.Add(weatherReport);
        await _context.SaveChangesAsync();

        return weatherReport;
    }

    private static decimal GetTotalRain(List<PrecipitationModel> precipData)
    {
        var totalRain = precipData
                            .Where(p => p.WeatherType == "rain")
                            .Sum(p => p.AmountInches);
        return Math.Ceiling(totalRain);
    }


    private static decimal GetTotalSnow(List<PrecipitationModel> precipData)
    {
        var totalSnow = precipData
                            .Where(p => p.WeatherType == "snow")
                            .Sum(p => p.AmountInches);
        return Math.Ceiling(totalSnow);
    }

    private async Task<List<PrecipitationModel>> FetchPrecipitationData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildPrecipitationServiceEndpoint(zip, days);
        var precipRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var precipData = await precipRecords
                                        .Content
                                        .ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializerOptions);
        return precipData ?? new List<PrecipitationModel>();
    }

    private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildTemperatureServiceEndpoint(zip, days);
        var temperatureRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var temperatureData = await temperatureRecords
                                        .Content
                                        .ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializerOptions);
        return temperatureData ?? new List<TemperatureModel>();
    }

    private string? BuildTemperatureServiceEndpoint(string zip, int days)
    {
        var tempServiceProtocol = _weatherDataConfig.TempDataProtocol;
        var tempServiceHost = _weatherDataConfig.TempDataHost;
        var tempServicePort = _weatherDataConfig.TempDataPort;

        return $"{tempServiceProtocol}://{tempServiceHost}:{tempServicePort}/api/temperature/observation/{zip}?days={days}";
    }

    private string? BuildPrecipitationServiceEndpoint(string zip, int days)
    {
        var precipServiceProtocol = _weatherDataConfig.PrecipDataProtocol;
        var precipServiceHost = _weatherDataConfig.PrecipDataHost;
        var precipServicePort = _weatherDataConfig.PrecipDataPort;

        return $"{precipServiceProtocol}://{precipServiceHost}:{precipServicePort}/api/precipitation/observation/{zip}?days={days}";
    }
}

public interface IWeatherReportAggregator
{
    /// <summary>
    /// Build and returns a weather report
    /// Persists weekly report data
    /// </summary>
    /// <param name="zip">The location</param>
    /// <param name="days">The number of days to include in the report</param>
    /// <returns></returns>
    public Task<WeatherReport> BuildWeeklyReport(string zip, int days);
}