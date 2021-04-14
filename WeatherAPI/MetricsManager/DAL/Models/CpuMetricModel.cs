using System;

namespace MetricsManager.DAL.Models
{
    public class CpuMetricModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
        public int IdAgent { get; set; }
    }
}
