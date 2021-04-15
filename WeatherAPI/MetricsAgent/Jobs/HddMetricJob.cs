using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private IHddMetricsRepository _repository;
        private PerformanceCounter _hddCounter;

        public HddMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IHddMetricsRepository>();
            _hddCounter = new PerformanceCounter("LogicalDisk", "Free Megabytes", "C:");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var hddFreeSpaceInMBytes = Convert.ToInt32(_hddCounter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new HddMetric { Time = time, FreeSize = hddFreeSpaceInMBytes });

            return Task.CompletedTask;
        }
    }
}