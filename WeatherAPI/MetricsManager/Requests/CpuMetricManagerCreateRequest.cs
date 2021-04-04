using System;

namespace MetricsManager.Requests
{
    public class CpuMetricManagerCreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int IdAgent { get; set; }
    }
}
