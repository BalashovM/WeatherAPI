using System;

namespace MetricsManager.Requests
{
    public class DotNetMetricManagerCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int IdAgent { get; set; }
    }
}
