using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllDotNetMetricsResponse
    {
        public List<DotNetMetricManagerDto> Metrics { get; set; }
    }
    public class DotNetMetricManagerDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int IdAgent { get; set; }
    }
}
