using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllNetworkMetricsResponse
    {
        public List<NetworkMetricManagerDto> Metrics { get; set; }
    }
    public class NetworkMetricManagerDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int IdAgent { get; set; }
    }
}
