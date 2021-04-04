using System;

namespace MetricsManager.Models
{
    public class DotNetMetricModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
        public int IdAgent { get; set; }
    }
}
