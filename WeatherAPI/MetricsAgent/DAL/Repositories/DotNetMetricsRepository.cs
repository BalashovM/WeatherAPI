using Dapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DAL.Repositories
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly SQLiteConnection _connection;

        public DotNetMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO dotnetmetrics (value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
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

        public void Update(DotNetMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE dotnetmetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    id = item.Id
                });
        }

        public IList<DotNetMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetric>($"SELECT id, time, value From dotnetmetrics")
                .ToList();
        }

        public DotNetMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<DotNetMetric>("SELECT Id, Time, Value FROM dotnetmetrics WHERE id = @id",
                    new
                    {
                        id = id
                    });
        }

        public IList<DotNetMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetric>(
                    "SELECT id, time, value From dotnetmetrics WHERE time > @fromTime AND time < @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }
        
        public IList<DotNetMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<DotNetMetric>($"SELECT * FROM dotnetmetrics WHERE time > @fromTime AND time < @toTime ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                .ToList();
        }
    }
}
