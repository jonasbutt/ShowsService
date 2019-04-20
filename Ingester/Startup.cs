using System;
using System.Net;
using System.Net.Http.Headers;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ShowsService.Data.Sql;
using ShowsService.Ingester.Hangfire;
using ShowsService.Ingester.Jobs;
using ShowsService.Ingester.TvMaze;
using ShowsService.Tools;

namespace ShowsService.Ingester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSql();
            services.AddTools();
            services.AddHostedService<ShowIngestionBackgroundService>();
            services.AddTransient<IDownloadShowsJob, DownloadShowsJob>();
            services.AddTransient<IDownloadCastJob, DownloadCastJob>();
            services.AddTransient<ISaveShowJob, SaveShowJob>();

            var connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Hangfire");
            services.AddHangfire(configuration =>
            {
                configuration.UseFilter(new ExtendedJobExpirationTimeoutFilter());
                configuration.UseSqlServerStorage(
                    connectionString ?? throw new InvalidOperationException("Connection string not set"),
                                  new SqlServerStorageOptions
                                  {
                                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                      QueuePollInterval = TimeSpan.FromSeconds(3),
                                      UseRecommendedIsolationLevel = true,
                                      UsePageLocksOnDequeue = true,
                                      DisableGlobalLocks = true
                                  });
            });
            services.AddHangfireServer();

            services.AddHttpClient<ITvMazeClient, TvMazeClient>(httpClient =>
                     {
                         httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("TheCodeArchitect")));
                         httpClient.BaseAddress = new Uri("https://api.tvmaze.com");
                     })
                    .AddPolicyHandler(_ =>
                         HttpPolicyExtensions.HandleTransientHttpError()
                                             .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                                             .WaitAndRetryAsync(10, __ => TimeSpan.FromSeconds(10)));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseHangfireDashboard(
                string.Empty,
                new DashboardOptions
                {
                    AppPath = null,
                    IsReadOnlyFunc = _ => true,
                    DisplayStorageConnectionString = false,
                    Authorization = new[] { new PublicAccessDashboardAuthorizationFilter() }
                });
        }
    }
}
