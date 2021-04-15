using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

            //ConfigureSqlLiteConnection(services);
            //services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            //services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
            //services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            //services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            //services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(ConnectionString)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
            
            //Добавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //Добавляем задачи
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton<DotNetMetricJob>();

            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд

            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд

            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд

            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд

            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд

            services.AddHostedService<QuartzHostedService>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            string connectionString = "Data Source=:memory:";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
            services.AddSingleton(connection);
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(10, 20)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(80, 60)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(34, 40)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(111, 10)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(56, 80)";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(10, 10)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(30, 20)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(20, 30)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(15, 40)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(25, 50)";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY, freesize DOUBLE)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO hddmetrics(freesize) VALUES(1002546.254775)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO hddmetrics(freesize) VALUES(257698.22003)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO hddmetrics(freesize) VALUES(96654232.502)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO hddmetrics(freesize) VALUES(5587.258)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO hddmetrics(freesize) VALUES(987456454.203)";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS rammetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY, available DOUBLE)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO rammetrics(available) VALUES(50.2)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO rammetrics(available) VALUES(689.12)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO rammetrics(available) VALUES(387.119)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO rammetrics(available) VALUES(897.6)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO rammetrics(available) VALUES(144.102)";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(87, 15)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(123, 25)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(98, 30)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(9, 12)";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(57, 16)";
                command.ExecuteNonQuery();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // запускаем миграции
            migrationRunner.MigrateUp();
        }
    }
}
