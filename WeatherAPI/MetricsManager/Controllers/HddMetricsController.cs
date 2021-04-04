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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private IHddMetricsRepository _repository;

        public HddMetricsController(IHddMetricsRepository repository, ILogger<HddMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в HddMetricsController");
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик Hdd FromPeriod для агента");
            }

            return Ok(response);
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
        [FromRoute] int agentId,
        [FromRoute] TimeSpan fromTime,
        [FromRoute] TimeSpan toTime,
        [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSortFromAgent(fromTime, toTime, "value", agentId);
            if (metrics.Count == 0) return NoContent();

            //int percentileThisList = metrics.IndexOf(x => x.value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var percentileMetric = metrics.Cast<HddMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricManagerDto>()
            };

            response.Metrics.Add(new HddMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
                /*Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
                */
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Hdd FromPeriod для агента");
            }

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
                    [FromRoute] TimeSpan fromTime,
                    [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик Hdd FromPeriod для кластера");
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

            //int percentileThisList = metrics.IndexOf(x => x.value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var percentileMetric = metrics.Cast<HddMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricManagerDto>()
            };

            response.Metrics.Add(new HddMetricManagerDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
                IdAgent = percentileMetric.IdAgent
                /*Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
                */
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Hdd FromPeriod для кластера");
            }

            return Ok(response);
        }
        private List<double> GetListValuesFromMetrics(IList<HddMetricModel> metricValues)
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
