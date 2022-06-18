using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleAnalytics.Blazor;


/// <summary>
/// The google analytics strategy implementing <see cref="IAnalytics"/>. NEED A BETTER DESCRIPTION.
/// </summary>
[Obsolete]
public sealed class GoogleAnalyticsStrategy : IAnalytics
{
    private readonly IJSRuntime _jsRuntime;
    
    private bool _isGloballyEnabledTracking = true;
    private string _trackingId = null;
    private Dictionary<string, object> _globalConfigData = new Dictionary<string, object>();
    private Dictionary<string, object> _globalEventData = new Dictionary<string, object>();
    private bool _isInitialized = false;
    private bool _debug = false;


    public GoogleAnalyticsStrategy(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
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
        if (!_isGloballyEnabledTracking)
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

    public async Task TrackEvent(string eventName, object eventData)
    {
        if (!_isGloballyEnabledTracking)
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
    public void Enable()
    {
        _isGloballyEnabledTracking = true;
    }


    /// <inheritdoc/>
    public void Disable()
    {
        _isGloballyEnabledTracking = false;
    }
}
