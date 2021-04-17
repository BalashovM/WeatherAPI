using System;

namespace MetricsManager.Client.ApiRequests
{
    public class GetAllHddMetricsApiRequest
    {
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
        public string IpAddress { get; set; }
    }
}
