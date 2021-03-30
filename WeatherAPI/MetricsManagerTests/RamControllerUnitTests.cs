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
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;
        private ILogger<RamMetricsController> _logger;
        private Mock<IRamMetricsRepository> _mock;

        public RamControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            _controller = new RamMetricsController(_mock.Object, _logger);
        }

        [Fact]
        public void GetMetricsFromAgentCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            int agentId = 1;
            _mock.Setup(a => a.GetByPeriodFromAgent(fromTime, toTime, agentId)).Returns(new List<RamMetricModel>()).Verifiable();
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
            _mock.Setup(a => a.GetByPeriodWithSortFromAgent(fromTime, toTime, sort, agentId))
                .Returns(new List<RamMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSortFromAgent(fromTime, toTime, sort, agentId), Times.AtMostOnce());
        }

        [Fact]
        public void GetRamMetricsFromClusterCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(5);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<RamMetricModel>()).Verifiable();
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
            _mock.Setup(a => a.GetByPeriodWithSort(fromTime, toTime, sort))
                .Returns(new List<RamMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromCluster(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSort(fromTime, toTime, sort), Times.AtMostOnce());
        }
    }
}

