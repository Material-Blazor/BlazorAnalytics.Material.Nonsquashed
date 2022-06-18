using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Threading.Tasks;

namespace GoogleAnalytics.Blazor;


/// <summary>
/// The navigation tracking component. Place at the end of <code>App.razor</code>.
/// </summary>
public class NavigationTracker : ComponentBase
{
    [Inject] private IGBAnalyticsManager AnalyticsManager { get; set; } = null;
    [Inject] private NavigationManager NavigationManager { get; set; } = null;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationManager.LocationChanged -= OnLocationChanged;
        NavigationManager.LocationChanged += OnLocationChanged;
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            // Track initial navigation
            await OnLocationChanged(NavigationManager.Uri);
        }
    }


    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }


    private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
    {
        await OnLocationChanged(args.Location).ConfigureAwait(false);
    }


    private async Task OnLocationChanged(string location)
    {
        if (!AnalyticsManager.IsTrackingSuppressed())
        {
            await AnalyticsManager.TrackNavigation(location);
        }

        AnalyticsManager.ReEnablePageHitTracking();
    }
}
