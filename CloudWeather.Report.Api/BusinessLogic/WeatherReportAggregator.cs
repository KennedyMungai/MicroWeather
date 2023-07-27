using CloudWeather.Report.Api.DataAccess;

namespace CloudWeather.Report.Api.BusinessLogic;

public class WeatherReportAggregator : IWeatherReportAggregator
{
    public Task<WeatherReport> BuildWeeklyReport(string zip, int days)
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