using MetricsLibrary;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class NetworkControllerUnitTests
    {
        private NetworkMetricsController _controller;
        private ILogger<NetworkMetricsController> _logger;
        private Mock<INetworkMetricsRepository> _mock;

        public NetworkControllerUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _controller = new NetworkMetricsController(_mock.Object, _logger);
        }

        [Fact]
        public void GetMetricsFromAgentCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            int agentId = 1;
            _mock.Setup(a => a.GetByPeriodFromAgent(fromTime, toTime, agentId)).Returns(new List<NetworkMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodFromAgent(fromTime, toTime, agentId), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetricsByPercentileFromAgentCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            int agentId = 1;
            Percentile percentile = Percentile.P99;
            string sort = "value";
            _mock.Setup(a => a.GetByPeriodWithSortFromAgent (fromTime, toTime, sort, agentId)).Returns(new List<NetworkMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSortFromAgent(fromTime, toTime, sort, agentId), Times.AtMostOnce());
        }

        [Fact]
        public void GetNetworkMetricsFromClusterCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<NetworkMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriod(fromTime, toTime), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetricsByPercentileFromClusterCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            Percentile percentile = Percentile.P99;
            string sort = "value";
            _mock.Setup(a => a.GetByPeriodWithSort(fromTime, toTime, sort)).Returns(new List<NetworkMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAllCluster(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSort(fromTime, toTime, sort), Times.AtMostOnce());
        }
    }
}
