using FluentMigrator;
using MetricsAgent.DBSettings;
using System;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        /// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IDBSettings dbSettings;


        public FirstMigration(IDBSettings dbSettings)
        {
            dbSettings = dbSettings;
        }

        public override void Up()
        {
            foreach (Tables tableName in Enum.GetValues(typeof(Tables)))
            {
                Create.Table(dbSettings[tableName])
                    .WithColumn(dbSettings[Columns.Id]).AsInt64().PrimaryKey().Identity()
                    .WithColumn(dbSettings[Columns.Value]).AsInt32()
                    .WithColumn(dbSettings[Columns.Time]).AsInt64();
            }
        }

        public override void Down()
        {
            foreach (Tables tableName in Enum.GetValues(typeof(Tables)))
            {
                Delete.Table(dbSettings[tableName]);
            }
        }
    }
}




