using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllCpuMetricsResponse
    {
        public List<CpuMetricManagerDto> Metrics { get; set; }
    }
    public class CpuMetricManagerDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
