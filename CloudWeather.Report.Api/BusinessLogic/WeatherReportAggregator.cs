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
    /// <param name="zip"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public Task<WeatherReport> BuildWeeklyReport(string zip, int days);
}