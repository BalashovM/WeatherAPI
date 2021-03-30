using MetricsLibrary;
using MetricsManager.DAL;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private IDotNetMetricsRepository _repository;

        public DotNetMetricsController(IDotNetMetricsRepository repository, ILogger<DotNetMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, 
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик DotNet FromPeriod для агента");
            }

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

            int percentileThisList = (int)percentile;
            percentileThisList = percentileThisList * metrics.Count / 100;

            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricManagerDto>()
            };

            response.Metrics.Add(new DotNetMetricManagerDto
            {
                Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile DotNet FromPeriod для агента");
            }

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, 
            [FromRoute] TimeSpan toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricManagerDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricManagerDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id,
                    IdAgent = metric.IdAgent
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик DotNet FromPeriod для кластера");
            }

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

            int percentileThisList = (int)percentile;
            percentileThisList = percentileThisList * metrics.Count / 100;

            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricManagerDto>()
            };

            response.Metrics.Add(new DotNetMetricManagerDto
            {
                Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id,
                IdAgent = metrics[percentileThisList].IdAgent
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile DotNet FromPeriod для кластера");
            }

            return Ok(response);
        }
    }
}
