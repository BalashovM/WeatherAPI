using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private ILogger<AgentsController> _logger;
        private AgentsController _controller;
        private Mock<IAgentsRepository> _mock;

        public AgentsControllerUnitTests()
        {
            _mock = new Mock<IAgentsRepository>();
            _controller = new AgentsController(_mock.Object, _logger);
        }

        [Fact]
        public void RegisterCheckRequestCreate()
        {
            //Arrange
            _mock.Setup(a => a.Create(new AgentModel())).Verifiable();
            //Act
            var result = _controller.RegisterAgent(new AgentModel());
            //Assert
            _mock.Verify(repository => repository.Create(new AgentModel()), Times.AtMostOnce());
        }

        [Fact]
        public void EnableAgentByIdCheckRequestCreate()
        {
            //Arrange
            _mock.Setup(a => a.GetById(0)).Returns(new AgentModel()).Verifiable();
            //Act
            var result = _controller.EnableAgentById(0);
            //Assert
            _mock.Verify(repository => repository.GetById(0), Times.AtMostOnce());
        }

        [Fact]
        public void DisableAgentByIdCheckRequestCreate()
        {
            //Arrange
            _mock.Setup(a => a.GetById(0)).Returns(new AgentModel()).Verifiable();
            //Act
            var result = _controller.DisableAgentById(0);
            //Assert
            _mock.Verify(repository => repository.GetById(0), Times.AtMostOnce());
        }

        [Fact]
        public void GetAllAgentsCheckRequestCreate()
        {
            //Arrange
            _mock.Setup(a => a.GetAll()).Returns(new List<AgentModel>()).Verifiable();
            //Act
            var result = _controller.GetAll();
            //Assert
            _mock.Verify(repository => repository.GetAll(), Times.AtMostOnce());
        }
    }
}
