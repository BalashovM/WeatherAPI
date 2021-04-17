using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Interfaces
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {
       IList<RamMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<RamMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
    }
}
