using loggertest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Stackify()
    //.WriteTo.Console()
    //.ReadFrom.Configuration(configuration)
    .CreateLogger();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IImportService, ImportService>();
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddSerilog(Log.Logger, true);
            
        });

        services.AddSingleton(serviceProvider =>
        {
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger("");
            return logger;
        });
    })
    .Build();



var importService = host.Services.GetService<IImportService>();

for(int i = 0;i<10000;i++)
{
    try
    {
        importService!.Import();
    }
    catch(Exception ex)
    {
        Log.Logger.Fatal(ex, "Fatal error");
    }
}

await host.RunAsync();
