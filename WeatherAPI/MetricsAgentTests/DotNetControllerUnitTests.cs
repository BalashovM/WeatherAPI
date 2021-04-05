using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;
using MetricsLibrary;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class DotNetControllerUnitTests
    {
        private readonly DotNetMetricsController _controller;
        private readonly Mock<IDotNetMetricsRepository> _mock;
        private readonly Mock<ILogger<DotNetMetricsController>> _logger;

        public DotNetControllerUnitTests()
        {
            _mock = new Mock<IDotNetMetricsRepository>();
            _logger = new Mock<ILogger<DotNetMetricsController>>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetric, DotNetMetricDto>());
            IMapper mapper = config.CreateMapper();

            _controller = new DotNetMetricsController(mapper, _mock.Object, _logger.Object);
        }

        [Fact]
        public void GetByPeriodCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);

            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<DotNetMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriod(fromTime, toTime), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetByPeriodPercentileCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            Percentile percentile = Percentile.P99;
            string sort = "value";

            _mock.Setup(a => a.GetByPeriodWithSorting(fromTime, toTime, sort)).Returns(new List<DotNetMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSorting(fromTime, toTime, sort), Times.AtMostOnce());
            _logger.Verify();
        }
        
        [Fact]
        public void CreateShouldCallCreateFromRepository()
        {
            //Arrange
            _mock.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();
            //Assert
            _mock.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
