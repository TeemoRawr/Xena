using Xena.MemoryBus.Interfaces;

namespace Xena.MemoryBus;

internal class XenaEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public XenaEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Publish<TEvent>(TEvent @event) where TEvent : IXenaEvent
    {
        var eventHandlers = _serviceProvider.GetServices<IXenaEventHandler<TEvent>>();

        var eventTasks = eventHandlers.Select(h => h.Handle(@event));

        await Task.WhenAll(eventTasks);
    }
}