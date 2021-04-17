using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.DBSettings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        /// <summary>
        /// Объект с именами и настройками базы данных
        /// </summary>
        private readonly IDBSettings _dbSettings;
        private readonly ILogger _logger;

        public AgentsRepository(IDBSettings dbSettings, ILogger<AgentsRepository> logger)
        {
            _dbSettings = dbSettings;
            _logger = logger;
        }

        public void Create(AgentModel item)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(
                    $"INSERT INTO {_dbSettings[Tables.Agent]}" +
                    $"({_dbSettings[Columns.Name]}, {_dbSettings[Columns.IpAddress]}, {_dbSettings[Columns.Status]}) " +
                    $"VALUES(@name, @ipaddress, @status)",
                new
                {
                    name = item.Name,
                    ipaddress = item.IpAddress,
                    status = Convert.ToInt32(item.Status)
                });
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogDebug(ex.Message);
            }
        }

        public void Delete(int id)
        {

            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(
                            $"DELETE FROM {_dbSettings[Tables.Agent]} " +
                            $"WHERE {_dbSettings[Columns.Id]} = @id",
                        new
                        {
                            id = id
                        });

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogDebug(ex.Message);
            }
        }

        public void Update(AgentModel item)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(
                            $"UPDATE {_dbSettings[Tables.Agent]} " +
                            $"SET {_dbSettings[Columns.Name]} = @name, {_dbSettings[Columns.IpAddress]} = @ipaddress, {_dbSettings[Columns.Status]} = @status " +
                            $"WHERE {_dbSettings[Columns.Id]} = @id",
                        new
                        {
                            name = item.Name,
                            ipaddress = item.IpAddress,
                            status = Convert.ToInt32(item.Status),
                            id = item.Id
                        });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogDebug(ex.Message);
            }
        }

        public IList<AgentModel> GetAll()
        {

            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<AgentModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Name]}, {_dbSettings[Columns.IpAddress]}, {_dbSettings[Columns.Status]} " +
                $"FROM {_dbSettings[Tables.Agent]}")
                .ToList();
        }

        public IList<AgentModel> GetAllActive()
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<AgentModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Name]}, {_dbSettings[Columns.IpAddress]}, {_dbSettings[Columns.Status]} " +
                $"FROM {_dbSettings[Tables.Agent]} " +
                $"WHERE {_dbSettings[Columns.Status]} = 1")
                .ToList();
        }

        public AgentModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .QuerySingle<AgentModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Name]}, {_dbSettings[Columns.IpAddress]}, {_dbSettings[Columns.Status]} " +
                $"FROM {_dbSettings[Tables.Agent]} " +
                $"WHERE {_dbSettings[Columns.Id]} = @id",
                new
                {
                    id = id
                });
        }
    }
}
