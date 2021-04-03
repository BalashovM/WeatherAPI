using Microsoft.AspNetCore.Mvc;
using System;
using MetricsManager.Controllers;
using MetricsManager.Enums;
using Xunit;

namespace MetricsManagerTests
{
    public class HddControllerUnitTests
    {
        private HddMetricsController _controller;
        public HddControllerUnitTests()
        {
            _controller = new HddMetricsController();
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var agentId = 1;
            //Act
            var result = _controller.GetMetricsFromAgent(agentId);
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
