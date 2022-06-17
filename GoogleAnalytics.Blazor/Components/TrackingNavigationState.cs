namespace GoogleAnalytics.Blazor;


/// <summary>
/// State determining whether to track or not.
/// </summary>
public class TrackingNavigationState : ITrackingNavigationState
{
    private bool _isTrackingEnabled = true;


    /// <inheritdoc/>
    public void DisableTracking()
    {
        _isTrackingEnabled = false;
    }


    /// <inheritdoc/>
    public void EnableTracking()
    {
        _isTrackingEnabled = true;
    }


    /// <inheritdoc/>
    public bool IsTrackingEnabled()
    {
        return _isTrackingEnabled;
    }
}
