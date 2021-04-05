using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL.Interfaces
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
        IList<NetworkMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<NetworkMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
    }
}
