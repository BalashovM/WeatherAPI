using System;
using System.Collections.Generic;


namespace MetricsManager.Client.ApiResponses
{
    public class AllCpuMetricsApiResponse
    {
        public List<CpuMetricApiResponse> Metrics { get; set; }
    }
    public class CpuMetricApiResponse
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
