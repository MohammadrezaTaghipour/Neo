using Neo.Infrastructure.Framework.Redis.Configurations;
using StackExchange.Redis;

namespace Neo.Infrastructure.Framework.Redis;

public interface IRedisConnectionFactory
{
    ConnectionMultiplexer GetConnection();
}

public class RedisConnectionFactory : IRedisConnectionFactory
{
    private readonly RedisOptions _options;

    public RedisConnectionFactory(RedisOptions options)
    {
        _options = options;
    }

    public ConnectionMultiplexer GetConnection()
    {
        var configuration = ConfigurationOptions.Parse(_options.ConnectionString);
        return ConnectionMultiplexer.Connect(configuration);
    }
}