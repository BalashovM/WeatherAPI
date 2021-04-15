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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _repository;
        private readonly IMapper _mapper;

        public RamMetricsController(IMapper mapper, IRamMetricsRepository repository, ILogger<RamMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("available")]
        public IActionResult GetMetricsFromAgent()
        {
            var metrics = _repository.GetAll();
            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            _logger.LogInformation("Запрос всех метрик RAM(Available)");

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllNetworkMetricsResponse() { Metrics = new List<NetworkMetricDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Network за период с {fromTime} по {toTime}");

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSorting(fromTime, toTime, "value");
            var percentileMetric = metrics.Cast<RamMetric>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricDto>() };

            response.Metrics.Add(_mapper.Map<RamMetricDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля метрик Network за период с {fromTime} по {toTime}");

            return Ok(response);
        }

        private List<double> GetListValuesFromMetrics(IList<RamMetric> metricValues)
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
