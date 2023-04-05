using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MongoDB.Driver;
public static class MongoClientExtensions
{
    private static readonly Type TableAttributeType = typeof(TableAttribute);

    public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase db)
    {
        var name = GetCollectionName<T>(db);
        return db.GetCollection<T>(name);
    }

    public static string GetCollectionName<T>(this IMongoDatabase db)
    {
        var type = typeof(T);
        var attr = type
            .GetTypeInfo()
            .GetCustomAttribute(TableAttributeType, true) as TableAttribute;

        if (attr == null)
        {
            return type.Name.ToLower();
        }

        return attr.Name;
    }

}
