using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class RamControllerUnitTests
    {
        private  RamMetricsController _controller;
        private readonly Mock<IRamMetricsRepository> _mock;
        private readonly Mock<ILogger<RamMetricsController>> _logger;

        public RamControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            _logger = new Mock<ILogger<RamMetricsController>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            IMapper mapper = config.CreateMapper();

            _controller = new RamMetricsController(mapper, _mock.Object, _logger.Object);
        }

        [Fact]
        public void GetMetricsAvailableCheckRequestSelect()
        {
            //Arrange
            _mock.Setup(a => a.GetAll()).Returns(new List<RamMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent();
            // Assert
            _mock.Verify(repository => repository.GetAll(), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void CreateShouldCallCreateFromRepository()
        {
            //Arrange
            _mock.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();
            //Assert
            _mock.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
