using Dapper;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Models;
using MetricsAgent.DBSettings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsAgent.DAL.Repositories
{
    /// <summary>
	/// Репозиторий для обработки Ram метрик
	/// </summary>
    public class RamMetricsRepository : IRamMetricsRepository
    {
        /// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IDBSettings _dbSettings;

        public RamMetricsRepository(IDBSettings dbSettings)
        {
            _dbSettings = dbSettings;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(RamMetric item)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                connection.Execute(
                $"INSERT INTO {_dbSettings[Tables.RamMetric]}" +
                $"({_dbSettings[Columns.Value]}, {_dbSettings[Columns.Time]}) " +
                $"VALUES(@value, @time);",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                connection.Execute(
                $"DELETE FROM {_dbSettings[Tables.RamMetric]} " +
                $"WHERE {_dbSettings[Columns.Id]} = @id",
                new
                {
                    id = id
                });
            }
        }

        public void Update(RamMetric item)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                connection.Execute(
                $"UPDATE {_dbSettings[Tables.RamMetric]} " +
                $"SET {_dbSettings[Columns.Value]} = @value, {_dbSettings[Columns.Time]} = @time "+
                $"WHERE {_dbSettings[Columns.Id]} = @id",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    id = item.Id
                });
            }
        }

        public IList<RamMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<RamMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} "+
                    $"FROM {_dbSettings[Tables.RamMetric]}")
                    .ToList();
            }
        }

        public RamMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .QuerySingle<RamMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " + 
                    $"FROM {_dbSettings[Tables.RamMetric]} "+
                    $"WHERE {_dbSettings[Columns.Id]} = @id",
                    new
                    {
                        id = id
                    });
            }
        }

        public IList<RamMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<RamMetric>(
                    $"SELECT  {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " + 
                    $"FROM {_dbSettings[Tables.RamMetric]} " + 
                    $"WHERE {_dbSettings[Columns.Time]} >= @fromTime AND {_dbSettings[Columns.Time]} <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                    .ToList();
            }
        }

        public IList<RamMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<RamMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " + 
                    $"FROM {_dbSettings[Tables.RamMetric]} " + 
                    $"WHERE {_dbSettings[Columns.Time]} >= @fromTime AND {_dbSettings[Columns.Time]} <= @toTime " +
                    $"ORDER BY {sortingField}",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                    .ToList();
            }
        }
    }
}
