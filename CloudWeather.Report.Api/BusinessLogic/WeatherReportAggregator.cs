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
        var tempClient = await FetchTemperatureData(httpClient, zip, days);
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
        var temperatureData = await temperatureRecords
                                        .Content
                                        .ReadFromJsonAsync<List<TemperatureModel>>();
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