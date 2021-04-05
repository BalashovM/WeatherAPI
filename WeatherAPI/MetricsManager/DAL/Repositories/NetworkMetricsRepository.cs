using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{ 
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private SQLiteConnection _connection;
        public NetworkMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(NetworkMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO networkmetrics(idagent, value, time) VALUES(@idagent, @value, @time)",
            new
            {
                idagent = item.IdAgent,
                value = item.Value,
                time = item.Time
            });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("DELETE FROM networkmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public void Update(NetworkMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE networkmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                idagent = item.IdAgent,
                id = item.Id
            });
        }

        public IList<NetworkMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<NetworkMetricModel>("SELECT id, time, value, idagent FROM networkmetrics")
                .ToList();
        }

        public NetworkMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<NetworkMetricModel>("SELECT id, time, value, idagent FROM networkmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public IList<NetworkMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<NetworkMetricModel>(
                    "SELECT id, time, value, idagent From networkmetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<NetworkMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<NetworkMetricModel>(
                    "SELECT id, time, value, idagent From networkmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)",
                    new
                    {
                        idagent = idAgent,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<NetworkMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<NetworkMetricModel>($"SELECT * FROM networkmetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<NetworkMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<NetworkMetricModel>($"SELECT * FROM networkmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY {sortingField}",
                    new
                    {
                        idagent = idAgent,
                        fromtime = fromTime.ToUnixTimeSeconds(),
                        totime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }
    }
}
