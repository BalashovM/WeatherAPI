using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly IRamMetricsRepository _repository;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IRamMetricsRepository>();
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramAvialableMBytes = Convert.ToInt32(_ramCounter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new RamMetric { Time = time, Available = ramAvialableMBytes });

            return Task.CompletedTask;

        }

    }
}