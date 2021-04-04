using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllCpuMetricsFromAgentResponse
    {
        public List<CpuMetricManagerDto> Metrics { get; set; }
    }
    public class CpuMetricManagerDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int IdAgent { get; set; }
    }
}
