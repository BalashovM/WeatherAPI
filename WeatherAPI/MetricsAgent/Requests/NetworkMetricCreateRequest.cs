using System;

namespace MetricsAgent.Requests
{
    public class NetworkMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
    }
}