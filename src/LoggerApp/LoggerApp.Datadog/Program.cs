using LoggerApp.Core;
using loggertest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Datadog.Logs;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.DatadogLogs("468eae9b05c0964e41efb5cdfbd2b822", configuration: new DatadogConfiguration() { Url = "https://http-intake.logs.datadoghq.eu" })
    .WriteTo.Console()
    .CreateLogger();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IImportService, ImportService>();
        services.AddSingleton<IAppRunner, AppRunner>();
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

host.Services.GetService<IAppRunner>().Run();

await host.RunAsync();
