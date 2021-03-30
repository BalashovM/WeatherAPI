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
    public class CpuControllerUnitTests
    {
        private ILogger<CpuMetricsController> _logger;
        private CpuMetricsController _controller;
        private Mock<ICpuMetricsRepository> _mock;

        public CpuControllerUnitTests()
        {
            _mock = new Mock<ICpuMetricsRepository>();
            _controller = new CpuMetricsController(_mock.Object, _logger);
        }

        [Fact]
        public void GetByPeriodCheckRequestSelect()
        {
            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(1);
            TimeSpan toTime = TimeSpan.FromSeconds(10);
            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<CpuMetric>()).Verifiable();

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
            _mock.Setup(a => a.GetByPeriodWithSort(fromTime, toTime, sort)).Returns(new List<CpuMetric>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSort(fromTime, toTime, sort), Times.AtMostOnce());
        }
    }
}
