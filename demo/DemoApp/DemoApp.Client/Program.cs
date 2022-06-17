using System;
using System.Net.Http;

using GoogleAnalytics.Blazor;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddGoogleAnalytics("G-V061TDSPDR");

await builder.Build().RunAsync();
