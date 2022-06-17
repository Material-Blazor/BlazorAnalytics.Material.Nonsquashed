namespace GoogleAnalytics.Blazor;


/// <summary>
/// Determines whether to track or not.
/// </summary>
public interface ITrackingNavigationState
{
    /// <summary>
    /// Enables GA tracking.
    /// </summary>
    void EnableTracking();


    /// <summary>
    /// Disables GA tracking.
    /// </summary>
    void DisableTracking();


    /// <summary>
    /// Returns whether tracking is enabled.
    /// </summary>
    /// <returns></returns>
    bool IsTrackingEnabled();
}
