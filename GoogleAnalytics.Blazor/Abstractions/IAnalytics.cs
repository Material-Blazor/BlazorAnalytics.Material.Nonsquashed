using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleAnalytics.Blazor;


/// <summary>
/// Google Analytics event and navigation tracking.
/// </summary>
public interface IAnalytics
{
    /// <summary>
    /// Remove Obsolete attribute once functionality is determined.
    /// </summary>
    /// <param name="globalConfigData"></param>
    /// <returns></returns>
    [Obsolete]
    Task ConfigureGlobalConfigData(Dictionary<string, object> globalConfigData);


    /// <summary>
    /// Remove Obsolete attribute once functionality is determined.
    /// </summary>
    /// <param name="globalConfigData"></param>
    /// <returns></returns>
    [Obsolete]
    Task ConfigureGlobalEventData(Dictionary<string, object> globalEventData);


    /// <summary>
    /// Tracks navigation to a new endpoint.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    Task TrackNavigation(string uri);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <param name="eventValue"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, string eventCategory = null, string eventLabel = null, int? eventValue = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventValue"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, int eventValue, string eventCategory = null, string eventLabel = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, object eventData);


    /// <summary>
    /// Enable global tracking.
    /// </summary>
    void Enable();


    /// <summary>
    /// Disable global tracking.
    /// </summary>
    void Disable();
}
