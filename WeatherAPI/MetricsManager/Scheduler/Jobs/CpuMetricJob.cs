using AutoMapper;
using MetricsManager.Client;
using MetricsManager.Client.ApiRequests;
using MetricsManager.Client.ApiResponses;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MetricsManager.Scheduler.Jobs
{
    /// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ICpuMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsManagerClient _client;
        private readonly ILogger _logger;

        public CpuMetricJob(IServiceProvider provider, IMetricsManagerClient client, ILogger<CpuMetricJob> logger)
        {
            _provider = provider;
            _client = client;
            _logger = logger;
            _repository = _provider.GetService<ICpuMetricsRepository>();
            _agentsRepository = _provider.GetService<IAgentsRepository>();
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"== CpuMetricJob Начало - {DateTimeOffset.UtcNow}");
            //Получаем из репозитория агентов список всех агентов
            var allAgentsInfo = _agentsRepository.GetAllActive();

            //Обрабатываем каждого агента в списке
            foreach (var agentInfo in allAgentsInfo)
            {
                //Последняя метрика
                var lastTime = _repository.GetLast(agentInfo.Id).Time;

                // Создаем запрос для получения метрик за период времени
                AllCpuMetricsApiResponse allCpuMetrics = _client.GetAllCpuMetrics(new GetAllCpuMetricsApiRequest
                {
                    FromTime = lastTime,
                    ToTime = DateTimeOffset.UtcNow,
                    IpAddress = agentInfo.IpAddress
                });

                foreach (var metric in allCpuMetrics.Metrics)
                {
                    _provider.GetService<ICpuMetricsRepository>()?.Create(new CpuMetricModel
                    {
                        AgentId = agentInfo.Id,
                        Time = metric.Time,
                        Value = metric.Value
                    });
                }

            }

            _logger.LogDebug($"!= CpuMetricJob Конец - {DateTimeOffset.UtcNow}");

            return Task.CompletedTask;
        }
    }
}
