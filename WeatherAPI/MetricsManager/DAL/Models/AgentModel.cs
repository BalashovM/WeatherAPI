using System;

namespace MetricsManager.DAL.Models
{
    public class AgentModel
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
    }
}
