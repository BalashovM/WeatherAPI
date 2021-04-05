using AutoMapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
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

        [HttpGet("left")]
        public IActionResult GetMetricsFromAgent()
        {
            var metrics = _repository.GetAll();
            var response = new AllHddMetricsResponse() { Metrics = new List<HddMetricDto>() };
            
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));
            }

            _logger.LogInformation("Запрос всех метрик Hdd(FreeSize)");

            return Ok(response);
        }
    }
}
