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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private ICpuMetricsRepository _repository;

        public CpuMetricsController(ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
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
            var response = new AllCpuMetricsFromAgentResponse()
            {
                Metrics = new List<CpuMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            _logger.LogInformation($"Запрос метрик Cpu за период с {fromTime} по {toTime} для агента {agentId}");

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

            var percentileMetric = metrics.Cast<CpuMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllCpuMetricsFromAgentResponse()
            {
                Metrics = new List<CpuMetricManagerDto>()
            };

            response.Metrics.Add(new CpuMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });
            
            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Cpu за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllCpuMetricsFromAgentResponse()
            {
                Metrics = new List<CpuMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            _logger.LogInformation($"Запрос метрик Cpu за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAllCluster(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime, 
            [FromRoute] Percentile percentile)
        {
            IList<CpuMetricModel> metrics = _repository.GetByPeriodWithSort(fromTime, toTime, "value");
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<CpuMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllCpuMetricsFromAgentResponse()
            {
                Metrics = new List<CpuMetricManagerDto>()
            };

            response.Metrics.Add(new CpuMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Cpu за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }
        private List<int> GetListValuesFromMetrics(IList<CpuMetricModel> metricValues)
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
