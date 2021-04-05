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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _repository;
        private readonly IMapper _mapper;

        public CpuMetricsController(IMapper mapper, ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, 
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agentId);

            var response = new AllCpuMetricsFromAgentResponse() { Metrics = new List<CpuMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Cpu за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAgent(
            [FromRoute] int agentId, 
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime, 
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, "value", agentId);

            var percentileMetric = metrics.Cast<CpuMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllCpuMetricsFromAgentResponse() { Metrics = new List<CpuMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<CpuMetricManagerDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Cpu за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllCpuMetricsFromAgentResponse() { Metrics = new List<CpuMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Cpu за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime, 
            [FromRoute] Percentile percentile)
        {
            var metrics = _repository.GetByPeriodWithSorting(fromTime, toTime, "value");
  
            var percentileMetric = metrics.Cast<CpuMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllCpuMetricsFromAgentResponse() { Metrics = new List<CpuMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<CpuMetricManagerDto>(percentileMetric));

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
