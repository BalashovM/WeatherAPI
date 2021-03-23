using Microsoft.AspNetCore.Mvc;
using System;
using WeatherAPI.Controllers;
using WeatherAPI.Enums;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuControllerUnitTests
    {
        private CpuMetricsController _controller;
        public CpuControllerUnitTests()
        {
            _controller = new CpuMetricsController();
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsByPercentileFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            var precentile = Percentile.P99;

            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, precentile);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
