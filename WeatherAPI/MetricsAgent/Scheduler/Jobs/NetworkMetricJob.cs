using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Scheduler.Jobs
{
    /// <summary>
	/// Задача сбора Network метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly INetworkMetricsRepository _repository;
        /// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Network Interface";
        /// <summary>Имя счетчика</summary>
        private readonly string counterName = "Bytes Received/sec";
        /// <summary>Список для хранения всех экземпляров счетчика</summary>
        private readonly List<PerformanceCounter> _counters;

        public NetworkMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<INetworkMetricsRepository>();
            _counters = new List<PerformanceCounter>();

            string[] networkCadr = new PerformanceCounterCategory(categoryName).GetInstanceNames();

            foreach (var item in networkCadr)
            {
                _counters.Add(new PerformanceCounter(categoryName, counterName, item));
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            var value = 0;

            foreach (var counter in _counters)
            {
                value += Convert.ToInt32(counter.NextValue());
            }

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new NetworkMetric{ Time = time, Value = value });

            return Task.CompletedTask;
        }
    }
}