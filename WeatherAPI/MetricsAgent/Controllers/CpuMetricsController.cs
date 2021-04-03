﻿using MetricsAgent.Enums;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
		[HttpGet("from/{fromTime}/to/{toTime}")]
		public IActionResult GetMetricsFromAgent(
			   [FromRoute] TimeSpan fromTime,
			   [FromRoute] TimeSpan toTime)
		{
			return Ok();
		}

		[HttpGet("from/{fromTime}/to/{toTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] TimeSpan fromTime,
			[FromRoute] TimeSpan toTime,
			[FromRoute] Percentile percentile)
		{
			return Ok();
		}
	}
}