using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Scheduler.Jobs
{
    /// <summary>
	/// Задача сбора Hdd метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class HddMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private IHddMetricsRepository _repository;
        /// <summary>Имя категории счетчика</summary>
        private readonly string categoryName = "LogicalDisk";
        /// <summary>Имя счетчика</summary>
        private readonly string counterName = "Free Megabytes";
        /// <summary>Счетчик</summary>
        private readonly PerformanceCounter _counter;

        public HddMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IHddMetricsRepository>();
            _counter = new PerformanceCounter(categoryName, counterName, "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var value = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new HddMetric { Time = time, Value = value });

            return Task.CompletedTask;
        }
    }
}