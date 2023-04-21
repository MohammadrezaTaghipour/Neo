using MassTransit;
using MongoDB.Driver;
using Neo.Application.Contracts.StreamEventTypes;
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
    private readonly IRequestClient<StreamEventTypeStatusRequested> _statusRequestClient;

    public StreamEventTypeQueryService(IMongoDatabase mongoDatabase,
        IRequestClient<StreamEventTypeStatusRequested> statusRequestClient)
    {
        _mongoDatabase = mongoDatabase;
        _statusRequestClient = statusRequestClient;
    }

    public async Task<StreamEventTypeResponse?> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var status = (await _statusRequestClient
               .GetResponse<StreamEventTypeStatusRequestExecuted>(
               new StreamEventTypeStatusRequested
               {
                   Id = id
               }).ConfigureAwait(false)).Message;
        if (status.Faulted)
            return StreamEventTypeResponse.CreateFaulted(
                new StatusResponse(status.Completed,
                status.ErrorCode,
                status.ErrorMessage));

        var filter = Builders<StreamEventTypeProjectionModel>
            .Filter
            .Eq(_ => _.Id, id);

        var projectionModel = await GetCollection<StreamEventTypeProjectionModel>()
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (projectionModel == null)
            return null;

        return MapFrom(projectionModel, status);
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
            response.Add(MapFrom(projectionModel, null));
        }
        return response;
    }

    private IMongoCollection<T> GetCollection<T>()
    {
        var collection = _mongoDatabase.GetCollection<T>();
        return collection;
    }

    static StreamEventTypeResponse MapFrom(StreamEventTypeProjectionModel projectionModel,
        StreamEventTypeStatusRequestExecuted? statusExecutedResponse)
    {
        StatusResponse status = null;
        if (statusExecutedResponse != null)
            status = new StatusResponse(statusExecutedResponse.Completed,
                statusExecutedResponse.ErrorCode,
                statusExecutedResponse.ErrorMessage);
        return new StreamEventTypeResponse(projectionModel.Id,
            projectionModel.Title, projectionModel.Removed,
            projectionModel.Metadata.Select(_ =>
                new StreamEventTypeMetadataResponse(_.Title)).ToList(),
            status);
    }
}