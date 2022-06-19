using GoogleAnalytics.Blazor;
using GoogleAnalytics.Blazor.Website.WebAssembly;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddGoogleAnalytics("UA-111742878-2");

builder.Logging.SetMinimumLevel(LogLevel.Debug);

/* Serilog configuration
 * here I use BrowserHttp sink to send log entries to my Server app
 */
var levelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("GoogleAnalytics.Blazor", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.Async(a => a.BrowserConsole(outputTemplate: "{Timestamp:HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}"))
        .CreateLogger();

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.ControlledBy(levelSwitch)
//    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
//    .WriteTo.BrowserHttp(endpointUrl: $"{builder.HostEnvironment.BaseAddress}ingest", controlLevelSwitch: levelSwitch)
//    .CreateLogger();

//builder.Host.UseSerilog((ctx, lc) => lc
//        .MinimumLevel.Debug()
//        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//        .MinimumLevel.Override("GoogleAnalytics.Blazor", LogEventLevel.Debug)
//        .Enrich.FromLogContext()
//        .WriteTo.Async(a => a.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}")));

/* this is used instead of .UseSerilog to add Serilog to providers */
builder.Logging.AddProvider(new SerilogLoggerProvider());

await builder.Build().RunAsync();
