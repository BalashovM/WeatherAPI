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
        private readonly HddMetricsController _controller;
        private readonly Mock<IHddMetricsRepository> _mock;
        private readonly Mock<ILogger<HddMetricsController>> _logger;

        public HddControllerUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();
            _logger = new Mock<ILogger<HddMetricsController>>();
            _controller = new HddMetricsController(_mock.Object, _logger.Object);
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
        [Fact]
        public void CreateShouldCallCreateFromRepository()
        {
            //Arrange
            _mock.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();
            //Assert
            _mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }
    }
}

