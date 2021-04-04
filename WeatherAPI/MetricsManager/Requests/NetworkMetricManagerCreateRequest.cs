using System;

namespace MetricsManager.Requests
{
    public class NetworkMetricManagerCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int IdAgent { get; set; }
    }
}
