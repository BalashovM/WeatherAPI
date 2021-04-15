using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Scheduler.Jobs
{
    /// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ICpuMetricsRepository _repository;
        /// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Processor";
        /// <summary>Имя счетчика</summary>
        private readonly string counterName = "% Processor Time";
        /// <summary>Счетчик</summary>
        private readonly PerformanceCounter _counter;

        public CpuMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<ICpuMetricsRepository>();
            _counter = new PerformanceCounter(categoryName, counterName, "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var value = Convert.ToInt32(_counter.NextValue());
            var time = DateTimeOffset.UtcNow;

            _repository.Create(new CpuMetric { Time = time, Value = value });

            return Task.CompletedTask;
        }
    }
}
