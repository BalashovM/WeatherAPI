using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL
{ 
    public interface IRamMetricsRepository : IRepository<RamMetricModel>
    {
        IList<RamMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<RamMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent);
        IList<RamMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField);
        IList<RamMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string sortingField, int idAgent);
    }

    public class RamMetricsRepository : IRamMetricsRepository
    {
        private SQLiteConnection _connection;
        public RamMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(RamMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO rammetrics(idagent, value, time) VALUES(@idagent, @value, @time)";
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM rammetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(RamMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE rammetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<RamMetricModel> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics";

            var returnList = new List<RamMetricModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public RamMetricModel GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<RamMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE time > @fromtime AND time < @totime";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<RamMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<RamMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<RamMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<RamMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string field)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE time > @fromtime AND time < @totime ORDER BY @field";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<RamMetricModel>();
           
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }

        public IList<RamMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string field, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM rammetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY @field";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<RamMetricModel>();
           
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new RamMetricModel
                    {
                        Id = reader.GetInt32(0),
                        IdAgent = reader.GetInt32(1),
                        Value = reader.GetDouble(2),
                        Time = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }
            return returnList;
        }
    }
}
