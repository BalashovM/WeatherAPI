using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{ 
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private SQLiteConnection _connection;
        public DotNetMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(DotNetMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO dotnetmetrics(idagent, value, time) VALUES(@idagent, @value, @time)",
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
            connection.Execute("DELETE FROM dotnetmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public void Update(DotNetMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE dotnetmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                idagent = item.IdAgent,
                id = item.Id
            });
        }

        public IList<DotNetMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetricModel>("SELECT id, time, value, idagent From dotnetmetrics")
                .ToList();
        }

        public DotNetMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<DotNetMetricModel>("SELECT id, time, value, idagent  FROM dotnetmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public IList<DotNetMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetricModel>(
                    "SELECT id, time, value, idagent From dotnetmetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetricModel>(
                    "SELECT id, time, value, idagent From dotnetmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)",
                    new
                    {
                        idagent = idAgent,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetricModel>($"SELECT * FROM dotnetmetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<DotNetMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetricModel>($"SELECT * FROM dotnetmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY {sortingField}",
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
