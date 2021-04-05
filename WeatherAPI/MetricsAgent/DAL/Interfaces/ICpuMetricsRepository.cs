using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Interfaces
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
        IList<CpuMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<CpuMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
    }
}
