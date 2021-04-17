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
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        /// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IDBSettings _dbSettings;
        private readonly ILogger _logger;
        public DotNetMetricsRepository(IDBSettings dbSettings, ILogger<CpuMetricsRepository> logger)
        {
            _dbSettings = dbSettings;
            _logger = logger;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(DotNetMetricModel item)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(
                    $"INSERT INTO {_dbSettings[Tables.DotNetMetric]}" +
                    $"({_dbSettings[Columns.AgentId]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.Time]}) " +
                    $"VALUES(@agentid, @value, @time)",
                new
                {
                    agentid = item.AgentId,
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
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
                            $"DELETE FROM {_dbSettings[Tables.DotNetMetric]} " +
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

        public void Update(DotNetMetricModel item)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                connection.Execute(
                            $"UPDATE {_dbSettings[Tables.DotNetMetric]} " +
                            $"SET {_dbSettings[Columns.Value]} = @value, {_dbSettings[Columns.Time]} = @time, {_dbSettings[Columns.AgentId]} = @agentid " +
                            $"WHERE {_dbSettings[Columns.Id]} = @id",
                        new
                        {
                            value = item.Value,
                            time = item.Time,
                            agentid = item.AgentId,
                            id = item.Id
                        });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogDebug(ex.Message);
            }
        }

        public IList<DotNetMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<DotNetMetricModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                $"FROM {_dbSettings[Tables.DotNetMetric]}")
                .ToList();
        }

        public DotNetMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .QuerySingle<DotNetMetricModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                $"WHERE {_dbSettings[Columns.Id]} = @id",
            new
            {
                id = id
            });
        }

        public IList<DotNetMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<DotNetMetricModel>(
                    $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                    $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                    $"WHERE {_dbSettings[Columns.Time]} >= @fromTime AND {_dbSettings[Columns.Time]} <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int agentId)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<DotNetMetricModel>(
                    $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                    $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                    $"WHERE {_dbSettings[Columns.AgentId]} = @agentid AND ({_dbSettings[Columns.Time]} >= @fromtime AND {_dbSettings[Columns.Time]} <= @totime)",
                    new
                    {
                        agentid = agentId,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<DotNetMetricModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                $"WHERE {_dbSettings[Columns.Time]} >= @fromTime AND {_dbSettings[Columns.Time]} <= @toTime " +
                $"ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int agentId)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .Query<DotNetMetricModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                $"WHERE {_dbSettings[Columns.AgentId]} = @agentid AND ({_dbSettings[Columns.Time]} >= @fromtime AND {_dbSettings[Columns.Time]} <= @totime) " +
                $"ORDER BY {sortingField}",
                    new
                    {
                        agentid = agentId,
                        fromtime = fromTime.ToUnixTimeSeconds(),
                        totime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }
        public DotNetMetricModel GetLast(int agentId)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            return connection
                .QuerySingle<DotNetMetricModel>(
                $"SELECT {_dbSettings[Columns.Id]}, {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]}, {_dbSettings[Columns.AgentId]} " +
                $"FROM {_dbSettings[Tables.DotNetMetric]} " +
                $"ORDER BY {_dbSettings[Columns.Time]} DESC " +
                $"LIMIT 1")
                ?? new DotNetMetricModel();
        }
    }
}
