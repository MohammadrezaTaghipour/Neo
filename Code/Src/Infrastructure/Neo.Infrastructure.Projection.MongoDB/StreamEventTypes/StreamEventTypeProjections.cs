using MongoDB.Bson;
using MongoDB.Driver;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Projections;

namespace Neo.Infrastructure.Projection.MongoDB.StreamEventTypes;

public class StreamEventTypeProjections :
    IDominEventProjector<StreamEventTypeDefined>,
    IDominEventProjector<StreamEventTypeModified>,
    IDominEventProjector<StreamEventTypeRemoved>
{
    private readonly IMongoDatabase _mongoDatabase;

    public StreamEventTypeProjections(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public async Task Project(StreamEventTypeDefined eventToProject,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = StreamEventTypeViewModelMapper.MapFrom(eventToProject);
            await GetCollection<StreamEventTypeProjectionModel>()
                  .InsertOneAsync(model, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.Message);
            throw;
        }
    }

    public async Task Project(StreamEventTypeModified eventToProject,
        CancellationToken cancellationToken)
    {
        var model = StreamEventTypeViewModelMapper.MapFrom(eventToProject);
        var filter = new FilterDefinitionBuilder<StreamEventTypeProjectionModel>()
            .Eq(o => o.Id, eventToProject.Id.Value);
        await GetCollection<StreamEventTypeProjectionModel>()
            .UpdateOneAsync(filter, new BsonDocument("$set", model.ToBsonDocument()),
                new UpdateOptions { IsUpsert = false }, cancellationToken);
    }

    public async Task Project(StreamEventTypeRemoved eventToProject,
        CancellationToken cancellationToken)
    {
        var filter = Builders<StreamEventTypeProjectionModel>
            .Filter
            .Eq(_ => _.Id, eventToProject.Id.Value);
        var update = Builders<StreamEventTypeProjectionModel>
            .Update
            .Set(_ => _.Removed, true);
        await GetCollection<StreamEventTypeProjectionModel>()
            .UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    private IMongoCollection<T> GetCollection<T>()
    {
        var collection = _mongoDatabase.GetCollection<T>();
        return collection;
    }
}