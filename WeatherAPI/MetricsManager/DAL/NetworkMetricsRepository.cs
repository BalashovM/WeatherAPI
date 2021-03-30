using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL
{ 
    public interface INetworkMetricsRepository : IRepository<NetworkMetricModel>
    {
        IList<NetworkMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<NetworkMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent);
        IList<NetworkMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField);
        IList<NetworkMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string sortingField, int idAgent);
    }

    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private SQLiteConnection _connection;
        public NetworkMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(NetworkMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"INSERT INTO networkmetrics(idagent, value, time) VALUES(@idagent, @value, @time)";
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
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

        public void Update(NetworkMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE networkmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<NetworkMetricModel> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics";

            var returnList = new List<NetworkMetricModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public NetworkMetricModel GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<NetworkMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE time > @fromtime AND time < @totime";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<NetworkMetricModel>();
           
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<NetworkMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<NetworkMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<NetworkMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string field)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE time > @fromtime AND time < @totime ORDER BY @field";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<NetworkMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<NetworkMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string field, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM networkmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY @field";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<NetworkMetricModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new NetworkMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetInt32(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }
    }
}
