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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private INetworkMetricsRepository _repository;
        private readonly IMapper _mapper;

        public NetworkMetricsController(IMapper mapper, INetworkMetricsRepository repository, ILogger<NetworkMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
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
            var percentileMetric = metrics.Cast<NetworkMetric>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var response = new AllNetworkMetricsResponse() { Metrics = new List<NetworkMetricDto>() };
            
            response.Metrics.Add(_mapper.Map<NetworkMetricDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля метрик Network за период с {fromTime} по {toTime}");

            return Ok(response);
        }
        private List<int> GetListValuesFromMetrics(IList<NetworkMetric> metricValues)
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
