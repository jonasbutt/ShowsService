using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace ShowsService.Data.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedis(this IServiceCollection services)
        {
            var redisConfiguration = RedisConfiguration();
            services.AddSingleton(redisConfiguration);
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
            services.AddSingleton<IShowsRepository, ShowsRepository>();
        }

        private static RedisConfiguration RedisConfiguration()
        {
            var connectionString = Environment.GetEnvironmentVariable("ShowsService_Redis");
            var configurationOptions = ConfigurationOptions.Parse(connectionString ?? throw new InvalidOperationException("Connection string not set"));
            var dnsEndPoint = (DnsEndPoint) configurationOptions.EndPoints.First();
            return new RedisConfiguration
            {
                AbortOnConnectFail = configurationOptions.AbortOnConnectFail,
                AllowAdmin = configurationOptions.AllowAdmin,
                ConnectTimeout = configurationOptions.ConnectTimeout,
                Database = 0,
                Password = configurationOptions.Password,
                SyncTimeout = configurationOptions.SyncTimeout,
                Ssl = configurationOptions.Ssl,
                Hosts = new[] {new RedisHost {Host = dnsEndPoint.Host, Port = dnsEndPoint.Port}},
            };
        }
    }
}
