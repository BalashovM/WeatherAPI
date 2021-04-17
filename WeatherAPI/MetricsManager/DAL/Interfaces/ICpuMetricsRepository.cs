using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface ICpuMetricsRepository : IRepository<CpuMetricModel>
    {
        IList<CpuMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<CpuMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent);
        IList<CpuMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField);
        IList<CpuMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent);
        CpuMetricModel GetLast(int agentId);
    }
}
