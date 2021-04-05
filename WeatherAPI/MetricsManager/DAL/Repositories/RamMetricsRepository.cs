using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{   
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private SQLiteConnection _connection;
        public RamMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(RamMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO rammetrics(idagent, value, time) VALUES(@idagent, @value, @time)",
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
            connection.Execute("DELETE FROM rammetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public void Update(RamMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE rammetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                idagent = item.IdAgent,
                id = item.Id
            });
        }

        public IList<RamMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetricModel>("SELECT id, time, value, idagent FROM rammetrics")
                .ToList();
        }

        public RamMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<RamMetricModel>("SELECT id, time, value, idagent FROM rammetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public IList<RamMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetricModel>(
                    "SELECT id, time, value, idagent From rammetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<RamMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetricModel>(
                    "SELECT id, time, value, idagent From rammetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)",
                    new
                    {
                        idagent = idAgent,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<RamMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetricModel>($"SELECT * FROM rammetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<RamMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetricModel>($"SELECT * FROM rammetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY {sortingField}",
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
