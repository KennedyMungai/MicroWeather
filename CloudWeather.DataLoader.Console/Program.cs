using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();

var servicesConfig = configuration.GetSection("Services");

var tempServiceConfig = servicesConfig.GetSection("Temperature");
var tempServiceHost = tempServiceConfig["Host"];
var tempServicePort = tempServiceConfig["Port"];

var precipServiceConfig = servicesConfig.GetSection("Precipitation");
var precipServiceHost = precipServiceConfig["Host"];
var precipServicePort = precipServiceConfig["Port"];

var zipCodes = new List<string>
{
    "73026",
    "68104",
    "04401",
    "32808",
    "19717"
};

Console.WriteLine("Starting Data Load");

var temperatureHttpClient = new HttpClient();
temperatureHttpClient.BaseAddress = new Uri($"http://{tempServiceHost}:{tempServicePort}");

var precipitationHttpClient = new HttpClient();
precipitationHttpClient.BaseAddress = new Uri($"http://{precipServiceHost}:{precipServicePort}");
