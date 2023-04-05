
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Neo.Infrastructure.Framework.Configurations;

namespace Neo.Infrastructure.Framework.MongoDB.Configurations;

public class MongoBootstraper : IBootstrapper
{
    private readonly IConfiguration _configuration;

    public MongoBootstraper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Bootstrap(IServiceCollection services)
    {
        var mongoOptions = _configuration.GetSection("mongoDB").Get<MongoOptions>();

        services.AddScoped((a) => ConfigureMongo(mongoOptions));
    }

    static IMongoDatabase ConfigureMongo(MongoOptions options)
    {
        var settings = MongoClientSettings
            .FromConnectionString(options.ConnectionString);
        return new MongoClient(settings).GetDatabase(options.DatabaseName);
    }
}