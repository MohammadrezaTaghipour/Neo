
namespace Suzianna.Core.Screenplay.Actors;

public static class ActorExtensions
{
    public static void Remember<T>(this Actor actor, T value)
    {
        var key = nameof(T);
        actor.Remember(key, value);
    }

    public static T Recall<T>(this Actor actor)
    {
        var key = nameof(T);
        return actor.Recall<T>(key);
    }
}
