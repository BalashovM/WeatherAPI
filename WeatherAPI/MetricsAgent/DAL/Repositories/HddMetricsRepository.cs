using Dapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DAL.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly SQLiteConnection _connection;
        public HddMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(HddMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO hddmetrics (freesize, time) VALUES(@freesize, @time)",
                new
                {
                    freesize = item.FreeSize,
                    time = item.Time.ToUnixTimeSeconds()
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

        public void Update(HddMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE hddmetrics SET freesize = @freesize, time = @time WHERE id = @id",
                new
                {
                    freesize = item.FreeSize,
                    time = item.Time.ToUnixTimeSeconds(),
                    id = item.Id
                });
        }

        public IList<HddMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<HddMetric>($"SELECT id, time, freesize From hddmetrics")
                .ToList();
        }

        public HddMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<HddMetric>("SELECT id, time, freesize FROM hddmetrics WHERE id = @id",
                    new
                    {
                        id = id
                    });
        }
    }
}
