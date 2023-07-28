using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();