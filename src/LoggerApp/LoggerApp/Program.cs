using LoggerApp.Core;
using loggertest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var apiKey = configuration.GetSection("Stackify").GetValue<string>("ApiKey");
var appName = configuration.GetSection("Stackify").GetValue<string>("AppName");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    //.WriteTo.NewRelicLogs(applicationName: "loggerTest", licenseKey: "eu01xxbafaf18affffad9840515856b2FFFFNRAL")
    //.WriteTo.TCPSink("tls://logs4.papertrailapp.com:30143")
    //.WriteTo.Sink(new StackifyRetraceSink(stackifyLogger))
    //.WriteTo.Http(
    //    requestUri: $"https://api.stackify.com/api/v1/Log/LogJson?apiKey=8Sr9Pq9Gl7Ms5Lb7Us5Jr9Qa3Qc2Wa1Wy4Iu3Nm", null)
    //.WriteTo.DatadogLogs("799d492e005bf55bd89ecb280f7761f4", configuration: new DatadogConfiguration() { Url = "https://http-intake.logs.datadoghq.eu" })
    //.WriteTo.Logger(lc => lc
    //    .WriteTo.Http("https://logs-01.loggly.com/inputs/c999cf17-3341-47bb-ade5-9e8052fc6da6/tag/loggly/", null))
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
