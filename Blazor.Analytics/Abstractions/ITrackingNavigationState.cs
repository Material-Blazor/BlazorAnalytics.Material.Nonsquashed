using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleAnalytics.Blazor;

public interface ITrackingNavigationState
{
    void EnableTracking();
    void DisableTracking();
    bool IsTrackingEnabled();
}
