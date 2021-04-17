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
	/// Задача сбора DotNet метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class DotNetMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly IDotNetMetricsRepository _repository;
        /// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = ".NET CLR Memory";
        /// <summary>Имя счетчика</summary>
        private readonly string counterName = "# Bytes in all Heaps";
        /// <summary>Счетчик</summary>
        private readonly PerformanceCounter _counter;

        public DotNetMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<IDotNetMetricsRepository>();
            _counter = new PerformanceCounter(categoryName, counterName, "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var value = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new DotNetMetric { Time = time, Value = value });

            return Task.CompletedTask;
        }
    }
}