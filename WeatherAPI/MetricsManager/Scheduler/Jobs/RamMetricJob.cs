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
	/// Задача сбора Ram метрик
	/// </summary>
    [DisallowConcurrentExecution]
    public class RamMetricJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly IRamMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsManagerClient _client;
        private readonly ILogger _logger;

        public RamMetricJob(IServiceProvider provider, IMetricsManagerClient client, ILogger<RamMetricJob> logger)
        {
            _provider = provider;
            _client = client;
            _logger = logger;
            _repository = _provider.GetService<IRamMetricsRepository>();
            _agentsRepository = _provider.GetService<IAgentsRepository>();
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"== RamMetricJob Начало - {DateTimeOffset.UtcNow}");
            //Получаем из репозитория агентов список всех агентов
            var allAgentsInfo = _agentsRepository.GetAllActive();

            //Обрабатываем каждого агента в списке
            foreach (var agentInfo in allAgentsInfo)
            {
                //Последняя метрика
                var lastTime = _repository.GetLast(agentInfo.Id).Time;

                // Создаем запрос для получения метрик за период времени
                AllRamMetricsApiResponse allRamMetrics = _client.GetAllRamMetrics(new GetAllRamMetricsApiRequest
                {
                    FromTime = lastTime,
                    ToTime = DateTimeOffset.UtcNow,
                    IpAddress = agentInfo.IpAddress
                });

                foreach (var metric in allRamMetrics.Metrics)
                {
                    _provider.GetService<IRamMetricsRepository>()?.Create(new RamMetricModel
                    {
                        AgentId = agentInfo.Id,
                        Time = metric.Time,
                        Value = metric.Value
                    });
                }
            }

            _logger.LogDebug($"!= RamMetricJob Конец - {DateTimeOffset.UtcNow}");

            return Task.CompletedTask;
        }
    }
}