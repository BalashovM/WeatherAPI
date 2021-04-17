using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Client;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;
using MetricsManager.DBSettings;
using MetricsManager.Scheduler;
using MetricsManager.Scheduler.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MetricsManager
{
    public class Startup
    {
        /// <summary>
        /// �������� ������� �������
        /// </summary>
        private const string CronExpression = "0/5 * * * * ?";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            // ������� ��� �������� � ������� ������
            services.AddHttpClient<IMetricsManagerClient, MetricsManagerClient>();

            services.AddFluentMigratorCore()
               .ConfigureRunner(rb => rb
                   // ��������� ��������� SQLite 
                   .AddSQLite()
                   // ������������� ������ �����������
                   .WithGlobalConnectionString(new SQLiteSettings().ConnectionString)
                   // ������������ ��� ������ ������ � ����������
                   .ScanIn(typeof(Startup).Assembly).For.Migrations()
               ).AddLogging(lb => lb
                   .AddFluentMigratorConsole());

            JobsSheduleSettings(services);

        }
        /// <summary>
		/// ��������� ����� ������ �� ����������
		/// </summary>
		/// <param name="services"></param>
		private void JobsSheduleSettings(IServiceCollection services)
        {
            // ������������ �������
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // ������ ��� ������ ������
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton<RamMetricJob>();

            // ������������� ������� �����
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: CronExpression));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: CronExpression));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: CronExpression));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: CronExpression));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: CronExpression));

            // ������ ��� ������� ����� � ������� Quarz
            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
