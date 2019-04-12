using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ShowsService.Ingester
{
    public class Program
    {
        public static void Main(string[] arguments)
        {
            CreateWebHostBuilder(arguments).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] arguments) =>
            WebHost.CreateDefaultBuilder(arguments)
                   .UseStartup<Startup>();
    }
}
