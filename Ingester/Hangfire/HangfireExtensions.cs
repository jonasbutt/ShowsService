using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ShowsService.Ingester.Hangfire
{
    public static class HangfireExtensions
    {
        public static void AddHangfire(this IServiceCollection services)
        {
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
        }

        public static void UseHangfireDashboard(this IApplicationBuilder app)
        {
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
