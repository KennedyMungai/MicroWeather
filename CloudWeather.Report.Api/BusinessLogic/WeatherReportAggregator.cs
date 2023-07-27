using CloudWeather.Report.Api.Config;
using CloudWeather.Report.Api.DataAccess;

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

    private async Task FetchPrecipitationData(HttpClient httpClient, string zip, int days)
    {
        throw new NotImplementedException();
    }

    private async Task FetchTemperatureData(HttpClient httpClient, string zip, int days)
    {
        throw new NotImplementedException();
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