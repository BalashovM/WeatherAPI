using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        private SQLiteConnection _connection;
        public AgentsRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void Create(AgentModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "INSERT INTO agents(status, ipaddress, name) VALUES(@status, @ipaddress, @name)";
            cmd.Parameters.AddWithValue("@status", Convert.ToInt32(item.Status));
            cmd.Parameters.AddWithValue("@ipaddress", item.IpAddress);
            cmd.Parameters.AddWithValue("@name", item.Name);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "DELETE FROM agents WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(AgentModel item)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "UPDATE agents SET status = @status, ipaddress = @ipaddress, name = @name WHERE id = @id";
            cmd.Parameters.AddWithValue("@status", Convert.ToInt32(item.Status));
            cmd.Parameters.AddWithValue("@ipaddress", item.IpAddress);
            cmd.Parameters.AddWithValue("@name", item.Name);
            cmd.Parameters.AddWithValue("@id", item.Id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public IList<AgentModel> GetAll()
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = "SELECT * FROM agents";

            var returnList = new List<AgentModel>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    returnList.Add(new AgentModel
                    {
                        Id = reader.GetInt32(0),
                        Status = Convert.ToBoolean(reader.GetInt32(1)),
                        IpAddress = reader.GetString(2),
                        Name = reader.GetString(3)
                    });
                }
            }
            return returnList;
        }

        public AgentModel GetById(int id)
        {
            using var cmd = new SQLiteCommand(_connection);

            cmd.CommandText = $"SELECT * FROM agents WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new AgentModel
                    {
                        Id = reader.GetInt32(0),
                        Status = Convert.ToBoolean(reader.GetInt32(1)),
                        IpAddress = reader.GetString(2),
                        Name = reader.GetString(3)
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
