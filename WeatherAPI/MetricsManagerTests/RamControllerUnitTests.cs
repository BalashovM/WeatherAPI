using Microsoft.AspNetCore.Mvc;
using System;
using MetricsManager.Controllers;
using MetricsManager.Enums;
using Xunit;

namespace MetricsManagerTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;
        public RamControllerUnitTests()
        {
            _controller = new RamMetricsController();
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var agentId = 1;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);
            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}

