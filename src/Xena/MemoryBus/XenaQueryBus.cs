using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus;

internal class XenaQueryBus : IXenaQueryBus
{
    private readonly IServiceProvider _serviceProvider;

    public XenaQueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> Query<TResult>(IXenaQuery<TResult> query)
    {
        var queryType = query.GetType();
        var castedQueryType = queryType.GetInterfaces()
            .Where(p => p.IsGenericType)
            .Single(p => p.GetGenericTypeDefinition() == typeof(IXenaQuery<>));

        var resultType = castedQueryType.GetGenericArguments().First();
        var queryHandlerType = typeof(IXenaQueryHandler<,>).MakeGenericType(queryType, resultType);

        dynamic queryHandler = _serviceProvider.GetRequiredService(queryHandlerType);

        var result = await queryHandler.Handle((dynamic)query);

        return result;
    }
}