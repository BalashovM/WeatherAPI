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
    [Route("api/metrics/dotnet/errors-count")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _repository;
        private readonly IAgentsRepository _agentRepository;
        private readonly IMapper _mapper;

        public DotNetMetricsController(
            IMapper mapper,
            IDotNetMetricsRepository repository,
            IAgentsRepository agentRepository,
            ILogger<DotNetMetricsController> logger)
        {
            _agentRepository = agentRepository;
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
            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricManagerDto>() };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<DotNetMetricManagerDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик DotNet за период с {fromTime} по {toTime} для агента {agentId}");

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

            var percentileMetric = metrics.Cast<DotNetMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricManagerDto>() };

            response.Metrics.Add(_mapper.Map<DotNetMetricManagerDto>(percentileMetric));

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик DotNet за период с {fromTime} по {toTime} для агента {agentId}");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime)
        {

            var agents = _agentRepository.GetAll();

            var metrics = _repository.GetByPeriod(fromTime, toTime);

            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricManagerDto>() };

            foreach (var agent in agents)
            {
                var currentAgentMetrics = _repository.GetByPeriodFromAgent(fromTime, toTime, agent.Id);

                foreach (var metric in metrics)
                {
                    response.Metrics.Add(_mapper.Map<DotNetMetricManagerDto>(metric));
                }
            }

            _logger.LogInformation($"Запрос метрик DotNet за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
        public IActionResult GetMetricsByPercentileFromAllCluster(
            [FromRoute] DateTimeOffset fromTime, 
            [FromRoute] DateTimeOffset toTime, 
            [FromRoute] Percentile percentile)
        {
            var agents = _agentRepository.GetAll();

            var response = new AllDotNetMetricsResponse() { Metrics = new List<DotNetMetricManagerDto>() };
            foreach (var agent in agents)
            {
                var metrics = _repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, "value", agent.Id);

                var percentileMetric = metrics.Cast<DotNetMetricModel>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));

                response.Metrics.Add(_mapper.Map<DotNetMetricManagerDto>(percentileMetric));
            }

            _logger.LogInformation($"Запрос персентиля = {percentile} метрик DotNet за период с {fromTime} по {toTime} для кластера");

            return Ok(response);
        }

        private List<int> GetListValuesFromMetrics(IList<DotNetMetricModel> metricValues)
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
