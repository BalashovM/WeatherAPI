using System;

namespace MetricsAgent.DAL.Models
{
    public class RamMetric
    {
        public int Id { get; set; }
        public double Available { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
