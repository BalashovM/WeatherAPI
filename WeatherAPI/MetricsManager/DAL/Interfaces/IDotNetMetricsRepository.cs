using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetricModel>
    {
        IList<DotNetMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<DotNetMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent);
        IList<DotNetMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
        IList<DotNetMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent);

    }
}
