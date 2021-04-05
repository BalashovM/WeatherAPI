using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

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

            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetric, HddMetricDto>());
            IMapper mapper = config.CreateMapper();

            _controller = new HddMetricsController(mapper, _mock.Object, _logger.Object);
        }

        [Fact]
        public void GetMetricsFreeHddCheckRequest()
        {
            //Arrange
            _mock.Setup(a => a.GetAll()).Returns(new List<HddMetric>()).Verifiable();
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
            _mock.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();
            //Assert
            _mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}

