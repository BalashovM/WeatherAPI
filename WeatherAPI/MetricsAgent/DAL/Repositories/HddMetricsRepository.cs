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
	/// Репозиторий для обработки Hdd метрик
	/// </summary>
    public class HddMetricsRepository : IHddMetricsRepository
    {
        /// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IDBSettings _dbSettings;
        public HddMetricsRepository(IDBSettings dbSettings)
        {
            _dbSettings = dbSettings;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void Create(HddMetric item)
        {
            using var connection = new SQLiteConnection(_dbSettings.ConnectionString);
            connection.Execute(
            $"INSERT INTO {_dbSettings[Tables.HddMetric]} " +
            $"(value, time) " +
            $"VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds()
                });
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                connection.Execute(
                $"DELETE FROM {_dbSettings[Tables.HddMetric]} " +
                $"WHERE {_dbSettings[Columns.Id]} = @id",
                new
                {
                    id = id
                });
            }
        }

        public void Update(HddMetric item)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                connection.Execute(
                $"UPDATE {_dbSettings[Tables.HddMetric]} " +
                $"SET {_dbSettings[Columns.Value]} = @value, {_dbSettings[Columns.Time]} = @time " +
                $"WHERE {_dbSettings[Columns.Id]} = @id",
                new
                {
                    value = item.Value,
                    time = item.Time.ToUnixTimeSeconds(),
                    id = item.Id
                });
            }
        }

        public IList<HddMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<HddMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " +
                    $"FROM {_dbSettings[Tables.HddMetric]}")
                    .ToList();
            }
        }

        public HddMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .QuerySingle<HddMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " +
                    $"FROM {_dbSettings[Tables.HddMetric]} " +
                    $"WHERE {_dbSettings[Columns.Id]} = @id",
                    new
                    {
                        id = id
                    });
            }
        }

        public IList<HddMetric> GetByPeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<HddMetric>(
                    $"SELECT  {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " +
                    $"FROM {_dbSettings[Tables.HddMetric]} " +
                    $"WHERE {_dbSettings[Columns.Time]} >= @fromTime AND {_dbSettings[Columns.Time]} <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    })
                    .ToList();
            }
        }

        public IList<HddMetric> GetByPeriodWithSorting(DateTimeOffset fromTime, DateTimeOffset toTime, string sortingField)
        {
            using (var connection = new SQLiteConnection(_dbSettings.ConnectionString))
            {
                return connection
                    .Query<HddMetric>(
                    $"SELECT {_dbSettings[Columns.Id]},  {_dbSettings[Columns.Time]}, {_dbSettings[Columns.Value]} " +
                    $"FROM {_dbSettings[Tables.HddMetric]} " +
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
