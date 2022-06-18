using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Service collection extensions to add a scoped <see cref="IGBAnalyticsManager"/> service.
/// </summary>
public static class GoogleAnalyticsExtensions
{
    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services) => AddGoogleAnalytics(services, null, false);


    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="trackingId"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId) => AddGoogleAnalytics(services, trackingId, false);


    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, bool debug) => AddGoogleAnalytics(services, null, debug);


    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="trackingId"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId, bool debug)
    {
        return services.AddScoped<IGBAnalyticsManager>(p =>
        {
            var googleAnalytics = ActivatorUtilities.CreateInstance<GBAnalyticsManager>(p);

            var tiSection = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("GoogleAnalytics.Blazor:TrackingId");

            trackingId ??= tiSection.Value;

            if (trackingId != null)
            {
                googleAnalytics.Configure(trackingId, debug);
            }

            return googleAnalytics;
        });
    }
}
