using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
        IList<NetworkMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<NetworkMetric> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField);
    }

    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private SQLiteConnection _connection;
        public NetworkMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(NetworkMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(@value, @time)";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);
            
            cmd.CommandText = "DELETE FROM networkmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(NetworkMetric item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE networkmetrics SET value = @value, time = @time WHERE id = @id";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<NetworkMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics";

            var returnList = new List<NetworkMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public NetworkMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<NetworkMetric> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"SELECT * FROM networkmetrics WHERE time >  @timeFrom AND time < @timeTo";
            cmd.Parameters.AddWithValue("@timeFrom", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@timeTo", toTime.TotalSeconds);
  
            var returnList = new List<NetworkMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }

        public IList<NetworkMetric> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"SELECT * FROM networkmetrics WHERE time > @timeFrom AND time < @timeTo ORDER BY @sortingField";
            cmd.Parameters.AddWithValue("@timeFrom", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@timeTo", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@sortingField", sortingField);

            var returnList = new List<NetworkMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }
            return returnList;
        }
    }
}
