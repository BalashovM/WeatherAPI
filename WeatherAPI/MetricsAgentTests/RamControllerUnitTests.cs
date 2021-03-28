using MetricsAgent.Controllers;
using System.Collections.Generic;
using MetricsAgent.Models;
using Xunit;
using Moq;
using MetricsAgent.DAL;
using Microsoft.Extensions.Logging;

namespace MetricsAgentTests
{
    public class RamControllerUnitTests
    {
        private ILogger<RamMetricsController> _logger;
        private RamMetricsController _controller;
        private Mock<IRamMetricsRepository> _mock;

        public RamControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            _controller = new RamMetricsController(_mock.Object, _logger);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            _mock.Setup(a => a.GetAll()).Returns(new List<RamMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent();
            // Assert
            _mock.Verify(repository => repository.GetAll(), Times.AtMostOnce());
        }
    }
}
