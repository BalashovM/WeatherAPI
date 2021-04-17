using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IHddMetricsRepository : IRepository<HddMetricModel>
        {
        IList<HddMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<HddMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent);
        IList<HddMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
        IList<HddMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent);
        HddMetricModel GetLast(int agentId);
    }
}
