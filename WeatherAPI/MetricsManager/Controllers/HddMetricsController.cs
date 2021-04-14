using AutoMapper;
using MetricsLibrary;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
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
        private readonly IHddMetricsRepository _repository;
        private readonly IMapper _mapper;

        public HddMetricsController(IMapper mapper, IHddMetricsRepository repository, ILogger<HddMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId,
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);
            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Hdd за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("agent/{idAgent}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
        [FromRoute] int agentId,
        [FromRoute] DateTimeOffset fromTime,
        [FromRoute] DateTimeOffset toTime,
        [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, "value", agentId);

            var percentileMetric = metrics.Cast<HddMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<HddMetricManagerDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Hdd за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
                    [FromRoute] DateTimeOffset fromTime,
                    [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Hdd за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromCluster(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSorting(fromTime, toTime, "value");

            var percentileMetric = metrics.Cast<HddMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<HddMetricManagerDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Hdd за период с {fromTime} по {toTime} для кластера");

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
