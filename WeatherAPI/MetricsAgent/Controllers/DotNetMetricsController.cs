using AutoMapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
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
        private readonly ILogger<DotNetMetricsController> _logger;
        private IDotNetMetricsRepository _repository;
        private readonly IMapper _mapper;

        public DotNetMetricsController(IMapper mapper, IDotNetMetricsRepository repository, ILogger<DotNetMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			   [FromRoute] DateTimeOffset fromTime,
			   [FromRoute] DateTimeOffset toTime)
		{
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllDotNetMetricsResponse(){ Metrics = new List<DotNetMetricDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<DotNetMetricDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик DotNet за период с {fromTime } по {toTime}");

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSorting(fromTime, toTime, "value");
            var percentileMetric = metrics.Cast<DotNetMetric>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricDto>() };

            response.Metrics.Add(_mapper.Map<DotNetMetricDto>(percentileMetric));

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
