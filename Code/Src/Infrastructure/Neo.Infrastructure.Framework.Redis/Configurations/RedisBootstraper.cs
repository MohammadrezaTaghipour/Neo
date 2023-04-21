using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neo.Infrastructure.Framework.Configurations;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Infrastructure.Framework.Redis.Configurations;

public class RedisBootstraper : IBootstrapper
{
    private readonly IConfiguration _configuration;

    public RedisBootstraper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Bootstrap(IServiceCollection services)
    {
        var redisOptions = _configuration.GetSection("redis").Get<RedisOptions>();

        services.AddSingleton<IRedisConnectionFactory>(_ =>
        {
            return new RedisConnectionFactory(redisOptions);
        });

        services.AddSingleton<INotificationPublisher, RedisNotificationPublisher>();
    }
}