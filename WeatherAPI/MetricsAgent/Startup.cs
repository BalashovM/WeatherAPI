using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DBSettings;
using MetricsAgent.Scheduler;
using MetricsAgent.Scheduler.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsAgent
{
    public class Startup
    {
        /// <summary>
		/// Интервал запуска заданий
		/// </summary>
		private const string CronExpression = "0/5 * * * * ?";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IDBSettings, SQLiteSettings>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(new SQLiteSettings().ConnectionString)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            JobsSheduleSettings(services);
        }

        /// <summary>
		/// Настройка сбора метрик по расписанию
		/// </summary>
		/// <param name="services"></param>
        private void JobsSheduleSettings(IServiceCollection services)
        {
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
                cronExpression: CronExpression)); 

            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: CronExpression));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: CronExpression));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: CronExpression));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: CronExpression));

            services.AddHostedService<QuartzHostedService>();

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
