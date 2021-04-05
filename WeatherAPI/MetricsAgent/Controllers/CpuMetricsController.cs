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
    [Route("api/metrics/Cpu")]
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

        [HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			   [FromRoute] DateTimeOffset fromTime,
			   [FromRoute] DateTimeOffset toTime)
		{
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllCpuMetricsResponse(){ Metrics = new List<CpuMetricDto>()};

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));
            }

            _logger.LogInformation($"Запрос метрик Cpu за период c {fromTime} по {toTime}");

            return Ok(response);
		}

		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] DateTimeOffset fromTime,
			[FromRoute] DateTimeOffset toTime,
			[FromRoute] Percentile percentile)
		{
            var metrics = _repository.GetByPeriodWithSorting(fromTime, toTime, "value");
            var percentileMetric = metrics.Cast<CpuMetric>().SingleOrDefault(i => i.Value == PercentileCalculator.Calculate(GetListValuesFromMetrics(metrics), (double)percentile / 100.0));
            var response = new AllCpuMetricsResponse() { Metrics = new List<CpuMetricDto>() };

            response.Metrics.Add(_mapper.Map<CpuMetricDto>(percentileMetric));

            _logger.LogInformation($"Запрос метрик Cpu персентиля = {percentile} за период c {fromTime} по {toTime}");

            return Ok(response);
        }

        private List<int> GetListValuesFromMetrics(IList<CpuMetric> metricValues)
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
