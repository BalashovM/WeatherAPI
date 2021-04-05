using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{ 
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private SQLiteConnection _connection;
        public HddMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(HddMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO hddmetrics(idagent, value, time) VALUES(@idagent, @value, @time)",
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
            connection.Execute("DELETE FROM hddmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public void Update(HddMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE hddmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                idagent = item.IdAgent,
                id = item.Id
            });
        }

        public IList<HddMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetricModel>("SELECT id, time, value, idagent FROM hddmetrics")
                .ToList();
        }

        public HddMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<HddMetricModel>("SELECT id, time, value, idagent FROM hddmetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public IList<HddMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetricModel>(
                    "SELECT id, time, value, idagent From hddmetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<HddMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetricModel>(
                    "SELECT id, time, value, idagent From hddmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)",
                    new
                    {
                        idagent = idAgent,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<HddMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetricModel>($"SELECT * FROM hddmetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<HddMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetricModel>($"SELECT * FROM hddmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY {sortingField}",
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
