using Microsoft.AspNetCore.Mvc;
using System;
using MetricsManager.Controllers;
using MetricsManager.Enums;
using Xunit;
using MetricsManager;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController _controller;
        public AgentsControllerUnitTests()
        {
            _controller = new AgentsController();
        }

        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            //Arrange
            var agentInfo = new AgentInfo();
            //Act
            var result = _controller.RegisterAgent(agentInfo);
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            //Arrange
            var agentId = 1;
            //Act
            var result = _controller.EnableAgentById(agentId);
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetRegistredObjects_ReturnsOk()
        {
            //Arrange
            
            //Act
            var result = _controller.GetRegistredObjects();
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
