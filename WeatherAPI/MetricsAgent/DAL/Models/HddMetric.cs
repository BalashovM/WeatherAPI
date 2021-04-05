using System;

namespace MetricsAgent.DAL.Models
{
    public class HddMetric
    {
        public int Id { get; set; }
        public double FreeSize { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
