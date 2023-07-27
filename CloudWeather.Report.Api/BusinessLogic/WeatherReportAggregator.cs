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
    public Task<WeatherReport> BuildWeeklyReport(string zip, int days);
}