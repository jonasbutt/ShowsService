using Microsoft.Extensions.DependencyInjection;
using ShowsService.Tools.Serialization;

namespace ShowsService.Tools
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTools(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
        }
    }
}
