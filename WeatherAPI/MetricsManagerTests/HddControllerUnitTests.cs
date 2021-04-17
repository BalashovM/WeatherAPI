﻿using AutoMapper;
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
    public class HddControllerUnitTests
    {
        private readonly HddMetricsController _controller;
        private readonly Mock<ILogger<HddMetricsController>> _logger;
        private readonly Mock<IHddMetricsRepository> _repository;
        private readonly Mock<IAgentsRepository> _agentsRepository;

        public HddControllerUnitTests()
        {
            _logger = new Mock<ILogger<HddMetricsController>>();
            _repository = new Mock<IHddMetricsRepository>();
            _agentsRepository = new Mock<IAgentsRepository>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<HddMetricModel, HddMetricManagerDto>());
            IMapper mapper = config.CreateMapper();

            _controller = new HddMetricsController(mapper, _repository.Object, _agentsRepository.Object, _logger.Object);
        }

        [Fact]
        public void GetMetricsFromAgentCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            int agentId = 1;

            _repository.Setup(a => a.GetByPeriodFromAgent(fromTime, toTime, agentId)).Returns(new List<HddMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            //Assert
            _repository.Verify(repository => repository.GetByPeriodFromAgent(fromTime, toTime, agentId), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetMetricsByPercentileFromAgentCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);
            int agentId = 1;
            Percentile percentile = Percentile.P90;
            string sort = "value";

            _repository.Setup(a => a.GetByPeriodWithSortingFromAgent(fromTime, toTime, sort, agentId))
                .Returns(new List<HddMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromAgent(agentId, fromTime, toTime, percentile);
            //Assert
            _repository.Verify(repository => repository.GetByPeriodWithSortingFromAgent(fromTime, toTime, sort, agentId), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetHddMetricsFromClusterCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);

            _repository.Setup(a => a.GetByPeriod(fromTime, toTime)).Returns(new List<HddMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);
            //Assert
            _repository.Verify(repository => repository.GetByPeriod(fromTime, toTime), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetMetricsByPercentileFromClusterCheckRequestSelect()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(3);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(15);

            Percentile percentile = Percentile.P90;
            string sort = "value";
            _repository.Setup(a => a.GetByPeriodWithSorting(fromTime, toTime, sort))
                .Returns(new List<HddMetricModel>()).Verifiable();
            //Act
            var result = _controller.GetMetricsByPercentileFromCluster(fromTime, toTime, percentile);
            //Assert
            _repository.Verify(repository => repository.GetByPeriodWithSorting(fromTime, toTime, sort), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
