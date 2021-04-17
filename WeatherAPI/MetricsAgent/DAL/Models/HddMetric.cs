using System;

namespace MetricsAgent.DAL.Models
{
    public class HddMetric
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
