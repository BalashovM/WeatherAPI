using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL
{ 
    public interface IDotNetMetricsRepository : IRepository<DotNetMetricModel>
    {
        IList<DotNetMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<DotNetMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent);
        IList<DotNetMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string sortingField);
        IList<DotNetMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string sortingField, int idAgent);
    }

    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private SQLiteConnection _connection;
        public DotNetMetricsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(DotNetMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO dotnetmetrics(idagent, value, time) VALUES(@idagent, @value, @time)";
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM dotnetmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(DotNetMetricModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE dotnetmetrics SET value = @value, time = @time, idagent = @idagent WHERE id = @id";
            cmd.Parameters.AddWithValue("@value", item.Value);
            cmd.Parameters.AddWithValue("@time", item.Time.TotalSeconds);
            cmd.Parameters.AddWithValue("@idagent", item.IdAgent);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetricModel> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics";

            var returnList = new List<DotNetMetricModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetricModel
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

        public DotNetMetricModel GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new DotNetMetricModel
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

        public IList<DotNetMetricModel> GetByPeriod(TimeSpan fromTime, TimeSpan toTime)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"SELECT * FROM dotnetmetrics WHERE time > @fromtime AND time < @totime";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<DotNetMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetricModel
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

        public IList<DotNetMetricModel> GetByPeriodFromAgent(TimeSpan fromTime, TimeSpan toTime, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime)";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);

            var returnList = new List<DotNetMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetricModel
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

        public IList<DotNetMetricModel> GetByPeriodWithSort(TimeSpan fromTime, TimeSpan toTime, string field)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE time > @fromtime AND time < @totime ORDER BY @field";
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<DotNetMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetricModel
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

        public IList<DotNetMetricModel> GetByPeriodWithSortFromAgent(TimeSpan fromTime, TimeSpan toTime, string field, int idAgent)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE idagent = @idagent AND (time > @fromtime AND time < @totime) ORDER BY @field";
            cmd.Parameters.AddWithValue("@idagent", idAgent);
            cmd.Parameters.AddWithValue("@fromtime", fromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", toTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@field", field);

            var returnList = new List<DotNetMetricModel>();
            
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new DotNetMetricModel
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
