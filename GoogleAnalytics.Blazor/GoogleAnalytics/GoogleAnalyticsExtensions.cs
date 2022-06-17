using Microsoft.Extensions.DependencyInjection;

namespace GoogleAnalytics.Blazor;

public static class GoogleAnalyticsExtensions
{
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services) => AddGoogleAnalytics(services, null, false);
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId) => AddGoogleAnalytics(services, trackingId, false);
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, bool debug) => AddGoogleAnalytics(services, null, debug);

    public static IServiceCollection AddGoogleAnalytics(
        this IServiceCollection services,
        string trackingId,
        bool debug)
    {
        services.AddScoped<ITrackingNavigationState, TrackingNavigationState>();
        return services.AddScoped<IAnalytics>(p =>
        {
            var googleAnalytics = ActivatorUtilities.CreateInstance<GoogleAnalyticsStrategy>(p);

            if (trackingId != null)
            {
                googleAnalytics.Configure(trackingId, debug);
            }

            return googleAnalytics;
        });
    }
}
