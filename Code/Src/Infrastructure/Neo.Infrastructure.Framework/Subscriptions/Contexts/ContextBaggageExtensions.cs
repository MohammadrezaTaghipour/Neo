namespace Neo.Infrastructure.Framework.Subscriptions.Contexts;

public static class ContextBaggageExtensions {
    public static T WithItem<T, TItem>(this T ctx, string key, TItem item) 
        where T : IMessageConsumeContext {
        ctx.Items.AddItem(key, item);
        return ctx;
    }
}