using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL
{ 
    public interface IHddMetricsRepository : IRepository<HddMetricModel>
    {
        IList<HddMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<HddMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent);
        IList<HddMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField);
        IList<HddMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string sortingField, int idAgent);
    }

    public class HddMetricsRepository : IHddMetricsRepository
    {
        private SQLiteConnection _connection;
        public HddMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(HddMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO hddmetrics(idagent, value, time) VALUES(@idagent, @value, @time)";
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM hddmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(HddMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE hddmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<HddMetricModel> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM hddmetrics";

            var returnList = new List<HddMetricModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new HddMetricModel
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

        public HddMetricModel GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"SELECT * FROM hddmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new HddMetricModel
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

        public IList<HddMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM hddmetrics WHERE time > @fromtime AND time < @totime";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<HddMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new HddMetricModel
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

        public IList<HddMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM hddmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<HddMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new HddMetricModel
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

        public IList<HddMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string field)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM hddmetrics WHERE time > @fromtime AND time < @totime ORDER BY @field";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<HddMetricModel>();
           
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new HddMetricModel
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

        public IList<HddMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string field, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM hddmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY @field";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<HddMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new HddMetricModel
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
