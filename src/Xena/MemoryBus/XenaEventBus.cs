using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus;

internal class XenaEventBus : IXenaEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public XenaEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IXenaEvent
    {
        var eventType = @event.GetType();
        var eventHandlerType = typeof(IXenaEventHandler<>).MakeGenericType(eventType);

        var eventHandlers = _serviceProvider.GetServices(eventHandlerType);

        var eventTasks = eventHandlers
            .Cast<IXenaEventHandler<TEvent>>()
            .Select(h => h.Handle(@event));

        await Task.WhenAll(eventTasks);
    }
}