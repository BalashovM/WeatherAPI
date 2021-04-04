using System;

namespace MetricsManager.Requests
{
    public class RamMetricManagerCreateRequest
    {
        public TimeSpan Time { get; set; }
        public double Value { get; set; }
        public int IdAgent { get; set; }
    }
}
