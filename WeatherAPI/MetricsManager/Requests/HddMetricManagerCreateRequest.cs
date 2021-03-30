using System;

namespace MetricsManager.Requests
{
    public class HddMetricManagerCreateRequest
    {
        public TimeSpan Time { get; set; }
        public double Value { get; set; }
        public int IdAgent { get; set; }
    }
}
