using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsLibrary;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkControllerUnitTests
    {
        private readonly NetworkMetricsController _controller;
        private readonly Mock<INetworkMetricsRepository> _mock;
        private readonly Mock<ILogger<NetworkMetricsController>> _logger;

        public NetworkControllerUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _logger = new Mock<ILogger<NetworkMetricsController>>();
            _controller = new NetworkMetricsController(_mock.Object, _logger.Object);
        }

        [Fact]
        public void GetByPeriodCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(1);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<NetworkMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent(fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriod(fromTime, toTime), Times.AtMostOnce());
        }

        [Fact]
        public void GetByPeriodWithSortPercentileCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(1);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            Percentile percentile = Percentile.P99;
            string sort = "value";
            _mock.Setup(a => a.GetByPeriodWithSort(fromTime, toTime, sort)).Returns(new List<NetworkMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSort(fromTime, toTime, sort), Times.AtMostOnce());
        }

        [Fact]
        public void CreateShouldCallCreateFromRepository()
        {
            //Arrange
            _mock.Setup(repository => repository.Create(It.IsAny<NetworkMetric>())).Verifiable();
            //Assert
            _mock.Verify(repository => repository.Create(It.IsAny<NetworkMetric>()), Times.AtMostOnce());
        }
    }
}
