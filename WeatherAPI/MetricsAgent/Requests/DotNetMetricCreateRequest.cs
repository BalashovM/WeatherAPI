using System;

namespace MetricsAgent.Requests
{
    public class DotNetMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
    }
}