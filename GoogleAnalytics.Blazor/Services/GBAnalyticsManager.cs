using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace GoogleAnalytics.Blazor;


/// <inheritdoc/>
[Obsolete]
public sealed class GBAnalyticsManager : IGBAnalyticsManager
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    
    private bool _performGlobalTracking = true;
    private bool _suppressPageHitTracking = false;
    private string _trackingId = null;
    private Dictionary<string, object> _globalConfigData = new Dictionary<string, object>();
    private Dictionary<string, object> _globalEventData = new Dictionary<string, object>();
    private bool _isInitialized = false;
    private bool _debug = false;


    public GBAnalyticsManager(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += OnLocationChanged;
    }


    /// <summary>
    /// Sets the tracking id and debug flag.
    /// </summary>
    /// <param name="trackingId"></param>
    /// <param name="debug"></param>
    public void Configure(string trackingId, bool debug)
    {
        _trackingId = trackingId;
        _debug = debug;

        _ = OnLocationChanged(_navigationManager.Uri);
    }


    private async Task InitializeAsync()
    {
        if (_trackingId == null)
        {
            throw new InvalidOperationException("Invalid TrackingId");
        }

        await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Configure, _trackingId, _globalConfigData, _debug);
        
        _isInitialized = true;
    }


    /// <inheritdoc/>
    public async Task ConfigureGlobalConfigData(Dictionary<string, object> globalConfigData)
    {
        if (!_isInitialized)
        {
            _globalConfigData = globalConfigData;

            await InitializeAsync().ConfigureAwait(false);
        }
    }


    /// <inheritdoc/>
    public Task ConfigureGlobalEventData(Dictionary<string, object> globalEventData)
    {
        _globalEventData = globalEventData;
        return Task.CompletedTask;
    }


    /// <inheritdoc/>
    public async Task TrackNavigation(string uri)
    {
        if (!_performGlobalTracking)
        {
            return;
        }

        if (!_isInitialized)
        {
            await InitializeAsync().ConfigureAwait(false);
        }

        await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Navigate, _trackingId, uri).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public async Task TrackEvent(string eventName, string eventCategory = null, string eventLabel = null, int? eventValue = null)
    {
        await TrackEvent(eventName, new
        {
            event_category = eventCategory, 
            event_label = eventLabel, 
            value = eventValue
        }).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public Task TrackEvent(string eventName, int eventValue, string eventCategory = null, string eventLabel = null)
    {
        return TrackEvent(eventName, eventCategory, eventLabel, eventValue);
    }


    /// <inheritdoc/>
    public async Task TrackEvent(string eventName, object eventData)
    {
        if (!_performGlobalTracking)
        {
            return;
        }

        if (!_isInitialized)
        {
            await InitializeAsync().ConfigureAwait(false);
        }

        await _jsRuntime.InvokeAsync<string>( GoogleAnalyticsInterop.TrackEvent, eventName, eventData, _globalEventData).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public void EnableGlobalTracking()
    {
        _performGlobalTracking = true;
    }


    /// <inheritdoc/>
    public void DisableGlobalTracking()
    {
        _performGlobalTracking = false;
    }


    /// <inheritdoc/>
    public bool IsGlobalTrackingEnabled()
    {
        return _performGlobalTracking;
    }


    /// <inheritdoc/>
    public void SuppressPageHitTracking()
    {
        _suppressPageHitTracking = true;
    }


    private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
    {
        await OnLocationChanged(args.Location).ConfigureAwait(false);
    }


    private async Task OnLocationChanged(string location)
    {
        if (!string.IsNullOrWhiteSpace(_trackingId) && !_suppressPageHitTracking)
        {
            await TrackNavigation(location);
        }

        _suppressPageHitTracking = false;
    }
}
