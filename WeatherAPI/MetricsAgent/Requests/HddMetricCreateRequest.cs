using System;

namespace MetricsAgent.Requests
{
    public class HddMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public double FreeSize { get; set; }
    }
}