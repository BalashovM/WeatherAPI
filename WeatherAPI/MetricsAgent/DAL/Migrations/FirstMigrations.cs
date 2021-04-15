using FluentMigrator;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("cpumetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();

            Create.Table("dotnetmetrics")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();

            Create.Table("hddmetrics")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();

            Create.Table("networkmetrics")
                 .WithColumn("id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Value").AsInt32()
                 .WithColumn("Time").AsInt64();

            Create.Table("rammetrics")
                 .WithColumn("id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Value").AsInt32()
                 .WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("cpumetrics");
            Delete.Table("dotnetmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("networkmetrics");
            Delete.Table("rammetrics");
        }
    }
}




