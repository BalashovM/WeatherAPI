using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private readonly AgentsController _controller;
        private readonly Mock<ILogger<AgentsController>> _logger;
        private readonly Mock<IAgentsRepository> _agentsRepository;

        public AgentsControllerUnitTests()
        {
            _logger = new Mock<ILogger<AgentsController>>();
            _agentsRepository = new Mock<IAgentsRepository>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
            var mapper = new Mapper(configuration);

            _controller = new AgentsController(mapper, _agentsRepository.Object, _logger.Object);
        }

        [Fact]
        public void RegisterCheckRequestCreate()
        {
            //Arrange
            _agentsRepository.Setup(a => a.Create(new AgentModel())).Verifiable();
            //Act
            var result = _controller.RegisterAgent(new AgentModel());
            //Assert
            _agentsRepository.Verify(repository => repository.Create(new AgentModel()), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void EnableAgentByIdCheckRequestCreate()
        {
            //Arrange
            _agentsRepository.Setup(a => a.GetById(0)).Returns(new AgentModel()).Verifiable();
            //Act
            var result = _controller.EnableAgentById(0);
            //Assert
            _agentsRepository.Verify(repository => repository.GetById(0), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void DisableAgentByIdCheckRequestCreate()
        {
            //Arrange
            _agentsRepository.Setup(a => a.GetById(0)).Returns(new AgentModel()).Verifiable();
            //Act
            var result = _controller.DisableAgentById(0);
            //Assert
            _agentsRepository.Verify(repository => repository.GetById(0), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetAllAgentsCheckRequestCreate()
        {
            //Arrange
            _agentsRepository.Setup(a => a.GetAll()).Returns(new List<AgentModel>()).Verifiable();
            //Act
            var result = _controller.GetAll();
            //Assert
            _agentsRepository.Verify(repository => repository.GetAll(), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
