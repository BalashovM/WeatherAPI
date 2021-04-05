using MetricsLibrary;
using MetricsManager.DAL;
using MetricsManager.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private INetworkMetricsRepository _repository;

        public NetworkMetricsController(INetworkMetricsRepository repository, ILogger<NetworkMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, 
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            _logger.LogInformation($"Запрос метрик Network за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] int agentId, 
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime, 
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSortFromAgent(fromTime, toTime, "value", agentId);
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<NetworkMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricManagerDto>()
            };

            response.Metrics.Add(new NetworkMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Network за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            _logger.LogInformation($"Запрос метрик Network за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAllCluster(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime, 
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSort(fromTime, toTime, "value");
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<NetworkMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricManagerDto>()
            };

            response.Metrics.Add(new NetworkMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Cpu за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }
        private List<int> GetListValuesFromMetrics(IList<NetworkMetricModel> metricValues)
        {
            HashSet<int> set = new HashSet<int>();

            foreach (var metric in metricValues)
            {
                set.Add(metric.Value);
            }

            return new List<int>(set);
        }
    }
}
