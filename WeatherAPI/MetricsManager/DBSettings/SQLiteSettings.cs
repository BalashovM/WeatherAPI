using System.Collections.Generic;

namespace MetricsManager.DBSettings
{
    /// <summary>Класс настроек подключения SQLite базы данных</summary>
    public class SQLiteSettings : IDBSettings
    {
		/// <summary>Словарь для хранения имен таблиц</summary>
		private readonly Dictionary<Tables, string> tablesNames = new Dictionary<Tables, string>
		{
			{Tables.Agent, "agent"},
			{Tables.CpuMetric, "cpumetric" },
			{Tables.DotNetMetric, "dotnetmetric" },
			{Tables.HddMetric, "hddmetric" },
			{Tables.NetworkMetric, "networkmetric" },
			{Tables.RamMetric, "rammetric" },
		};

		/// <summary>Словарь для хранения имен рядов в таблицах</summary>
		private Dictionary<Columns, string> rowsNames = new Dictionary<Columns, string>
		{
			{Columns.Id, "id" },
			{Columns.Value, "value" },
			{Columns.Time, "time" },
			{Columns.Name, "name" },
			{Columns.Status, "status" },
			{Columns.IpAddress, "ipaddress" },
		};

		/// <summary> Строка для подключения к базе данных </summary>
		private readonly string connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";

		public string ConnectionString
		{
			get
			{
				return connectionString;
			}
		}

		public string this[Tables key]
		{
			get
			{
				return tablesNames[key];
			}
		}

		public string this[Columns key]
		{
			get
			{
				return rowsNames[key];
			}
		}
	}
}
