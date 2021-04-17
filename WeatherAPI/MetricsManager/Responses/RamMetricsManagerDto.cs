using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllRamMetricsResponse
    {
        public List<RamMetricManagerDto> Metrics { get; set; }
    }
    public class RamMetricManagerDto
    {
        public DateTimeOffset Time { get; set; }
        public double Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
