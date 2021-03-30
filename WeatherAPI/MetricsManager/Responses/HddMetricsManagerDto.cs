using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllHddMetricsResponse
    {
        public List<HddMetricManagerDto> Metrics { get; set; }
    }
    public class HddMetricManagerDto
    {
        public TimeSpan Time { get; set; }
        public double Value { get; set; }
        public int Id { get; set; }
        public int IdAgent { get; set; }
    }
}
