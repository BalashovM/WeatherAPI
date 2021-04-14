using Dapper;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.DAL.Repositories
{ 
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private SQLiteConnection _connection;
        public CpuMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(CpuMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO cpumetrics(idagent, value, time) VALUES(@idagent, @value, @time)",
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
            connection.Execute("DELETE FROM cpumetrics WHERE id = @id",
            new
            {
                id = id
            });
        }
  
        public void Update(CpuMetricModel item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE cpumetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                idagent = item.IdAgent,
                id = item.Id
            });
        }

        public IList<CpuMetricModel> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<CpuMetricModel>("SELECT id, time, value, idagent FROM cpumetrics")
                .ToList();
        }

        public CpuMetricModel GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<CpuMetricModel>("SELECT id, time, value, idagent FROM cpumetrics WHERE id = @id",
            new
            {
                id = id
            });
        }

        public IList<CpuMetricModel> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<CpuMetricModel>(
                    "SELECT id, time, value, idagent From cpumetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<CpuMetricModel> GetByPeriodFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<CpuMetricModel>(
                    "SELECT id, time, value, idagent From cpumetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)",
                    new
                    {
                        idagent = idAgent,
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<CpuMetricModel> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<CpuMetricModel>($"SELECT * FROM cpumetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }

        public IList<CpuMetricModel> GetByPeriodWithSortingFromAgent(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField, int idAgent)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<CpuMetricModel>($"SELECT * FROM cpumetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY {sortingField}",
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
