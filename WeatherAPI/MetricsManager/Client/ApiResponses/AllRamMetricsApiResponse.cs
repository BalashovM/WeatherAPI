using System;
using System.Collections.Generic;


namespace MetricsManager.Client.ApiResponses
{
    public class AllRamMetricsApiResponse
    {
        public List<RamMetricApiResponse> Metrics { get; set; }
    }
    public class RamMetricApiResponse
    {
        public DateTimeOffset Time { get; set; }
        public double Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }
}
