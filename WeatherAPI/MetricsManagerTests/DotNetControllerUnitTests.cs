using AutoMapper;
using MetricsLibrary;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class DotNetControllerUnitTests
    {
        private DotNetMetricsController _controller;
        private Mock<ILogger<DotNetMetricsController>> _logger;
        private Mock<IDotNetMetricsRepository> _mock;

        public DotNetControllerUnitTests()
        {
            _logger = new Mock<ILogger<DotNetMetricsController>>();
            _mock = new Mock<IDotNetMetricsRepository>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DotNetMetricModel, DotNetMetricManagerDto>());
            IMapper mapper = config.CreateMapper();

            _controller = new DotNetMetricsController(mapper, _mock.Object, _logger.Object);
        }

        [Fact]
        public void GetMetricsFromAgentCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            int agentId = 1;

            _mock.Setup(a => a.GetByPeriodFromAgent(fromTime, toTime, agentId)).Returns(new List<DotNetMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodFromAgent(fromTime, toTime, agentId), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetMetricsByPercentileFromAgentCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            int agentId = 1;
            Percentile percentile = Percentile.P99;
            string sort = "value";

            _mock.Setup(a => a.GetByPeriodWithSortingFromAgent(fromTime, toTime, sort, agentId))
                .Returns(new List<DotNetMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, sort, agentId), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetDotNetMetricsFromClusterCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);

            _mock.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<DotNetMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);
            //Assert
            _mock.Verify(repository => repository.GetByPeriod(fromTime, toTime), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetMetricsByPercentileFromClusterCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            Percentile percentile = Percentile.P99;
            string sort = "value";

            _mock.Setup(a => a.GetByPeriodWithSorting(fromTime, toTime, sort)).Returns(new List<DotNetMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAllCluster(fromTime, toTime, percentile);
            //Assert
            _mock.Verify(repository => repository.GetByPeriodWithSorting(fromTime, toTime, sort), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
