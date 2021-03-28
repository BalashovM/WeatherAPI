using MetricsAgent.Controllers;
using System.Collections.Generic;
using MetricsAgent.Models;
using Xunit;
using Moq;
using MetricsAgent.DAL;
using Microsoft.Extensions.Logging;

namespace MetricsAgentTests
{
    public class HddControllerUnitTests
    {
        private ILogger<HddMetricsController> _logger;
        private HddMetricsController _controller;
        private Mock<IHddMetricsRepository> _mock;

        public HddControllerUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();
            _controller = new HddMetricsController(_mock.Object, _logger);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            _mock.Setup(a => a.GetAll()).Returns(new List<HddMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent();
            // Assert
            _mock.Verify(repository => repository.GetAll(), Times.AtMostOnce());
        }
    }
}

