using System;

namespace MetricsManager.Client.ApiRequests 
{ 
    public class GetAllDotNetMetricsApiRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string IpAddress { get; set; }
    }
}
