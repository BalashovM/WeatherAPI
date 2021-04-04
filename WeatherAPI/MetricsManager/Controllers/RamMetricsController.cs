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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private IRamMetricsRepository _repository;

        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик Ram FromPeriod для агента");
            }

            return Ok(response);
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] int idAgent,
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSortFromAgent(fromTime, toTime, "value", idAgent);
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<RamMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            response.Metrics.Add(new RamMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Ram FromPeriod для агента");
            }

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
                    [FromRoute] TimeSpan fromTime,
                    [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик Ram FromPeriod для кластера");
            }

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromCluster(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSort(fromTime, toTime, "value");
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<RamMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            response.Metrics.Add(new RamMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Ram FromPeriod для кластера");
            }

            return Ok(response);
        }
        private List<double> GetListValuesFromMetrics(IList<RamMetricModel> metricValues)
        {
            HashSet<double> set = new HashSet<double>();

            foreach (var metric in metricValues)
            {
                set.Add(metric.Value);
            }

            return new List<double>(set);
        }
    }
}
