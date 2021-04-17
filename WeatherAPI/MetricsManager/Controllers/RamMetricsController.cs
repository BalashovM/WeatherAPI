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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _repository;
        private readonly IAgentsRepository _agentRepository;
        private readonly IMapper _mapper;

        public RamMetricsController(
            IMapper mapper,
            IRamMetricsRepository repository,
            IAgentsRepository agentRepository,
            ILogger<RamMetricsController> logger)
        {
            _agentRepository = agentRepository;
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

            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Ram за период с {fromTime} по {toTime} для агента {agentId}");

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

            var percentileMetric = metrics.Cast<RamMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<RamMetricManagerDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Ram за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
                    [FromRoute] DateTimeOffset fromTime,
                    [FromRoute] DateTimeOffset toTime)
        {
            var agents = _agentRepository.GetAll();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricManagerDto>() };

            foreach (var agent in agents)
            {
                var currentAgentMetrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agent.Id);

                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<RamMetricManagerDto>(metric));
                }
            }

            _logger.LogInformation($"Запрос метрик Ram за период с {fromTime} по {toTime} для клстера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromCluster(
            [FromRoute] DateTimeOffset fromTime,
            [FromRoute] DateTimeOffset toTime,
            [FromRoute] Percentile percentile)
        {
            var agents = _agentRepository.GetAll();

            var response = new AllRamMetricsResponse() { Metrics = new List<RamMetricManagerDto>() };
            foreach (var agent in agents)
            {
                var metrics = _repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, "value", agent.Id);

                var percentileMetric = metrics.Cast<RamMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

                response.Metrics.Add(_mapper.Map<RamMetricManagerDto>(percentileMetric));
            }

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик Ram за период с {fromTime} по {toTime} для кластера");

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
