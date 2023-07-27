using CloudWeather.Report.Api.BusinessLogic;
using CloudWeather.Report.Api.Config;
using CloudWeather.Report.Api.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WeatherReportDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("WeatherReportDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("WeatherReportDb"))
    );
});
builder.Services.AddHttpClient();
builder.Services.AddOptions();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
