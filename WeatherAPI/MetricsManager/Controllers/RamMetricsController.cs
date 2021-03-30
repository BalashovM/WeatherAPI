using MetricsLibrary;
using MetricsManager.DAL;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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

            int percentileThisList = (int)percentile;
            percentileThisList = percentileThisList * metrics.Count / 100;

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            response.Metrics.Add(new RamMetricManagerDto
            {
                Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
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

            int percentileThisList = (int)percentile;
            percentileThisList = percentileThisList * metrics.Count / 100;

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricManagerDto>()
            };

            response.Metrics.Add(new RamMetricManagerDto
            {
                Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Ram FromPeriod для кластера");
            }

            return Ok(response);
        }
    }
}
