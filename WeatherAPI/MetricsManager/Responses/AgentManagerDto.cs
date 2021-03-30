using System;
using System.Collections.Generic;


namespace MetricsManager.Responses
{
    public class AllAgentsResponse
    {
        public List<AgentManagerDto> Metrics { get; set; }
    }
    public class AgentManagerDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
    }
}
