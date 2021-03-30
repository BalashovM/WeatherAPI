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
        private ILogger<NetworkMetricsController> _logger;
        private NetworkMetricsController _controller;
        private Mock<INetworkMetricsRepository> _mock;

        public NetworkControllerUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _controller = new NetworkMetricsController(_mock.Object, _logger);
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
    }
}
