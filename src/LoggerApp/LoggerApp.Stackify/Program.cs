using LoggerApp.Core;
using loggertest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var apiKey = configuration.GetSection("Stackify").GetValue<string>("ApiKey");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Stackify(apiKey)
    .WriteTo.Console()
    .CreateLogger();

StackifyLib.Utils.StackifyAPILogger.LogEnabled = true;
StackifyLib.Utils.StackifyAPILogger.OnLogMessage += StackifyAPILogger_OnLogMessage;

static void StackifyAPILogger_OnLogMessage(string data)
{
    Debug.WriteLine(data);
}

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
