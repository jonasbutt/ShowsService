using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ShowsService.Api
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
