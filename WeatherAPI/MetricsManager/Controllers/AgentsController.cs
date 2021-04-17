﻿using AutoMapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly IAgentsRepository _repository;
        private readonly IMapper _mapper;

        public AgentsController(IMapper mapper, IAgentsRepository repository, ILogger<AgentsController> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentModel agentInfo)
        {
            _repository.Create(agentInfo);

            _logger.LogInformation("Добавление в базу агента: " +
                                   $"Id = {agentInfo.Id}" +
                                   $" IpAddress = {agentInfo.IpAddress}" +
                                   $" Name = {agentInfo.Name}" +
                                   $" Status = {agentInfo.Status}");
            
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            AgentModel agent = _repository.GetById(agentId);
            agent.Status = true;
            _repository.Update(agent);

            _logger.LogInformation($"Подключение агента {agentId}");

            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            AgentModel agent = _repository.GetById(agentId);
            agent.Status = false;
            _repository.Update(agent);

            _logger.LogInformation($"Отключение агента {agentId}");

            return Ok();
        }

        [HttpGet("registred")]
        public IActionResult GetAll()
        {
            var allAgentsInfo = _repository.GetAll();
            var response = new AllAgentsResponse()
            {
                Metrics = new List<AgentManagerDto>()
            };

            foreach (var agentInfo in allAgentsInfo)
            {
                response.Metrics.Add(_mapper.Map<AgentManagerDto>(agentInfo));
            }

            _logger.LogInformation("Запрос всех агентов");

            return Ok(response);
        }
    }
}
