using MassTransit;

namespace Neo.Application.StreamContexts;

public static class RoutingSlipAddress
{
    private static readonly KebabCaseEndpointNameFormatter _formatter = new(false);

    public static Uri ForQueue<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
            where TArguments : class
    {
        return new Uri($"queue:{_formatter.ExecuteActivity<T, TArguments>()}");
    }
}