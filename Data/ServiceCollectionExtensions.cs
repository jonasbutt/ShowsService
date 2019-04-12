using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ShowsService.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddData(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Shows");
            services.AddTransient<IShowsRepository, ShowsRepository>();
            services.AddDbContext<ShowsContext>(
                options => options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string not set")),
                ServiceLifetime.Transient,
                ServiceLifetime.Transient);
        }
    }
}
