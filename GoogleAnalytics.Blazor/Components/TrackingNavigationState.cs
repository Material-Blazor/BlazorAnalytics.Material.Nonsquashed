using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleAnalytics.Blazor;

public class TrackingNavigationState : ITrackingNavigationState
{
    private bool _isTrackingEnabled = true;

    public void DisableTracking() => _isTrackingEnabled = false;

    public void EnableTracking() => _isTrackingEnabled = true;

    public bool IsTrackingEnabled() => _isTrackingEnabled;
}
