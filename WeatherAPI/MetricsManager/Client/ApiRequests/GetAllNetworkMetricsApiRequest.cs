using System;

namespace MetricsManager.Client.ApiRequests
{ 
    public class GetAllNetworkMetricsApiRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string IpAddress { get; set; }
    }
}
