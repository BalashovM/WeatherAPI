using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly INetworkMetricsRepository _repository;
        private List<PerformanceCounter> _networkCounter;

        public NetworkMetricJob(IServiceProvider provider)
        {
            _provider = provider;
            _repository = _provider.GetService<INetworkMetricsRepository>();
            _networkCounter = new List<PerformanceCounter>();

            PerformanceCounterCategory categoryNetwork = new PerformanceCounterCategory("Network Interface");
            string[] networkCadr = categoryNetwork.GetInstanceNames();

            foreach (var item in networkCadr)
            {
                _networkCounter.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", item));
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            var networkUsageRx = 0;

            foreach (var item in _networkCounter)
            {
                networkUsageRx += (int)item.NextValue();
            }

            var time = DateTimeOffset.UtcNow;

            _repository.Create(new NetworkMetric{ Time = time, Value = networkUsageRx });

            return Task.CompletedTask;
        }
    }
}