using System;

namespace MetricsManager.Requests
{
    public class AgentCreateRequest
    {
        public bool Status { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
    }
}
