using Microsoft.Extensions.DependencyInjection;

namespace ShowsService.Data.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddTransient<IShowsRepository, ShowsRepository>();
        }
    }
}
