using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Responses;
using MetricsLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/DotNet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private IDotNetMetricsRepository _repository;
        private readonly ILogger<DotNetMetricsController> _logger;

        public DotNetMetricsController(IDotNetMetricsRepository repository, ILogger<DotNetMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			   [FromRoute] TimeSpan fromTime,
			   [FromRoute] TimeSpan toTime)
		{
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new DotNetMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }
            
            _logger.LogInformation($"Запрос метрик DotNet за период с {fromTime } по {toTime}");

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] TimeSpan fromTime,
            [FromRoute] TimeSpan toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSort(fromTime, toTime, "value");
            if (metrics.Count == 0) return NoContent();

            var percentileMetric = metrics.Cast<DotNetMetric>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };

            response.Metrics.Add(new DotNetMetricDto
            {
                Time = percentileMetric.Time,
                Value = percentileMetric.Value,
                Id = percentileMetric.Id,
            });

            _logger.LogInformation($"Запрос метрик DotNet персентиля за период с {fromTime} по {toTime}");

            return Ok(response);
        }
        private List<int> GetListValuesFromMetrics(IList<DotNetMetric> metricValues)
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
