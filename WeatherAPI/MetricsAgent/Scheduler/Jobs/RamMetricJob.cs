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
	/// Задача сбора Ram метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class RamMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly IRamMetricsRepository _repository;
        /// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Memory";
        /// <summary>Имя счетчика</summary>
        private readonly string counterName = "Available MBytes";
        /// <summary>Счетчик</summary>
        private readonly PerformanceCounter _counter;

        public RamMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IRamMetricsRepository>();
            _counter = new PerformanceCounter(categoryName, counterName);
        }

        public Task Execute(IJobExecutionContext context)
        {
            var value = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new RamMetric { Time = time, Value = value });

            return Task.CompletedTask;
        }
    }
}