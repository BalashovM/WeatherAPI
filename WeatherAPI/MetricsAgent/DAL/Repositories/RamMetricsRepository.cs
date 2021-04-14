using Dapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DAL.Repositories
{
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private SQLiteConnection _connection;
        public RamMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(RamMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("INSERT INTO rammetrics (available, time) VALUES(@available, @time)",
                new
                {
                    available = item.Available,
                    time = item.Time
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("DELETE FROM rammetrics WHERE id=@id",
                new
                {
                    id = id
                });
        }

        public void Update(RamMetric item)
        {
            using var connection = new SQLiteConnection(_connection);
            connection.Execute("UPDATE rammetrics SET available = @available, time = @time WHERE id = @id",
                new
                {
                    available = item.Available,
                    time = item.Time,
                    id = item.Id
                });
        }

        public IList<RamMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .Query<RamMetric>("SELECT id, time, available From rammetrics")
                .ToList();
        }

        public RamMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_connection);
            return connection
                .QuerySingle<RamMetric>("SELECT id, time, available FROM rammetrics WHERE id = @id",
                    new
                    {
                        id = id
                    });
        }
    }
}
