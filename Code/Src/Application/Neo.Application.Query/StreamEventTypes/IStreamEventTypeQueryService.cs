using MongoDB.Driver;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Projection.MongoDB.StreamEventTypes;

namespace Neo.Application.Query.StreamEventTypes;

public interface IStreamEventTypeQueryService : IQueryService
{
    Task<StreamEventTypeResponse?> Get(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<StreamEventTypeResponse>> GetAll(CancellationToken cancellationToken);
}

public class StreamEventTypeQueryService : IStreamEventTypeQueryService
{
    private readonly IMongoDatabase _mongoDatabase;

    public StreamEventTypeQueryService(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    public async Task<StreamEventTypeResponse?> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var filter = Builders<StreamEventTypeProjectionModel>
            .Filter
            .Eq(_ => _.Id, id);

        var projectionModel = await GetCollection<StreamEventTypeProjectionModel>()
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (projectionModel == null)
            return null;

        return MapFrom(projectionModel);
    }


    public async Task<IReadOnlyCollection<StreamEventTypeResponse>> GetAll(
        CancellationToken cancellationToken)
    {
        var filter = Builders<StreamEventTypeProjectionModel>
            .Filter
            .Empty;

        var projectionModels = await GetCollection<StreamEventTypeProjectionModel>()
            .Find(filter)
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (projectionModels == null)
            return Array.Empty<StreamEventTypeResponse>();

        var response = new List<StreamEventTypeResponse>();
        foreach (var projectionModel in projectionModels)
        {
            response.Add(MapFrom(projectionModel));
        }
        return response;
    }

    private IMongoCollection<T> GetCollection<T>()
    {
        var collection = _mongoDatabase.GetCollection<T>();
        return collection;
    }

    static StreamEventTypeResponse MapFrom(
        StreamEventTypeProjectionModel projectionModel)
    {
        return new StreamEventTypeResponse(projectionModel.Id,
            projectionModel.Title, projectionModel.Removed,
            projectionModel.Metadata.Select(_ =>
                new StreamEventTypeMetadataResponse(_.Title)).ToList());
    }
}