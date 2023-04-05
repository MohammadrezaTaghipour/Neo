namespace MongoDB.Driver;

public abstract class BaseMongoProjectionHandler
{
    private readonly IMongoDatabase _mongoDatabase;

    protected BaseMongoProjectionHandler(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }

    protected IMongoCollection<T> GetCollection<T>()
    {
        var collection = _mongoDatabase.GetCollection<T>();
        return collection;
    }
}