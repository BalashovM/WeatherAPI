using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository _repository;
        private readonly ILogger<CpuMetricsController> _logger;

        public CpuMetricsController(ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			   [FromRoute] TimeSpan fromTime,
			   [FromRoute] TimeSpan toTime)
		{
            var metrics = _repository.GetByPeriod(fromTime, toTime);
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto
                {
                    Time = metric.Time,
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            if (_logger != null)
            {
                _logger.LogInformation("Запрос метрик Cpu за период");
            }

            return Ok();
		}

		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime,
			[FromRoute] Percentile percentile)
		{
            var metrics = _repository.GetByPeriodWithSort(fromTime, toTime, "value");
            if (metrics.Count == 0) return NoContent();

            HashSet<int> values = new HashSet<int>();

            foreach (var metric in metrics)
            {
                values.Add(metric.Value);
            }

            int percentileThisList = PercentileCalc(new List<int>(values).ToArray(), (double)percentile / 100.0);

            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            response.Metrics.Add(new CpuMetricDto
            {
                Time = metrics[percentileThisList].Time,
                Value = metrics[percentileThisList].Value,
                Id = metrics[percentileThisList].Id
            });

            if (_logger != null)
            {
                _logger.LogInformation("Запрос percentile Cpu за период");
            }

            return Ok(response);
        }

        private int PercentileCalc(int[] sequence, double PercentileValue )
        {
                Array.Sort(sequence);
                int N = sequence.Length;
                double n = (N - 1) * PercentileValue + 1;
                
                if (n == 1d) return sequence[0];
                else if (n == N) return sequence[N - 1];
                else
                {
                    int k = (int)n;
                    double d = n - k;
                
                return Array.Find(sequence, p => p > (sequence[k - 1] + d * (sequence[k] - sequence[k - 1]))); 
                }
        }
    }
}
